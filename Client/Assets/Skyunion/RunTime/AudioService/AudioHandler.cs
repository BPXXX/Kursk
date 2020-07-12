using System;
using UnityEngine;

namespace Skyunion
{
    public class AudioHandler
    {
        private bool _isSelfCreated;

        private AudioSource _audioSource;

        public GameObject gameObject;

        public Vector3 pos = Vector3.zero;

        public AudioConfig Config;

        public AudioGroup Group;

        public AudioGroupConfig GroupConfig;

        public string AudioName;

        public bool IsSfx = true;

        public bool NeedDestroyGameObject;

        public bool IsPlaying;

        public bool IsLoop;

        public float Volume = 1f;

        public int AudioIndex;

        public float TimeToNext;

        public bool IsFading;

        public bool DestroyWhenDoneFade;

        public float FadeToVolume;

        public float FadeSpeed;

        public bool IsDestroyed;

        public AudioSource audioSource
        {
            get
            {
                if (this._audioSource == null)
                {
                    this._audioSource = this.gameObject.AddComponent<AudioSource>();
                    this._isSelfCreated = true;
                }
                return this._audioSource;
            }
        }

        private AudioHandler(GameObject obj)
        {
            this.gameObject = obj;
        }

        public static AudioHandler GetHandler(GameObject obj)
        {
            return new AudioHandler(obj);
        }

        public string GetAudioName()
        {
            return this.AudioName;
        }

        public void FadeVolume(float toVolume, float speed, bool destroyWhenDone)
        {
            if (this.IsFading && this.DestroyWhenDoneFade)
            {
                return;
            }
            this.IsFading = true;
            this.FadeToVolume = toVolume;
            this.FadeSpeed = speed;
            this.DestroyWhenDoneFade = destroyWhenDone;
        }

        public void SetAudioSource(AudioSource audioSource)
        {
            this._audioSource = audioSource;
            this._isSelfCreated = false;
        }

        public void OnDestroy()
        {
            CoreUtils.audioService.OnHandlerDestroy(this);
            if (this._isSelfCreated)
            {
                UnityEngine.Object.Destroy(this._audioSource);
            }
            this.IsDestroyed = true;
        }
    }
}