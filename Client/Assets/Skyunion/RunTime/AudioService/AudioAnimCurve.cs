using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skyunion
{
    public class AudioAnimCurve : MonoBehaviour
    {
        [Serializable]
        public struct CurvePair
        {
            public string Key;

            public AnimationCurve Curve;
        }

        public CurvePair[] m_curveArray;

        [HideInInspector]
        public Dictionary<string, AnimationCurve> m_curves;

        private void Awake()
        {
            if (this.m_curveArray != null)
            {
                this.m_curves = new Dictionary<string, AnimationCurve>();
                for (int i = 0; i < this.m_curveArray.Length; i++)
                {
                    this.m_curves[this.m_curveArray[i].Key] = this.m_curveArray[i].Curve;
                }
            }
        }

        public AnimationCurve GetCurve(string key)
        {
            AnimationCurve result = null;
            if (this.m_curves != null)
            {
                this.m_curves.TryGetValue(key, out result);
            }
            return result;
        }

        public float Evaluate(string key, float t)
        {
            AnimationCurve curve = this.GetCurve(key);
            if (curve != null)
            {
                return curve.Evaluate(t);
            }
            return 0f;
        }
    }
}