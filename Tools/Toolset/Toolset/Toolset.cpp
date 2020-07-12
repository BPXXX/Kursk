// Toolset.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include "pch.h"

#include <iostream>
#include "../Plugins/IPlugin.h"
#include "DynLib.h"
#include <time.h>

typedef IPlugin* LoadPlugin();
typedef void UnloadPlugin();

int main(int argc, char* argv[])
{
	if (argc < 2)
	{
		return 0;
	}

	DynLib *pLib = new DynLib(argv[1]);
	IPlugin *pPlugin = NULL;
	if (pLib->Load())
	{
		LoadPlugin *pFun = (LoadPlugin*)pLib->GetSymbol("LoadPlugin");
		DWORD dw = GetLastError();
		if (pFun)
		{
			pPlugin = (*pFun)();
		}
	}

	if (pPlugin)
	{
		try
		{
			pPlugin->ParseArgv(argc - 1, &argv[1]);
		}
		catch(...)
		{

		}

		auto t = time(0);
		pPlugin->Run();
		std::cout << "耗时:" << (time(0) - t) / 1000.0f << "s" << std::endl;
	}

	pLib->UnLoad();

	delete pLib;

	system("pause");

	return 0;

    std::cout << "Hello World!\n"; 
}