#pragma once
#include <vector>
#include "Global.h"

class TankDefine
{
public:
 	/// <summary> 
 	/// 编号
	/// </summary>
	int ID;
 	/// <summary> 
 	/// 最大血量
	/// </summary>
	int HP;
 	/// <summary> 
 	///  车前护甲值
	/// </summary>
	int Armor_front;
 	/// <summary> 
 	///  侧身护甲值 
	/// </summary>
	int  Armor_side;
 	/// <summary> 
 	/// 背面护甲值
	/// </summary>
	int  Armor_back;
 	/// <summary> 
 	/// 炮弹种类
	/// </summary>
	const char*  Mag;
 	/// <summary> 
 	/// 坦克类型
	/// </summary>
	const char* TankKind;
 	/// <summary> 
 	///  坦克系列
	/// </summary>
	const char* TankSerial;
 	/// <summary> 
 	///  坦克速度
	/// </summary>
	int Speed;
 	/// <summary> 
 	/// 道具携带数 
	/// </summary>
	std::vector<int> MaxItem;
 	/// <summary> 
 	///  射击僵直
	/// </summary>
	int ShootGap;
 	/// <summary> 
 	/// 移动僵直
	/// </summary>
	int  MoveGap;
 	/// <summary> 
 	/// 装填时间
	/// </summary>
	int ReloadGap;
};