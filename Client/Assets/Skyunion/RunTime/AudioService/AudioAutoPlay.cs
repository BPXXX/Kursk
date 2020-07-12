using System;
using UnityEngine;

namespace Skyunion
{
    // ��Ҫ�ǿɻ��ն���ſ���
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
