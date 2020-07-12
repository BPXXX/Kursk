#pragma once
#include <vector>
#include "Global.h"

class LoginConfigDefine
{
public:
 	/// <summary> 
 	/// 程序使用
	/// </summary>
	int id;
 	/// <summary> 
 	/// 创角登录服务器IP
	/// </summary>
	const char* serverIP;
 	/// <summary> 
 	/// 创角登录用于初始创角登陆,代表轻型坦克的血量
	/// </summary>
	int health_lite;
 	/// <summary> 
 	/// 创角登录用于初始创角登陆，代表中型坦克的血量
	/// </summary>
	int health_mid;
 	/// <summary> 
 	/// 创角登录用于初始创角登陆，代表重型坦克的血量
	/// </summary>
	int health_heavy;
 	/// <summary> 
 	/// 创角登录用于初始创角登陆，轻型坦克的装填时间
	/// </summary>
	int load_lite;
 	/// <summary> 
 	/// 创角登录用于初始创角登陆，中型坦克的装填时间
	/// </summary>
	int load_mid;
 	/// <summary> 
 	/// 创角登录用于初始创角登陆，重型坦克的装填时间
	/// </summary>
	int load_heavy;
};