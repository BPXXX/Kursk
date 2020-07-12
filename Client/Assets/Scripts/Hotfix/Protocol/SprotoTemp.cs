/*
 *自定义的临时类 
 */

using System;
using Sproto;
using System.Collections.Generic;

namespace SprotoTemp
{
    public class CityBuildInfoTemp
    {
        public const string HomeBuildInfoChange = "HomeBuildInfoChange";
        public Int64 buildingId;
        public Int64 type;//类型指向EnumCityBuildingType
        public Int64 level;
        public bool isupgrade;//是否正在升级
        public Int64 timeStamp;
        public Int64 posx;
        public Int64 posy;
        public string modelId;
    }
}