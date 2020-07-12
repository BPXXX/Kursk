#pragma once
#include <vector>
#include "Global.h"

class LanguageSetDefine
{
public:
 	/// <summary> 
 	/// 编号
	/// </summary>
	int ID;
 	/// <summary> 
 	/// 本机语言代码
	/// </summary>
	const char* language;
 	/// <summary> 
 	/// 语言包ID
	/// </summary>
	int l_languageID;
 	/// <summary> 
 	/// 手机默认语言编号
	/// </summary>
	std::vector<int> telephone;
 	/// <summary> 
 	/// 语言开关,1=开启，0 =关闭
	/// </summary>
	int enumSwitch;
 	/// <summary> 
 	/// 翻译语言代码
	/// </summary>
	const char* translate;
 	/// <summary> 
 	/// 游戏版本号
	/// </summary>
	const char* gameID;
};