// DataPlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "DataPlugin.h"
#include "DataService.h"

IPlugin *g_DataPlugin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_DataPlugin = new DataPlugin(pm);
	pm->Registered(g_DataPlugin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_DataPlugin);
}

DataPlugin::DataPlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void DataPlugin::Install()
{
	m_pPluginManager->AddModule<IDataService>(new DataService(m_pPluginManager));
}

void DataPlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<IDataService>();
}
