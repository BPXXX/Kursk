// ChatPlugin.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "ChatPlugin.h"
#include "ChatModule.h"

IPlugin *g_ChatPlugin;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_ChatPlugin = new ChatPlugin(pm);
	pm->Registered(g_ChatPlugin);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_ChatPlugin);
}

ChatPlugin::ChatPlugin(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void ChatPlugin::Install()
{
	m_pPluginManager->AddModule<IChatModule>(new ChatModule(m_pPluginManager));
}

void ChatPlugin::Uninstall()
{
	m_pPluginManager->RemoveModule<IChatModule>();
}
