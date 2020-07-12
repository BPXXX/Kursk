#include "stdafx.h"
#include "NetServer.h"
#include <iostream>

NetServer::NetServer(IPluginManager *pluginManager)
	: m_pluginManager(pluginManager)
{
}


NetServer::~NetServer()
{
}

bool NetServer::Init()
{
	WSADATA wsaData;
	WSAStartup(MAKEWORD(2, 2), &wsaData);//初始化

	m_serverSocket = INVALID_SOCKET;

	m_nClientGUID = 0;

	return true;
}

bool NetServer::Update()
{
	if (m_serverSocket != INVALID_SOCKET)
	{
		SOCKET  client;
		int nsize = sizeof(SOCKADDR);
		SOCKADDR    clientAddr;

		// 处理客户端连接请求
		client = accept(m_serverSocket, &clientAddr, &nsize);//等待客户端连接
		if (client != INVALID_SOCKET)
		{
			AddClient(client);
		}

		std::list<uint64_t> m_removeClient;
		//处理消息接收
		for (auto kv : m_mapClient)
		{
			// 收消息
			char szBuf[25500];
			int nLen = recv(kv.first, szBuf, 1024, 0);
			bool bError = false;
			if (nLen > 0)
			{
				kv.second->m_recvData.append(szBuf, nLen);
			}
			else
			{
				if (GetLastError() != WSAEWOULDBLOCK)
				{
					bError = true;
				}
			}

			// 处理发送消息
			if (!bError)
			{
				if (kv.second->m_sendData.length() > 0)
				{
					int nLen = send(kv.first, kv.second->m_sendData.data(), kv.second->m_sendData.length(), 0);
					if (nLen > 0)
					{
						kv.second->m_sendData.erase(0, nLen);
					}
					else
					{
						if (GetLastError() != WSAEWOULDBLOCK)
						{
							bError = true;
						}
					}
				}
			}
			if(bError)
			{
				m_removeClient.push_back(kv.first);
			}
			else
			{
				// 处理消息的派发 
				if (kv.second->m_recvData.length() >= 4)
				{
					const char *pData = kv.second->m_recvData.data();
					size_t nIndex = 0;
					uint16_t nSize = *(uint16_t*)pData;
					uint16_t nType = *(uint16_t*)(pData+sizeof(uint16_t));

					while ((kv.second->m_recvData.length()-nIndex) >= nSize)
					{
						// 处理消息派发
						auto itr = m_onReciveCB.find(nType);
						if (itr != m_onReciveCB.end())
						{
							for (auto onRecive : itr->second)
							{
								(*onRecive)(kv.second->m_guid, nType, kv.second->m_recvData.data()+nIndex, nSize);
							}
						}
						// 处理索引后移
						nIndex += nSize;
					}
					if (nIndex != 0)
					{
						kv.second->m_recvData.erase(0, nIndex);
					}
				}
			}
		}
		for (auto client : m_removeClient)
		{
			RemoveClient(client);
		}
	}
	return true;
}

bool NetServer::Shut()
{
	if (m_serverSocket != INVALID_SOCKET)
	{
		closesocket(m_serverSocket);
		m_serverSocket = INVALID_SOCKET;
	}
	WSACleanup();

	m_onEnterCB.clear();
	m_onLeaveCB.clear();

	return true;
}

int NetServer::Initialization(const unsigned short nPort, const char *ip /*= nullptr*/)
{
	m_serverSocket = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (ip == nullptr)
	{
		ip = "127.0.0.1";
	}
	sockaddr_in sockaddr;
	sockaddr.sin_family = PF_INET;
	sockaddr.sin_addr.S_un.S_addr = inet_addr(ip);
	sockaddr.sin_port = htons(nPort);

	bind(m_serverSocket, (const struct sockaddr *)&sockaddr, sizeof(sockaddr));

	listen(m_serverSocket, 1);//开始监听

	int mode = 1;
	ioctlsocket(m_serverSocket, FIONBIO, (u_long *)&mode);

	return 0;
}

bool NetServer::AddReceiveCallBack(const uint32_t nMsgID, const NET_RECEIVE_FUNCTOR_PTR & cb)
{
	m_onReciveCB[nMsgID].push_back(cb);
	return true;
}

bool NetServer::AddEventCallBack(const NET_EVENT_FUNCTOR_PTR &enter_cb, const NET_EVENT_FUNCTOR_PTR &leave_cb)
{
	m_onEnterCB.push_back(enter_cb);
	m_onLeaveCB.push_back(leave_cb);
	return true;
}

void NetServer::SendMsg(const uint64_t nClientID, void* msg)
{
	auto itr = m_mapUser.find(nClientID);
	if (itr != m_mapUser.end())
	{
		const char *pData = (const char*)msg;
		itr->second->m_sendData.append(pData, *(uint16_t*)(pData));
	}
}
void NetServer::SendMsg(const uint64_t nClientID, int nMsgID, google::protobuf::Message &msg)
{
	auto itr = m_mapUser.find(nClientID);
	if (itr != m_mapUser.end())
	{
		std::string strData;
		msg.SerializeToString(&strData);
		uint16_t nSize = strData.size() + 4;
		uint16_t nType = nMsgID;

		itr->second->m_sendData.append((const char*)&nSize, 2);
		itr->second->m_sendData.append((const char*)&nType, 2);
		itr->second->m_sendData.append(strData.c_str(), strData.size());
		std::cout << "SendMsg:" << nClientID << "\t" << nMsgID << std::endl;
	}
}

User* NetServer::GetFreeUser()
{
	User *pUser = nullptr;
	if (m_listFreeUser.size() == 0)
	{
		pUser = new User();
	}
	else
	{
		pUser = m_listFreeUser.front();
		m_listFreeUser.pop_front();
	}
	pUser->Reset(m_nClientGUID++);
	return pUser;
}

void NetServer::FreeUser(User* pUser)
{
	m_mapUser.erase(pUser->m_guid);
	m_listFreeUser.push_back(pUser);
}

void NetServer::AddClient(SOCKET socket)
{
	auto pUser = GetFreeUser();
	m_mapClient.insert(std::make_pair(socket, pUser));
	for (auto cb : m_onEnterCB)
	{
		(*cb)(pUser->m_guid);
		m_mapUser.insert(std::make_pair(pUser->m_guid, pUser));
	}
}

void NetServer::RemoveClient(SOCKET socket)
{
	auto itr = m_mapClient.find(socket);
	if (itr != m_mapClient.end())
	{
		auto pUser = itr->second;
		for (auto cb : m_onLeaveCB)
		{
			(*cb)(pUser->m_guid);
		}
		FreeUser(pUser);
		m_mapClient.erase(itr);
	}
}