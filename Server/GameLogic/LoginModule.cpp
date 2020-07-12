#include "stdafx.h"
#include "LoginModule.h"
#include <iostream>
#include <ctime>
#include "../Common/NetMsgDefine/Msg.pb.h"

LoginModule::LoginModule(IPluginManager *pluginManager)
	: m_pluginManager(pluginManager)
{
}

LoginModule::~LoginModule()
{
}

bool LoginModule::Init()
{
	m_pNetServer = m_pluginManager->FindModule<INetServer>();
	//m_pSQLService = m_pluginManager->FindModule<ISQLService>();
	//m_pSQLService->Open("filename=sqlite3.db;");

	//m_nMaxUserID = 0;
	//IQueryResult *result;
	//m_pSQLService->ExcuteQuery(&result, "SELECT max(User.userId) FROM User");
	//if (result && result->Read())
	//{
	//	m_nMaxUserID = result->get_int32(0);
	//}


	m_pNetServer->Initialization(3000, "127.0.0.1");

	m_pNetServer->AddEventCallBack(this, &LoginModule::OnClientConnected, &LoginModule::OnClientLeave);
//	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::LOGIN_C2S,this, &LoginModule::OnMsgRecive);

	return true;
}

bool LoginModule::Update()
{
	return true;
}

bool LoginModule::Shut()
{
	return true;
}

//void LoginModule::OnMsgRecive(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen)
//{
//	Msg_Login_C2S xMsg;
//	xMsg.ParseFromArray(msg + 4, nLen);
//	//switch (nMsgID)
//	//{
//	//case MsgType::LOGIN_C2S:
//	//{
//	//	Msg_Login_C2S *pMsg = (Msg_Login_C2S*)msg;
//
//	//	int userId = m_nMaxUserID;;
//	//	IQueryResult *result;
//	//	m_pSQLService->ExcuteQueryf(&result, "select userId from User where userName='%s'", pMsg->szName);
//	//	if (result && result->Read())
//	//	{
//	//		userId = result->get_int32(0);
//	//	}
//	//	else
//	//	{
//	//		userId = ++m_nMaxUserID;
//	//		m_pSQLService->ExcuteQueryf(&result, "INSERT INTO User(userId, userName) VALUES(%d, '%s');", userId, pMsg->szName);
//	//	}
//	//	std::cout << "UserLogin:" << pMsg->szName  << ":" << userId << std::endl;
//	//	Msg_Login_S2C login;
//	//	login.id = userId;
//	//	m_pNetServer->SendMsg(nClientID, &login);
//	//}
//	//break;
//	//case MsgType::CHAT_C2S:
//	//{
//	//	Msg_Chat *pMsg = (Msg_Chat*)msg;
//	//	pMsg->type = MsgType::CHAT_S2C;
//	//	m_pNetServer->SendMsg(nClientID, &pMsg);
//	//}
//	//break;
//	//}
//}

void LoginModule::OnClientConnected(uint64_t nClientID)
{
	std::cout << "ClientConnected:" << nClientID << std::endl;
}

void LoginModule::OnClientLeave(uint64_t nClientID)
{
	std::cout << "ClientDisConnected:" << nClientID << std::endl;
}
