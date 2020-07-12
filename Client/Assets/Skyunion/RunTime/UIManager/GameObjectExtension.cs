/////////////////////////////////////////////////////////////////////////////////
// @desc 拓展GameObject,在热更新层使用
// @copyright ©2018 iGG
// @release 2018年4月11日 星期三
// @author BobWong
// @mail 15959187562@qq.com
/////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace Skyunion
{
    public static class GameObjectExtension
    {
        public static MonoLikeEntity AddHotFixComponent(this GameObject go, MonoLikeEntity entity)
        {
            var binder = go.GetComponent<BehaviourBinder>();
            if (binder == null)
            {
                binder = go.AddComponent<BehaviourBinder>();
            }
            entity.transform = go.transform;
            return binder.AddHotFixComponent(entity);
        }

        public static void RemoveHotFixComponent(this GameObject go, MonoLikeEntity entity)
        {
            var binder = go.GetComponent<BehaviourBinder>();
            if (binder != null)
            {
                binder.RemoveHotFixComponent(entity);
            }
        }

        public static MonoLikeEntity GetHotFixComponent(this GameObject go, System.Type monoLikeType)
        {
            var binder = go.GetComponent<BehaviourBinder>();
            if (binder != null)
            {
                return binder.GetHotFixComponent(monoLikeType);
            }
            return null;
        }

        public static MonoLikeEntity[] GetHotFixComponentInChildren(this GameObject go, System.Type monoLikeType)
        {
            List<MonoLikeEntity> entityList = new List<MonoLikeEntity>();
            foreach (var item in go.GetComponentsInChildren<Transform>())
            {
                var binder = item.GetComponent<BehaviourBinder>();
                if (binder != null)
                {
                    entityList.Add(binder.GetHotFixComponent(monoLikeType));
                }
            }

            return entityList.ToArray();
        }

        public static GameView AddHotFixViewComponent(this GameObject go, GameView entity)
        {
            var binder = go.GetComponent<ViewBinder>();
            if (binder == null)
            {
                binder = go.AddComponent<ViewBinder>();
            }

            var com = binder.AddHotFixViewComponent(entity);
            com.BindSingleUI(go);
            return com;
        }

        public static void RemoveHotFixViewComponent(this GameObject go, GameView entity)
        {
            var binder = go.GetComponent<ViewBinder>();
            if (binder != null)
            {
                binder.RemoveHotFixViewComponent(entity);
            }
        }

        public static GameView GetHotFixViewComponent(this GameObject go, System.Type gameView)
        {
            var binder = go.GetComponent<ViewBinder>();
            if (binder != null)
            {
                return binder.GetHotFixViewComponent(gameView);
            }
            return null;
        }
    }
}