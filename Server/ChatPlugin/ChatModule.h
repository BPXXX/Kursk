#pragma once
#include <IChatModule.h>
#include <IPluginManager.h>
#include <INetServer.h>
#include <IDataBaseModule.h>
#include <SQLService/ISQLService.h>

class ChatModule :
	public IChatModule
{
public:
	ChatModule(IPluginManager *pluginManager);
	~ChatModule();

	// 通过 IModule 继承
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;

	// 通过 IChatModel 继承


	//void OnUserLogin(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	//void OnUserInput(const uint64_t nClientID, const uint32_t nMsgID, const char* msg1, const uint32_t nLen);
	//void OnUserQunliao(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	//void OnUserExitQunliao(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	//void OnUserJinyan(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen); 
	//void OnUserQuxiaoJinyan(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	//void OnCreateHouse(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	//void OnEnterHouse(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnFANGJIANLT(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	//void OnFANGJIANEXIT(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	//void OnUserSiliao(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	//void OnUserTiren(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnClientConnected(uint64_t nClientID);
	void OnClientLeave(uint64_t nClientID);

private:
	IPluginManager *m_pluginManager;
	uint64_t m_sayTime;
	IDataBaseModule *m_pDataBase;
	INetServer *m_pNetServer;
	ISQLService *sqlService;
};

