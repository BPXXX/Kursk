#pragma once
#include <vector>
#include "Global.h"

class ItemDefine
{
public:
 	/// <summary> 
 	/// 编号
	/// </summary>
	int id;
 	/// <summary> 
 	/// 道具类型
	/// </summary>
	int type;
 	/// <summary> 
 	/// 道具名称
	/// </summary>
	const char* name;
 	/// <summary> 
 	/// 道具品质
	/// </summary>
	int quality;
};