using System;

namespace Skyunion
{
    public class Audio3DConfig
    {
        public float DopplerLevel;

        public float MinDistance = 1f;

        public float MaxDistance = 90f;

        public float SpatialBlend = 1f;

        public float Spread;

        public string CurveKey = string.Empty;
    }
}