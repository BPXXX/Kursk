using System;
using System.Collections.Generic;

namespace Skyunion
{
    public class AudioConfig
    {
        public enum EPlayMode
        {
            Normal,
            List,
            Random
        }

        public string GroupId;

        public int MaxPlayCount = -1;

        public EAudioMaxCountAction MaxCountAction;

        public string AudioName;

        public AudioConfig.EPlayMode PlayMode;

        public List<string> AudioNames;

        public float[] Intervals;

        public float FadeInTime;

        public string Config3DKey = "default";

        public string ChannelKey = string.Empty;

        public List<List<string>> ChannelAudioNames;
    }
}