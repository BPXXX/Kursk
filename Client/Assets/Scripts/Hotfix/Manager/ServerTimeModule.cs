
using System;
using Hotfix;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class ServerTimeModule : TSingleton<ServerTimeModule>
    {
        private Int64 Lose_Time = 0;

        public Int64 Ping = 0;

        private long LostMaintentTime = 0;

        public void UpdateServerTime(Int64 serverTime, Int64 clientTime)
        {
            Int64 local_time = GetTimestamp();
            Ping = (Int64)Math.Floor((float)(local_time - clientTime) / 2);
            Lose_Time = serverTime + Ping - clientTime;
        }

        public long GetServerTime()
        {
            return GetTimestamp() + Lose_Time;
        }

        // 获取本地时间戳
        public Int64 GetTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public void UpdateMaintentTime(Int64 serverTime, Int64 clientTime)
        {
            long local_time = GetTimestamp();
            long Ping = (long)Math.Floor((float)(local_time - clientTime) / 2);
            LostMaintentTime = serverTime + Ping - local_time;
        }

        public DateTime GetMaintentTime()
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(GetTimestamp()+ LostMaintentTime));
        }
    }
}

