#pragma once
#include <INetServer.h>
#include <IPluginManager.h>

#include <unordered_map>
#include <stdint.h>
#include <winsock2.h>
#include <stdio.h>
#pragma comment (lib,"ws2_32")

class User
{
public:
	std::string m_sendData;
	std::string m_recvData;
	uint64_t m_guid;
	void Reset(uint64_t guid)
	{
		m_guid = guid;
		m_sendData.clear();
		m_recvData.clear();
	}
};

class NetServer :
	public INetServer
{
public:
	NetServer(IPluginManager *pluginManager);
	~NetServer();

	// 通过 IModule 继承
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;

	// 通过 INetServer 继承
	virtual int Initialization(const unsigned short nPort, const char *ip = nullptr) override;
	virtual bool AddReceiveCallBack(const uint32_t nMsgID, const NET_RECEIVE_FUNCTOR_PTR & cb) override;
	virtual bool AddEventCallBack(const NET_EVENT_FUNCTOR_PTR &enter_cb, const NET_EVENT_FUNCTOR_PTR &leave_cb) override;
	virtual void SendMsg(const uint64_t nClientID, void* msg) override;
	virtual void SendMsg(const uint64_t nClientID, int nMsgID, google::protobuf::Message &msg) override;

private:
	User* GetFreeUser();
	void FreeUser(User* pUser);

	void AddClient(SOCKET socket);
	void RemoveClient(SOCKET socket);

private:
	IPluginManager *m_pluginManager;
	SOCKET m_serverSocket;

	uint32_t m_nClientGUID;

	std::unordered_map<uint64_t, User*> m_mapClient;
	std::unordered_map<uint64_t, User*> m_mapUser;
	std::list<User*> m_listFreeUser;

	std::unordered_map<uint32_t, std::list<NET_RECEIVE_FUNCTOR_PTR>> m_onReciveCB;
	std::list<NET_EVENT_FUNCTOR_PTR> m_onEnterCB;
	std::list<NET_EVENT_FUNCTOR_PTR> m_onLeaveCB;
};

