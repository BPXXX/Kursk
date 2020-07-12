#pragma once

#include <IPlugin.h>
#include <IPluginManager.h>

class GameLogic : public IPlugin
{
public:
	GameLogic(IPluginManager* pluginManager);
	// 通过 IPlugin 继承
	virtual void Install() override;
	virtual void Uninstall() override;

private:
	IPluginManager *m_pPluginManager;
};
