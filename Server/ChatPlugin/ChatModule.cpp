#include "stdafx.h"
#include "ChatModule.h"
#include <iostream>
#include <ctime>
#include <DataService/IDataService.h>
#include <DataService/automake/LanguageSetConfig.h>
#include <DataService/automake/ConfigConfig.h>
#include <DataService/automake/LanguageConfig.h>
#include <DataService/automake/ItemConfig.h>

#include <SQLService/ISQLService.h>

#include "../Common/NetMsgDefine/Msg.pb.h"

#include <list>
#include<map>
#include<string>
#include<cstring>

ChatModule::ChatModule(IPluginManager *pluginManager)
	: m_pluginManager(pluginManager)
{
}


ChatModule::~ChatModule()
{
}

class User;
class lthouse;
std::list<User*>g_user;
std::list<lthouse*>g_house;
std::map<std::string, int>g_mapUserName;


class lthouse
{
public:
	lthouse(int id1)
	{
		houseId = id1;
		idcount = 0;
		id1 = 0;
		for (int i = 0; i < 50; i++)
		{
			id[i] = 0;
		}
	}
	std::map<unsigned short, int>houseuser;

	int id[50];
	int idcount;
	//std::map<int,int>house_user;
	int houseId;

};
std::map<int, int>g_mapHouseName;
/*class User
{
public:
	User()
	{
		m_msgLen = 0;
		m_msgSendLen = 0;
		id = 0;
		qunliao = false;
		jinyan = false;
		houseid = 0;
	}
	char szName[MaxName];
	int  houseid;
	char szSendBuf[4096];
	char szReciceBuf[4096];
	int m_msgLen;
	int m_msgSendLen;
	unsigned short id;
	bool qunliao;
	bool jinyan;
};*/




bool ChatModule::Init()
{
	m_pDataBase = m_pluginManager->FindModule<IDataBaseModule>();
	sqlService = m_pluginManager->FindModule<ISQLService>();
	sqlService->Open("filename=Game.db;");

	m_pNetServer = m_pluginManager->FindModule<INetServer>();

	m_pNetServer->Initialization(3000, "127.0.0.1");

	m_pNetServer->AddEventCallBack(this, &ChatModule::OnClientConnected, &ChatModule::OnClientLeave);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::FANGJIANLT_C2S, this, &ChatModule::OnFANGJIANLT);

	return true;
}

bool ChatModule::Update()
{
	
	return true;
}

bool ChatModule::Shut()
{
	return true;
}



void ChatModule::OnFANGJIANLT(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {


	Msg_FANGJIANLT_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	int ret = -1;

	Msg_FANGJIANLT_S2C msgtoc;

	m_pNetServer->SendMsg(nClientID, MsgType::LOGIN_S2C, msgtoc);

	msgtoc.set_szcontent(pMsg.szcontent());
	msgtoc.set_szname(pMsg.szname());

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ROOM == pMsg.houseid())//查找房间内所有玩家
		{
			ret = 0;
			msgtoc.set_result(0);//发送消息成功
			m_pNetServer->SendMsg((*itr)->ID, MsgType::FANGJIANLT_S2C, msgtoc);//把房间聊天的消息发送给房间内所有的玩家
		}
		++itr;
	}

	if (ret != 0)//查询失败
	{
		msgtoc.set_result(-1);//发送消息失败
		m_pNetServer->SendMsg(nClientID, MsgType::FANGJIANLT_S2C, msgtoc);//将发送失败的消息发送给该玩家

	}

}

void ChatModule::OnClientConnected(uint64_t nClientID)
{
	std::cout << "ClientConnected Chat:" << nClientID << std::endl;
}

void ChatModule::OnClientLeave(uint64_t nClientID)
{
	std::cout << "ClientDisConnected Chat:" << nClientID << std::endl;
}
