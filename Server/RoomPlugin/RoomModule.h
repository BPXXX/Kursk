#pragma once
#include <IRoomModule.h>
#include <IPluginManager.h>
#include <INetServer.h>
#include <IDataBaseModule.h>
#include <SQLService/ISQLService.h>

class RoomModule :
	public IRoomModule
{
public:
	RoomModule(IPluginManager *pluginManager);
	~RoomModule();

	// Í¨¹ý IModule ¼Ì³Ð
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;



	void OnUserRegister(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserLogin(const uint64_t nClientID, const uint32_t nMsgID, const char* msg1, const uint32_t nLen);
	void OnUserCreatRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserEnterRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserChangeState(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserExpel(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserStartGame(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);

	void OnUserGetRoomMen(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);

	void OnUserExitRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);

	void OnClientConnected(uint64_t nClientID);
	void OnClientLeave(uint64_t nClientID);

private:
	IPluginManager *m_pluginManager;
	uint64_t m_sayTime;
	IDataBaseModule *m_pDataBase;
	INetServer *m_pNetServer;
	ISQLService *sqlService;
};

