using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace Skyunion
{
    internal class EnvSoundMgr
    {
        internal class EnvSoundObj
        {
            public EnvSoundHandler EnvHandler;

            public AudioHandler AudioHandler;
        }

        public float LodDistance { get; set; }

        public bool IsActive { get; set; }

        public float MinUpdateCamDist { get; set; }

        public int MaxPlayCount { get; set; }

        private Vector3 m_lastCameraPos;

        private float m_lastUpdateTime;

        private int m_curPlayCount;

        private Dictionary<EnvSoundHandler, EnvSoundObj> m_allHandlers;

        private Dictionary<string, List<AudioHandler>> m_audioHanders;
        
        public EnvSoundMgr()
        {
            IsActive = false;
            MinUpdateCamDist = 5f;
            MaxPlayCount = 3;
            m_lastCameraPos = Vector3.zero;
            m_lastUpdateTime = 0f;
            m_curPlayCount = 0;
            m_allHandlers = new Dictionary<EnvSoundHandler, EnvSoundObj>();
            m_audioHanders = new Dictionary<string, List<AudioHandler>>();
        }

        public void Register(EnvSoundHandler comp)
        {
            EnvSoundObj EnvSoundObj = new EnvSoundObj();
            EnvSoundObj.EnvHandler = comp;
            m_allHandlers.Add(comp, EnvSoundObj);
            m_lastCameraPos = Vector3.zero;
        }

        public void UnRegister(EnvSoundHandler comp)
        {
            EnvSoundObj EnvSoundObj = null;
            if (m_allHandlers.TryGetValue(comp, out EnvSoundObj))
            {
                if (EnvSoundObj.AudioHandler != null)
                {
                    if (EnvSoundObj.AudioHandler.gameObject != null)
                    {
                        PutToCache(EnvSoundObj.AudioHandler);
                    }
                    EnvSoundObj.AudioHandler = null;
                    if (m_curPlayCount > 0)
                    {
                        m_curPlayCount--;
                    }
                }
                m_allHandlers.Remove(comp);
            }
        }

        public void Update()
        {
            if (!IsActive)
            {
                return;
            }
            if (Camera.main == null)
            {
                return;
            }
            Vector3 position = Camera.main.transform.position;
            float num = Vector3.Distance(position, m_lastCameraPos);
            if (num < MinUpdateCamDist)
            {
                return;
            }
            float lodDistance = LodDistance;
            m_lastCameraPos = position;
            Vector3 b = position;
            b.y = 0f;
            b.z += Mathf.Tan(0.0174532924f * Camera.main.transform.localEulerAngles.x) * position.y;
            m_lastUpdateTime = Time.realtimeSinceStartup;
            List<EnvSoundObj> list = new List<EnvSoundObj>();
            foreach (EnvSoundObj current in m_allHandlers.Values)
            {
                if (!(current.EnvHandler == null))
                {
                    Vector3 position2 = current.EnvHandler.transform.position;
                    float sqrMagnitude = (position2 - b).sqrMagnitude;
                    bool flag = false;
                    for (int i = 0; i < list.Count; i++)
                    {
                        EnvSoundObj EnvSoundObj = list[i];
                        Vector3 position3 = EnvSoundObj.EnvHandler.transform.position;
                        if (sqrMagnitude < (position3 - b).sqrMagnitude)
                        {
                            list.Insert(i, current);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        list.Add(current);
                    }
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                EnvSoundObj EnvSoundObj2 = list[j];
                EnvSoundHandler envHandler = EnvSoundObj2.EnvHandler;
                if (m_curPlayCount <= MaxPlayCount && envHandler.m_name != string.Empty && lodDistance <= envHandler.m_maxDxf)
                {
                    if (EnvSoundObj2.AudioHandler == null)
                    {
                        AudioHandler AudioHandler = GetFromCache(envHandler.m_name);
                        if (AudioHandler != null)
                        {
                            EnvSoundObj2.AudioHandler = AudioHandler;
                            AudioHandler.gameObject.transform.position = envHandler.transform.position;
                            m_curPlayCount++;
                        }
                        else
                        {
                            CoreUtils.audioService.PlayLoopAtPos(envHandler.m_name, envHandler.transform.position, (AudioHandler handler)=>
                            {
                                AudioHandler = handler;
                                if (AudioHandler != null)
                                {
                                    EnvSoundObj2.AudioHandler = AudioHandler;
                                    m_curPlayCount++;
                                }
                                else
                                {
                                    for (int k = list.Count - 1; k > j; k--)
                                    {
                                        EnvSoundObj EnvSoundObj3 = list[k];
                                        if (EnvSoundObj3.AudioHandler != null && EnvSoundObj3.EnvHandler.m_name == envHandler.m_name)
                                        {
                                            AudioHandler = EnvSoundObj3.AudioHandler;
                                            EnvSoundObj3.AudioHandler = null;
                                            EnvSoundObj2.AudioHandler = AudioHandler;
                                            AudioHandler.gameObject.transform.position = envHandler.transform.position;
                                            break;
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
                else if (EnvSoundObj2.AudioHandler != null)
                {
                    PutToCache(EnvSoundObj2.AudioHandler);
                    EnvSoundObj2.AudioHandler = null;
                    if (m_curPlayCount > 0)
                    {
                        m_curPlayCount--;
                    }
                }
            }
        }

        private AudioHandler GetFromCache(string name)
        {
            List<AudioHandler> list = null;
            if (m_audioHanders.TryGetValue(name, out list) && list.Count > 0)
            {
                AudioHandler AudioHandler = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                if (AudioHandler.gameObject != null)
                {
                    AudioHandler.gameObject.SetActive(true);
                    return AudioHandler;
                }
            }
            return null;
        }

        private void PutToCache(AudioHandler handler)
        {
            handler.gameObject.SetActive(false);
            List<AudioHandler> list = null;
            if (!m_audioHanders.TryGetValue(handler.AudioName, out list))
            {
                list = new List<AudioHandler>();
                m_audioHanders[handler.AudioName] = list;
            }
            list.Add(handler);
        }
    }
}