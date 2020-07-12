
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class AddSocresCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            Msg.Msg_ADDSCORES_S2C xMsg =(Msg.Msg_ADDSCORES_S2C)notification.Body;
            for(int i=0;i<TmpData.userInfoAll.Length;i++)
            {
                var index = i;
                if (TmpData.userInfoAll[index].user_id == xMsg.id)
                {
                    TmpData.userInfoAll[index].user_point = xMsg.socres;
                    if (TmpData.userInfoSelf.user_id == xMsg.id)
                        TmpData.userInfoSelf.user_point = xMsg.socres;
                    SendNotification(CmdConstant.TatalScores);
                }
                   

            }
            
        }
    }
}

