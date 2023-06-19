using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        Dictionary<string, AudioClip> music = new Dictionary<string, AudioClip>();
        AudioSource musicSource;

        static MusicManager instance;
        // Start is called before the first frame update
        void Awake()
        {
            fadingOut = false;
            instance = this;
            musicClip = null;
            musicSource = GetComponent<AudioSource>();
            AddMusicToDictionary();
        }
        public string songsPath;
        void AddMusicToDictionary()
        {
            var clips = Resources.LoadAll<AudioClip>(songsPath);
            foreach (var clip in clips)
            {
                music.Add(clip.name, clip);
            }
        }

        static string musicClip;
        public static string MusicClip
        {
            get => musicClip;
            set
            {
                if (musicClip != value)
                {
                    if (value == null)
                    {
                        instance.musicSource.clip = null;
                    }
                    else
                    {
                        AudioClip clip;
                        instance.music.TryGetValue(value, out clip);
                        instance.musicSource.clip = clip;
                    }
                }
                musicClip = value;
            }
        }
        public static void Stop()
        {
            MusicClip = null;
            instance.musicSource.Stop();
        }
        public static Coroutine Fadeout(float time = 1f)
        {
            if (!fadingOut && instance)
            {
                return instance.StartCoroutine(FadeoutIEnum(time));
            } return null;
        }
        static bool fadingOut;
        static IEnumerator FadeoutIEnum(float time)
        {
            fadingOut = true;
            var initialVolume = instance.musicSource.volume;
            for (float i = 0; i <= time; i += Time.unscaledDeltaTime)
            {
                instance.musicSource.volume = initialVolume * (1 - i/time);
                yield return null;
            }
            Stop();
            instance.musicSource.volume = initialVolume;
            fadingOut = false;
        }
        void Update()
        {
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }
}