// SQLitePlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "SQLitePlugin.h"
#include "SQLiteService.h"

IPlugin *g_SQLitePlugin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_SQLitePlugin = new SQLitePlugin(pm);
	pm->Registered(g_SQLitePlugin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_SQLitePlugin);
}

SQLitePlugin::SQLitePlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void SQLitePlugin::Install()
{
	m_pPluginManager->AddModule<ISQLService>(new SQLiteService(m_pPluginManager));
}

void SQLitePlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<ISQLService>();
}
