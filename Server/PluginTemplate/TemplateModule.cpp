#include "stdafx.h"
#include "TemplateModule.h"
#include <iostream>
#include <ctime>
#include <DataService/IDataService.h>
#include <DataService/automake/LanguageSetConfig.h>
#include <DataService/automake/ConfigConfig.h>
#include <DataService/automake/LanguageConfig.h>
#include <DataService/automake/ItemConfig.h>

#include <SQLService/ISQLService.h>

#include "../Common/NetMsgDefine/Msg.pb.h"

TemplateModule::TemplateModule(IPluginManager *pluginManager)
	: m_pluginManager(pluginManager)
{
}


TemplateModule::~TemplateModule()
{
}

bool TemplateModule::Init()
{
	//m_sayTime = GetTickCount();

	m_pNetServer = m_pluginManager->FindModule<INetServer>();	

	m_pNetServer->Initialization(3000, "127.0.0.1");

	m_pNetServer->AddEventCallBack(this, &TemplateModule::OnClientConnected, &TemplateModule::OnClientLeave);
	//m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::LOGIN_C2S,this, &TemplateModule::OnMsgRecive);


	//auto dataService = m_pluginManager->FindModule<IDataService>();
	//auto languageSet = dataService->QueryRecord<LanguageSetDefine>(1);
	//auto language = dataService->QueryRecord<LanguageDefine>(760017);
	//auto config = dataService->QueryRecord<ConfigDefine>(0);


	//std::cout << "----------------------------------------------------" << std::endl;
	//auto itemConfig = dataService->QueryRecord<ItemDefine>(20001);
	//std::cout
	//	<< "id" << ":" << itemConfig->id << "\t"
	//	<< "name" << ":" << itemConfig->name << "\t"
	//	<< "type" << ":" << itemConfig->type << "\t"
	//	<< "quality" << ":" << itemConfig->quality << "\t"
	//	<< std::endl;

	//std::cout << "----------------------------------------------------" << std::endl;
	//auto config = dataService->QueryRecord<ConfigDefine>(0);
	//std::cout
	//	<< "serverIp" << ":" << config->serverIP << "\t"
	//	<< std::endl;
	//std::cout << "----------------------------------------------------" << std::endl;
	//int count;
	//auto datas = dataService->QueryRecords<ItemDefine>(count);
	//for (int i = 0; i < count; i++)
	//{
	//	std::cout
	//		<< "id" << ":" << datas[i]->id << "\t"
	//		<< "name" << ":" << datas[i]->name << "\t"
	//		<< "type" << ":" << datas[i]->type << "\t"
	//		<< "quality" << ":" << datas[i]->quality << "\t"
	//		<< std::endl;
	//}
	//std::cout << "----------------------------------------------------" << std::endl;

	//auto sqlService = m_pluginManager->FindModule<ISQLService>();
	////sqlService->Open("filename=sqlite3.db;");
	//sqlService->Open("host=127.0.0.1;user=root;passwd=root;db=example;port=3306");

	//for (int i = 0; i < 10; i++)
	//{
	//	sqlService->ExcuteQueryf("INSERT INTO Example(age, name, score) VALUES(%d, 'test13', 1.2);", 10*(i+1));
	//}

	//IQueryResult *result;
	//sqlService->ExcuteQuery(&result, "SELECT max(id) FROM Example");
	//if (result != nullptr && result->Read())
	//{
	//	std::cout << "userid:" << result->get_int32(0) << std::endl;
	//}

	//sqlService->ExcuteQueryf("UPDATE Example SET age=%d WHERE id=%d", 30, 2);


	//sqlService->ExcuteQueryf("DELETE FROM Example WHERE id=%d", 5);


	//std::cout << "----------------------------------------------------" << std::endl;
	//std::cout << "1 select begin" << std::endl;
	////IQueryResult *result;
	//sqlService->ExcuteQuery(&result, "SELECT * FROM Example");
	//if (result != nullptr)
	//{
	//	while (result->Read())
	//	{
	//		std::cout 
	//			<< result->field_name(0) << ":" << result->get_int32(0) << "\t"
	//			<< result->field_name(1) << ":" << result->get_string(1) << "\t"
	//			<< result->field_name(2) << ":" << result->get_int32(2) << "\t"
	//			<< result->field_name(3) << ":" << result->get_float(3) << "\t"
	//			<< std::endl;
	//	}
	//}
	//std::cout << "1 select end" << std::endl;

	//std::cout << "2 select begin" << std::endl;
	//sqlService->ExcuteQueryAsync("SELECT * FROM Example", [this](IQueryResult *result, int nRetCode)
	//{
	//	if (result != nullptr)
	//	{
	//		while (result->Read())
	//		{
	//			std::cout
	//				<< result->field_name(0) << ":" << result->get_int32(0) << "\t"
	//				<< result->field_name(1) << ":" << result->get_string(1) << "\t"
	//				<< result->field_name(2) << ":" << result->get_int32(2) << "\t"
	//				<< result->field_name(3) << ":" << result->get_float(3) << "\t"
	//				<< std::endl;
	//		}
	//	}
	//	std::cout << "2 select end" << std::endl;
	//});

	//std::cout << "----------------------------------------------------" << std::endl;

	//IQueryResult *result;
	//std::cout << "normal call back" << std::endl;
	//sqlService->ExcuteQueryf(&result, "select * from example");
	//if (result != nullptr)
	//{
	//	while (result->Read())
	//	{
	//		auto id = result->get_int32(0);
	//		auto name = result->get_string(1);
	//		auto score = result->get_float(2);
	//		auto age = result->get_int32(3);
	//		std::cout << id << "," << name << "," << score << "," << age << std::endl;
	//	}
	//}

	//sqlService->ExcuteQueryAsync("select * from example where id=2", [this](IQueryResult *result, int nRetCode)
	//{
	//	if (result != nullptr)
	//	{
	//		std::cout << "async call back" << std::endl;
	//		while (result->Read())
	//		{
	//			auto id = result->get_int32(0);
	//			auto name = result->get_string(1);
	//			auto score = result->get_float(2);
	//			auto age = result->get_int32(3);
	//			std::cout << id << "," << name << "," << score << "," << age << std::endl;
	//		}
	//	}
	//});


	//sqlService->ExcuteQuery("INSERT INTO Example(age, name, score) VALUES(12, 'test13', 1.2);");
	//sqlService->ExcuteQuery("Update Example SET name='new Name234' where id=1");
	return true;
}

bool TemplateModule::Update()
{
	/*if (GetTickCount() > m_sayTime)
	{
		SayHellow();
		m_sayTime = GetTickCount() + 4000;
	}*/
	return true;
}

bool TemplateModule::Shut()
{
	return true;
}

void TemplateModule::SayHellow()
{
	//std::cout << "Helow" << std::endl;
}

//void TemplateModule::OnMsgRecive(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen)
//{
//	//if (nMsgID == MsgType::LOGIN_C2S)
//	//{
//	//	auto pMsg = (Msg_Login_C2S*)msg;
//	//	std::cout << "ClientLogin: id:" << nClientID << "\tname:" << pMsg->szName << std::endl;
//	//	Msg_Login_S2C login;
//	//	login.id = nClientID;
//	//	m_pNetServer->SendMsg(nClientID, &login);
//	//} 
//	//else
//	//{
//	//	std::cout << "ReicveMsg: id:" << nClientID << "\tid:" << nMsgID << std::endl;
//	//}
//
//	/*Msg_Login_C2S xMsg;
//	xMsg.ParseFromArray(msg + 4, nLen);
//
//	Msg_Login_S2C xMsg2;
//
//	xMsg2.set_param1("123456789");
//	xMsg2.set_param2(254);
//	xMsg2.set_param3("Test");
//
//	xMsg2.add_param4(25);
//	xMsg2.add_param4(26);
//	xMsg2.add_param4(27);
//
//	m_pNetServer->SendMsg(nClientID, MsgType::LOGIN_S2C, xMsg2);*/
//
//}

void TemplateModule::OnClientConnected(uint64_t nClientID)
{
	std::cout << "ClientConnected:" << nClientID << std::endl;
}

void TemplateModule::OnClientLeave(uint64_t nClientID)
{
	std::cout << "ClientDisConnected:" << nClientID << std::endl;
}
