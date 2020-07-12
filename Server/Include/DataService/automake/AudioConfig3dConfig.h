#pragma once
#include <vector>
#include "Global.h"

class AudioConfig3dDefine
{
public:
 	/// <summary> 
 	/// 编号
	/// </summary>
	int ID;
 	/// <summary> 
 	/// 组名称
	/// </summary>
	const char* name;
	float dopplerLevel;
 	/// <summary> 
 	/// 最小距离
	/// </summary>
	float minDistance;
 	/// <summary> 
 	/// 最大距离
	/// </summary>
	float maxDistance;
	float spatialBlend;
	int spread;
	const char* curve;
};