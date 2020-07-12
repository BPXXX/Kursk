#pragma once
#include <IPlayModule.h>
#include <IPluginManager.h>
#include <INetServer.h>
#include <IDataBaseModule.h>
#include <SQLService/ISQLService.h>
#include <DataService/IDataService.h>
class PlayModule :
	public IPlayModule
{
public:
	PlayModule(IPluginManager *pluginManager);
	~PlayModule();

	// 通过 IModule 继承
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;

	// 通过 IPlayModule 继承
	virtual void SayHellow() override;
	//void OnUserLogin(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnVecProc(const uint64_t nClientID, const uint32_t nMsgID, const char* msg1, const uint32_t nLen);
	void OnAmmoProc(const uint64_t nClientID, const uint32_t nMsgID, const char* msg1, const uint32_t nLen);
	void OnClientConnected(uint64_t nClientID);
	void OnUserStartGame(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserCreateRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserEnterRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserChangeState(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserExpel(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnUserExitRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void RoomInfoRequest(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void DamageProc(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnInfoAc(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnClientLeave(uint64_t nClientID);
	void LoadPoint();
	void EndProc();
	void pointProc();
	//void StartCheck();

	
private:
	IPluginManager *m_pluginManager;
	uint64_t m_sayTime;
	uint64_t m_pointTime;
	INetServer *m_pNetServer;
	ISQLService *sqlService;
	IDataService *m_pDataService;
	IDataBaseModule *m_pDataBase;
};

