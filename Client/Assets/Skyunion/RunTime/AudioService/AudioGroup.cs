using System;
using System.Collections.Generic;

namespace Skyunion
{
    public class AudioGroup
    {
        public string GroupId;

        public bool Mute;

        public int PlayCount;

        public bool IsHasPlayingStateDirty;

        public List<AudioHandler> PlayingHandler = new List<AudioHandler>();
    }
}