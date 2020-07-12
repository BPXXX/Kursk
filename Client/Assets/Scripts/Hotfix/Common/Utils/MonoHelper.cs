/////////////////////////////////////////////////////////////////////////////////
// @desc 热更新层的Mono帮助类,方便使用者
// @copyright ©2018 iGG
// @release 2018年4月12日 星期四
// @author BobWong
// @mail 15959187562@qq.com
/////////////////////////////////////////////////////////////////////////////////

using Skyunion;
using UnityEngine;

namespace Game
{
    public class MonoHelper
    {
        public static T AddHotFixComponent<T>(GameObject go) where T : MonoLikeEntity, new()
        {
            return go.AddHotFixComponent(new T()) as T;
        }

        public static T GetHotFixComponent<T>(GameObject go) where T : MonoLikeEntity
        {
            return go.GetHotFixComponent(typeof(T)) as T;
        }

        public static void DestroyHotFixComponent(GameObject go, MonoLikeEntity entity)
        {
            go.RemoveHotFixComponent(entity);
        }

        public static T AddHotFixViewComponent<T>(GameObject go) where T : GameView, new()
        {
            return go.AddHotFixViewComponent(new T()) as T;
        }

        public static T GetHotFixViewComponent<T>(GameObject go) where T : GameView
        {
            return go.GetHotFixViewComponent(typeof(T)) as T;
        }
        public static T GetOrAddHotFixViewComponent<T>(GameObject go) where T : GameView, new()
        {
            T view = go.GetHotFixViewComponent(typeof(T)) as T;
            if (view == null)
            {
                view = go.AddHotFixViewComponent(new T()) as T;
            }
            return view;
        }

        public static void DestroyHotFixViewComponent(GameObject go, GameView entity)
        {
            go.RemoveHotFixViewComponent(entity);
        }
    }
}