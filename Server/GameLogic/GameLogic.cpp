// PluginTemplate.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "GameLogic.h"
#include "LoginModule.h"

IPlugin *g_pGameLogic;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_pGameLogic = new GameLogic(pm);
	pm->Registered(g_pGameLogic);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_pGameLogic);
}

GameLogic::GameLogic(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void GameLogic::Install()
{
	m_pPluginManager->AddModule<LoginModule>(new LoginModule(m_pPluginManager));
}

void GameLogic::Uninstall()
{
	m_pPluginManager->RemoveModule<LoginModule>();
}
