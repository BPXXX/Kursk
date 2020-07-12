using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Hotfix
{    
    public class TSingleton<T> where T : class
    {
        static T m_s_instance;

        public static T Instance
        {
            get
            {
                if (m_s_instance == null)
                {
                    Type type = typeof(T);
                    ConstructorInfo ctor;
                    ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                  null, new Type[0], new ParameterModifier[0]);
                    m_s_instance = (T)ctor.Invoke(new object[0]);
                }
                return m_s_instance;
            }
        }

        protected TSingleton()
        {
            Init();
        }

        protected virtual void Init()
        {

        }
    }
}