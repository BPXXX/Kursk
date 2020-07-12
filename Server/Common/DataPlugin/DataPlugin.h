#pragma once

#include <IPlugin.h>
#include <IPluginManager.h>

class DataPlugin : public IPlugin
{
public:
	DataPlugin(IPluginManager* pluginManager);
	// 通过 IPlugin 继承
	virtual void Install() override;
	virtual void Uninstall() override;

private:
	IPluginManager *m_pPluginManager;
};
