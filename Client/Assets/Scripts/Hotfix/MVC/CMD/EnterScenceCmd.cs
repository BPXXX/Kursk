
using ProtoBuf;
using PureMVC.Interfaces;
using Skyunion;
using System.IO;
using UnityEngine;
namespace Game
{
    public class EnterScenceCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            CoreUtils.uiManager.CloseAll();
            //显示主界面
            //CoreUtils.uiManager.ShowUI();
            /*int sceneIndex = (int)notification.Body;
            CoreUtils.assetService.LoadSceneAssetAsync($"scene_{sceneIndex}", true, (IAsset asset) =>
              {
                  Tip.CreateTip("加载成功").Show();
              });*/

        }
    }
}

