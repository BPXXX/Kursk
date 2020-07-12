using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILRuntime.Reflection;
using Skyunion;
using UnityEngine;
using UnityEngine.Networking;

namespace Skyunion
{
    public static class CoreUtils
    {
        #region 服务
        static IADService _adService = null;
        public static IADService adService
        {
            get
            {
                return _adService ?? (_adService = PluginManager.Instance().FindModule<IADService>());
            }
            set
            {
                _adService = value;
            }
        }
        static ILogService _logService = null;
        public static ILogService logService
        {
            get
            {
                return _logService ?? (_logService = PluginManager.Instance().FindModule<ILogService>());
            }
            set
            {
                _logService = value;
            }
        }
        static IAssetService _assetService;
        public static IAssetService assetService
        {
            get
            {
                return _assetService ?? (_assetService=PluginManager.Instance().FindModule<IAssetService>());
            }
            set
            {
                _assetService = value;
            }
        }
        static INetServcice _netService;
        public static INetServcice netService
        {
            get
            {
                return _netService ?? (_netService=PluginManager.Instance().FindModule<INetServcice>());
            }
            set
            {
                _netService = value;
            }
        }
        static IHotFixService _hotService;
        public static IHotFixService hotService
        {
            get
            {
                return _hotService ?? (_hotService=PluginManager.Instance().FindModule<IHotFixService>());
            }
            set
            {
                _hotService = value;
            }
        }
        static IDataService _dataService;
        public static IDataService dataService
        {
            get
            {
                return _dataService ?? (_dataService=PluginManager.Instance().FindModule<IDataService>());
            }
            set
            {
                _dataService = value;
            }
        }
        static IAudioService _audioService;
        public static IAudioService audioService
        {
            get
            {
                return _audioService ?? (_audioService=PluginManager.Instance().FindModule<IAudioService>());
            }
            set
            {
                _audioService = value;
            }
        }
        #endregion
        #region 管理器
        static IInputManager _inputManager;
        public static IInputManager inputManager
        {
            get
            {
                return _inputManager ?? (_inputManager=PluginManager.Instance().FindModule<IInputManager>());
            }
            set
            {
                _inputManager = value;
            }
        }
        static IUIManager _uiManager;
        public static IUIManager uiManager
        {
            get
            {
                return _uiManager ?? (_uiManager=PluginManager.Instance().FindModule<IUIManager>());
            }
            set
            {
                _uiManager = value;
            }
        }
        #endregion
        public static void ClearCore()
        {
            _uiManager = null;
            _inputManager = null;
            _audioService = null;
            _dataService = null;
            _hotService = null;
            _netService = null;
            _logService = null;
            _assetService = null;
            _adService = null;
        }

        public enum GraphicLevel 
        {
            NONE,
            LOW,
            MEDIUM,
            HIGH
        }
        private static GraphicLevel m_graphic_level = GraphicLevel.HIGH;
        public static void SetGraphicLevel(int level)
        {
            m_graphic_level = (GraphicLevel)level;
        }

        public static GraphicLevel GetGraphicLevel()
        {
            return m_graphic_level;
        }


        internal static void LoadFileAsync(string path, Action<byte[]> completed)
        {
            if (path.IndexOf(Path.PathSeparator) == -1)
            {
                path = Path.Combine(Application.streamingAssetsPath, path);
            }
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            path = path.Insert(0, "file://");
#endif

            UnityWebRequest webFile = UnityWebRequest.Get(path);
            webFile.SendWebRequest().completed += (AsyncOperation op) =>
            {
                completed?.Invoke(webFile.downloadHandler.data);
            };
        }
        
        
        
        
    }
}
