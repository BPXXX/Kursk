using System;
using System.Collections.Generic;

namespace Skyunion
{
    public class AudioGroupConfig
    {
        public string GroupId;

        public int MaxPlayCount = -1;

        public EAudioMaxCountAction MaxCountAction;

        public float BaseVolume = 1f;

        public bool IsSfx = true;

        public Dictionary<string, float> VolumeModifyGroups;

        public Dictionary<string, float> AffectedVolumeModifyGroups;
    }
}
