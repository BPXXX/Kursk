#include "stdafx.h"
#include "DataBaseModule.h"
#include <iostream>
#include <ctime>
#include <DataService/IDataService.h>
#include <DataService/automake/LanguageSetConfig.h>
#include <DataService/automake/ConfigConfig.h>
#include <DataService/automake/LanguageConfig.h>
#include <DataService/automake/ItemConfig.h>

#include <SQLService/ISQLService.h>

#include "../Common/NetMsgDefine/Msg.pb.h"

DataBaseModule::DataBaseModule(IPluginManager *pluginManager)
	: m_pluginManager(pluginManager)
{
}


DataBaseModule::~DataBaseModule()
{
}

bool DataBaseModule::Init()
{

	/*auto sqlService = m_pluginManager->FindModule<ISQLService>();
	sqlService->Open("filename=Game.db;");

	IQueryResult *result;
	sqlService->ExcuteQuery(&result, "SELECT ID FROM POINT");
	if (result != nullptr && result->Read())
	{
		std::cout << "userid:" << result->get_int32(0) << std::endl;
	}*/


	return true;
}

bool DataBaseModule::Update()
{
	if (GetTickCount() > m_sayTime)
	{
		SayHellow();
		m_sayTime = GetTickCount() + 4000;
	}
	return true;
}

bool DataBaseModule::Shut()
{
	return true;
}

void DataBaseModule::SayHellow()
{
	//std::cout << "Helow" << std::endl;
}

//void DataBaseModule::OnMsgRecive(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen)
//{
//	/*if (nMsgID == MsgType::LOGIN_C2S)
//	{
//		auto pMsg = (Msg_Login_C2S*)msg;
//		std::cout << "ClientLogin: id:" << nClientID << "\tname:" << pMsg->szName << std::endl;
//		Msg_Login_S2C login;
//		login.id = nClientID;
//		m_pNetServer->SendMsg(nClientID, &login);
//	}
//	else
//	{
//		std::cout << "ReicveMsg: id:" << nClientID << "\tid:" << nMsgID << std::endl;
//	}*/
//}

void DataBaseModule::OnClientConnected(uint64_t nClientID)
{
	std::cout << "ClientConnected:" << nClientID << std::endl;
}

void DataBaseModule::OnClientLeave(uint64_t nClientID)
{
	std::cout << "ClientDisConnected:" << nClientID << std::endl;
}

