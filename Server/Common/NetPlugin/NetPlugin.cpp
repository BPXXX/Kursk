// NetPlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "NetPlugin.h"
#include "NetServer.h"

IPlugin *g_NetPlguin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_NetPlguin = new NetPlugin(pm);
	pm->Registered(g_NetPlguin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_NetPlguin);
}

NetPlugin::NetPlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void NetPlugin::Install()
{
	m_pPluginManager->AddModule<INetServer>(new NetServer(m_pPluginManager));
}

void NetPlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<INetServer>();
}
