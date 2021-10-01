using Runner.ObjectPool;
using Runner.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.SoundSystem
{
    public class SoundsEmitter : MonoBehaviour
    {
        [SerializeField] private GameObject _soundEmitterPrefab;
        [SerializeField] private string _soundOnAwake;

        private Dictionary<string, AudioSource> _playing;

        void Awake()
        {
            _playing = new Dictionary<string, AudioSource>();
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(_soundOnAwake))
            {
                Play(_soundOnAwake);
            }
        }

        public AudioSource Play(string path, string soundName = null, float delay = 0f)
        {
            SoundData sound = SoundLoader.Get(path);

            if (!sound)
            {
                Debug.LogWarning($"[SoundsEmitter] Failed to get sound {path}");
                return null;
            }

            AudioSource source = GetSource(path);
            InitSource(source, sound);
            RemoveTimer(source, delay);

            if (delay > 0)
            {
                source.playOnAwake = false;
                source.PlayDelayed(delay);
            }
            else
            {
                source.Play();
            }

            if (soundName != null)
            {
                _playing.Add(soundName, source);
            }

            return source;
        }

        public void Stop(string soundName, float delay = 0f)
        {
            if (!_playing.ContainsKey(soundName)) return;

            AudioSource source = _playing[soundName];
            if (delay > 0f)
            {
                DestroySource(source, delay);
                return;
            }

            DestroySource(source);
        }

        private AudioSource GetSource(string soundName)
        {
            GameObject obj = ObjectPooler.Inst.GetObject(_soundEmitterPrefab);
            obj.name = "SoundEmitter: " + soundName;
            obj.transform.parent = gameObject.transform;

            return obj.GetComponent<AudioSource>();
        }

        private void InitSource(AudioSource source, SoundData sound)
        {
            source.clip = ArrayUtil.GetRandomItem<AudioClip>(sound.clips);
            source.pitch = ArrayUtil.GetRandomRangeFromArray(sound.pitch);
            source.volume = sound.volume;
            source.spatialBlend = (float)sound.soundType;
            source.outputAudioMixerGroup = sound.mixerGroup;
            source.loop = sound.looping;
        }

        // After the sound is done playing we don't really need it. Looping sound have to be stopped through StopSound()
        private void RemoveTimer(AudioSource source, float delay)
        {
            if (!source.loop)
            {
                StartCoroutine(DestroySource(source, delay + source.clip.length));
            }
        }

        private IEnumerator DestroySource(AudioSource source, float delay = 0f)
        {
            void DestroyObj()
            {
                source.Stop();
                ObjectPooler.Inst.ReturnObject(source.gameObject);
            }

            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
                DestroyObj();
            }
            else
            {
                DestroyObj();
            }
        }
    }
}
