// LogicPlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "LogicPlugin.h"
#include "PlayModule.h"

IPlugin *g_LogicPlugin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_LogicPlugin = new LogicPlugin(pm);
	pm->Registered(g_LogicPlugin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_LogicPlugin);
}

LogicPlugin::LogicPlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void LogicPlugin::Install()
{
	m_pPluginManager->AddModule<IPlayModule>(new PlayModule(m_pPluginManager));
}

void LogicPlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<IPlayModule>();
}
