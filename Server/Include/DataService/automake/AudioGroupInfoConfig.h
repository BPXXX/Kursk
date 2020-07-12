#pragma once
#include <vector>
#include "Global.h"

class AudioGroupInfoDefine
{
public:
 	/// <summary> 
 	/// 声音事件组ID
	/// </summary>
	int ID;
 	/// <summary> 
 	/// 组名称
	/// </summary>
	const char* name;
 	/// <summary> 
 	/// 音量
	/// </summary>
	float baseVolume;
 	/// <summary> 
 	/// 同时播放数
	/// </summary>
	int maxCount;
 	/// <summary> 
 	/// 同时播放上限处理
	/// </summary>
	int maxCountAction;
 	/// <summary> 
 	/// 是否为背景音乐
	/// </summary>
	int bgm;
 	/// <summary> 
 	/// 修改的音量组
	/// </summary>
	std::vector<int> modifyGroup;
 	/// <summary> 
 	/// 修正的音量
	/// </summary>
	std::vector<float> modifyVolume;
};