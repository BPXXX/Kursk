using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Skyunion;

namespace Sproto
{
	public class ProtocolFunctionDictionary
	{
		public class MetaInfo {
			public Type ProtocolType;
			public KeyValuePair<Type, typeFunc> Request;
			public KeyValuePair<Type, typeFunc> Response;
		};

		public delegate SprotoTypeBase typeFunc (byte[] buffer, int offset);
        private Dictionary<int, MetaInfo> MetaDictionary;

		public ProtocolFunctionDictionary () {
            this.MetaDictionary = new Dictionary<int, MetaInfo> ();
		}

        private MetaInfo _getMeta(int tag) {
			MetaInfo data;
			if (!this.MetaDictionary.TryGetValue (tag, out data)) {
				data = new MetaInfo ();
				this.MetaDictionary.Add (tag, data);
			}
			return data;
		}

		/// <summary>
		/// 是否存在Response包
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public bool HasResponse(int tag) {
			MetaInfo data;
			if (!this.MetaDictionary.TryGetValue (tag, out data)) {
				data = new MetaInfo ();
				this.MetaDictionary.Add (tag, data);
			}
			return  (data.Response.Key != null);
		}


        public void SetRequest<T>(int tag,string fullname =null) where T: SprotoTypeBase, new() {
			MetaInfo data = this._getMeta (tag);
            _set<T> (tag, out data.Request,fullname);
		}


        public void SetResponse<T>(int tag,string fullname = null) where T: SprotoTypeBase, new() {
			MetaInfo data = this._getMeta (tag);
            _set<T> (tag, out data.Response, fullname);
		}


        private void _set<T>(int tag, out KeyValuePair<Type, typeFunc> field,string fullname = null) where T : SprotoTypeBase, new() {
			typeFunc _func = delegate (byte[] buffer, int offset) {
                if (CoreUtils.hotService.GetHotfixMode() != HotfixMode.ILRT)
                {
                    T obj = new T();
                    obj.init(buffer, offset);
                    return obj;
                }
                // UnityEngine.Log.Info(tag + " fullname: "+fullname);
                var objHotfix = CoreUtils.hotService.Instantiate<T>(fullname);
                objHotfix.init(buffer, offset);
                return objHotfix;
			};

			field = new KeyValuePair<Type, typeFunc> (typeof(T), _func);
		}


        private SprotoTypeBase _gen(KeyValuePair<Type, typeFunc> field, int tag, byte[] buffer, int offset=0) {
			if (field.Value != null) {
				SprotoTypeBase obj = field.Value (buffer, offset);
#if (!INCLUDE_IL2CPP)
				if (obj.GetType () != field.Key) {
                    throw new Exception(string.Format("sproto type: {0} not is expected. [{1}]",obj.GetType().ToString(),field.Key.ToString()));
				}
#endif
				return obj;
			}
			return null;
		}


        public SprotoTypeBase GenResponse(int tag, byte[] buffer, int offset=0) {
			MetaInfo data = this.MetaDictionary[tag];
			return _gen (data.Response, tag, buffer, offset);
		}

        public SprotoTypeBase GenRequest(int tag, byte[] buffer, int offset=0) {
            if (!this.MetaDictionary.ContainsKey(tag))
            {
                Debug.LogError("Protocol tag not find :" + tag);
                return null;
            }
			MetaInfo data = this.MetaDictionary[tag];
			return _gen (data.Request, tag, buffer, offset);
		}


        public MetaInfo this[int tag] {
			get {
				return this.MetaDictionary [tag];
			}
		}

		//public int this[Type protocolType] {
		//	get {
		//		return this.ProtocolDictionary [protocolType];
		//	}
		//}
	}
}
