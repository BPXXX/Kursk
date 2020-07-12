// DataBasePlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "DataBasePlugin.h"
#include "DataBaseModule.h"

IPlugin *g_DataBasePlugin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_DataBasePlugin = new DataBasePlugin(pm);
	pm->Registered(g_DataBasePlugin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_DataBasePlugin);
}

DataBasePlugin::DataBasePlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void DataBasePlugin::Install()
{
	m_pPluginManager->AddModule<IDataBaseModule>(new DataBaseModule(m_pPluginManager));
}

void DataBasePlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<IDataBaseModule>();
}
