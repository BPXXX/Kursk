#pragma once
#include <ITemplateModule.h>
#include <IPluginManager.h>
#include <INetServer.h>
#include <SQLService/ISQLService.h>

#include <unordered_map>


class LoginModule :
	public IModule
{
public:
	LoginModule(IPluginManager *pluginManager);
	~LoginModule();

	// Í¨¹ý IModule ¼Ì³Ð
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;


	void OnClientConnected(uint64_t nClientID);
	void OnClientLeave(uint64_t nClientID);

private:
	IPluginManager *m_pluginManager;
	INetServer *m_pNetServer;
	ISQLService *m_pSQLService;
	int m_nMaxUserID;
};

