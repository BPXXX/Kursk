using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ConfigDefine
    {
        /// <summary> 
        /// 程序使用
        /// </summary>
        [PrimaryKey] [AutoIncrement] public int id  { get; set; }

        /// <summary> 
        /// 创角登录服务器IP
        /// </summary>
        public string serverIP;

        /// <summary> 
        /// 创角登录用于初始创角登陆，初始自带的粮食资源
        /// </summary>
        public int initialFood;

        /// <summary> 
        /// 创角登录用于初始创角登陆，初始自带的木材资源
        /// </summary>
        public int initialWood;

        /// <summary> 
        /// 创角登录用于初始创角登陆，初始自带的石料资源
        /// </summary>
        public int initialStone;

        /// <summary> 
        /// 创角登录用于初始创角登陆，初始自带的金币资源
        /// </summary>
        public int initialGold;

        /// <summary> 
        /// 创角登录用于初始创角登陆，初始自带的钻石资源
        /// </summary>
        public int initialDiamond;

        /// <summary> 
        /// 地图场景地图宽度，暂定7200
        /// </summary>
        public int kingdomMapWidth;

        /// <summary> 
        /// 地图场景地图长度，暂定7200
        /// </summary>
        public int kingdomMapLength;

        /// <summary> 
        /// 地图场景客户端网格间距，暂定6
        /// </summary>
        public int kingdomClientDistance;

        /// <summary> 
        /// 地图场景主场景外层级显示瓦片尺寸，暂定40
        /// </summary>
        public int kingdomMapTileSize;

        /// <summary> 
        /// 兵种训练训练终止，返还资源比例，单位：千分比
        /// </summary>
        public int trainingTerminate;

        /// <summary> 
        /// 飘字常量货币数量≤1000时，飘飞数量
        /// </summary>
        public int flyingNum_1;

        /// <summary> 
        /// 飘字常量货币数量＞1000，≤1万时，飘飞数量
        /// </summary>
        public int flyingNum_2;

        /// <summary> 
        /// 飘字常量货币数量＞1万，≤5万时，飘飞数量
        /// </summary>
        public int flyingNum_3;

        /// <summary> 
        /// 飘字常量货币数量＞5万，≤15万时，飘飞数量
        /// </summary>
        public int flyingNum_4;

        /// <summary> 
        /// 飘字常量货币数量＞15万，≤50万时，飘飞数量
        /// </summary>
        public int flyingNum_5;

        /// <summary> 
        /// 飘字常量货币数量＞50万，≤150万时，飘飞数量
        /// </summary>
        public int flyingNum_6;

        /// <summary> 
        /// 飘字常量货币数量＞150万时，飘飞数量
        /// </summary>
        public int flyingNum_7;

        /// <summary> 
        /// 飘字常量飘飞动画阶段1持续时间，单位：毫秒
        /// </summary>
        public int flyingTimePhase1;

        /// <summary> 
        /// 飘字常量飘飞动画阶段2持续时间，单位：毫秒
        /// </summary>
        public int flyingTimePhase2;

        /// <summary> 
        /// 飘字常量初始飘飞缩放比例，单位：千分比
        /// </summary>
        public int flyingInitialZoom;

        /// <summary> 
        /// 飘字常量结束飘飞缩放比例，单位：千分比
        /// </summary>
        public int flyingFinishZoom;

        /// <summary> 
        /// 飘字常量粮食图标资源位置
        /// </summary>
        public string flyingFoodRes;

        /// <summary> 
        /// 飘字常量木材图标资源位置
        /// </summary>
        public string flyingWoodRes;

        /// <summary> 
        /// 飘字常量石料图标资源位置
        /// </summary>
        public string flyingStoneRes;

        /// <summary> 
        /// 飘字常量金币图标资源位置
        /// </summary>
        public string flyingGlodRes;

        /// <summary> 
        /// 飘字常量粮食飘飞目标点，x轴坐标
        /// </summary>
        public int flyingFood_X;

        /// <summary> 
        /// 飘字常量粮食飘飞目标点，y轴坐标
        /// </summary>
        public int flyingFood_Y;

        /// <summary> 
        /// 飘字常量木材飘飞目标点，x轴坐标
        /// </summary>
        public int flyingWood_X;

        /// <summary> 
        /// 飘字常量木材飘飞目标点，y轴坐标
        /// </summary>
        public int flyingWood_Y;

        /// <summary> 
        /// 飘字常量石料飘飞目标点，x轴坐标
        /// </summary>
        public int flyingStone_X;

        /// <summary> 
        /// 飘字常量石料飘飞目标点，y轴坐标
        /// </summary>
        public int flyingStone_Y;

        /// <summary> 
        /// 飘字常量金币飘飞目标点，x轴坐标
        /// </summary>
        public int flyingGlod_X;

        /// <summary> 
        /// 飘字常量金币飘飞目标点，y轴坐标
        /// </summary>
        public int flyingGlod_Y;

        /// <summary> 
        /// 稀有度语言包稀有度语言包，普通+0，优秀+1，精英+2，传说+3，史诗+4
        /// </summary>
        public int rareLanguage;

        /// <summary> 
        /// 资源结算资源建筑统计间隔时间，单位：秒
        /// </summary>
        public int resStatisticsTime;

        /// <summary> 
        /// 资源采集采集本联盟归属资源点获得的采集速度加成，25%。
        /// </summary>
        public int allianceResourceGatherAdd;

        /// <summary> 
        /// 资源采集玩家进行资源点搜索时，可支持的最大范围区域半径
        /// </summary>
        public int resourceGatherFindRadius;

        /// <summary> 
        /// 资源采集服务器中两次进行资源区域重刷的时间间隔，暂定3600秒。
        /// </summary>
        public int resourceGatherRefreshFreq;

        /// <summary> 
        /// 资源采集该服务器中各资源点的最高等级，普通服务器暂定6
        /// </summary>
        public int resourceGatherPointLevelMax;

        /// <summary> 
        /// 资源采集客户端两次进行相同条件搜索的CD时间
        /// </summary>
        public int resourceGatherSearchCd;

        /// <summary> 
        /// 资源采集资源点占据的地图空间半径
        /// </summary>
        public int resourceGatherRadius;

        /// <summary> 
        /// 资源采集每单位粮食需要的负载，暂定为1
        /// </summary>
        public int foodRaito;

        /// <summary> 
        /// 资源采集每单位木材需要的负载，暂定为1
        /// </summary>
        public int woodRaito;

        /// <summary> 
        /// 资源采集每单位石头需要的负载，暂定为1
        /// </summary>
        public int stoneRaito;

        /// <summary> 
        /// 资源采集每单位金币需要的负载，暂定为1
        /// </summary>
        public int goldRaito;

        /// <summary> 
        /// 资源采集每单位宝石需要的负载，暂定为1000
        /// </summary>
        public int diamonRaito;

        /// <summary> 
        /// 新手引导攻击野蛮人新手引导结束后赠送的统帅ID
        /// </summary>
        public int guideHero;

        /// <summary> 
        /// 战斗普通攻击系数
        /// </summary>
        public int ordinaryAttackConstant;

        /// <summary> 
        /// 战斗反击系数
        /// </summary>
        public int counterAttackConstant;

        /// <summary> 
        /// 战斗治疗系数
        /// </summary>
        public int healAttackConstant;

        /// <summary> 
        /// 战斗单位默认攻击距离（1为相邻的1个坐标点，2为相邻的2个坐标点）
        /// </summary>
        public int attkRange;

        /// <summary> 
        /// 战斗未造成伤害或受到伤害X秒后脱离战斗
        /// </summary>
        public int outOfCombat;

        /// <summary> 
        /// 战斗受到增援时闪屏的次数
        /// </summary>
        public int reinforceTips;

        /// <summary> 
        /// 科技研究科技研究半途终止，资源返还比例，单位：千分比
        /// </summary>
        public int studyTerminate;

        /// <summary> 
        /// 邮件邮件保存数量
        /// </summary>
        public int emailSave;

        /// <summary> 
        /// 邮件采集邮件保存数量
        /// </summary>
        public int emailResourceSave;

        /// <summary> 
        /// 怪物-野蛮人刷新野蛮人的时间间隔，暂定3600秒
        /// </summary>
        public int barbarianFreq;

        /// <summary> 
        /// 怪物-野蛮人每个瓦片平均刷新的野蛮人数量
        /// </summary>
        public int barbarianNum;

        /// <summary> 
        /// 怪物-野蛮人客户端两次进行相同条件搜索的CD时间，暂定4秒
        /// </summary>
        public int monsterSearchCd;

        /// <summary> 
        /// 怪物-野蛮人会自动刷新的野蛮人等级，12级以内
        /// </summary>
        public int barbarianLevelLimit;

        /// <summary> 
        /// 任务章节显示主线/支线面板需完成的章节需求
        /// </summary>
        public int preChapter;

        /// <summary> 
        /// 工人队列可以临时扩充工人队列的道具ID
        /// </summary>
        public int workQueueItem;

        /// <summary> 
        /// 工人队列临时扩充工人队列需要的
        /// </summary>
        public int workQueueDenar;

        /// <summary> 
        /// 工人队列临时工人队列持续时间，暂定48小时
        /// </summary>
        public int workQueueTime;

        /// <summary> 
        /// 工人队列玩家可同时拥有的建筑队列的最大数量
        /// </summary>
        public int workQueueMax;

        /// <summary> 
        /// 军队溃散部队行军速度
        /// </summary>
        public int ArmsDefeatedSpeed;

        /// <summary> 
        /// 怪物-野蛮人野蛮人占据空间半径
        /// </summary>
        public int barbarianRadius;

        /// <summary> 
        /// 行动力钻石兑换行动力，钻石数量
        /// </summary>
        public int denarChangeEnery1;

        /// <summary> 
        /// 行动力钻石兑换行动力，行动力数量
        /// </summary>
        public int denarChangeEnery2;

        /// <summary> 
        /// 迷雾王国地图的补偿掉落组ID
        /// </summary>
        public int worldMapCompensate;

        /// <summary> 
        /// 迷雾行军参数1
        /// </summary>
        public int marchParameter1;

        /// <summary> 
        /// 迷雾行军参数2
        /// </summary>
        public int marchParameter2;

        /// <summary> 
        /// 斥候斥候1Icon
        /// </summary>
        public string toScoutsIcon1;

        /// <summary> 
        /// 斥候斥候2Icon
        /// </summary>
        public string toScoutsIcon2;

        /// <summary> 
        /// 斥候斥候3Icon
        /// </summary>
        public string toScoutsIcon3;

        /// <summary> 
        /// 斥候斥候1模型
        /// </summary>
        public string toScoutsModel1;

        /// <summary> 
        /// 斥候斥候2模型
        /// </summary>
        public string toScoutsModel2;

        /// <summary> 
        /// 斥候斥候3模型
        /// </summary>
        public string toScoutsModel3;

        /// <summary> 
        /// 治疗治疗所需最少时间，单位：秒
        /// </summary>
        public int cureMinTime;

        /// <summary> 
        /// 帮助帮助每次提升比例，单位：千分比
        /// </summary>
        public int helpAddProportion;

        /// <summary> 
        /// 帮助帮助每次最少提升基础时间，单位：秒
        /// </summary>
        public int helpMinAddTime;

        /// <summary> 
        /// 帮助帮助今日积分上限
        /// </summary>
        public int individualPointsLimit;

        /// <summary> 
        /// 帮助每次帮助可获得积分
        /// </summary>
        public int individualPointsAward;

        /// <summary> 
        /// 聊天王国世界频道，市政厅等级到达X，可以在该频道发言
        /// </summary>
        public int worldCannelLvLimit;

        /// <summary> 
        /// 聊天私聊频道，市政厅等级到达X，可以在该频道发言
        /// </summary>
        public int playerChatCannelLvLimit;

        /// <summary> 
        /// 聊天王国世界频道，发言间隔：距离上一条信息发送的时间，单位：秒
        /// </summary>
        public int worldCannelTimeLimit;

        /// <summary> 
        /// 聊天联盟频道，发言间隔：距离上一条信息发送的时间，单位：秒
        /// </summary>
        public int unionCannelTimeLimit;

        /// <summary> 
        /// 聊天私聊频道，发言间隔：距离上一条信息发送的时间，单位：秒
        /// </summary>
        public int playerChatCannelTimeLimit;

        /// <summary> 
        /// 聊天玩家输入的文本字符限制
        /// </summary>
        public int channelWordLimit;

        /// <summary> 
        /// 聊天王国世界频道&联盟频道,信息存储X天
        /// </summary>
        public int cannelSave1;

        /// <summary> 
        /// 聊天王国世界频道&联盟频道,信息存储X条
        /// </summary>
        public int cannelSave2;

        /// <summary> 
        /// 聊天私聊频道，信息存储X天
        /// </summary>
        public int playerChatCannelSave1;

        /// <summary> 
        /// 聊天私聊频道，信息存储X条
        /// </summary>
        public int playerChatCannelSave2;

        /// <summary> 
        /// 聊天时间戳间隔，距离上一条消息大于X秒时需要显示时间戳
        /// </summary>
        public int timeStamp;

        /// <summary> 
        /// 场景城堡占地半径
        /// </summary>
        public int cityRadius;

    }
}