// GameServer.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include "pch.h"
#include "PluginManager.h"
#include <thread>
#include <chrono>

int main(int argc, char *args[])
{
	const char *pName = "";
	if (argc > 1)
	{
		pName = args[1];
	}

	PluginManager *pPluginMgr = new PluginManager(pName);


	if (!pPluginMgr->LoadPlugin())
	{
		return -1;
	}
	if (!pPluginMgr->Init())
	{
		return -1;
	}
	while (1)
	{
		if (!pPluginMgr->Update())
			break;
		std::this_thread::sleep_for(std::chrono::milliseconds(1));
	}
	pPluginMgr->Shut();
	pPluginMgr->UnLoadPlugin();
}