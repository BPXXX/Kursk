// ODBCPlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "ODBCPlugin.h"
#include "ODBCService.h"

IPlugin *g_ODBCPlugin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_ODBCPlugin = new ODBCPlugin(pm);
	pm->Registered(g_ODBCPlugin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_ODBCPlugin);
}

ODBCPlugin::ODBCPlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void ODBCPlugin::Install()
{
	m_pPluginManager->AddModule<ISQLService>(new ODBCService(m_pPluginManager));
}

void ODBCPlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<ISQLService>();
}
