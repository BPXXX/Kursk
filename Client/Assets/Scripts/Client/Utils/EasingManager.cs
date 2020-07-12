﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{

    public enum EasingEquation
    {
        Linear,
        QuadEaseOut, QuadEaseIn, QuadEaseInOut, QuadEaseOutIn,
        ExpoEaseOut, ExpoEaseIn, ExpoEaseInOut, ExpoEaseOutIn,
        CubicEaseOut, CubicEaseIn, CubicEaseInOut, CubicEaseOutIn,
        QuartEaseOut, QuartEaseIn, QuartEaseInOut, QuartEaseOutIn,
        QuintEaseOut, QuintEaseIn, QuintEaseInOut, QuintEaseOutIn,
        CircEaseOut, CircEaseIn, CircEaseInOut, CircEaseOutIn,
        SineEaseOut, SineEaseIn, SineEaseInOut, SineEaseOutIn,
        ElasticEaseOut, ElasticEaseIn, ElasticEaseInOut, ElasticEaseOutIn,
        BounceEaseOut, BounceEaseIn, BounceEaseInOut, BounceEaseOutIn,
        BackEaseOut, BackEaseIn, BackEaseInOut, BackEaseOutIn
    }

    public enum TextfxTextAnchor
    {
        UpperLeft,
        UpperCenter,
        UpperRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        LowerLeft,
        LowerCenter,
        LowerRight
    }

    public class EasingManager
    {
        public static float GetEaseProgress(EasingEquation ease_type, float linear_progress)
        {
            switch (ease_type)
            {
                case EasingEquation.Linear:
                    return linear_progress;
                case EasingEquation.BackEaseIn:
                    return EasingManager.BackEaseIn(linear_progress, 0, 1, 1);

                case EasingEquation.BackEaseInOut:
                    return EasingManager.BackEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.BackEaseOut:
                    return EasingManager.BackEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.BackEaseOutIn:
                    return EasingManager.BackEaseOutIn(linear_progress, 0, 1, 1);
                case EasingEquation.BounceEaseIn:
                    return EasingManager.BounceEaseIn(linear_progress, 0, 1, 1);
                case EasingEquation.BounceEaseInOut:
                    return EasingManager.BounceEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.BounceEaseOut:
                    return EasingManager.BounceEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.BounceEaseOutIn:
                    return EasingManager.BounceEaseOutIn(linear_progress, 0, 1, 1);
                case EasingEquation.CircEaseIn:
                    return EasingManager.CircEaseIn(linear_progress, 0, 1, 1);
                case EasingEquation.CircEaseInOut:
                    return EasingManager.CircEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.CircEaseOut:
                    return EasingManager.CircEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.CircEaseOutIn:
                    return EasingManager.CircEaseOutIn(linear_progress, 0, 1, 1);
                case EasingEquation.CubicEaseIn:
                    return EasingManager.CubicEaseIn(linear_progress, 0, 1, 1);
                case EasingEquation.CubicEaseInOut:
                    return EasingManager.CubicEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.CubicEaseOut:
                    return EasingManager.CubicEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.CubicEaseOutIn:
                    return EasingManager.CubicEaseOutIn(linear_progress, 0, 1, 1);
                case EasingEquation.ElasticEaseIn:
                    return EasingManager.ElasticEaseIn(linear_progress, 0, 1, 1);

                case EasingEquation.ElasticEaseInOut:
                    return EasingManager.ElasticEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.ElasticEaseOut:
                    return EasingManager.ElasticEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.ElasticEaseOutIn:
                    return EasingManager.ElasticEaseOutIn(linear_progress, 0, 1, 1);
                case EasingEquation.ExpoEaseIn:
                    return EasingManager.ExpoEaseIn(linear_progress, 0, 1, 1);
                case EasingEquation.ExpoEaseInOut:
                    return EasingManager.ExpoEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.ExpoEaseOut:
                    return EasingManager.ExpoEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.ExpoEaseOutIn:
                    return EasingManager.ExpoEaseOutIn(linear_progress, 0, 1, 1);
                case EasingEquation.QuadEaseIn:
                    return EasingManager.QuadEaseIn(linear_progress, 0, 1, 1);
                case EasingEquation.QuadEaseInOut:
                    return EasingManager.QuadEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.QuadEaseOut:
                    return EasingManager.QuadEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.QuadEaseOutIn:
                    return EasingManager.QuadEaseOutIn(linear_progress, 0, 1, 1);
                case EasingEquation.QuartEaseIn:
                    return EasingManager.QuartEaseIn(linear_progress, 0, 1, 1);
                case EasingEquation.QuartEaseInOut:
                    return EasingManager.QuartEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.QuartEaseOut:
                    return EasingManager.QuartEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.QuartEaseOutIn:
                    return EasingManager.QuartEaseOutIn(linear_progress, 0, 1, 1);
                case EasingEquation.QuintEaseIn:
                    return EasingManager.QuintEaseIn(linear_progress, 0, 1, 1);
                case EasingEquation.QuintEaseInOut:
                    return EasingManager.QuintEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.QuintEaseOut:
                    return EasingManager.QuintEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.QuintEaseOutIn:
                    return EasingManager.QuintEaseOutIn(linear_progress, 0, 1, 1);

                case EasingEquation.SineEaseIn:
                    return EasingManager.SineEaseIn(linear_progress, 0, 1, 1);
                case EasingEquation.SineEaseInOut:
                    return EasingManager.SineEaseInOut(linear_progress, 0, 1, 1);
                case EasingEquation.SineEaseOut:
                    return EasingManager.SineEaseOut(linear_progress, 0, 1, 1);
                case EasingEquation.SineEaseOutIn:
                    return EasingManager.SineEaseOutIn(linear_progress, 0, 1, 1);

                default:
                    return linear_progress;
            }
        }

        public static float GetEaseProgress(EasingEquation ease_type, float currentTime, float fistValue, float finalValue, float duration)
        {
            switch (ease_type)
            {
                case EasingEquation.Linear:
                    return EasingManager.Linear(currentTime, fistValue, finalValue, duration);

                case EasingEquation.BackEaseIn:
                    return EasingManager.BackEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.BackEaseInOut:
                    return EasingManager.BackEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.BackEaseOut:
                    return EasingManager.BackEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.BackEaseOutIn:
                    return EasingManager.BackEaseOutIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.BounceEaseIn:
                    return EasingManager.BounceEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.BounceEaseInOut:
                    return EasingManager.BounceEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.BounceEaseOut:
                    return EasingManager.BounceEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.BounceEaseOutIn:
                    return EasingManager.BounceEaseOutIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.CircEaseIn:
                    return EasingManager.CircEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.CircEaseInOut:
                    return EasingManager.CircEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.CircEaseOut:
                    return EasingManager.CircEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.CircEaseOutIn:
                    return EasingManager.CircEaseOutIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.CubicEaseIn:
                    return EasingManager.CubicEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.CubicEaseInOut:
                    return EasingManager.CubicEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.CubicEaseOut:
                    return EasingManager.CubicEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.CubicEaseOutIn:
                    return EasingManager.CubicEaseOutIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.ElasticEaseIn:
                    return EasingManager.ElasticEaseIn(currentTime, fistValue, finalValue, duration);

                case EasingEquation.ElasticEaseInOut:
                    return EasingManager.ElasticEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.ElasticEaseOut:
                    return EasingManager.ElasticEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.ElasticEaseOutIn:
                    return EasingManager.ElasticEaseOutIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.ExpoEaseIn:
                    return EasingManager.ExpoEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.ExpoEaseInOut:
                    return EasingManager.ExpoEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.ExpoEaseOut:
                    return EasingManager.ExpoEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.ExpoEaseOutIn:
                    return EasingManager.ExpoEaseOutIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuadEaseIn:
                    return EasingManager.QuadEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuadEaseInOut:
                    return EasingManager.QuadEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuadEaseOut:
                    return EasingManager.QuadEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuadEaseOutIn:
                    return EasingManager.QuadEaseOutIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuartEaseIn:
                    return EasingManager.QuartEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuartEaseInOut:
                    return EasingManager.QuartEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuartEaseOut:
                    return EasingManager.QuartEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuartEaseOutIn:
                    return EasingManager.QuartEaseOutIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuintEaseIn:
                    return EasingManager.QuintEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuintEaseInOut:
                    return EasingManager.QuintEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuintEaseOut:
                    return EasingManager.QuintEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.QuintEaseOutIn:
                    return EasingManager.QuintEaseOutIn(currentTime, fistValue, finalValue, duration);

                case EasingEquation.SineEaseIn:
                    return EasingManager.SineEaseIn(currentTime, fistValue, finalValue, duration);
                case EasingEquation.SineEaseInOut:
                    return EasingManager.SineEaseInOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.SineEaseOut:
                    return EasingManager.SineEaseOut(currentTime, fistValue, finalValue, duration);
                case EasingEquation.SineEaseOutIn:
                    return EasingManager.SineEaseOutIn(currentTime, fistValue, finalValue, duration);

                default:
                    return EasingManager.Linear(currentTime, fistValue, finalValue, duration); ;
            }
        }

        public static EasingEquation GetEaseTypeOpposite(EasingEquation ease_type)
        {
            switch (ease_type)
            {
                case EasingEquation.Linear:
                    return EasingEquation.Linear;
                case EasingEquation.BackEaseIn:
                    return EasingEquation.BackEaseOut;
                case EasingEquation.BackEaseInOut:
                    return EasingEquation.BackEaseOutIn;
                case EasingEquation.BackEaseOut:
                    return EasingEquation.BackEaseIn;
                case EasingEquation.BackEaseOutIn:
                    return EasingEquation.BackEaseInOut;
                case EasingEquation.BounceEaseIn:
                    return EasingEquation.BounceEaseOut;


                case EasingEquation.BounceEaseInOut:
                    return EasingEquation.BounceEaseOutIn;
                case EasingEquation.BounceEaseOut:
                    return EasingEquation.BounceEaseIn;
                case EasingEquation.BounceEaseOutIn:
                    return EasingEquation.BounceEaseInOut;
                case EasingEquation.CircEaseIn:
                    return EasingEquation.CircEaseOut;


                case EasingEquation.CircEaseInOut:
                    return EasingEquation.CircEaseOutIn;
                case EasingEquation.CircEaseOut:
                    return EasingEquation.CircEaseIn;


                case EasingEquation.CircEaseOutIn:
                    return EasingEquation.CircEaseInOut;
                case EasingEquation.CubicEaseIn:
                    return EasingEquation.CubicEaseOut;
                case EasingEquation.CubicEaseInOut:
                    return EasingEquation.CubicEaseOutIn;
                case EasingEquation.CubicEaseOut:
                    return EasingEquation.CubicEaseIn;
                case EasingEquation.CubicEaseOutIn:
                    return EasingEquation.CubicEaseInOut;
                case EasingEquation.ElasticEaseIn:
                    return EasingEquation.ElasticEaseOut;

                case EasingEquation.ElasticEaseInOut:
                    return EasingEquation.ElasticEaseOutIn;
                case EasingEquation.ElasticEaseOut:
                    return EasingEquation.ElasticEaseIn;
                case EasingEquation.ElasticEaseOutIn:
                    return EasingEquation.ElasticEaseInOut;
                case EasingEquation.ExpoEaseIn:
                    return EasingEquation.ExpoEaseOut;
                case EasingEquation.ExpoEaseInOut:
                    return EasingEquation.ExpoEaseOutIn;
                case EasingEquation.ExpoEaseOut:
                    return EasingEquation.ExpoEaseIn;
                case EasingEquation.ExpoEaseOutIn:
                    return EasingEquation.ExpoEaseInOut;
                case EasingEquation.QuadEaseIn:
                    return EasingEquation.QuadEaseOut;


                case EasingEquation.QuadEaseInOut:
                    return EasingEquation.QuadEaseOutIn;
                case EasingEquation.QuadEaseOut:
                    return EasingEquation.QuadEaseIn;
                case EasingEquation.QuadEaseOutIn:
                    return EasingEquation.QuadEaseInOut;
                case EasingEquation.QuartEaseIn:
                    return EasingEquation.QuartEaseOut;
                case EasingEquation.QuartEaseInOut:
                    return EasingEquation.QuartEaseOutIn;
                case EasingEquation.QuartEaseOut:
                    return EasingEquation.QuartEaseIn;
                case EasingEquation.QuartEaseOutIn:
                    return EasingEquation.QuartEaseInOut;
                case EasingEquation.QuintEaseIn:
                    return EasingEquation.QuintEaseOut;
                case EasingEquation.QuintEaseInOut:
                    return EasingEquation.QuintEaseOutIn;
                case EasingEquation.QuintEaseOut:
                    return EasingEquation.QuintEaseIn;
                case EasingEquation.QuintEaseOutIn:
                    return EasingEquation.QuintEaseInOut;

                case EasingEquation.SineEaseIn:
                    return EasingEquation.SineEaseOut;
                case EasingEquation.SineEaseInOut:
                    return EasingEquation.SineEaseOutIn;
                case EasingEquation.SineEaseOut:
                    return EasingEquation.SineEaseIn;
                case EasingEquation.SineEaseOutIn:
                    return EasingEquation.SineEaseInOut;

                default:
                    return EasingEquation.Linear;
            }
        }


        /* EASING FUNCTIONS */

        #region Linear

        /// <summary>
        /// Easing equation function for a simple linear tweening, with no easing.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float Linear(float t, float b, float c, float d)
        {
            return c * t / d + b;
        }

        #endregion

        #region Expo

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseOut(float t, float b, float c, float d)
        {
            return (t == d) ? b + c : c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseIn(float t, float b, float c, float d)
        {
            return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseInOut(float t, float b, float c, float d)
        {
            if (t == 0)
                return b;

            if (t == d)
                return b + c;

            if ((t /= d / 2) < 1)
                return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;

            return c / 2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return ExpoEaseOut(t * 2, b, c / 2, d);

            return ExpoEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Circular

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseOut(float t, float b, float c, float d)
        {
            return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseIn(float t, float b, float c, float d)
        {
            return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
                return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;

            return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return CircEaseOut(t * 2, b, c / 2, d);

            return CircEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Quad

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseOut(float t, float b, float c, float d)
        {
            return -c * (t /= d) * (t - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t + b;

            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return QuadEaseOut(t * 2, b, c / 2, d);

            return QuadEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Sine

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseOut(float t, float b, float c, float d)
        {
            return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseIn(float t, float b, float c, float d)
        {
            return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * (Mathf.Sin(Mathf.PI * t / 2)) + b;

            return -c / 2 * (Mathf.Cos(Mathf.PI * --t / 2) - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return SineEaseOut(t * 2, b, c / 2, d);

            return SineEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Cubic

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseOut(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t + b;

            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return CubicEaseOut(t * 2, b, c / 2, d);

            return CubicEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Quartic

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseOut(float t, float b, float c, float d)
        {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t * t + b;

            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return QuartEaseOut(t * 2, b, c / 2, d);

            return QuartEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Quintic

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseOut(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * t * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return QuintEaseOut(t * 2, b, c / 2, d);
            return QuintEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Elastic

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseOut(float t, float b, float c, float d)
        {
            if ((t /= d) == 1)
                return b + c;

            float p = d * 0.3f;
            float s = p / 4;

            return (c * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b);
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseIn(float t, float b, float c, float d)
        {
            if ((t /= d) == 1)
                return b + c;

            float p = d * 0.3f;
            float s = p / 4;

            return -(c * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseInOut(float t, float b, float c, float d)
        {
            if ((t /= d / 2f) == 2)
                return b + c;

            float p = d * (0.3f * 1.5f);
            float s = p / 4;

            if (t < 1)
                return -0.5f * (c * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
            return c * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * 0.5f + c + b;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return ElasticEaseOut(t * 2, b, c / 2, d);
            return ElasticEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Bounce

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseOut(float t, float b, float c, float d)
        {
            if ((t /= d) < (1 / 2.75f))
                return c * (7.5625f * t * t) + b;
            else if (t < (2 / 2.75f))
                return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f) + b;
            else if (t < (2.5f / 2.75f))
                return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f) + b;
            else
                return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseIn(float t, float b, float c, float d)
        {
            return c - BounceEaseOut(d - t, 0, c, d) + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseInOut(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return BounceEaseIn(t * 2, 0, c, d) * 0.5f + b;
            else
                return BounceEaseOut(t * 2 - d, 0, c, d) * 0.5f + c * 0.5f + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return BounceEaseOut(t * 2, b, c / 2, d);
            return BounceEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion

        #region Back

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseOut(float t, float b, float c, float d)
        {
            return c * ((t = t / d - 1) * t * ((1.70158f + 1) * t + 1.70158f) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseIn(float t, float b, float c, float d)
        {
            return c * (t /= d) * t * ((1.70158f + 1) * t - 1.70158f) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseInOut(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            if ((t /= d / 2) < 1)
                return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseOutIn(float t, float b, float c, float d)
        {
            if (t < d / 2)
                return BackEaseOut(t * 2, b, c / 2, d);
            return BackEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

        #endregion
    }
}