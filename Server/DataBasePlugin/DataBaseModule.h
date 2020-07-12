#pragma once
#include <IDataBaseModule.h>
#include <IPluginManager.h>
#include <INetServer.h>

class DataBaseModule :
	public IDataBaseModule
{
public:
	DataBaseModule(IPluginManager *pluginManager);
	~DataBaseModule();

	// ͨ�� IModule �̳�
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;

	// ͨ�� IDataBaseModule �̳�
	virtual void SayHellow() override;

	//void OnMsgRecive(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen);
	void OnClientConnected(uint64_t nClientID);
	void OnClientLeave(uint64_t nClientID);

private:
	IPluginManager *m_pluginManager;
	uint64_t m_sayTime;

	INetServer *m_pNetServer;
};

