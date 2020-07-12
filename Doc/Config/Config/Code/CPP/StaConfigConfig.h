#pragma once
#include <vector>
#include "Global.h"

class StaConfigDefine
{
public:
 	/// <summary> 
 	/// 游戏参数轻型坦克动作的缓冲时间
	/// </summary>
	int sens_lite;
 	/// <summary> 
 	/// 游戏参数中型坦克动作的缓冲时间
	/// </summary>
	int sens_mid;
 	/// <summary> 
 	/// 游戏参数重型坦克动作的缓冲时间
	/// </summary>
	int sens_heavy;
 	/// <summary> 
 	/// 游戏参数炮弹飞行的速度
	/// </summary>
	int bullet_speed;
 	/// <summary> 
 	/// 游戏参数召唤重型坦克的冷却时间
	/// </summary>
	int heavy_cooldown;
 	/// <summary> 
 	/// 游戏参数玩家视角中队友的坦克隐藏时的透明度
	/// </summary>
	int hide_alpha;
 	/// <summary> 
 	/// 游戏参数高爆弹命中轻型坦克正面时的伤害值
	/// </summary>
	int expbullet_to_lite_front;
 	/// <summary> 
 	/// 游戏参数高爆弹命中轻型坦克侧面时的伤害值
	/// </summary>
	int expbullet_to_lite_side;
 	/// <summary> 
 	/// 游戏参数高爆弹命中轻型坦克背面时的伤害值
	/// </summary>
	int expbullet_to_lite_back;
 	/// <summary> 
 	/// 游戏参数高爆弹命中中型坦克正面时的伤害值
	/// </summary>
	int expbullet_to_mid_front;
 	/// <summary> 
 	/// 游戏参数高爆弹命中中型坦克侧面时的伤害值
	/// </summary>
	int expbullet_to_mid_side;
 	/// <summary> 
 	/// 游戏参数高爆弹命中中型坦克背面时的伤害值
	/// </summary>
	int expbullet_to_mid_back;
 	/// <summary> 
 	/// 游戏参数高爆弹命中重型坦克正面时的伤害值
	/// </summary>
	int expbullet_to_heavy_front;
 	/// <summary> 
 	/// 游戏参数高爆弹命中重型坦克侧面时的伤害值
	/// </summary>
	int expbullet_to_heavy_side;
 	/// <summary> 
 	/// 游戏参数高爆弹命中重型坦克背面时的伤害值
	/// </summary>
	int expbullet_to_heavy_back;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中轻型坦克正面时的伤害值
	/// </summary>
	int pierbullet_to_lite_front;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中轻型坦克侧面时的伤害值
	/// </summary>
	int pierbullet_to_lite_side;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中轻型坦克背面时的伤害值
	/// </summary>
	int pierbullet_to_lite_back;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中中型坦克正面时的伤害值
	/// </summary>
	int pierbullet_to_mid_front;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中中型坦克侧面时的伤害值
	/// </summary>
	int pierbullet_to_mid_side;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中中型坦克背面时的伤害值
	/// </summary>
	int pierbullet_to_mid_back;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中重型坦克正面时的伤害值
	/// </summary>
	int pierbullet_to_heavy_front;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中重型坦克侧面时的伤害值
	/// </summary>
	int pierbullet_to_heavy_side;
 	/// <summary> 
 	/// 游戏参数穿甲弹命中重型坦克背面时的伤害值
	/// </summary>
	int pierbullet_to_heavy_back;
 	/// <summary> 
 	/// 游戏参数烟雾弹扩散的地图块数量
	/// </summary>
	int smogbullet_spread;
 	/// <summary> 
 	/// 游戏参数烟雾弹的持续时间
	/// </summary>
	int smogbullet_time;
 	/// <summary> 
 	/// 游戏参数烟雾弹的冷却时间
	/// </summary>
	int smogbullet_cooldown;
 	/// <summary> 
 	/// 游戏参数灭火器的冷却时间
	/// </summary>
	int exitin_cooldown;
 	/// <summary> 
 	/// 游戏参数维修工具的冷却时间
	/// </summary>
	int fixtool_cooldown;
 	/// <summary> 
 	/// 游戏参数灭火器掉落的概率
	/// </summary>
	int extin_rate;
 	/// <summary> 
 	/// 游戏参数维修工具掉落的概率
	/// </summary>
	int fixtool_rate;
 	/// <summary> 
 	/// 游戏参数烟雾弹掉落的概率
	/// </summary>
	int smogbullet_rate;
 	/// <summary> 
 	/// 游戏参数医疗工具掉落的概率
	/// </summary>
	int medp_rate;
 	/// <summary> 
 	/// 游戏参数坦克被高爆弹击中后“着火”的概率
	/// </summary>
	int fired_ratio;
 	/// <summary> 
 	/// 游戏参数坦克“着火”时，每秒钟减少的血量
	/// </summary>
	int fired_power;
 	/// <summary> 
 	/// 游戏参数“着火”状态的持续时间
	/// </summary>
	int fired_time;
 	/// <summary> 
 	/// 游戏参数坦克“零件损毁”时，缓冲时间和装填时间上升的倍率
	/// </summary>
	int shutdown_rate;
 	/// <summary> 
 	/// 游戏参数坦克被穿甲弹击中后“零件损毁”的概率
	/// </summary>
	int shutdown_power;
};