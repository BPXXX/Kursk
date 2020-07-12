using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class TmpData 
    {
        public struct AccountInfo
        {
            public string Account { get; set; }
            public string Passwords { get; set; }
            public AccountInfo(string account, string passwords)
            {
                Account = account;
                Passwords = passwords;
            }
        }
        public static AccountInfo Data;


        public struct UserInfoSelf
        {
            public int user_id { get; set; }
            public string user_name { get; set; }
            public int position_number { get; set; }
            public int tank_type { get; set; }
            public int user_point { get; set; }
            public int user_team { get; set; }

            public UserInfoSelf(int team,int id, string name, int number,int type, int point)
            {
                user_team = team;
                user_id = id;
                user_name = name;
                position_number = number;
                tank_type = type;
                user_point = point;
            }
            
        }

        public struct UserInfoAll {
            public int user_id { get; set; }
            public string user_name { get; set; }
            public Vector3 init_position { get; set; }
            public int tank_type { get; set; }
            public int user_point { get; set; }
            public int user_team { get; set; }

            public UserInfoAll(int team,int id, string name, Vector3 positon, int type,int point)
            {
                user_team = team;
                user_id = id;
                user_name = name;
                init_position = positon;
                tank_type = type;
                user_point = point;
            }

        }
        public static UserInfoSelf userInfoSelf;
        public static UserInfoAll []userInfoAll = new UserInfoAll[12];
    }
}

