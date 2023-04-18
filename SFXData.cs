using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class SFXData : MonoBehaviour
    {
        public string sourceFolder;
        public AudioClip this[string clip] => sfx[clip];
        Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();
        public AudioClip GetSFX(string name)
        {
            return sfx[name];
        }
        public void PlaySFX(string name, float volume = 1f, float pitch = 1f)
        {
            var sfx = GetSFX(name);
            float oldPitch = source.pitch;
            source.pitch = pitch;
            source.PlayOneShot(sfx, volume);
            source.pitch = oldPitch;
        }
        public Coroutine PlaySFXIndirect(string name, float volume = 1f, float pitch = 1f)
        {
            return StartCoroutine(IEnumPlaySFXIndirect(name, volume, pitch));
        }
        public IEnumerator IEnumPlaySFXIndirect(string name, float volume = 1f, float pitch = 1f) 
        {
            var sfx = GetSFX(name);
            source.clip = sfx;
            float oldPitch = source.pitch, oldVolume = source.volume;
            source.loop = false; // just incase
            source.Play();
            yield return new WaitUntil(() => { print(!source.isPlaying); return !source.isPlaying; });
            source.volume = oldVolume;
            source.pitch = oldPitch;
        }
        AudioSource source;
        public AudioSource Source => source;
        void Awake()
        {
            AddSFXToDictionary();
            source = GetComponent<AudioSource>();
        }

        public string[] clipsPaths;
        void AddSFXToDictionary()
        {
            for (int i = 0; i < clipsPaths.Length; i++)
            {
                var clipsPath = clipsPaths[i];
                AudioClip[] clips;
                if (!string.IsNullOrWhiteSpace(sourceFolder))
                {
                    clips = Resources.LoadAll<AudioClip>(sourceFolder + "/" + clipsPath);
                }
                else
                {
                    clips = Resources.LoadAll<AudioClip>(clipsPath);
                }
                foreach (var clip in clips)
                {
                    sfx.TryAdd(clip.name, clip);
                }
            }
        }
    }
}