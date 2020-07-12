using UnityEngine;
using System.Collections.Generic;
using System;
using SprotoType;
namespace Game
{
	public class PosInfoEntity
	{
		public const string PosInfoChange = "PosInfoChange";
		public System.Int64 x;
		public System.Int64 y;

		public static HashSet<string> updateEntity(PosInfoEntity et ,SprotoType.PosInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasX){
				et.x = data.x;
				if(ret)ET.ATTR.Add("x");
			}
			if(data.HasY){
				et.y = data.y;
				if(ret)ET.ATTR.Add("y");
			}
			return ET.ATTR;
		}
	}
	public class CollectReportEntity
	{
		public const string CollectReportChange = "CollectReportChange";
		public System.Int64 resourceTypeId;
		public SprotoType.PosInfo pos;
		public System.Int64 resource;
		public System.Int64 extraResource;

		public static HashSet<string> updateEntity(CollectReportEntity et ,SprotoType.CollectReport data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasResourceTypeId){
				et.resourceTypeId = data.resourceTypeId;
				if(ret)ET.ATTR.Add("resourceTypeId");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasResource){
				et.resource = data.resource;
				if(ret)ET.ATTR.Add("resource");
			}
			if(data.HasExtraResource){
				et.extraResource = data.extraResource;
				if(ret)ET.ATTR.Add("extraResource");
			}
			return ET.ATTR;
		}
	}
	public class BattleReportEntity
	{
		public const string BattleReportChange = "BattleReportChange";

		public static HashSet<string> updateEntity(BattleReportEntity et ,SprotoType.BattleReport data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			return ET.ATTR;
		}
	}
	public class RewardInfoEntity
	{
		public const string RewardInfoChange = "RewardInfoChange";
		public System.Int64 food;
		public System.Int64 wood;
		public System.Int64 stone;
		public System.Int64 gold;
		public System.Int64 denar;
		public System.Collections.Generic.List<SprotoType.RewardInfo.RewardItem> items;
		public System.Collections.Generic.List<SprotoType.SoldierInfo> soldiers;

		public static HashSet<string> updateEntity(RewardInfoEntity et ,SprotoType.RewardInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasFood){
				et.food = data.food;
				if(ret)ET.ATTR.Add("food");
			}
			if(data.HasWood){
				et.wood = data.wood;
				if(ret)ET.ATTR.Add("wood");
			}
			if(data.HasStone){
				et.stone = data.stone;
				if(ret)ET.ATTR.Add("stone");
			}
			if(data.HasGold){
				et.gold = data.gold;
				if(ret)ET.ATTR.Add("gold");
			}
			if(data.HasDenar){
				et.denar = data.denar;
				if(ret)ET.ATTR.Add("denar");
			}
			if(data.HasItems){
				et.items = data.items;
				if(ret)ET.ATTR.Add("items");
			}
			if(data.HasSoldiers){
				et.soldiers = data.soldiers;
				if(ret)ET.ATTR.Add("soldiers");
			}
			return ET.ATTR;
		}
	}
	public class SkillInfoEntity
	{
		public const string SkillInfoChange = "SkillInfoChange";
		public System.Int64 skillId;
		public System.Int64 skillLevel;

		public static HashSet<string> updateEntity(SkillInfoEntity et ,SprotoType.SkillInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasSkillId){
				et.skillId = data.skillId;
				if(ret)ET.ATTR.Add("skillId");
			}
			if(data.HasSkillLevel){
				et.skillLevel = data.skillLevel;
				if(ret)ET.ATTR.Add("skillLevel");
			}
			return ET.ATTR;
		}
	}
	public class SoldierInfoEntity
	{
		public const string SoldierInfoChange = "SoldierInfoChange";
		public System.Int64 id;
		public System.Int64 type;
		public System.Int64 level;
		public System.Int64 num;

		public static HashSet<string> updateEntity(SoldierInfoEntity et ,SprotoType.SoldierInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasId){
				et.id = data.id;
				if(ret)ET.ATTR.Add("id");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public class RewardItemEntity
	{
		public const string RewardItemChange = "RewardItemChange";
		public System.Int64 itemId;
		public System.Int64 itemNum;

		public static HashSet<string> updateEntity(RewardItemEntity et ,SprotoType.RewardInfo.RewardItem data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasItemId){
				et.itemId = data.itemId;
				if(ret)ET.ATTR.Add("itemId");
			}
			if(data.HasItemNum){
				et.itemNum = data.itemNum;
				if(ret)ET.ATTR.Add("itemNum");
			}
			return ET.ATTR;
		}
	}
	public class QueueInfoEntity
	{
		public const string QueueInfoChange = "QueueInfoChange";
		public System.Int64 queueIndex;
		public System.Boolean main;
		public System.Int64 finishTime;
		public System.Int64 buildingIndex;
		public System.Int64 expiredTime;
		public System.Int64 timerId;
		public System.Int64 type;
		public System.Int64 armyType;
		public System.Int64 newArmyLevel;
		public System.Int64 armyNum;
		public System.Int64 oldArmyLevel;
		public System.Int64 beginTime;
		public System.Int64 technologyType;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> treatmentSoldiers;

		public static HashSet<string> updateEntity(QueueInfoEntity et ,SprotoType.QueueInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasQueueIndex){
				et.queueIndex = data.queueIndex;
				if(ret)ET.ATTR.Add("queueIndex");
			}
			if(data.HasMain){
				et.main = data.main;
				if(ret)ET.ATTR.Add("main");
			}
			if(data.HasFinishTime){
				et.finishTime = data.finishTime;
				if(ret)ET.ATTR.Add("finishTime");
			}
			if(data.HasBuildingIndex){
				et.buildingIndex = data.buildingIndex;
				if(ret)ET.ATTR.Add("buildingIndex");
			}
			if(data.HasExpiredTime){
				et.expiredTime = data.expiredTime;
				if(ret)ET.ATTR.Add("expiredTime");
			}
			if(data.HasTimerId){
				et.timerId = data.timerId;
				if(ret)ET.ATTR.Add("timerId");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasArmyType){
				et.armyType = data.armyType;
				if(ret)ET.ATTR.Add("armyType");
			}
			if(data.HasNewArmyLevel){
				et.newArmyLevel = data.newArmyLevel;
				if(ret)ET.ATTR.Add("newArmyLevel");
			}
			if(data.HasArmyNum){
				et.armyNum = data.armyNum;
				if(ret)ET.ATTR.Add("armyNum");
			}
			if(data.HasOldArmyLevel){
				et.oldArmyLevel = data.oldArmyLevel;
				if(ret)ET.ATTR.Add("oldArmyLevel");
			}
			if(data.HasBeginTime){
				et.beginTime = data.beginTime;
				if(ret)ET.ATTR.Add("beginTime");
			}
			if(data.HasTechnologyType){
				et.technologyType = data.technologyType;
				if(ret)ET.ATTR.Add("technologyType");
			}
			if(data.HasTreatmentSoldiers){

				if (et.treatmentSoldiers == null) {
					 et.treatmentSoldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.treatmentSoldiers){ 
					if(et.treatmentSoldiers.ContainsKey(item.Key)){
						et.treatmentSoldiers[item.Key] = item.Value;
					}else{
						et.treatmentSoldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("treatmentSoldiers"); 
			}
			return ET.ATTR;
		}
	}
	public class ArmyInfoEntity
	{
		public const string ArmyInfoChange = "ArmyInfoChange";
		public System.Int64 armyIndex;
		public System.Int64 mainHeroId;
		public System.Int64 deputyHeroId;
		public System.Collections.Generic.List<SprotoType.SoldierInfo> soldiers;
		public System.Collections.Generic.List<SprotoType.RoleInfo.ResourceCollectInfo> resourceLoads;
		public SprotoType.RoleInfo.ResourceCollectInfo collectResource;
		public System.Collections.Generic.List<SprotoType.RoleInfo.CollectSpeedInfo> collectSpeeds;

		public static HashSet<string> updateEntity(ArmyInfoEntity et ,SprotoType.RoleInfo.ArmyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasArmyIndex){
				et.armyIndex = data.armyIndex;
				if(ret)ET.ATTR.Add("armyIndex");
			}
			if(data.HasMainHeroId){
				et.mainHeroId = data.mainHeroId;
				if(ret)ET.ATTR.Add("mainHeroId");
			}
			if(data.HasDeputyHeroId){
				et.deputyHeroId = data.deputyHeroId;
				if(ret)ET.ATTR.Add("deputyHeroId");
			}
			if(data.HasSoldiers){
				et.soldiers = data.soldiers;
				if(ret)ET.ATTR.Add("soldiers");
			}
			if(data.HasResourceLoads){
				et.resourceLoads = data.resourceLoads;
				if(ret)ET.ATTR.Add("resourceLoads");
			}
			if(data.HasCollectResource){
				et.collectResource = data.collectResource;
				if(ret)ET.ATTR.Add("collectResource");
			}
			if(data.HasCollectSpeeds){
				et.collectSpeeds = data.collectSpeeds;
				if(ret)ET.ATTR.Add("collectSpeeds");
			}
			return ET.ATTR;
		}
	}
	public class TechnologyInfoEntity
	{
		public const string TechnologyInfoChange = "TechnologyInfoChange";
		public System.Int64 technologyType;
		public System.Int64 level;

		public static HashSet<string> updateEntity(TechnologyInfoEntity et ,SprotoType.TechnologyInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTechnologyType){
				et.technologyType = data.technologyType;
				if(ret)ET.ATTR.Add("technologyType");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			return ET.ATTR;
		}
	}
	public class FinishTaskInfoEntity
	{
		public const string FinishTaskInfoChange = "FinishTaskInfoChange";
		public System.Int64 taskId;

		public static HashSet<string> updateEntity(FinishTaskInfoEntity et ,SprotoType.RoleInfo.FinishTaskInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTaskId){
				et.taskId = data.taskId;
				if(ret)ET.ATTR.Add("taskId");
			}
			return ET.ATTR;
		}
	}
	public class TaskStatisticsEntity
	{
		public const string TaskStatisticsChange = "TaskStatisticsChange";
		public System.Int64 type;
		public System.Collections.Generic.List<SprotoType.RoleInfo.Statistics> statistics;

		public static HashSet<string> updateEntity(TaskStatisticsEntity et ,SprotoType.RoleInfo.TaskStatistics data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasStatistics){
				et.statistics = data.statistics;
				if(ret)ET.ATTR.Add("statistics");
			}
			return ET.ATTR;
		}
	}
	public class RoleStatisticsEntity
	{
		public const string RoleStatisticsChange = "RoleStatisticsChange";
		public System.Int64 type;
		public System.Int64 num;

		public static HashSet<string> updateEntity(RoleStatisticsEntity et ,SprotoType.RoleInfo.RoleStatistics data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public class SoldierKillInfoEntity
	{
		public const string SoldierKillInfoChange = "SoldierKillInfoChange";
		public System.Int64 level;
		public System.Int64 num;

		public static HashSet<string> updateEntity(SoldierKillInfoEntity et ,SprotoType.RoleInfo.SoldierKillInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public class ChapterTaskInfoEntity
	{
		public const string ChapterTaskInfoChange = "ChapterTaskInfoChange";
		public System.Int64 taskId;
		public System.Int64 status;

		public static HashSet<string> updateEntity(ChapterTaskInfoEntity et ,SprotoType.RoleInfo.ChapterTaskInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasTaskId){
				et.taskId = data.taskId;
				if(ret)ET.ATTR.Add("taskId");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			return ET.ATTR;
		}
	}
	public class DenseFogInfoEntity
	{
		public const string DenseFogInfoChange = "DenseFogInfoChange";
		public System.Int64 index;
		public System.Int64 denseFog;

		public static HashSet<string> updateEntity(DenseFogInfoEntity et ,SprotoType.RoleInfo.DenseFogInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasIndex){
				et.index = data.index;
				if(ret)ET.ATTR.Add("index");
			}
			if(data.HasDenseFog){
				et.denseFog = data.denseFog;
				if(ret)ET.ATTR.Add("denseFog");
			}
			return ET.ATTR;
		}
	}
	public class BuildingInfoEntity
	{
		public const string BuildingInfoChange = "BuildingInfoChange";
		public System.Int64 buildingIndex;
		public System.Int64 type;
		public System.Int64 level;
		public SprotoType.PosInfo pos;
		public System.Int64 finishTime;
		public System.Int64 version;
		public System.Int64 lastRewardTime;

		public static HashSet<string> updateEntity(BuildingInfoEntity et ,SprotoType.BuildingInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasBuildingIndex){
				et.buildingIndex = data.buildingIndex;
				if(ret)ET.ATTR.Add("buildingIndex");
			}
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasFinishTime){
				et.finishTime = data.finishTime;
				if(ret)ET.ATTR.Add("finishTime");
			}
			if(data.HasVersion){
				et.version = data.version;
				if(ret)ET.ATTR.Add("version");
			}
			if(data.HasLastRewardTime){
				et.lastRewardTime = data.lastRewardTime;
				if(ret)ET.ATTR.Add("lastRewardTime");
			}
			return ET.ATTR;
		}
	}
	public class EmailInfoEntity
	{
		public const string EmailInfoChange = "EmailInfoChange";
		public System.Int64 emailIndex;
		public System.Int64 emailId;
		public System.Int64 sendTime;
		public System.Int64 status;
		public System.Boolean takeEnclosure;
		public System.Boolean isCollect;
		public SprotoType.CollectReport resourceCollectReport;
		public SprotoType.BattleReport battleReport;
		public System.Int64 subType;
		public SprotoType.RewardInfo rewards;

		public static HashSet<string> updateEntity(EmailInfoEntity et ,SprotoType.EmailInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasEmailIndex){
				et.emailIndex = data.emailIndex;
				if(ret)ET.ATTR.Add("emailIndex");
			}
			if(data.HasEmailId){
				et.emailId = data.emailId;
				if(ret)ET.ATTR.Add("emailId");
			}
			if(data.HasSendTime){
				et.sendTime = data.sendTime;
				if(ret)ET.ATTR.Add("sendTime");
			}
			if(data.HasStatus){
				et.status = data.status;
				if(ret)ET.ATTR.Add("status");
			}
			if(data.HasTakeEnclosure){
				et.takeEnclosure = data.takeEnclosure;
				if(ret)ET.ATTR.Add("takeEnclosure");
			}
			if(data.HasIsCollect){
				et.isCollect = data.isCollect;
				if(ret)ET.ATTR.Add("isCollect");
			}
			if(data.HasResourceCollectReport){
				et.resourceCollectReport = data.resourceCollectReport;
				if(ret)ET.ATTR.Add("resourceCollectReport");
			}
			if(data.HasBattleReport){
				et.battleReport = data.battleReport;
				if(ret)ET.ATTR.Add("battleReport");
			}
			if(data.HasSubType){
				et.subType = data.subType;
				if(ret)ET.ATTR.Add("subType");
			}
			if(data.HasRewards){
				et.rewards = data.rewards;
				if(ret)ET.ATTR.Add("rewards");
			}
			return ET.ATTR;
		}
	}
	public class HeroInfoEntity
	{
		public const string HeroInfoChange = "HeroInfoChange";
		public System.Int64 heroId;
		public System.Int64 star;
		public System.Int64 starExp;
		public System.Int64 level;
		public System.Int64 exp;
		public System.Int64 summonTime;
		public System.Int64 soldierKillNum;
		public System.Int64 savageKillNum;
		public System.Collections.Generic.List<SprotoType.SkillInfo> skills;

		public static HashSet<string> updateEntity(HeroInfoEntity et ,SprotoType.HeroInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasHeroId){
				et.heroId = data.heroId;
				if(ret)ET.ATTR.Add("heroId");
			}
			if(data.HasStar){
				et.star = data.star;
				if(ret)ET.ATTR.Add("star");
			}
			if(data.HasStarExp){
				et.starExp = data.starExp;
				if(ret)ET.ATTR.Add("starExp");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasExp){
				et.exp = data.exp;
				if(ret)ET.ATTR.Add("exp");
			}
			if(data.HasSummonTime){
				et.summonTime = data.summonTime;
				if(ret)ET.ATTR.Add("summonTime");
			}
			if(data.HasSoldierKillNum){
				et.soldierKillNum = data.soldierKillNum;
				if(ret)ET.ATTR.Add("soldierKillNum");
			}
			if(data.HasSavageKillNum){
				et.savageKillNum = data.savageKillNum;
				if(ret)ET.ATTR.Add("savageKillNum");
			}
			if(data.HasSkills){
				et.skills = data.skills;
				if(ret)ET.ATTR.Add("skills");
			}
			return ET.ATTR;
		}
	}
	public class ItemInfoEntity
	{
		public const string ItemInfoChange = "ItemInfoChange";
		public System.Int64 itemIndex;
		public System.Int64 itemId;
		public System.Int64 overlay;

		public static HashSet<string> updateEntity(ItemInfoEntity et ,SprotoType.ItemInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasItemIndex){
				et.itemIndex = data.itemIndex;
				if(ret)ET.ATTR.Add("itemIndex");
			}
			if(data.HasItemId){
				et.itemId = data.itemId;
				if(ret)ET.ATTR.Add("itemId");
			}
			if(data.HasOverlay){
				et.overlay = data.overlay;
				if(ret)ET.ATTR.Add("overlay");
			}
			return ET.ATTR;
		}
	}
	public class MarchTargetArgEntity
	{
		public const string MarchTargetArgChange = "MarchTargetArgChange";
		public SprotoType.PosInfo pos;
		public System.Int64 targetObjectIndex;

		public static HashSet<string> updateEntity(MarchTargetArgEntity et ,SprotoType.MarchTargetArg data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasTargetObjectIndex){
				et.targetObjectIndex = data.targetObjectIndex;
				if(ret)ET.ATTR.Add("targetObjectIndex");
			}
			return ET.ATTR;
		}
	}
	public class ResourcePosInfoEntity
	{
		public const string ResourcePosInfoChange = "ResourcePosInfoChange";
		public System.Int64 resourceLevel;
		public SprotoType.PosInfo pos;

		public static HashSet<string> updateEntity(ResourcePosInfoEntity et ,SprotoType.Map_SearchResource.response.ResourcePosInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasResourceLevel){
				et.resourceLevel = data.resourceLevel;
				if(ret)ET.ATTR.Add("resourceLevel");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			return ET.ATTR;
		}
	}
	public class ResourceCollectInfoEntity
	{
		public const string ResourceCollectInfoChange = "ResourceCollectInfoChange";
		public System.Int64 type;
		public System.Int64 level;
		public SprotoType.PosInfo pos;
		public System.Int64 load;
		public System.Int64 resourceId;

		public static HashSet<string> updateEntity(ResourceCollectInfoEntity et ,SprotoType.RoleInfo.ResourceCollectInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasType){
				et.type = data.type;
				if(ret)ET.ATTR.Add("type");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasLoad){
				et.load = data.load;
				if(ret)ET.ATTR.Add("load");
			}
			if(data.HasResourceId){
				et.resourceId = data.resourceId;
				if(ret)ET.ATTR.Add("resourceId");
			}
			return ET.ATTR;
		}
	}
	public class CollectSpeedInfoEntity
	{
		public const string CollectSpeedInfoChange = "CollectSpeedInfoChange";
		public System.Int64 collectSpeed;
		public System.Int64 collectTime;

		public static HashSet<string> updateEntity(CollectSpeedInfoEntity et ,SprotoType.RoleInfo.CollectSpeedInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasCollectSpeed){
				et.collectSpeed = data.collectSpeed;
				if(ret)ET.ATTR.Add("collectSpeed");
			}
			if(data.HasCollectTime){
				et.collectTime = data.collectTime;
				if(ret)ET.ATTR.Add("collectTime");
			}
			return ET.ATTR;
		}
	}
	public class StatisticsEntity
	{
		public const string StatisticsChange = "StatisticsChange";
		public System.Int64 arg;
		public System.Int64 num;

		public static HashSet<string> updateEntity(StatisticsEntity et ,SprotoType.RoleInfo.Statistics data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasArg){
				et.arg = data.arg;
				if(ret)ET.ATTR.Add("arg");
			}
			if(data.HasNum){
				et.num = data.num;
				if(ret)ET.ATTR.Add("num");
			}
			return ET.ATTR;
		}
	}
	public class RoleInfoEntity
	{
		public const string RoleInfoChange = "RoleInfoChange";
		public System.Int64 rid;
		public System.String name;
		public SprotoType.PosInfo pos;
		public System.Int64 country;
		public System.Int64 headId;
		public System.Int64 level;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.QueueInfo> buildQueue;
		public System.Int64 actionForce;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> soldiers;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.ArmyInfo> armies;
		public System.Int64 food;
		public System.Int64 wood;
		public System.Int64 stone;
		public System.Int64 gold;
		public System.Int64 denar;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.QueueInfo> armyQueue;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.TechnologyInfo> technologies;
		public System.Int64 buildVersion;
		public System.Int64 mainLineTaskId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.FinishTaskInfo> finishSideTasks;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.TaskStatistics> taskStatisticsSum;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.TaskStatistics> taskStatisticsDaily;
		public SprotoType.QueueInfo technologyQueue;
		public SprotoType.QueueInfo treatmentQueue;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo> seriousInjured;
		public System.Int64 historyPower;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.RoleStatistics> roleStatistics;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.SoldierKillInfo> soldierKills;
		public System.Int64 chapterId;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.ChapterTaskInfo> chapterTasks;
		public System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.DenseFogInfo> denseFog;
		public System.Int64 serverTime;

		public static HashSet<string> updateEntity(RoleInfoEntity et ,SprotoType.RoleInfo data,bool ret = false){
			if(ret)ET.ATTR.Clear();
			if(data.HasRid){
				et.rid = data.rid;
				if(ret)ET.ATTR.Add("rid");
			}
			if(data.HasName){
				et.name = data.name;
				if(ret)ET.ATTR.Add("name");
			}
			if(data.HasPos){
				et.pos = data.pos;
				if(ret)ET.ATTR.Add("pos");
			}
			if(data.HasCountry){
				et.country = data.country;
				if(ret)ET.ATTR.Add("country");
			}
			if(data.HasHeadId){
				et.headId = data.headId;
				if(ret)ET.ATTR.Add("headId");
			}
			if(data.HasLevel){
				et.level = data.level;
				if(ret)ET.ATTR.Add("level");
			}
			if(data.HasBuildQueue){

				if (et.buildQueue == null) {
					 et.buildQueue = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.QueueInfo>();
				}
				foreach(var item in data.buildQueue){ 
					if(et.buildQueue.ContainsKey(item.Key)){
						et.buildQueue[item.Key] = item.Value;
					}else{
						et.buildQueue.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("buildQueue"); 
			}
			if(data.HasActionForce){
				et.actionForce = data.actionForce;
				if(ret)ET.ATTR.Add("actionForce");
			}
			if(data.HasSoldiers){

				if (et.soldiers == null) {
					 et.soldiers = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.soldiers){ 
					if(et.soldiers.ContainsKey(item.Key)){
						et.soldiers[item.Key] = item.Value;
					}else{
						et.soldiers.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldiers"); 
			}
			if(data.HasArmies){

				if (et.armies == null) {
					 et.armies = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.ArmyInfo>();
				}
				foreach(var item in data.armies){ 
					if(et.armies.ContainsKey(item.Key)){
						et.armies[item.Key] = item.Value;
					}else{
						et.armies.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("armies"); 
			}
			if(data.HasFood){
				et.food = data.food;
				if(ret)ET.ATTR.Add("food");
			}
			if(data.HasWood){
				et.wood = data.wood;
				if(ret)ET.ATTR.Add("wood");
			}
			if(data.HasStone){
				et.stone = data.stone;
				if(ret)ET.ATTR.Add("stone");
			}
			if(data.HasGold){
				et.gold = data.gold;
				if(ret)ET.ATTR.Add("gold");
			}
			if(data.HasDenar){
				et.denar = data.denar;
				if(ret)ET.ATTR.Add("denar");
			}
			if(data.HasArmyQueue){

				if (et.armyQueue == null) {
					 et.armyQueue = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.QueueInfo>();
				}
				foreach(var item in data.armyQueue){ 
					if(et.armyQueue.ContainsKey(item.Key)){
						et.armyQueue[item.Key] = item.Value;
					}else{
						et.armyQueue.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("armyQueue"); 
			}
			if(data.HasTechnologies){

				if (et.technologies == null) {
					 et.technologies = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.TechnologyInfo>();
				}
				foreach(var item in data.technologies){ 
					if(et.technologies.ContainsKey(item.Key)){
						et.technologies[item.Key] = item.Value;
					}else{
						et.technologies.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("technologies"); 
			}
			if(data.HasBuildVersion){
				et.buildVersion = data.buildVersion;
				if(ret)ET.ATTR.Add("buildVersion");
			}
			if(data.HasMainLineTaskId){
				et.mainLineTaskId = data.mainLineTaskId;
				if(ret)ET.ATTR.Add("mainLineTaskId");
			}
			if(data.HasFinishSideTasks){

				if (et.finishSideTasks == null) {
					 et.finishSideTasks = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.FinishTaskInfo>();
				}
				foreach(var item in data.finishSideTasks){ 
					if(et.finishSideTasks.ContainsKey(item.Key)){
						et.finishSideTasks[item.Key] = item.Value;
					}else{
						et.finishSideTasks.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("finishSideTasks"); 
			}
			if(data.HasTaskStatisticsSum){

				if (et.taskStatisticsSum == null) {
					 et.taskStatisticsSum = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.TaskStatistics>();
				}
				foreach(var item in data.taskStatisticsSum){ 
					if(et.taskStatisticsSum.ContainsKey(item.Key)){
						et.taskStatisticsSum[item.Key] = item.Value;
					}else{
						et.taskStatisticsSum.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("taskStatisticsSum"); 
			}
			if(data.HasTaskStatisticsDaily){

				if (et.taskStatisticsDaily == null) {
					 et.taskStatisticsDaily = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.TaskStatistics>();
				}
				foreach(var item in data.taskStatisticsDaily){ 
					if(et.taskStatisticsDaily.ContainsKey(item.Key)){
						et.taskStatisticsDaily[item.Key] = item.Value;
					}else{
						et.taskStatisticsDaily.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("taskStatisticsDaily"); 
			}
			if(data.HasTechnologyQueue){
				et.technologyQueue = data.technologyQueue;
				if(ret)ET.ATTR.Add("technologyQueue");
			}
			if(data.HasTreatmentQueue){
				et.treatmentQueue = data.treatmentQueue;
				if(ret)ET.ATTR.Add("treatmentQueue");
			}
			if(data.HasSeriousInjured){

				if (et.seriousInjured == null) {
					 et.seriousInjured = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.SoldierInfo>();
				}
				foreach(var item in data.seriousInjured){ 
					if(et.seriousInjured.ContainsKey(item.Key)){
						et.seriousInjured[item.Key] = item.Value;
					}else{
						et.seriousInjured.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("seriousInjured"); 
			}
			if(data.HasHistoryPower){
				et.historyPower = data.historyPower;
				if(ret)ET.ATTR.Add("historyPower");
			}
			if(data.HasRoleStatistics){

				if (et.roleStatistics == null) {
					 et.roleStatistics = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.RoleStatistics>();
				}
				foreach(var item in data.roleStatistics){ 
					if(et.roleStatistics.ContainsKey(item.Key)){
						et.roleStatistics[item.Key] = item.Value;
					}else{
						et.roleStatistics.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("roleStatistics"); 
			}
			if(data.HasSoldierKills){

				if (et.soldierKills == null) {
					 et.soldierKills = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.SoldierKillInfo>();
				}
				foreach(var item in data.soldierKills){ 
					if(et.soldierKills.ContainsKey(item.Key)){
						et.soldierKills[item.Key] = item.Value;
					}else{
						et.soldierKills.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("soldierKills"); 
			}
			if(data.HasChapterId){
				et.chapterId = data.chapterId;
				if(ret)ET.ATTR.Add("chapterId");
			}
			if(data.HasChapterTasks){

				if (et.chapterTasks == null) {
					 et.chapterTasks = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.ChapterTaskInfo>();
				}
				foreach(var item in data.chapterTasks){ 
					if(et.chapterTasks.ContainsKey(item.Key)){
						et.chapterTasks[item.Key] = item.Value;
					}else{
						et.chapterTasks.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("chapterTasks"); 
			}
			if(data.HasDenseFog){

				if (et.denseFog == null) {
					 et.denseFog = new System.Collections.Generic.Dictionary<System.Int64,SprotoType.RoleInfo.DenseFogInfo>();
				}
				foreach(var item in data.denseFog){ 
					if(et.denseFog.ContainsKey(item.Key)){
						et.denseFog[item.Key] = item.Value;
					}else{
						et.denseFog.Add(item.Key, item.Value);
					}
				}
				ET.ATTR.Add("denseFog"); 
			}
			if(data.HasServerTime){
				et.serverTime = data.serverTime;
				if(ret)ET.ATTR.Add("serverTime");
			}
			return ET.ATTR;
		}
	}
}

