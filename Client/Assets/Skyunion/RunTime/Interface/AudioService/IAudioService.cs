using System;
using UnityEngine;

namespace Skyunion
{
    public interface IAudioService : IModule
    {
        float GetSfxVolume();
        void SetSfxVolume(float v);

        float GetMusicVolume();
        void SetMusicVolume(float v);

        void Enable3DSound(bool enable);
        bool IsEnabled3DSound();

        string GetCurBgmName();
        void PlayBgm(string name);

        void GetSfxTime(string name, Action<float> action);
        void PlayOneShot(string name, Action<AudioHandler> action = null);
        void PlayOneShot3D(string name, GameObject obj, Action<AudioHandler> action = null);
        void PlayOneShotAtPos(string name, Vector3 pos, Action<AudioHandler> action = null);
        void PlayLoop(string name, Action<AudioHandler> action = null);
        void PlayLoop2D(string name, GameObject obj, Action<AudioHandler> action = null);
        void PlayLoop3D(string name, GameObject obj, Action<AudioHandler> action = null);
        void PlayLoopAtPos(string name, Vector3 pos, Action<AudioHandler> action = null);

        void SetHandlerVolume(AudioHandler handler, float volume);
        void FadeHandlerVolume(AudioHandler handler, float toVolume, float speed, bool destroyWhenDone);
        AudioHandler FadeToStop(AudioHandler handler, float speed, bool isDetach);
        void StopByHandler(AudioHandler handler);
        void StopByName(string name);
        void StopAllByName(string name);
        bool IsHandlerNull(AudioHandler handler);
        AudioHandler GetHandlerByName(string name);
        AudioHandler[] GetHandlersByName(string name);
        void OnGamePause(bool isPause);
        void FadeAudioListener(float toVolume, float speed);
        int GetChannel(string key);
        void SetChannel(string key, int channel);
        void Clear();
        void AddHandler(AudioHandler handler);
        void OnHandlerDestroy(AudioHandler handler);
        void StopGroupByName(string groupName);
        void MuteGroup(string groupName, bool mute);
        void SetEnvSoundMinUpdateCamDist(float minUpdateCamDist);
        void SetEnvSoundMaxPlayCount(int maxCount);
        void SetEnvSoundLodDistance(float lodDistance);
        void SetLodDistance(float lodDistance);
        void Register(EnvSoundHandler comp);
        void UnRegister(EnvSoundHandler comp);
    }
}
