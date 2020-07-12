#pragma once

#include <IPluginManager.h>
#include "DynLib.h"

#include <set>
#include <unordered_map>
#include <iostream>

class PluginManager : IPluginManager
{
public:
	PluginManager(const std::string &appName);
	// Í¨¹ý IPluginManager ¼Ì³Ð
	virtual bool LoadPlugin() override;
	virtual bool UnLoadPlugin() override;
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;
	virtual void Registered(IPlugin * plugin) override;
	virtual void UnRegistered(IPlugin * plugin) override;
	virtual void AddModule(const std::string & name, IModule * pModule) override;
	virtual IModule* FindModule(const std::string &name) override;
	virtual void RemoveModule(const std::string & name) override;

private:

	typedef void(*DLL_START_PLUGIN_FUNC)(IPluginManager* pm);
	typedef void(*DLL_STOP_PLUGIN_FUNC)(IPluginManager* pm);

	std::string m_strAppName;
	std::set<IPlugin*> m_setPlugins;
	std::unordered_map<std::string, IModule*> m_mapModules;
	std::unordered_map<std::string, DynLib*> m_mapPluginLibs;
};
