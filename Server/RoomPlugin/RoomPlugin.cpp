// RoomPlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "RoomPlugin.h"
#include "RoomModule.h"

IPlugin *g_RoomPlugin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_RoomPlugin = new RoomPlugin(pm);
	pm->Registered(g_RoomPlugin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_RoomPlugin);
}

RoomPlugin::RoomPlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void RoomPlugin::Install()
{
	m_pPluginManager->AddModule<IRoomModule>(new RoomModule(m_pPluginManager));
}

void RoomPlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<IRoomModule>();
}
