#include "stdafx.h"
#include "RoomModule.h"
#include <iostream>
#include <ctime>
#include <DataService/IDataService.h>
#include <DataService/automake/LanguageSetConfig.h>
#include <DataService/automake/ConfigConfig.h>
#include <DataService/automake/LanguageConfig.h>
#include <DataService/automake/ItemConfig.h>

#include <SQLService/ISQLService.h>

#include "../Common/NetMsgDefine/Msg.pb.h"

RoomModule::RoomModule(IPluginManager *pluginManager)
	: m_pluginManager(pluginManager)
{
}


RoomModule::~RoomModule()
{
}

bool RoomModule::Init()
{
	m_pDataBase = m_pluginManager->FindModule<IDataBaseModule>();
	sqlService = m_pluginManager->FindModule<ISQLService>();

	sqlService->Open("filename=Game.db;");

	/*IQueryResult *result;
	sqlService->ExcuteQuery(&result, "SELECT ID FROM POINT");
	if (result != nullptr && result->Read())
	{
		std::cout << "userid:" << result->get_int32(0) << std::endl;
	}*/

	m_pNetServer = m_pluginManager->FindModule<INetServer>();

	m_pNetServer->Initialization(3000, "127.0.0.1");

	m_pNetServer->AddEventCallBack(this, &RoomModule::OnClientConnected, &RoomModule::OnClientLeave);

	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::REGISTER_C2S, this, &RoomModule::OnUserRegister);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::LOGIN_C2S, this, &RoomModule::OnUserLogin);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::CREATE_ROOM_C2S, this, &RoomModule::OnUserCreatRoom);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::ENTER_ROOM_C2S, this, &RoomModule::OnUserEnterRoom);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::CHANGE_STATE_C2S, this, &RoomModule::OnUserChangeState);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::EXPEL_C2S, this, &RoomModule::OnUserExpel);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::START_GAME_C2S, this, &RoomModule::OnUserStartGame);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::GET_ROOMMEN_C2S, this, &RoomModule::OnUserGetRoomMen);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::EXIT_ROOM_C2S, this, &RoomModule::OnUserExitRoom);





	return true;
}

bool RoomModule::Update()
{

	return true;
}

bool RoomModule::Shut()
{
	return true;
}


void RoomModule::OnUserRegister(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_Register_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	char sql[100];
	sprintf(sql, "INSERT INTO LOGIN(USERNAME,PASSWORD) VALUES('%s','%s')", pMsg.szname().c_str(), pMsg.szpassword().c_str());
	int ret = sqlService->ExcuteQuery(sql);

	Msg_Register_S2C msgtoc;
	if (ret != 0)//!=SQLITE_OK
	{
		msgtoc.set_result(-1);//插入失败
	}
	else
	{
		msgtoc.set_result(0);//插入成功
	}
	m_pNetServer->SendMsg(nClientID, MsgType::REGISTER_S2C, msgtoc);
}
void RoomModule::OnUserLogin(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_Login_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	IQueryResult *result;
	char sql[100];
	int match = -1;
	sprintf(sql, "SELECT PASSWORD FROM LOGIN WHERE USERNAME='%s'", pMsg.szname().c_str());
	printf("数据库登录\n");
	int ret = sqlService->ExcuteQuery(&result, sql);
	if (result != nullptr && result->Read())
	{
		match = result->get_string(0).compare(pMsg.szpassword());
	}

	Msg_Login_S2C msgtoc;
	if (ret != 0 || match != 0)//查询错误或密码不匹配
	{
		msgtoc.set_result(-1);//登录失败
	}
	else
	{
		PLAYER *player = new PLAYER();//创建一个新玩家节点，并向PLAYER链表里插入
		player->ID = nClientID;
		player->USERNAME=pMsg.szname().c_str();
		m_pDataBase->g_player.push_back(player);

		msgtoc.set_result(0);//登录成功
		msgtoc.set_id(nClientID);
	}
	m_pNetServer->SendMsg(nClientID,MsgType::LOGIN_S2C, msgtoc);
}
void RoomModule::OnUserCreatRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_CreateRoom_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	bool flag = true;

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ROOM == pMsg.id())//如果存在该id号的房间
		{
			flag = false;//则创建房间失败
		}
		++itr;
	}

	Msg_CreateRoom_S2C msgtoc;

	if (flag == false)//房间已经被创建
	{
		msgtoc.set_result(-1);//创建失败
	}
	else
	{
		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ID == nClientID)//通过id查找该玩家
			{
				(*itr)->ROOM = pMsg.id();//更改玩家所在房间
				(*itr)->MASTER = 1;//设置玩家为房主
				(*itr)->NUMBER = 1;//默认房间位置为1号
				(*itr)->TEAM = 0;//默认队伍为0队
				(*itr)->TYPE = 1;//默认坦克为1号坦克
				(*itr)->READY = 1;//房主一直是准备的
			}
			++itr;
		}
		msgtoc.set_result(0);//创建成功
	}
	m_pNetServer->SendMsg(nClientID,MsgType::CREATE_ROOM_S2C, msgtoc);
}
void RoomModule::OnUserEnterRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_EnterRoom_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	int count = 0;
	bool number[12] = { true,true,true,true,true,true,true,true,true,true,true,true };
	int match = 0;

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ROOM == pMsg.id())//如果存在该id号的房间
		{
			count++;//房间内玩家数
			number[(*itr)->NUMBER - 1] = false;//该位置已经被占用
		}
		++itr;
	}

	int i = 0;
	while (count != 0 && i < 12)
	{
		if (number[i] == true) {
			match = i + 1;
			break;
		}
		i++;
	}


	Msg_EnterRoom_S2C msgtoc;

	if (match != 0) {//有空余位置
		int team;
		if (match <= 6)
			team = 0;
		else
			team = 1;//计算队伍值

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ID == nClientID)//通过id查找该玩家
			{
				(*itr)->ROOM = pMsg.id();//更改玩家所在房间
				(*itr)->MASTER = 0;//设置玩家不为房主
				(*itr)->NUMBER = match;//设置玩家位置
				(*itr)->TEAM = team;//设置玩家队伍
				(*itr)->TYPE = 1;//默认坦克为1号坦克
				(*itr)->READY = 0;//默认不准备
				msgtoc.set_szname((*itr)->USERNAME.c_str());//传递用户名
			}
			++itr;
		}
		msgtoc.set_result(0);//加入房间成功
		msgtoc.set_number(match);

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == pMsg.id())//查找房间内所有玩家
			{
				m_pNetServer->SendMsg((*itr)->ID,MsgType::ENTER_ROOM_S2C, msgtoc);//把有人加入的消息发送给房间内所有的玩家
			}
			++itr;
		}

	}
	else //房间已满
	{
		msgtoc.set_result(-1);//加入房间失败
		m_pNetServer->SendMsg(nClientID,MsgType::ENTER_ROOM_S2C, msgtoc);//把加入失败的消息发送给该玩家
	}


}
void RoomModule::OnUserChangeState(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {//每当玩家准备、换位置、换坦克时调用
	Msg_ChangeState_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	int ret = -1;
	int room = -1;//要写入的房间

	Msg_ChangeState_S2C msgtoc;

	int team;
	if (pMsg.number2() <= 6)
		team = 0;
	else
		team = 1;//计算队伍值

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ID == nClientID)//根据ID查找该玩家
		{
			(*itr)->NUMBER = pMsg.number2();//更新玩家位置
			(*itr)->TEAM = team;//更新玩家队伍
			(*itr)->TYPE = pMsg.type();//更新玩家选择的坦克
			(*itr)->READY = pMsg.ready();//更新玩家准备状态
			ret = 0;//查找成功
			room = (*itr)->ROOM;//取出玩家所在房间
		}
		++itr;
	}

	if (ret == 0) {//更新状态成功
		msgtoc.set_result(0);
		msgtoc.set_number1(pMsg.number1());//写入玩家之前位置
		msgtoc.set_number2(pMsg.number2());//写入玩家现在位置
		msgtoc.set_type(pMsg.type());//写入玩家选择坦克类型
		msgtoc.set_ready(pMsg.ready());//写入玩家准备状态


		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == room)//查找房间内所有玩家
			{
				m_pNetServer->SendMsg((*itr)->ID,MsgType::CHANGE_STATE_S2C, msgtoc);//把状态转换的消息发送给房间内所有的玩家
			}
			++itr;
		}
	}
	else //更新状态失败
	{
		msgtoc.set_result(-1);//转换失败
		m_pNetServer->SendMsg(nClientID, MsgType::CHANGE_STATE_S2C, msgtoc);//将更新失败的消息发送给该玩家
	}

}
void RoomModule::OnUserExpel(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_Expel_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	int ret = -1;
	int room = -1;

	Msg_Expel_S2C msgtoc;


	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ROOM == pMsg.id() && (*itr)->NUMBER == pMsg.number())//根据ID查找该玩家
		{
			(*itr)->ROOM = NULL;
			(*itr)->MASTER = NULL;
			(*itr)->NUMBER = NULL;
			(*itr)->TEAM = NULL;
			(*itr)->TYPE = NULL;
			(*itr)->READY = NULL; //将玩家属性设置为默认值
			ret = 0;//查找成功
			room = (*itr)->ROOM;//取出玩家所在房间
		}
		++itr;
	}

	if (ret == 0) {//更新状态成功
		msgtoc.set_result(0);
		msgtoc.set_number(pMsg.number());

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == room)//查找房间内所有玩家
			{
				m_pNetServer->SendMsg((*itr)->ID,MsgType::EXPEL_S2C, msgtoc);//把玩家被踢的消息发送给房间内所有的玩家
			}
			++itr;
		}
	}

	else //更新状态失败
	{
		msgtoc.set_result(-1);//踢人失败
		m_pNetServer->SendMsg(nClientID,MsgType::EXPEL_S2C, msgtoc);//将更新失败的消息发送给该玩家
	}
}
void RoomModule::OnUserStartGame(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_StartGame_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	int count = 0;
	bool flag = true;
	bool flagR = false;//红、蓝队是否有玩家
	bool flagB = false;

	Msg_StartGame_S2C msgtoc;


	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ROOM == pMsg.id())//查找房间内所有玩家
		{
			count++;
			if ((*itr)->READY != 1)
				flag = false;//有玩家没有准备
			if ((*itr)->TEAM == 0)
				flagR = true;//如果队伍里有玩家
			if ((*itr)->TEAM == 1)
				flagB = true;
		}
		++itr;
	}

	if (flagR == false || flagB == false || flag == false)//队伍中玩家人数不足或有人未准备
	{//
		msgtoc.set_result(-1);//开始游戏失败
		m_pNetServer->SendMsg(nClientID,MsgType::START_GAME_S2C, msgtoc);//将开始失败的消息发送给该玩家
	}
	else {
		msgtoc.set_result(0);//开始游戏成功

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == pMsg.id())//查找房间内所有玩家
			{
				m_pNetServer->SendMsg((*itr)->ID, MsgType::START_GAME_S2C, msgtoc);//把游戏开始的消息发送给房间内所有的玩家
			}
			++itr;
		}
	}
}

void RoomModule::OnUserGetRoomMen(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_GetRoomMen_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);


	int count = 0;

	Msg_GetRoomMen_S2C msgtoc;

	msgtoc.set_result(-1);//默认查询失败

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ROOM == pMsg.id())//查找房间内所有玩家
		{
			msgtoc.add_number((*itr)->NUMBER);
			msgtoc.add_szname((*itr)->USERNAME);
			msgtoc.add_tanktype((*itr)->TYPE);
			msgtoc.add_ready((*itr)->READY);
			msgtoc.set_result(0);//查询成功
			count++;
		}
		++itr;
	}
	m_pNetServer->SendMsg(nClientID, MsgType::GET_ROOMMEN_S2C, msgtoc);//把房间内玩家的位置信息、名称发送给玩家
}

void RoomModule::OnUserExitRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_ExitRoom_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	int ret = -1;


	IQueryResult *result;
	char sql[100];

	Msg_ExitRoom_S2C msgtoc;

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ID == nClientID)//根据ID查找该玩家
		{
			(*itr)->ROOM = NULL;
			(*itr)->MASTER = NULL;
			(*itr)->NUMBER = NULL;
			(*itr)->TEAM = NULL;
			(*itr)->TYPE = NULL;
			(*itr)->READY = NULL; //将玩家属性设置为默认值
			ret = 0;//查找成功
		}
		++itr;
	}

	if (ret == 0) {//退出房间成功
		msgtoc.set_result(0);
		msgtoc.set_number(pMsg.number());

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == pMsg.id())//查找房间内所有玩家
			{
				m_pNetServer->SendMsg((*itr)->ID,MsgType::EXIT_ROOM_S2C, msgtoc);//把玩家退出的消息发送给房间内所有的玩家
			}
			++itr;
		}
	}

	else //退出房间失败
	{
		msgtoc.set_result(-1);//退出失败
		m_pNetServer->SendMsg(nClientID, MsgType::EXIT_ROOM_S2C, msgtoc);//将退出失败的消息发送给该玩家
	}
}


void RoomModule::OnClientConnected(uint64_t nClientID)
{
	std::cout << "ClientConnected Room:" << nClientID << std::endl;
}

void RoomModule::OnClientLeave(uint64_t nClientID)
{
	std::cout << "ClientDisConnected Room:" << nClientID << std::endl;

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ID == nClientID)//根据ID查找该玩家
		{
			m_pDataBase->g_player.erase(itr);//删除玩家在线信息
		}
		++itr;
	}
}
