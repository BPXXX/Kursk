// MySQLPlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "MySQLPlugin.h"
#include "MySQLService.h"

IPlugin *g_MySQLPlugin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_MySQLPlugin = new MySQLPlugin(pm);
	pm->Registered(g_MySQLPlugin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_MySQLPlugin);
}

MySQLPlugin::MySQLPlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void MySQLPlugin::Install()
{
	m_pPluginManager->AddModule<ISQLService>(new MySQLService(m_pPluginManager));
}

void MySQLPlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<ISQLService>();
}
