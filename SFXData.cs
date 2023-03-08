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
                var clips = Resources.LoadAll<AudioClip>(sourceFolder + "/" + clipsPath);
                foreach (var clip in clips)
                {
                    sfx.TryAdd(clip.name, clip);
                }
            }
        }
    }
}