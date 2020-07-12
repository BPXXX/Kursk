using System;
using UnityEngine;

namespace Skyunion
{
    // 需要是可回收对象才可以
    public class AudioAutoPlay : MonoBehaviour
    {
        public string m_audio_name = string.Empty;

        private void OnSpawn()
        {
            if (this.m_audio_name != string.Empty)
            {
                CoreUtils.audioService.PlayOneShot(this.m_audio_name, null);
            }
        }
    }
}
