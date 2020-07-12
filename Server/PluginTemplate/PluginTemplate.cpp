// PluginTemplate.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "PluginTemplate.h"
#include "TemplateModule.h"

IPlugin *g_PlguinTemplate;

extern "C" __declspec(dllexport) void DllStartPlugin(IPluginManager* pm)
{
	g_PlguinTemplate = new PlguinTemplate(pm);
	pm->Registered(g_PlguinTemplate);
};

extern "C"  __declspec(dllexport) void DllStopPlugin(IPluginManager* pm)
{
	pm->UnRegistered(g_PlguinTemplate);
}

PlguinTemplate::PlguinTemplate(IPluginManager* pluginManager)
	: m_pPluginManager(pluginManager)
{
}

void PlguinTemplate::Install()
{
	m_pPluginManager->AddModule<ITemplateModule>(new TemplateModule(m_pPluginManager));
}

void PlguinTemplate::Uninstall()
{
	m_pPluginManager->RemoveModule<ITemplateModule>();
}
