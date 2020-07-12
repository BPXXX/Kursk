#pragma once
#include <vector>
#include "Global.h"

class MapDefine
{
public:
 	/// <summary> 
 	/// 编号
	/// </summary>
	int ID;
 	/// <summary> 
 	/// 地图宽度
	/// </summary>
	int Width;
 	/// <summary> 
 	/// 地图长度
	/// </summary>
	int Length;
 	/// <summary> 
 	/// 客户端间距 
	/// </summary>
	int ClientDistance;
 	/// <summary> 
 	/// 瓦片尺寸
	/// </summary>
	int MapTileSize;
 	/// <summary> 
 	/// 据点
	/// </summary>
	std::vector<Point2> PointVec;
};