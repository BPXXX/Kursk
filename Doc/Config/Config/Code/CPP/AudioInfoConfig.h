#pragma once
#include <vector>
#include "Global.h"

class AudioInfoDefine
{
public:
 	/// <summary> 
 	/// 声音事件ID
	/// </summary>
	int ID;
 	/// <summary> 
 	/// 声音事件名称
	/// </summary>
	const char* name;
 	/// <summary> 
 	/// 分组
	/// </summary>
	int group;
 	/// <summary> 
 	/// 播放模式
	/// </summary>
	int mode;
 	/// <summary> 
 	/// 同时播放数
	/// </summary>
	int maxCount;
 	/// <summary> 
 	/// 同时播放上限处理
	/// </summary>
	int maxCountAction;
 	/// <summary> 
 	/// 间隔时间
	/// </summary>
	std::vector<float> interval;
 	/// <summary> 
 	/// 音量
	/// </summary>
	float volume;
 	/// <summary> 
 	/// 淡出时间
	/// </summary>
	float fadeTime;
 	/// <summary> 
 	/// 3D音效
	/// </summary>
	int config3D;
 	/// <summary> 
 	/// 音效文件
	/// </summary>
	std::vector<const char*> audios;
};