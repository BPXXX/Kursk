// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "stdafx.h"

#include "Plugin.h"

extern "C" __declspec(dllexport) IPlugin *LoadPlugin()
{
	return new CPlugin();
}

extern "C" __declspec(dllexport) void UnloadPlugin()
{
}

