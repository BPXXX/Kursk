using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Skyunion
{
    class AudioService : Module, IAudioService
    {
        private bool _isFadingAudioListener = false;
                
        private float _fadingAudioListenerVolume = 0f;
                
        private float _fadingAudioListenerSpeed = 0f;
                
        private GameObject _HandlerObj;
                
        private AudioHandler _2dSimpleHandler;
                
        private List<AudioHandler> AllHandlers = new List<AudioHandler>();
                
        private Dictionary<string, Audio3DConfig> All3DConfigs = new Dictionary<string, Audio3DConfig>();
                
        private Dictionary<string, AudioConfig> AllConfigs = new Dictionary<string, AudioConfig>();
                
        private Dictionary<string, AudioGroup> AllGroups = new Dictionary<string, AudioGroup>();
                
        private Dictionary<string, AudioGroupConfig> AllGroupConfigs = new Dictionary<string, AudioGroupConfig>();
                
        private Dictionary<string, AudioPlayInfo> AllAudioPlayInfos = new Dictionary<string, AudioPlayInfo>();
                
        private Dictionary<string, AnimationCurve> AllAudioCurves = new Dictionary<string, AnimationCurve>();
                
        private Dictionary<string, int> AllChannels = new Dictionary<string, int>();
                
        private float _sfxVolume = 1f;
                
        private float _musicVolume = 1f;
                
        private bool _isEnabled3DSound = false;
                
        private string _curBgmName;
                
        private AudioHandler _curBgmHandler;

        private GameObject HandlerGameObject
        {
            get
            {
                if (_HandlerObj == null)
                {
                    _HandlerObj = new GameObject("2DAudioHandler");
                    UnityEngine.Object.DontDestroyOnLoad(_HandlerObj);
                }
                return _HandlerObj;
            }
        }

        private AudioHandler SimpleHandler
        {
            get
            {
                if (_2dSimpleHandler == null)
                {
                    _2dSimpleHandler = AudioHandler.GetHandler(HandlerGameObject);
                    _2dSimpleHandler.audioSource.volume = _sfxVolume;
                }
                return _2dSimpleHandler;
            }
        }

        public float GetSfxVolume()
        {
            return _sfxVolume;
        }

        public void SetSfxVolume(float v)
        {
            if (_sfxVolume != v)
            {
                _sfxVolume = v;
                OnVolumeChanged();
            }
        }

        public float GetMusicVolume()
        {
            return _musicVolume;
        }

        public void SetMusicVolume(float v)
        {
            if (_musicVolume != v)
            {
                _musicVolume = v;
                OnVolumeChanged();
            }
        }

        public void Enable3DSound(bool enable)
        {
            _isEnabled3DSound = enable;
        }

        public bool IsEnabled3DSound()
        {
            return _isEnabled3DSound;
        }

        public string GetCurBgmName()
        {
            return _curBgmName;
        }

        public void PlayBgm(string name)
        {
            if (_curBgmName != name)
            {
                if (_curBgmHandler != null && _curBgmHandler.IsPlaying)
                {
                    FadeToStop(_curBgmHandler, 0.8f, false);
                }
                _curBgmName = name;
                string empty = string.Empty;
                AudioConfig playConfig = GetPlayConfig(name, out empty);
                AudioHandler handler = AudioHandler.GetHandler(HandlerGameObject);
                _curBgmHandler = handler;
                handler.IsSfx = false;
                SetupAndPlay(handler, name, empty, playConfig, true, false, (bool bRet) =>
                {
                    if (bRet)
                    {
                        AddHandler(handler);
                    }
                });
            }
        }

        public void GetSfxTime(string name, Action<float> action)
        {
            string empty = string.Empty;
            AudioConfig playConfig = GetPlayConfig(name, out empty);
            if (empty != string.Empty)
            {
                LoadAudioRes(empty, (AudioClip audioRes) =>
                {
                    if (audioRes != null)
                    {
                        action?.Invoke(audioRes.length);
                    }
                });
            }
            action?.Invoke(0.0f);
        }

        public void PlayOneShot(string name, Action<AudioHandler> action)
        {
            string empty = string.Empty;
            AudioConfig playConfig = GetPlayConfig(name, out empty);
            if (empty != string.Empty)
            {
                if (playConfig == null)
                {
                    LoadAudioRes(empty, (AudioClip audioRes) =>
                    {
                        if (audioRes != null)
                        {
                            SimpleHandler.audioSource.PlayOneShot(audioRes);
                        }
                    });
                }
                else
                {
                    AudioHandler handler = AudioHandler.GetHandler(HandlerGameObject);
                    SetupAndPlay(handler, name, empty, playConfig, false, false, (bool bRet) =>
                    {
                        if (bRet)
                        {
                            AddHandler(handler);
                            action?.Invoke(handler);
                        }
                    });
                }
            }
            action?.Invoke(null);
        }

        public void PlayOneShot3D(string name, GameObject obj, Action<AudioHandler> action)
        {
            string empty = string.Empty;
            AudioConfig playConfig = GetPlayConfig(name, out empty);
            if (empty != string.Empty)
            {
                AudioHandler handler = AudioHandler.GetHandler(obj);
                SetupAndPlay(handler, name, empty, playConfig, false, true, (bool bRet) =>
                {
                    if (bRet)
                    {
                        AddHandler(handler);
                        action?.Invoke(handler);
                    }
                });
            }
            action?.Invoke(null);
        }

        public void PlayOneShotAtPos(string name, Vector3 pos, Action<AudioHandler> action)
        {
            string empty = string.Empty;
            AudioConfig playConfig = GetPlayConfig(name, out empty);
            if (empty != string.Empty)
            {
                AudioHandler handler = AudioHandler.GetHandler(null);
                handler.pos = pos;
                SetupAndPlay(handler, name, empty, playConfig, false, true, (bool bRet) =>
                {
                    if (bRet)
                    {
                        AddHandler(handler);
                        action?.Invoke(handler);
                    }
                });
            }
            action?.Invoke(null);
        }

        public void PlayLoop(string name, Action<AudioHandler> action)
        {
            string empty = string.Empty;
            AudioConfig playConfig = GetPlayConfig(name, out empty);
            if (empty != string.Empty)
            {
                AudioHandler handler = AudioHandler.GetHandler(HandlerGameObject);
                SetupAndPlay(handler, name, empty, playConfig, true, false, (bool bRet) =>
                {
                    if (bRet)
                    {
                        AddHandler(handler);
                        action?.Invoke(handler);
                    }
                });
            }
            action?.Invoke(null);
        }

        public void PlayLoop2D(string name, GameObject obj, Action<AudioHandler> action)
        {
            string empty = string.Empty;
            AudioConfig playConfig = GetPlayConfig(name, out empty);
            if (empty != string.Empty)
            {
                AudioHandler handler = AudioHandler.GetHandler(obj);
                SetupAndPlay(handler, name, empty, playConfig, true, false, (bool bRet) =>
                {
                    if (bRet)
                    {
                        AddHandler(handler);
                        action?.Invoke(handler);
                    }
                });
            }
            action?.Invoke(null);
        }

        public void PlayLoop3D(string name, GameObject obj, Action<AudioHandler> action)
        {
            string empty = string.Empty;
            AudioConfig playConfig = GetPlayConfig(name, out empty);
            if (empty != string.Empty)
            {
                AudioHandler handler = AudioHandler.GetHandler(obj);
                SetupAndPlay(handler, name, empty, playConfig, true, true, (bool bRet) =>
                {
                    if (bRet)
                    {
                        AddHandler(handler);
                        action?.Invoke(handler);
                    }
                });
            }
            action?.Invoke(null);
        }

        public void PlayLoopAtPos(string name, Vector3 pos, Action<AudioHandler> action)
        {
            string empty = string.Empty;
            AudioConfig playConfig = GetPlayConfig(name, out empty);
            if (empty != string.Empty)
            {
                AudioHandler handler = AudioHandler.GetHandler(null);
                handler.pos = pos;
                SetupAndPlay(handler, name, empty, playConfig, true, true, (bool bRet) =>
                {
                    if (bRet)
                    {
                        AddHandler(handler);
                        action?.Invoke(handler);
                    }
                });
            }
            action?.Invoke(null);
        }

        public void SetHandlerVolume(AudioHandler handler, float volume)
        {
            handler.Volume = volume;
            handler.audioSource.volume = volume * ((!handler.IsSfx) ? _musicVolume : _sfxVolume);
            handler.IsFading = false;
        }

        public void FadeHandlerVolume(AudioHandler handler, float toVolume, float speed, bool destroyWhenDone)
        {
            handler.FadeVolume(toVolume, speed, destroyWhenDone);
        }

        public AudioHandler FadeToStop(AudioHandler handler, float speed, bool isDetach)
        {
            if (handler == _curBgmHandler)
            {
                _curBgmName = string.Empty;
            }
            if (isDetach && !handler.NeedDestroyGameObject)
            {
                AudioHandler handler2 = AudioHandler.GetHandler(new GameObject
                {
                    name = handler.audioSource.clip.name,
                    transform =
                {
                    position = handler.gameObject.transform.position
                }
                });
                handler2.Config = handler.Config;
                handler2.Group = handler.Group;
                handler2.GroupConfig = handler.GroupConfig;
                handler2.NeedDestroyGameObject = true;
                handler2.IsPlaying = true;
                AudioSource audioSource = handler2.audioSource;
                AudioSource audioSource2 = handler.audioSource;
                audioSource.clip = audioSource2.clip;
                audioSource.timeSamples = audioSource2.timeSamples;
                audioSource.loop = audioSource2.loop;
                audioSource.volume = audioSource2.volume;
                audioSource.rolloffMode = audioSource2.rolloffMode;
                audioSource.dopplerLevel = audioSource2.dopplerLevel;
                audioSource.minDistance = audioSource2.minDistance;
                audioSource.maxDistance = audioSource2.maxDistance;
                audioSource.spatialBlend = audioSource2.spatialBlend;
                if (audioSource.rolloffMode == AudioRolloffMode.Custom)
                {
                    audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioSource2.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
                }
                audioSource2.Stop();
                handler.IsPlaying = false;
                AllHandlers.Remove(handler);
                audioSource.Play();
                AddHandler(handler2);
                handler2.FadeVolume(0f, speed, true);
                return handler2;
            }
            handler.FadeVolume(0f, speed, true);
            return handler;
        }

        public void StopByHandler(AudioHandler handler)
        {
            if (handler == _curBgmHandler)
            {
                _curBgmName = string.Empty;
            }
            if (AllHandlers.Contains(handler))
            {
                DestroyHandler(handler);
            }
        }

        public void StopByName(string name)
        {
            AudioHandler handlerByName = GetHandlerByName(name);
            if (handlerByName != null)
            {
                StopByHandler(handlerByName);
            }
        }

        public void StopAllByName(string name)
        {
            AudioHandler[] handlersByName = GetHandlersByName(name);
            for (int i = 0; i < handlersByName.Length; i++)
            {
                AudioHandler handler = handlersByName[i];
                StopByHandler(handler);
            }
        }

        public bool IsHandlerNull(AudioHandler handler)
        {
            return handler == null || handler.IsDestroyed;
        }

        public AudioHandler GetHandlerByName(string name)
        {
            for (int i = 0; i < AllHandlers.Count; i++)
            {
                AudioHandler AudioHandler = AllHandlers[i];
                if (AudioHandler.GetAudioName() == name)
                {
                    return AudioHandler;
                }
            }
            return null;
        }

        public AudioHandler[] GetHandlersByName(string name)
        {
            List<AudioHandler> list = new List<AudioHandler>();
            for (int i = 0; i < AllHandlers.Count; i++)
            {
                AudioHandler AudioHandler = AllHandlers[i];
                if (AudioHandler.GetAudioName() == name)
                {
                    list.Add(AudioHandler);
                }
            }
            return list.ToArray();
        }

        public void OnGamePause(bool isPause)
        {
            if (_isFadingAudioListener)
            {
                return;
            }
            UnityEngine.AudioListener.volume = ((!isPause) ? 1f : 0f);
        }

        public void FadeAudioListener(float toVolume, float speed)
        {
            _isFadingAudioListener = true;
            _fadingAudioListenerVolume = toVolume;
            _fadingAudioListenerSpeed = speed;
        }

        public int GetChannel(string key)
        {
            int result = 0;
            AllChannels.TryGetValue(key, out result);
            return result;
        }

        public void SetChannel(string key, int channel)
        {
            int channel2 = GetChannel(key);
            if (channel2 == channel)
            {
                return;
            }
            AllChannels[key] = channel;
            if (key == "bgm")
            {
                SetBgmChannel(key, channel, channel2);
                return;
            }
            for (int i = 0; i < AllHandlers.Count; i++)
            {
                AudioHandler AudioHandler = AllHandlers[i];
                if (!IsHandlerNull(AudioHandler))
                {
                    if (AudioHandler.Config != null && !(AudioHandler.Config.ChannelKey != key))
                    {
                        if (AudioHandler.audioSource.isPlaying)
                        {
                            AudioClip clip = AudioHandler.audioSource.clip;
                            if (!(clip == null))
                            {
                                if (AudioHandler.audioSource.time <= clip.length - 2f)
                                {
                                    List<string> list = AudioHandler.Config.ChannelAudioNames[channel2];
                                    List<string> list2 = AudioHandler.Config.ChannelAudioNames[channel];
                                    for (int j = 0; j < list.Count; j++)
                                    {
                                        if (list[j] == clip.name)
                                        {
                                            LoadAudioRes(list2[j], (AudioClip audioClip)=>
                                            {
                                                if (!(audioClip == null))
                                                {
                                                    float time = AudioHandler.audioSource.time;
                                                    if (time > audioClip.length - 2f)
                                                    {
                                                        DestroyAudioRes(audioClip);
                                                    }
                                                    else
                                                    {
                                                        AudioHandler.audioSource.clip = audioClip;
                                                        AudioHandler.audioSource.time = time;
                                                        AudioHandler.audioSource.Play();
                                                    }
                                                }
                                            });
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetBgmChannel(string key, int channel, int oldChannel)
        {
            if (IsHandlerNull(_curBgmHandler))
            {
                return;
            }
            if (_curBgmHandler.Config == null || _curBgmHandler.Config.ChannelKey != key)
            {
                return;
            }
            if (!_curBgmHandler.audioSource.isPlaying)
            {
                return;
            }
            AudioClip clip = _curBgmHandler.audioSource.clip;
            if (clip == null)
            {
                return;
            }
            if (_curBgmHandler.audioSource.time > clip.length - 2f)
            {
                return;
            }
            string text = string.Empty;
            List<string> list = _curBgmHandler.Config.ChannelAudioNames[oldChannel];
            List<string> list2 = _curBgmHandler.Config.ChannelAudioNames[channel];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == clip.name)
                {
                    text = list2[i];
                    break;
                }
            }
            if (text == string.Empty)
            {
                return;
            }

            LoadAudioRes(text, (AudioClip audioRes)=>
            {
                float time = _curBgmHandler.audioSource.time;
                if (time > audioRes.length - 2f)
                {
                    DestroyAudioRes(audioRes);
                    return;
                }
                AudioConfig config = _curBgmHandler.Config;
                FadeToStop(_curBgmHandler, 1f, false);
                AudioHandler handler = AudioHandler.GetHandler(HandlerGameObject);
                _curBgmHandler = handler;
                handler.IsSfx = false;

                SetupAndPlay(handler, _curBgmName, text, config, true, false, (bool bRet) =>
                {
                    if (bRet)
                    {
                        AddHandler(handler);
                        handler.audioSource.time = time;
                        handler.audioSource.Play();
                        float handlerVolume = GetHandlerVolume(handler, false);
                        handler.Volume = 0f;
                        handler.audioSource.volume = 0f;
                        FadeHandlerVolume(handler, handlerVolume, 1f, false);
                    }
                });
            });
        }

        private bool m_bLoadedConfig = false;
        private bool m_bLoadedCurves = false;

        public override void Init()
        {
            CoreUtils.dataService.WaitInitAsync(() =>
            {
                m_envSoundMgr = new EnvSoundMgr();
                LoadConfig();
            });
        }
        private EnvSoundMgr m_envSoundMgr;
        public override void AfterInit()
        {
            m_envSoundMgr.IsActive = true;
        }

        public void LoadConfig()
        {
            AllConfigs.Clear();
            AllGroupConfigs.Clear();
            AllGroups.Clear();
            All3DConfigs.Clear();
            AllAudioCurves.Clear();

            CoreUtils.assetService.LoadAssetAsync<GameObject>("CustomAudioCurves", LoadAudioCurves);
            //CoreUtils.assetService.LoadFileAsync("AudioConfig.xml", LoadAudioConfig);
            LoadAudioConfigByExcel();

        }

        private void LoadAudioConfigByExcel()
        {
            var config3ds = CoreUtils.dataService.QueryRecords<AudioConfig3dDefine>();
            var groups = CoreUtils.dataService.QueryRecords<AudioGroupInfoDefine>();
            var audios = CoreUtils.dataService.QueryRecords<AudioInfoDefine>();

            for (int i = 0; i < config3ds.Count; i++)
            {
                var data = config3ds[i];
                Audio3DConfig Audio3DConfig = new Audio3DConfig();
                Audio3DConfig.DopplerLevel = data.dopplerLevel;
                Audio3DConfig.MinDistance = data.minDistance;
                Audio3DConfig.MaxDistance = data.maxDistance;
                Audio3DConfig.SpatialBlend = data.spatialBlend;
                Audio3DConfig.Spread = data.spread;
                Audio3DConfig.CurveKey = data.curve;

                All3DConfigs[data.name] = Audio3DConfig;
            }

            for (int i = 0; i < groups.Count; i++)
            {
                var data = groups[i];
                AudioGroupConfig AudioGroupConfig = new AudioGroupConfig();
                AudioGroupConfig.BaseVolume = data.baseVolume;
                AudioGroupConfig.MaxPlayCount = data.maxCount;
                AudioGroupConfig.MaxCountAction = (EAudioMaxCountAction)data.maxCountAction;
                AudioGroupConfig.IsSfx = data.bgm == 0;
                AllGroupConfigs.Add(data.name, AudioGroupConfig);
                AudioGroup AudioGroup = new AudioGroup();
                AudioGroup.GroupId = data.name;
                AllGroups.Add(data.name, AudioGroup);
            }
            for (int i = 0; i < groups.Count; i++)
            {
                var data = groups[i];
                AudioGroupConfig AudioGroupConfig = AllGroupConfigs[data.name];
                if (data.modifyGroup != null)
                {
                    for (int j = 0; j < data.modifyGroup.Count; j++)
                    {
                        if (AudioGroupConfig.AffectedVolumeModifyGroups == null)
                        {
                            AudioGroupConfig.AffectedVolumeModifyGroups = new Dictionary<string, float>();
                        }
                        AudioGroupConfig.AffectedVolumeModifyGroups.Add(groups[data.modifyGroup[j] - 1].name, data.modifyVolume[j]);
                    }
                }
            }


            for (int i = 0; i < audios.Count; i++)
            {
                var data = audios[i];
                AudioConfig AudioConfig = new AudioConfig();
                AudioConfig.AudioName = data.name;
                AudioConfig.GroupId = groups[data.group-1].name;
                AudioConfig.MaxPlayCount = data.maxCount;
                AudioConfig.MaxCountAction= (EAudioMaxCountAction)data.maxCountAction;
                AudioConfig.PlayMode = (AudioConfig.EPlayMode)data.mode;
                AudioConfig.AudioNames = data.audios;
                if (data.interval != null && data.interval.Count > 0)
                {
                    AudioConfig.Intervals = data.interval.ToArray();
                }
                AudioConfig.FadeInTime = data.fadeTime;
                AllConfigs.Add(data.name, AudioConfig);
            }

            foreach (string current in AllGroupConfigs.Keys)
            {
                AudioGroupConfig AudioGroupConfig2 = AllGroupConfigs[current];
                if (AudioGroupConfig2.AffectedVolumeModifyGroups != null)
                {
                    foreach (string current2 in AudioGroupConfig2.AffectedVolumeModifyGroups.Keys)
                    {
                        AudioGroupConfig AudioGroupConfig3 = AllGroupConfigs[current2];
                        if (AudioGroupConfig3.VolumeModifyGroups == null)
                        {
                            AudioGroupConfig3.VolumeModifyGroups = new Dictionary<string, float>();
                        }
                        AudioGroupConfig3.VolumeModifyGroups.Add(current, AudioGroupConfig2.AffectedVolumeModifyGroups[current2]);
                    }
                }
            }
            m_bLoadedConfig = true;
            OnLoadConfig();
        }

        private void OnLoadConfig()
        {
            if(m_bLoadedCurves && m_bLoadedConfig)
            {
                OnInitialized();
            }
        }
        private void LoadAudioCurves(IAsset asset)
        {
            if(asset == null)
            {
                CoreUtils.logService.Fatal("Can not load CustomAudioCurves prefab!", Color.red);
                return;
            }
            var gameObject = asset.asset() as GameObject;
            AllAudioCurves = gameObject.GetComponent<AudioAnimCurve>().m_curves;
            m_bLoadedCurves = true;
            OnLoadConfig();
        }
        private void LoadAudioConfig(byte[] fileContent)
        {
            if (fileContent == null)
            {
                CoreUtils.logService.Fatal("Can not load AudioConfig.xml in StreamingAssets !", Color.red);
                return;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(Encoding.UTF8.GetString(fileContent));
            XmlNode xmlNode = null;
            XmlNode xmlNode2 = null;
            XmlNode xmlNode3 = null;
            for (int i = 0; i < xmlDocument.ChildNodes.Count; i++)
            {
                if (xmlDocument.ChildNodes[i].Name == "allconfigs")
                {
                    xmlNode = xmlDocument.ChildNodes[i];
                }
            }
            for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
            {
                if (xmlNode.ChildNodes[j].Name == "groups")
                {
                    xmlNode2 = xmlNode.ChildNodes[j];
                }
                else if (xmlNode.ChildNodes[j].Name == "config3d")
                {
                    xmlNode3 = xmlNode.ChildNodes[j];
                }
            }
            for (int k = 0; k < xmlNode3.ChildNodes.Count; k++)
            {
                XmlNode xmlNode4 = xmlNode3.ChildNodes[k];
                if (!(xmlNode4.Name != "config"))
                {
                    XmlAttribute xmlAttribute = xmlNode4.Attributes["name"];
                    XmlAttribute xmlAttribute2 = xmlNode4.Attributes["dopplerLevel"];
                    XmlAttribute xmlAttribute3 = xmlNode4.Attributes["minDistance"];
                    XmlAttribute xmlAttribute4 = xmlNode4.Attributes["maxDistance"];
                    XmlAttribute xmlAttribute5 = xmlNode4.Attributes["spatialBlend"];
                    XmlAttribute xmlAttribute6 = xmlNode4.Attributes["spread"];
                    XmlAttribute xmlAttribute7 = xmlNode4.Attributes["curve"];
                    string value = xmlAttribute.Value;
                    Audio3DConfig Audio3DConfig = new Audio3DConfig();
                    Audio3DConfig.DopplerLevel = (float)double.Parse(xmlAttribute2.Value);
                    Audio3DConfig.MinDistance = (float)double.Parse(xmlAttribute3.Value);
                    Audio3DConfig.MaxDistance = (float)double.Parse(xmlAttribute4.Value);
                    Audio3DConfig.SpatialBlend = (float)double.Parse(xmlAttribute5.Value);
                    Audio3DConfig.Spread = (float)double.Parse(xmlAttribute6.Value);
                    if (xmlAttribute7 != null)
                    {
                        Audio3DConfig.CurveKey = xmlAttribute7.Value;
                    }
                    All3DConfigs[value] = Audio3DConfig;
                }
            }
            for (int l = 0; l < xmlNode2.ChildNodes.Count; l++)
            {
                XmlNode xmlNode5 = xmlNode2.ChildNodes[l];
                if (!(xmlNode5.Name != "group"))
                {
                    XmlAttribute xmlAttribute8 = xmlNode5.Attributes["name"];
                    XmlAttribute xmlAttribute9 = xmlNode5.Attributes["baseVolume"];
                    XmlAttribute xmlAttribute10 = xmlNode5.Attributes["maxCount"];
                    XmlAttribute xmlAttribute11 = xmlNode5.Attributes["maxCountAction"];
                    XmlAttribute xmlAttribute12 = xmlNode5.Attributes["bgm"];
                    XmlAttribute xmlAttribute13 = xmlNode5.Attributes["sfx"];
                    string value2 = xmlAttribute8.Value;
                    AudioGroupConfig AudioGroupConfig = new AudioGroupConfig();
                    AudioGroupConfig.GroupId = value2;
                    if (xmlAttribute9 != null)
                    {
                        AudioGroupConfig.BaseVolume = (float)double.Parse(xmlAttribute9.Value);
                    }
                    if (xmlAttribute10 != null)
                    {
                        AudioGroupConfig.MaxPlayCount = int.Parse(xmlAttribute10.Value);
                    }
                    if (xmlAttribute11 != null && xmlAttribute11.Value == "stop")
                    {
                        AudioGroupConfig.MaxCountAction = EAudioMaxCountAction.StopPrevious;
                    }
                    if (xmlAttribute12 != null)
                    {
                        AudioGroupConfig.IsSfx = (xmlAttribute12.Value != "true");
                    }
                    if (xmlAttribute13 != null)
                    {
                        AudioGroupConfig.IsSfx = (xmlAttribute13.Value != "false");
                    }
                    AllGroupConfigs.Add(value2, AudioGroupConfig);
                    AudioGroup AudioGroup = new AudioGroup();
                    AudioGroup.GroupId = value2;
                    AllGroups.Add(value2, AudioGroup);
                    for (int m = 0; m < xmlNode5.ChildNodes.Count; m++)
                    {
                        XmlNode xmlNode6 = xmlNode5.ChildNodes[m];
                        if (xmlNode6.Name == "modify")
                        {
                            string value3 = xmlNode6.Attributes["group"].Value;
                            float value4 = (float)double.Parse(xmlNode6.Attributes["volume"].Value);
                            if (AudioGroupConfig.AffectedVolumeModifyGroups == null)
                            {
                                AudioGroupConfig.AffectedVolumeModifyGroups = new Dictionary<string, float>();
                            }
                            AudioGroupConfig.AffectedVolumeModifyGroups.Add(value3, value4);
                        }
                        else if (xmlNode6.Name == "audio")
                        {
                            string value5 = xmlNode6.Attributes["name"].Value;
                            XmlAttribute xmlAttribute14 = xmlNode6.Attributes["mode"];
                            XmlAttribute xmlAttribute15 = xmlNode6.Attributes["audios"];
                            XmlAttribute xmlAttribute16 = xmlNode6.Attributes["maxCount"];
                            XmlAttribute xmlAttribute17 = xmlNode6.Attributes["maxCountAction"];
                            XmlAttribute xmlAttribute18 = xmlNode6.Attributes["interval"];
                            XmlAttribute xmlAttribute19 = xmlNode6.Attributes["fadeInTime"];
                            XmlAttribute xmlAttribute20 = xmlNode6.Attributes["config3d"];
                            XmlAttribute xmlAttribute21 = xmlNode6.Attributes["channelKey"];
                            AudioConfig AudioConfig = new AudioConfig();
                            AudioConfig.AudioName = value5;
                            AudioConfig.GroupId = value2;
                            if (xmlAttribute16 != null)
                            {
                                AudioConfig.MaxPlayCount = int.Parse(xmlAttribute16.Value);
                            }
                            if (xmlAttribute17 != null && xmlAttribute17.Value == "stop")
                            {
                                AudioConfig.MaxCountAction = EAudioMaxCountAction.StopPrevious;
                            }
                            if (xmlAttribute14 != null)
                            {
                                if (xmlAttribute14.Value == "list")
                                {
                                    AudioConfig.PlayMode = AudioConfig.EPlayMode.List;
                                }
                                else if (xmlAttribute14.Value == "random")
                                {
                                    AudioConfig.PlayMode = AudioConfig.EPlayMode.Random;
                                }
                            }
                            if (xmlAttribute15 != null)
                            {
                                AudioConfig.AudioNames = new List<string>();
                                string[] array = xmlAttribute15.Value.Split(new char[]
                                {
                                ','
                                });
                                for (int n = 0; n < array.Length; n++)
                                {
                                    string text = array[n];
                                    AudioConfig.AudioNames.Add(text.Trim());
                                }
                            }
                            if (xmlAttribute18 != null)
                            {
                                string[] array2 = xmlAttribute18.Value.Split(new char[]
                                {
                                ','
                                });
                                AudioConfig.Intervals = new float[array2.Length];
                                for (int num = 0; num < array2.Length; num++)
                                {
                                    AudioConfig.Intervals[num] = (float)double.Parse(array2[num]);
                                }
                            }
                            if (xmlAttribute19 != null)
                            {
                                AudioConfig.FadeInTime = (float)double.Parse(xmlAttribute19.Value);
                            }
                            if (xmlAttribute20 != null)
                            {
                                AudioConfig.Config3DKey = xmlAttribute20.Value;
                            }
                            if (xmlAttribute21 != null)
                            {
                                AudioConfig.ChannelKey = xmlAttribute21.Value;
                            }
                            for (int num2 = 0; num2 < xmlNode6.ChildNodes.Count; num2++)
                            {
                                XmlNode xmlNode7 = xmlNode6.ChildNodes[num2];
                                if (xmlNode7.Name == "channel")
                                {
                                    if (AudioConfig.ChannelAudioNames == null)
                                    {
                                        AudioConfig.ChannelAudioNames = new List<List<string>>();
                                    }
                                    XmlAttribute xmlAttribute22 = xmlNode7.Attributes["audios"];
                                    if (xmlAttribute22 != null)
                                    {
                                        List<string> list = new List<string>();
                                        string[] array3 = xmlAttribute22.Value.Split(new char[]
                                        {
                                        ','
                                        });
                                        for (int num3 = 0; num3 < array3.Length; num3++)
                                        {
                                            string text2 = array3[num3];
                                            list.Add(text2.Trim());
                                        }
                                        AudioConfig.ChannelAudioNames.Add(list);
                                    }
                                }
                            }
                            if (AudioConfig.ChannelKey == string.Empty)
                            {
                                if (AudioConfig.PlayMode == AudioConfig.EPlayMode.Random && (AudioConfig.AudioNames == null || AudioConfig.AudioNames.Count == 0))
                                {
                                    Debug.LogError("Audio: " + value5 + " has no random list");
                                }
                                else if (AudioConfig.PlayMode == AudioConfig.EPlayMode.List && (AudioConfig.AudioNames == null || AudioConfig.AudioNames.Count == 0))
                                {
                                    Debug.LogError("Audio: " + value5 + " has no list");
                                }
                            }
                            if (AudioConfig.ChannelKey != string.Empty && AudioConfig.ChannelAudioNames == null)
                            {
                                Debug.LogError("Audio: " + value5 + " has no channel audios");
                            }
                            AllConfigs.Add(value5, AudioConfig);
                        }
                    }
                }
            }
            foreach (string current in AllGroupConfigs.Keys)
            {
                AudioGroupConfig AudioGroupConfig2 = AllGroupConfigs[current];
                if (AudioGroupConfig2.AffectedVolumeModifyGroups != null)
                {
                    foreach (string current2 in AudioGroupConfig2.AffectedVolumeModifyGroups.Keys)
                    {
                        AudioGroupConfig AudioGroupConfig3 = AllGroupConfigs[current2];
                        if (AudioGroupConfig3.VolumeModifyGroups == null)
                        {
                            AudioGroupConfig3.VolumeModifyGroups = new Dictionary<string, float>();
                        }
                        AudioGroupConfig3.VolumeModifyGroups.Add(current, AudioGroupConfig2.AffectedVolumeModifyGroups[current2]);
                    }
                }
            }
            m_bLoadedConfig = true;
            OnLoadConfig();
        }

        public void Clear()
        {
            AudioHandler[] array = AllHandlers.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                AudioHandler handler = array[i];
                StopByHandler(handler);
            }
            AllGroups.Clear();
            AllAudioPlayInfos.Clear();
            _curBgmName = string.Empty;
            _curBgmHandler = null;
        }

        public void AddHandler(AudioHandler handler)
        {
            AllHandlers.Add(handler);
            if (handler.Group != null)
            {
                handler.Group.PlayingHandler.Add(handler);
            }
        }

        private void SetupAndPlay(AudioHandler handler, string audioName, string clipName, AudioConfig config, bool isLoop, bool is3D, Action<bool> action)
        {
            AudioGroup group = GetGroup(config);
            handler.AudioName = audioName;
            AudioPlayInfo playInfo = GetPlayInfo(audioName, true);
            handler.Group = group;
            if (config != null)
            {
                AllGroupConfigs.TryGetValue(config.GroupId, out handler.GroupConfig);
            }
            handler.Config = config;
            if (handler.Config != null && handler.Config.MaxPlayCount != -1 && playInfo.PlayCount >= handler.Config.MaxPlayCount)
            {
                if (handler.Config.MaxCountAction == EAudioMaxCountAction.DontPlay)
                {
                    action?.Invoke(false);
                }
                for (int i = 0; i < AllHandlers.Count; i++)
                {
                    AudioHandler AudioHandler = AllHandlers[i];
                    if (AudioHandler.AudioName == audioName)
                    {
                        StopByHandler(AudioHandler);
                        break;
                    }
                }
            }
            if (handler.GroupConfig != null && handler.Group != null && handler.GroupConfig.MaxPlayCount != -1 && handler.Group.PlayCount >= handler.GroupConfig.MaxPlayCount)
            {
                if (handler.GroupConfig.MaxCountAction == EAudioMaxCountAction.DontPlay)
                {
                    action?.Invoke(false);
                }
                if (handler.GroupConfig.MaxCountAction == EAudioMaxCountAction.StopPrevious && handler.Group.PlayingHandler.Count > 0)
                {
                    StopByHandler(handler.Group.PlayingHandler[0]);
                }
            }
            LoadAudioRes(clipName, (AudioClip audioRes)=>
            {
                if (audioRes == null)
                {
                    action?.Invoke(false);
                }
                if (is3D && handler.gameObject == null)
                {
                    handler.gameObject = new GameObject();
                    handler.gameObject.name = audioName;
                    handler.gameObject.transform.position = handler.pos;
                    handler.NeedDestroyGameObject = true;
                }
                AudioSource audioSource = handler.audioSource;
                if (is3D && _isEnabled3DSound)
                {
                    string key = "default";
                    if (handler.Config != null)
                    {
                        key = handler.Config.Config3DKey;
                    }
                    Audio3DConfig config3D = null;
                    if (All3DConfigs.TryGetValue(key, out config3D))
                    {
                        Setup3DAudioSource(audioSource, config3D);
                    }
                }
                handler.IsLoop = isLoop;
                handler.Volume = GetHandlerVolume(handler, handler.IsSfx);
                audioSource.volume = handler.Volume * ((!handler.IsSfx) ? _musicVolume : _sfxVolume);
                audioSource.clip = audioRes;
                bool flag = isLoop;
                if (flag && handler.Config != null)
                {
                    AudioConfig.EPlayMode playMode = handler.Config.PlayMode;
                    if (playMode == AudioConfig.EPlayMode.List || playMode == AudioConfig.EPlayMode.Random)
                    {
                        flag = false;
                    }
                }
                audioSource.loop = flag;
                audioSource.Play();
                if (handler.Group != null)
                {
                    if (handler.Group.PlayCount == 0)
                    {
                        handler.Group.IsHasPlayingStateDirty = true;
                    }
                    handler.Group.PlayCount++;
                }
                playInfo.PlayCount++;
                handler.IsPlaying = true;
                if (handler.Config != null && handler.Config.FadeInTime > 0f)
                {
                    float volume = handler.Volume;
                    handler.Volume = 0f;
                    handler.audioSource.volume = 0f;
                    FadeHandlerVolume(handler, volume, 1f / handler.Config.FadeInTime, false);
                }
                action?.Invoke(true);
            });
        }

        public void OnHandlerDestroy(AudioHandler handler)
        {
            if (AllHandlers.Contains(handler))
            {
                if (handler.IsPlaying)
                {
                    if (handler.Group != null)
                    {
                        if (handler.gameObject != null)
                        {
                        }
                        handler.Group.PlayCount--;
                        if (handler.Group.PlayCount == 0)
                        {
                            handler.Group.IsHasPlayingStateDirty = true;
                        }
                    }
                    AudioPlayInfo playInfo = GetPlayInfo(handler.AudioName, false);
                    if (playInfo != null)
                    {
                        playInfo.PlayCount--;
                    }
                }
                AllHandlers.Remove(handler);
                if (handler.Group != null)
                {
                    if (handler.gameObject != null)
                    {
                    }
                    handler.Group.PlayingHandler.Remove(handler);
                }
            }
        }

        private void OnVolumeChanged()
        {
            SimpleHandler.audioSource.volume = SimpleHandler.Volume * _sfxVolume;
            AudioHandler[] array = AllHandlers.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                AudioHandler AudioHandler = array[i];
                if (!(AudioHandler.gameObject == null))
                {
                    AudioSource audioSource = AudioHandler.audioSource;
                    if (audioSource.isPlaying)
                    {
                        audioSource.volume = AudioHandler.Volume * ((!AudioHandler.IsSfx) ? _musicVolume : _sfxVolume);
                    }
                }
            }
        }

        public override void Update()
        {
            float unscaledDeltaTime = Time.unscaledDeltaTime;
            if (AllHandlers.Count > 0)
            {
                for (int i = 0; i < AllHandlers.Count; i++)
                {
                    AudioHandler handler = AllHandlers[i];
                    UpdateHandler(handler, unscaledDeltaTime);
                }
            }
            foreach (AudioGroup current in AllGroups.Values)
            {
                UpdateGroup(current, unscaledDeltaTime);
            }
            m_envSoundMgr.Update();
            if (_isFadingAudioListener)
            {
                if (UnityEngine.AudioListener.volume > _fadingAudioListenerVolume)
                {
                    float num = UnityEngine.AudioListener.volume - _fadingAudioListenerSpeed * unscaledDeltaTime;
                    if (num <= _fadingAudioListenerVolume)
                    {
                        num = _fadingAudioListenerVolume;
                        _isFadingAudioListener = false;
                    }
                    UnityEngine.AudioListener.volume = num;
                }
                else
                {
                    float num2 = UnityEngine.AudioListener.volume + _fadingAudioListenerSpeed * unscaledDeltaTime;
                    if (num2 >= _fadingAudioListenerVolume)
                    {
                        num2 = _fadingAudioListenerVolume;
                        _isFadingAudioListener = false;
                    }
                    UnityEngine.AudioListener.volume = num2;
                }
            }
        }

        private void UpdateHandler(AudioHandler handler, float delta)
        {
            if (handler.gameObject == null)
            {
                DestroyHandler(handler);
                return;
            }
            AudioSource audioSource = handler.audioSource;
            if (!audioSource.gameObject.activeInHierarchy)
            {
                return;
            }
            if (!audioSource.enabled)
            {
                return;
            }
            if (!audioSource.isPlaying && !audioSource.loop)
            {
                if (!handler.IsLoop)
                {
                    DestroyHandler(handler);
                    return;
                }
                if (handler.Config != null && handler.Config.Intervals != null)
                {
                    if (handler.TimeToNext > 0f)
                    {
                        handler.TimeToNext -= Time.deltaTime;
                        if (handler.TimeToNext <= 0f)
                        {
                            handler.TimeToNext = 0f;
                            PlayNextAudio(handler, handler.Config);
                        }
                    }
                    else
                    {
                        float timeToNext;
                        if (handler.Config.Intervals.Length == 1)
                        {
                            timeToNext = handler.Config.Intervals[0];
                        }
                        else
                        {
                            timeToNext = UnityEngine.Random.Range(handler.Config.Intervals[0], handler.Config.Intervals[1]);
                        }
                        handler.TimeToNext = timeToNext;
                    }
                }
                else
                {
                    PlayNextAudio(handler, handler.Config);
                }
            }
            if (handler.IsFading)
            {
                if (handler.Volume > handler.FadeToVolume)
                {
                    float num = handler.Volume - handler.FadeSpeed * delta;
                    if (num <= handler.FadeToVolume)
                    {
                        num = handler.FadeToVolume;
                        handler.IsFading = false;
                    }
                    handler.Volume = num;
                }
                else
                {
                    float num2 = handler.Volume + handler.FadeSpeed * delta;
                    if (num2 >= handler.FadeToVolume)
                    {
                        num2 = handler.FadeToVolume;
                        handler.IsFading = false;
                    }
                    handler.Volume = num2;
                }
                audioSource.volume = handler.Volume * ((!handler.IsSfx) ? _musicVolume : _sfxVolume);
                if (handler.DestroyWhenDoneFade && !handler.IsFading)
                {
                    DestroyHandler(handler);
                    return;
                }
            }
        }

        private void UpdateGroup(AudioGroup group, float delta)
        {
            if (group.IsHasPlayingStateDirty)
            {
                group.IsHasPlayingStateDirty = false;
                AudioGroupConfig AudioGroupConfig = AllGroupConfigs[group.GroupId];
                if (AudioGroupConfig.AffectedVolumeModifyGroups != null)
                {
                    foreach (string current in AudioGroupConfig.AffectedVolumeModifyGroups.Keys)
                    {
                        AudioGroup group2 = GetGroup(current);
                        AudioGroupConfig AudioGroupConfig2 = AllGroupConfigs[current];
                        if (group2 != null)
                        {
                            foreach (AudioHandler current2 in group2.PlayingHandler)
                            {
                                if (!current2.IsFading || !current2.DestroyWhenDoneFade)
                                {
                                    float handlerVolume = GetHandlerVolume(current2, AudioGroupConfig2.IsSfx);
                                    if (current2.IsFading)
                                    {
                                        current2.FadeToVolume = handlerVolume;
                                    }
                                    else
                                    {
                                        FadeHandlerVolume(current2, handlerVolume, 2f, false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DestroyHandler(AudioHandler handler)
        {
            handler.OnDestroy();
            if (handler.NeedDestroyGameObject && handler.gameObject != null)
            {
                UnityEngine.Object.DestroyImmediate(handler.gameObject);
            }
        }

        private AudioConfig GetPlayConfig(string name, out string acName)
        {
            AudioConfig AudioConfig = null;
            if (AllConfigs.TryGetValue(name, out AudioConfig))
            {
                if (AudioConfig.ChannelKey == string.Empty)
                {
                    if (AudioConfig.PlayMode == AudioConfig.EPlayMode.Random)
                    {
                        int index = UnityEngine.Random.Range(0, AudioConfig.AudioNames.Count);
                        acName = AudioConfig.AudioNames[index];
                    }
                    else if (AudioConfig.PlayMode == AudioConfig.EPlayMode.List)
                    {
                        acName = AudioConfig.AudioNames[0];
                    }
                    else
                    {
                        acName = AudioConfig.AudioName;
                    }
                }
                else
                {
                    int channel = GetChannel(AudioConfig.ChannelKey);
                    List<string> list = AudioConfig.ChannelAudioNames[channel];
                    int index2 = 0;
                    if (AudioConfig.PlayMode == AudioConfig.EPlayMode.Random)
                    {
                        index2 = UnityEngine.Random.Range(0, list.Count);
                    }
                    acName = list[index2];
                }
                return AudioConfig;
            }
            acName = name;
            return null;
        }

        private void PlayNextAudio(AudioHandler handler, AudioConfig config)
        {
            List<string> list = config.AudioNames;
            if (config != null && config.ChannelKey != string.Empty)
            {
                list = config.ChannelAudioNames[GetChannel(config.ChannelKey)];
            }
            if (config.PlayMode == AudioConfig.EPlayMode.List)
            {
                handler.AudioIndex++;
                if (handler.AudioIndex >= list.Count)
                {
                    handler.AudioIndex = 0;
                }
            }
            else if (config.PlayMode == AudioConfig.EPlayMode.Random)
            {
                handler.AudioIndex = UnityEngine.Random.Range(0, list.Count);
                if (handler.audioSource.clip != null)
                {
                    string name = handler.audioSource.clip.name;
                    if (list[handler.AudioIndex] == name)
                    {
                        for (int i = 1; i < list.Count; i++)
                        {
                            int num = (handler.AudioIndex + i) % list.Count;
                            if (list[num] != name)
                            {
                                handler.AudioIndex = num;
                                break;
                            }
                        }
                    }
                }
            }
            LoadAudioRes(list[handler.AudioIndex], (AudioClip audioRes)=>
            {
                handler.audioSource.clip = audioRes;
                handler.audioSource.Play();
                if (config != null && config.FadeInTime > 0f)
                {
                    float volume = handler.Volume;
                    handler.Volume = 0f;
                    handler.audioSource.volume = 0f;
                    FadeHandlerVolume(handler, volume, 1f / handler.Config.FadeInTime, false);
                }
            });
        }

        private AudioGroup GetGroup(string groupId)
        {
            AudioGroup result = null;
            AllGroups.TryGetValue(groupId, out result);
            return result;
        }

        private AudioGroup GetGroup(AudioConfig config)
        {
            if (config != null && config.GroupId != null)
            {
                return GetGroup(config.GroupId);
            }
            return null;
        }

        private AudioPlayInfo GetPlayInfo(string name, bool isCreate)
        {
            AudioPlayInfo AudioPlayInfo = null;
            if (!AllAudioPlayInfos.TryGetValue(name, out AudioPlayInfo) && isCreate)
            {
                AudioPlayInfo = new AudioPlayInfo();
                AudioPlayInfo.AudioName = name;
                AllAudioPlayInfos.Add(name, AudioPlayInfo);
            }
            return AudioPlayInfo;
        }

        private void LoadAudioRes(string name, Action<AudioClip> action)
        {
            CoreUtils.assetService.LoadAssetAsync<AudioClip>(name, (IAsset asset) =>
            {
                action?.Invoke(asset.asset() as AudioClip);
            });
        }

        private void DestroyAudioRes(AudioClip clip)
        {
            UnityEngine.Object.Destroy(clip);
        }

        private void Setup3DAudioSource(AudioSource audioSource, Audio3DConfig config3D)
        {
            bool flag = true;
            if (config3D.CurveKey != string.Empty)
            {
                AnimationCurve curve = null;
                if (AllAudioCurves.TryGetValue(config3D.CurveKey, out curve))
                {
                    audioSource.rolloffMode = AudioRolloffMode.Custom;
                    audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
                    flag = false;
                }
            }
            if (flag)
            {
                audioSource.rolloffMode = AudioRolloffMode.Linear;
            }
            audioSource.dopplerLevel = config3D.DopplerLevel;
            audioSource.minDistance = config3D.MinDistance;
            audioSource.maxDistance = config3D.MaxDistance;
            audioSource.spatialBlend = config3D.SpatialBlend;
            audioSource.spread = config3D.Spread;
        }

        private float GetHandlerVolume(AudioHandler handler, bool isSfx)
        {
            float num = 1f;
            if (num <= 0f)
            {
                return 0f;
            }
            AudioGroupConfig groupConfig = handler.GroupConfig;
            if (groupConfig != null)
            {
                if (handler.Group.Mute)
                {
                    return 0f;
                }
                num *= groupConfig.BaseVolume;
                if (groupConfig.VolumeModifyGroups != null)
                {
                    foreach (string current in groupConfig.VolumeModifyGroups.Keys)
                    {
                        AudioGroup AudioGroup = null;
                        if (AllGroups.TryGetValue(current, out AudioGroup) && AudioGroup.PlayCount > 0)
                        {
                            num *= groupConfig.VolumeModifyGroups[current];
                        }
                    }
                }
            }
            return num;
        }

        public void StopGroupByName(string groupName)
        {
            AudioGroup group = GetGroup(groupName);
            if (group == null)
            {
                return;
            }
            for (int i = group.PlayingHandler.Count - 1; i >= 0; i--)
            {
                StopByHandler(group.PlayingHandler[i]);
            }
        }

        public void MuteGroup(string groupName, bool mute)
        {
            AudioGroup group = GetGroup(groupName);
            if (group == null)
            {
                return;
            }
            group.Mute = mute;
            for (int i = 0; i < group.PlayingHandler.Count; i++)
            {
                AudioHandler AudioHandler = group.PlayingHandler[i];
                AudioHandler.Volume = GetHandlerVolume(AudioHandler, AudioHandler.IsSfx);
                AudioHandler.FadeVolume(AudioHandler.Volume, 0.5f, false);
            }
        }
        public void SetEnvSoundMinUpdateCamDist(float minUpdateCamDist)
        {
            m_envSoundMgr.MinUpdateCamDist = minUpdateCamDist;
        }
        public void SetEnvSoundMaxPlayCount(int maxCount)
        {
            m_envSoundMgr.MaxPlayCount = maxCount;
        }
        public void SetEnvSoundLodDistance(float lodDistance)
        {
            m_envSoundMgr.LodDistance = lodDistance;
        }
        public void SetLodDistance(float lodDistance)
        {
            m_envSoundMgr.LodDistance = lodDistance;
        }
        public void Register(EnvSoundHandler comp)
        {
            m_envSoundMgr.Register(comp);
        }
        public void UnRegister(EnvSoundHandler comp)
        {
            m_envSoundMgr.UnRegister(comp);
        }
    }
}
