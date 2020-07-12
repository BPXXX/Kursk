#include "PluginManager.h"
#include "IPlugin.h"
#include "tinyxml2/tinyxml2.h"
#include <assert.h>

PluginManager::PluginManager(const std::string &appName)
	: m_strAppName(appName)
{

}
bool PluginManager::LoadPlugin()
{
	tinyxml2::XMLDocument doc;
	if (doc.LoadFile("Plugin.xml") != tinyxml2::XML_SUCCESS)
	{
		std::cout << "read config error: Plugin.xml" << std::endl;
		return false;
	}

	auto root = doc.RootElement();

	const char *appName = 0;
	if (m_strAppName.length() > 0)
	{
		appName = m_strAppName.c_str();
	}
	auto plugins = root->FirstChildElement(appName);
	auto child = plugins->FirstChildElement();
	while (child)
	{
		std::string name = child->Attribute("Name");
		auto lib = new DynLib(name.c_str());
		if (lib->Load())
		{
			DLL_START_PLUGIN_FUNC pFunc = (DLL_START_PLUGIN_FUNC)lib->GetSymbol("DllStartPlugin");
			if (!pFunc)
			{
				std::cout << "Load DllStartPlugin Failure:" << name << std::endl;
				return false;
			}
			else
			{
				std::cout << "Load Plugin :" << name << std::endl;
			}
			pFunc(this);
			m_mapPluginLibs.insert(std::make_pair(name, lib));
		}
		else
		{
			std::cout << "Load PluginFailure:" << name << std::endl;
			return false;
		}
		child = child->NextSiblingElement();
	}

	return true;
}
bool PluginManager::UnLoadPlugin()
{
	for (auto itr : m_mapPluginLibs)
	{
		auto pLib = itr.second;
		DLL_STOP_PLUGIN_FUNC pFunc = (DLL_STOP_PLUGIN_FUNC)pLib->GetSymbol("DllStopPlugin");
		if (pFunc)
		{
			pFunc(this);
		}
		pLib->UnLoad();

		delete pLib;
	}

	m_mapPluginLibs.clear();

	return true;
}

bool PluginManager::Init()
{
	for (auto itr : m_mapModules)
	{
		if (!itr.second->Init())
			return false;
	}
	return true;
}

bool PluginManager::Update()
{
	for (auto itr : m_mapModules)
	{
		if (!itr.second->Update())
			return false;
	}
	return true;
}

bool PluginManager::Shut()
{
	for (auto itr : m_mapModules)
	{
		itr.second->Shut();
	}
	return false;
}

void PluginManager::Registered(IPlugin * plugin)
{
	m_setPlugins.insert(plugin);
	plugin->Install();
}

void PluginManager::UnRegistered(IPlugin * plugin)
{
	m_setPlugins.erase(plugin);
	plugin->Uninstall();
}

void PluginManager::AddModule(const std::string & name, IModule * pModule)
{
	if (!FindModule(name))
	{
		m_mapModules.insert(std::make_pair(name, pModule));
	}
}

IModule* PluginManager::FindModule(const std::string &name)
{
	auto itr = m_mapModules.find(name);
	if (itr != m_mapModules.end())
	{
		return itr->second;
	}
	return nullptr;
}

void PluginManager::RemoveModule(const std::string & name)
{
	auto itr = m_mapModules.find(name);
	if (itr != m_mapModules.end())
	{
		m_mapModules.erase(itr);
	}
}
