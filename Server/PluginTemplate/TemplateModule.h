#pragma once
#include <ITemplateModule.h>
#include <IPluginManager.h>
#include <INetServer.h>

class TemplateModule :
	public ITemplateModule
{
public:
	TemplateModule(IPluginManager *pluginManager);
	~TemplateModule();

	// 通过 IModule 继承
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;

	// 通过 ITemplateModule 继承
	virtual void SayHellow() override;

	void OnClientConnected(uint64_t nClientID);
	void OnClientLeave(uint64_t nClientID);

private:
	IPluginManager *m_pluginManager;
	uint64_t m_sayTime;

	INetServer *m_pNetServer;
};

