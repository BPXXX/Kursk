
using ProtoBuf;
using PureMVC.Interfaces;
using Skyunion;
using System.IO;
using UnityEngine;
namespace Game
{
    public class ShowSingleInfoCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.SingleName:
                    {

                    }
                    break;
                case CmdConstant.SingleScore:
                    {

                    }
                    break;
                case CmdConstant.TatalScores:
                    {

                    }
                    break;
            }
         }
    }
}

