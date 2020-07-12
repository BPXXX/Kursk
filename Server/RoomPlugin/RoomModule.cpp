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
		msgtoc.set_result(-1);//����ʧ��
	}
	else
	{
		msgtoc.set_result(0);//����ɹ�
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
	printf("���ݿ��¼\n");
	int ret = sqlService->ExcuteQuery(&result, sql);
	if (result != nullptr && result->Read())
	{
		match = result->get_string(0).compare(pMsg.szpassword());
	}

	Msg_Login_S2C msgtoc;
	if (ret != 0 || match != 0)//��ѯ��������벻ƥ��
	{
		msgtoc.set_result(-1);//��¼ʧ��
	}
	else
	{
		PLAYER *player = new PLAYER();//����һ������ҽڵ㣬����PLAYER���������
		player->ID = nClientID;
		player->USERNAME=pMsg.szname().c_str();
		m_pDataBase->g_player.push_back(player);

		msgtoc.set_result(0);//��¼�ɹ�
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

		if ((*itr)->ROOM == pMsg.id())//������ڸ�id�ŵķ���
		{
			flag = false;//�򴴽�����ʧ��
		}
		++itr;
	}

	Msg_CreateRoom_S2C msgtoc;

	if (flag == false)//�����Ѿ�������
	{
		msgtoc.set_result(-1);//����ʧ��
	}
	else
	{
		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ID == nClientID)//ͨ��id���Ҹ����
			{
				(*itr)->ROOM = pMsg.id();//����������ڷ���
				(*itr)->MASTER = 1;//�������Ϊ����
				(*itr)->NUMBER = 1;//Ĭ�Ϸ���λ��Ϊ1��
				(*itr)->TEAM = 0;//Ĭ�϶���Ϊ0��
				(*itr)->TYPE = 1;//Ĭ��̹��Ϊ1��̹��
				(*itr)->READY = 1;//����һֱ��׼����
			}
			++itr;
		}
		msgtoc.set_result(0);//�����ɹ�
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

		if ((*itr)->ROOM == pMsg.id())//������ڸ�id�ŵķ���
		{
			count++;//�����������
			number[(*itr)->NUMBER - 1] = false;//��λ���Ѿ���ռ��
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

	if (match != 0) {//�п���λ��
		int team;
		if (match <= 6)
			team = 0;
		else
			team = 1;//�������ֵ

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ID == nClientID)//ͨ��id���Ҹ����
			{
				(*itr)->ROOM = pMsg.id();//����������ڷ���
				(*itr)->MASTER = 0;//������Ҳ�Ϊ����
				(*itr)->NUMBER = match;//�������λ��
				(*itr)->TEAM = team;//������Ҷ���
				(*itr)->TYPE = 1;//Ĭ��̹��Ϊ1��̹��
				(*itr)->READY = 0;//Ĭ�ϲ�׼��
				msgtoc.set_szname((*itr)->USERNAME.c_str());//�����û���
			}
			++itr;
		}
		msgtoc.set_result(0);//���뷿��ɹ�
		msgtoc.set_number(match);

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == pMsg.id())//���ҷ������������
			{
				m_pNetServer->SendMsg((*itr)->ID,MsgType::ENTER_ROOM_S2C, msgtoc);//�����˼������Ϣ���͸����������е����
			}
			++itr;
		}

	}
	else //��������
	{
		msgtoc.set_result(-1);//���뷿��ʧ��
		m_pNetServer->SendMsg(nClientID,MsgType::ENTER_ROOM_S2C, msgtoc);//�Ѽ���ʧ�ܵ���Ϣ���͸������
	}


}
void RoomModule::OnUserChangeState(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {//ÿ�����׼������λ�á���̹��ʱ����
	Msg_ChangeState_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	int ret = -1;
	int room = -1;//Ҫд��ķ���

	Msg_ChangeState_S2C msgtoc;

	int team;
	if (pMsg.number2() <= 6)
		team = 0;
	else
		team = 1;//�������ֵ

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ID == nClientID)//����ID���Ҹ����
		{
			(*itr)->NUMBER = pMsg.number2();//�������λ��
			(*itr)->TEAM = team;//������Ҷ���
			(*itr)->TYPE = pMsg.type();//�������ѡ���̹��
			(*itr)->READY = pMsg.ready();//�������׼��״̬
			ret = 0;//���ҳɹ�
			room = (*itr)->ROOM;//ȡ��������ڷ���
		}
		++itr;
	}

	if (ret == 0) {//����״̬�ɹ�
		msgtoc.set_result(0);
		msgtoc.set_number1(pMsg.number1());//д�����֮ǰλ��
		msgtoc.set_number2(pMsg.number2());//д���������λ��
		msgtoc.set_type(pMsg.type());//д�����ѡ��̹������
		msgtoc.set_ready(pMsg.ready());//д�����׼��״̬


		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == room)//���ҷ������������
			{
				m_pNetServer->SendMsg((*itr)->ID,MsgType::CHANGE_STATE_S2C, msgtoc);//��״̬ת������Ϣ���͸����������е����
			}
			++itr;
		}
	}
	else //����״̬ʧ��
	{
		msgtoc.set_result(-1);//ת��ʧ��
		m_pNetServer->SendMsg(nClientID, MsgType::CHANGE_STATE_S2C, msgtoc);//������ʧ�ܵ���Ϣ���͸������
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

		if ((*itr)->ROOM == pMsg.id() && (*itr)->NUMBER == pMsg.number())//����ID���Ҹ����
		{
			(*itr)->ROOM = NULL;
			(*itr)->MASTER = NULL;
			(*itr)->NUMBER = NULL;
			(*itr)->TEAM = NULL;
			(*itr)->TYPE = NULL;
			(*itr)->READY = NULL; //�������������ΪĬ��ֵ
			ret = 0;//���ҳɹ�
			room = (*itr)->ROOM;//ȡ��������ڷ���
		}
		++itr;
	}

	if (ret == 0) {//����״̬�ɹ�
		msgtoc.set_result(0);
		msgtoc.set_number(pMsg.number());

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == room)//���ҷ������������
			{
				m_pNetServer->SendMsg((*itr)->ID,MsgType::EXPEL_S2C, msgtoc);//����ұ��ߵ���Ϣ���͸����������е����
			}
			++itr;
		}
	}

	else //����״̬ʧ��
	{
		msgtoc.set_result(-1);//����ʧ��
		m_pNetServer->SendMsg(nClientID,MsgType::EXPEL_S2C, msgtoc);//������ʧ�ܵ���Ϣ���͸������
	}
}
void RoomModule::OnUserStartGame(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_StartGame_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);

	int count = 0;
	bool flag = true;
	bool flagR = false;//�졢�����Ƿ������
	bool flagB = false;

	Msg_StartGame_S2C msgtoc;


	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ROOM == pMsg.id())//���ҷ������������
		{
			count++;
			if ((*itr)->READY != 1)
				flag = false;//�����û��׼��
			if ((*itr)->TEAM == 0)
				flagR = true;//��������������
			if ((*itr)->TEAM == 1)
				flagB = true;
		}
		++itr;
	}

	if (flagR == false || flagB == false || flag == false)//����������������������δ׼��
	{//
		msgtoc.set_result(-1);//��ʼ��Ϸʧ��
		m_pNetServer->SendMsg(nClientID,MsgType::START_GAME_S2C, msgtoc);//����ʼʧ�ܵ���Ϣ���͸������
	}
	else {
		msgtoc.set_result(0);//��ʼ��Ϸ�ɹ�

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == pMsg.id())//���ҷ������������
			{
				m_pNetServer->SendMsg((*itr)->ID, MsgType::START_GAME_S2C, msgtoc);//����Ϸ��ʼ����Ϣ���͸����������е����
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

	msgtoc.set_result(-1);//Ĭ�ϲ�ѯʧ��

	std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
	while (itr != m_pDataBase->g_player.end())
	{

		if ((*itr)->ROOM == pMsg.id())//���ҷ������������
		{
			msgtoc.add_number((*itr)->NUMBER);
			msgtoc.add_szname((*itr)->USERNAME);
			msgtoc.add_tanktype((*itr)->TYPE);
			msgtoc.add_ready((*itr)->READY);
			msgtoc.set_result(0);//��ѯ�ɹ�
			count++;
		}
		++itr;
	}
	m_pNetServer->SendMsg(nClientID, MsgType::GET_ROOMMEN_S2C, msgtoc);//�ѷ�������ҵ�λ����Ϣ�����Ʒ��͸����
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

		if ((*itr)->ID == nClientID)//����ID���Ҹ����
		{
			(*itr)->ROOM = NULL;
			(*itr)->MASTER = NULL;
			(*itr)->NUMBER = NULL;
			(*itr)->TEAM = NULL;
			(*itr)->TYPE = NULL;
			(*itr)->READY = NULL; //�������������ΪĬ��ֵ
			ret = 0;//���ҳɹ�
		}
		++itr;
	}

	if (ret == 0) {//�˳�����ɹ�
		msgtoc.set_result(0);
		msgtoc.set_number(pMsg.number());

		itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == pMsg.id())//���ҷ������������
			{
				m_pNetServer->SendMsg((*itr)->ID,MsgType::EXIT_ROOM_S2C, msgtoc);//������˳�����Ϣ���͸����������е����
			}
			++itr;
		}
	}

	else //�˳�����ʧ��
	{
		msgtoc.set_result(-1);//�˳�ʧ��
		m_pNetServer->SendMsg(nClientID, MsgType::EXIT_ROOM_S2C, msgtoc);//���˳�ʧ�ܵ���Ϣ���͸������
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

		if ((*itr)->ID == nClientID)//����ID���Ҹ����
		{
			m_pDataBase->g_player.erase(itr);//ɾ�����������Ϣ
		}
		++itr;
	}
}
