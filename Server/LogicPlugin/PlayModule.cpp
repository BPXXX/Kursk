#pragma warning(disable:4996)
#include "stdafx.h"
#include "PlayModule.h"
#include <iostream>
#include <ctime>

#include <DataService/IDataService.h>
#include <DataService/automake/TankConfig.h>
#include <DataService/automake/ConfigConfig.h>
#include <DataService/automake/MapConfig.h>
#include <DataService/automake/ItemConfig.h>
#include "../Common/NetMsgDefine/Msg.pb.h"
#include <SQLService/ISQLService.h>
#include<map>
#include<list>

std::list<Point2*> g_point;

PlayModule::PlayModule(IPluginManager *pluginManager)
	: m_pluginManager(pluginManager)
{
}


PlayModule::~PlayModule()
{
}

bool PlayModule::Init()
{
	m_sayTime = GetTickCount();
	m_pointTime= GetTickCount();
	sqlService = m_pluginManager->FindModule<ISQLService>();
	sqlService->Open("filename=Game.db;");
	m_pDataService = m_pluginManager->FindModule<IDataService>();
	m_pNetServer = m_pluginManager->FindModule<INetServer>();
	m_pDataBase = m_pluginManager->FindModule<IDataBaseModule>();
	m_pNetServer->AddEventCallBack(this, &PlayModule::OnClientConnected, &PlayModule::OnClientLeave);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::VEC_C2S, this, &PlayModule::OnVecProc);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::Ammo_C2S, this, &PlayModule::OnAmmoProc);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::Damage_C2S, this, &PlayModule::DamageProc);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::CREATE_ROOM_C2S, this, &PlayModule::OnUserCreateRoom);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::ENTER_ROOM_C2S, this, &PlayModule::OnUserEnterRoom);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::CHANGE_STATE_C2S, this, &PlayModule::OnUserChangeState);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::EXPEL_C2S, this, &PlayModule::OnUserExpel);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::START_GAME_C2S, this, &PlayModule::OnUserStartGame);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::EXIT_ROOM_C2S, this, &PlayModule::OnUserExitRoom);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::RoomInfoRequest, this, &PlayModule::RoomInfoRequest);
	m_pNetServer->AddReceiveCallBack((uint32_t)MsgType::InfoAc, this, &PlayModule::OnInfoAc);
	LoadPoint();

	return true;
}

bool PlayModule::Update()
{
	
	if (GetTickCount() > m_sayTime)
	{
	
		m_sayTime = GetTickCount() + 20000;
	}
	if (GetTickCount() > m_pointTime)
	{
		EndProc();
		m_pointTime = GetTickCount() + 2000;
	}
	return true;
}

bool PlayModule::Shut()
{
	return true;
}


void PlayModule::SayHellow()
{
	//std::cout << "Helow" << std::endl;
}


/*void PlayModule::OnUserLogin(const uint64_t nClientID, const uint32_t nMsgID, const char* msg1, const uint32_t nLen)
{
	printf("PlayLogin\n");
	auto pMsg = (Msg_Login_C2S*)msg1;

	Msg_Login_S2C msg;
	std::map<std::string, int>::iterator itr = g_mapUserName.find(pMsg->szName);
	if (itr == g_mapUserName.end())
	{
		msg.id = g_mapUserName.size() + 1;
		g_mapUserName[pMsg->szName] = msg.id;
	}
	else
	{
		msg.id = itr->second;
	}
	User *user = new User();
	user->id = nClientID;
	strcpy_s(user->szName, pMsg->szName);
	
	g_user.push_back(user);
	std::cout << "ClientLogin: id:" << nClientID << "\tname:" << pMsg->szName << std::endl;
	Msg_Login_S2C login;
	login.id = nClientID;
	m_pNetServer->SendMsg(nClientID, &login);

}*/

void PlayModule::OnClientConnected(uint64_t nClientID)
{
	std::cout << "ClientConnected Play:" << nClientID << std::endl;
}

void PlayModule::OnVecProc(const uint64_t nClientID, const uint32_t nMsgID, const char* msg1, const uint32_t nLen)
 {
	
	Msg_VEC_C2S pMsg;
	pMsg.ParseFromArray(msg1 + 4, nLen);
	

	
		Msg_VEC_S2C msg_vec1;
		//Msg_VEC_S2C msg_vec2;

		float a = 0; float b = 0;int type = 0;bool taken;bool hide = false;
		if (pMsg.ingrass() == 1)
		{
			hide = true;
		}


	
		std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();

		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		msg_vec1.set_x(pMsg.x());
		msg_vec1.set_y(pMsg.y());
		msg_vec1.set_direction(pMsg.direction());
		msg_vec1.set_id(nClientID);
		while (itr != m_pDataBase->g_player.end())//将玩家发来的位置信息储存到服务器中
		{

			if ((*itr)->ID == nClientID)
			{

				(*itr)->LOCATIONX = pMsg.x();
				(*itr)->LOCATIONY = pMsg.y();
				
				break;
			}
	
			++itr;
		}

		while (itr_room != m_pDataBase->g_room.end())//
		{
			if ((*itr_room)->ID == (*itr)->ROOM)
			{
				if ((*itr)->TEAM == 0)
				{
					std::list<PLAYER*>::iterator itr_playerinroom = (*itr_room)->PLAYER_BLUE_LIST.begin();
					std::list<PLAYER*>::iterator itr_playerinroom1 = (*itr_room)->PLAYER_RED_LIST.begin();
					if (hide)
					{
						msg_vec1.set_hide(1);
					}
					else
					{
						msg_vec1.set_hide(0);
					}
					while (itr_playerinroom != (*itr_room)->PLAYER_BLUE_LIST.end())
					{
						if ((*itr_playerinroom)->ID == (*itr)->ID)
						{
							(*itr_playerinroom)->LOCATIONX = pMsg.x();
							(*itr_playerinroom)->LOCATIONY = pMsg.y();

						}
						
							m_pNetServer->SendMsg((*itr_playerinroom)->ID, MsgType::VEC_S2C, msg_vec1);
							printf("发送包");
					
						++itr_playerinroom;
					}
					if (hide)
					{
						msg_vec1.set_hide(2);
					}
					else
					{
						msg_vec1.set_hide(0);
					}
					while (itr_playerinroom1 != (*itr_room)->PLAYER_RED_LIST.end())
					{
						m_pNetServer->SendMsg((*itr_playerinroom1)->ID, MsgType::VEC_S2C, msg_vec1);
						printf("发送包");
						++itr_playerinroom1;
					}
				}
				else if ((*itr)->TEAM == 1)
				{
					std::list<PLAYER*>::iterator itr_playerinroom = (*itr_room)->PLAYER_RED_LIST.begin();
					std::list<PLAYER*>::iterator itr_playerinroom1 = (*itr_room)->PLAYER_BLUE_LIST.begin();
					if (hide)
					{
						msg_vec1.set_hide(1);
					}
					else
					{
						msg_vec1.set_hide(0);
					}
					while (itr_playerinroom != (*itr_room)->PLAYER_RED_LIST.end())
					{
						if ((*itr_playerinroom)->ID == (*itr)->ID)
						{
							(*itr_playerinroom)->LOCATIONX = pMsg.x();
							(*itr_playerinroom)->LOCATIONY = pMsg.y();
						
						}
					
							m_pNetServer->SendMsg((*itr_playerinroom)->ID, MsgType::VEC_S2C, msg_vec1);
							printf("发送包");
						++itr_playerinroom;
					}
					if (hide)
					{
						msg_vec1.set_hide(2);
					}
					else
					{
						msg_vec1.set_hide(0);
					}
					while (itr_playerinroom1 != (*itr_room)->PLAYER_BLUE_LIST.end())
					{
						m_pNetServer->SendMsg((*itr_playerinroom1)->ID, MsgType::VEC_S2C, msg_vec1);
						printf("发送包");
						++itr_playerinroom1;
					}
				}
				break;
			}
			++itr_room;
		}
	
	
		//itr = m_pDataBase->g_player.begin();
		//std::list<Item*>::iterator itr1 = (*itr_room)->g_Item.begin();
		//for (;itr1 != (*itr_room)->g_Item.end();itr1++)//道具的处理
		//{


		//	a = (*itr1)->x;

		//	b = (*itr1)->y;
		//	type = (*itr1)->kind;
		//	taken = (*itr1)->taken;
		//	if (((pMsg.x() + 0.05) > a && (pMsg.x() - 0.05) < a) && ((pMsg.y() + 0.05) > b && (pMsg.y() -0.05) < b) && !taken)
		//	{

		//		while (itr != m_pDataBase->g_player.end())
		//		{

		//			if ((*itr)->ID == nClientID)
		//			{
		//				Msg_GETITEM_S2C msgitem;
		//				if (type == 1)
		//				{
		//					msgitem.set_kind(1);
		//					if ((*itr)->item1 >= 2)
		//					{
		//					
		//						msgitem.set_result(-1);
		//					}
		//					else if ((*itr)->item1 <= 2)
		//					{
		//						(*itr)->item1++;
		//				
		//						(*itr1)->taken = true;
		//						msgitem.set_result(0);
		//					}
		//				}
		//				else if (type == 2)
		//				{
		//					msgitem.set_kind(2);
		//					if ((*itr)->item2 >= 2)
		//					{
		//					
		//						msgitem.set_result(-1);
		//					}
		//					else if ((*itr)->item2 <= 2)
		//					{
		//						(*itr)->item2++;
		//					
		//						(*itr1)->taken = true;
		//						msgitem.set_result(0);
		//					}
		//				}
		//				m_pNetServer->SendMsg(nClientID, MsgType::GetItem_S2C, msgitem);
		//				break;
		//			}

		//			++itr;
		//		}
		//	}


		//}


}


void PlayModule::OnAmmoProc(const uint64_t nClientID, const uint32_t nMsgID, const char* msg1, const uint32_t nLen)
{
	Msg_AMMO_C2S pMsg;
	pMsg.ParseFromArray(msg1 + 4, nLen);
	if (m_pDataBase->g_room.size() > 0 && m_pDataBase->g_player.size() > 0)
	{

		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		Msg_AMMO_S2C msg_d;
		msg_d.set_id(nClientID);
	//	std::list<PLAYER*>::iterator itr_shooter = m_pDataBase->g_player.begin();
		msg_d.set_direction(pMsg.direction());
		msg_d.set_type(pMsg.type());
		msg_d.set_x(pMsg.x());
		msg_d.set_y(pMsg.y());
		/*while (itr_shooter != m_pDataBase->g_player.begin())
		{
			if ((*itr_shooter)->ID == nClientID)
				break;
			++itr_shooter;
		}*/
		while (itr_room != m_pDataBase->g_room.end())//向房间内其他玩家转发消息
		{
			std::map<int, int>::iterator itr_map = (*itr_room)->PLAYER.find((nClientID));
			if (itr_map == (*itr_room)->PLAYER.end())
			{
				continue;
			}
			else
			{
				itr_map = (*itr_room)->PLAYER.begin();
				while (itr_map != (*itr_room)->PLAYER.end())
				{
					
					if (itr_map->first != nClientID)
					{
						m_pNetServer->SendMsg(itr_map->first, MsgType::Ammo_S2C, msg_d);
					}
					++itr_map;
				}
			}
			++itr_room;
		}
	}

	
}

void PlayModule::DamageProc(const uint64_t nClientID, const uint32_t nMsgID, const char* msg1, const uint32_t nLen)
{
	Msg_DAMAGE_C2S pMsg;
	pMsg.ParseFromArray(msg1 + 4, nLen);
	if (m_pDataBase->g_room.size() > 0 && m_pDataBase->g_player.size() > 0)
	{
		int flag=0;
		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
		Msg_ADDSCORES_S2C msg_s;
		while (itr != m_pDataBase->g_player.end())
		{
			if ((*itr)->ID == nClientID)
			{
				break;
			}
			++itr;
		}
	
		while (itr_room != m_pDataBase->g_room.end())//向房间内其他玩家转发消息
		{
			if ((*itr_room)->ID == (*itr)->ROOM)
			{
				break;
			}
			++itr_room;
		}

		std::list<PLAYER*>::iterator itr_blue = (*itr_room)->PLAYER_BLUE_LIST.begin();
		
		while (itr_blue != (*itr_room)->PLAYER_BLUE_LIST.end())
		{
			if ((*itr_blue)->ID == pMsg.id_hurt())
			{
				int temp = (*itr_blue)->HP;
				(*itr_blue)->HP = temp - pMsg.damage();
				printf("蓝色被击中！");
				if ((*itr_blue)->HP <= 0)
				{
					(*itr_blue)->DEAD = 1;
					flag = 1;
				
				}
				else
				{
			
					flag = 0;
				}
				std::list<PLAYER*>::iterator itr_red = (*itr_room)->PLAYER_RED_LIST.begin();
				 int temp1 = (*itr_red)->scores;
				(*itr_red)->scores = temp1 + 10;
				msg_s.set_socres((*itr_red)->scores);
				msg_s.set_id((*itr_red)->ID);
				
			}
			/*if ((*itr_blue)->ID == nClientID)
			{
				int temp = (*itr_blue)->scores;
				(*itr_blue)->scores = temp + 10;
				printf("蓝色加分！\n");
				msg_s.set_socres((*itr_blue)->scores);
				msg_s.set_id(nClientID);
			}*/
			++itr_blue;
		}
		std::list<PLAYER*>::iterator itr_red = (*itr_room)->PLAYER_RED_LIST.begin();
		while (itr_red != (*itr_room)->PLAYER_RED_LIST.end())
		{
			if ((*itr_red)->ID == pMsg.id_hurt())
			{
				int temp = (*itr_red)->HP;
				(*itr_red)->HP = temp - pMsg.damage();
				printf("红色被击中！");
				if ((*itr_red)->HP <= 0)
				{
					(*itr_red)->DEAD = 1;
					flag = 1;
				}
				else
				{

					flag = 0;
				}
				itr_blue = (*itr_room)->PLAYER_BLUE_LIST.begin();
				int temp1 = (*itr_blue)->scores;
				(*itr_blue)->scores = temp1 + 10;
				msg_s.set_socres((*itr_blue)->scores);
				msg_s.set_id((*itr_blue)->ID);
			}
		
			/*if ((*itr_red)->ID == nClientID)
			{
				int temp = (*itr_red)->scores;
				(*itr_red)->scores = temp + 10;
				printf("红色加分！\n");
				msg_s.set_socres((*itr_red)->scores);
				msg_s.set_id(nClientID);
			}*/
			++itr_red;
			
		}
		std::map<int, int>::iterator itr_map = (*itr_room)->PLAYER.begin();
		if (flag == 0)
		{
			Msg_DAMAGE_S2C msg_d;
			msg_d.set_damage(pMsg.damage());
			msg_d.set_id(pMsg.id_hurt());
			while (itr_map != (*itr_room)->PLAYER.end())
			{
			    m_pNetServer->SendMsg(itr_map->first, MsgType::AddSocres, msg_s);
				m_pNetServer->SendMsg(itr_map->first, MsgType::Damage_S2C, msg_d);
				++itr_map;
			}
		}
		else
		{
			Msg_DEATH_S2C msg_d;
			msg_d.set_id(pMsg.id_hurt());
			while (itr_map != (*itr_room)->PLAYER.end())
			{
				m_pNetServer->SendMsg(itr_map->first, MsgType::AddSocres, msg_s);
				//m_pNetServer->SendMsg(itr_map->first, MsgType::Death_S2C, msg_d);
				++itr_map;
			}
		}
	}

}
void PlayModule::LoadPoint()
{
	std::list<Point2*>::iterator itr = g_point.begin();
	
	auto pconfig = m_pDataService->QueryRecord<MapDefine>(101);
	
	Point2 *point = new Point2[3];
	point[0] = pconfig->PointVec[0];
	point[1] = pconfig->PointVec[1];
	point[2] = pconfig->PointVec[2];
	printf("正在获取据点的坐标。。。\n");

	

	while (point->s==0)
	{
		Point2 *temp = new Point2();
		temp->x1 = point->x1;
		temp->x2 = point->x2;
		temp->y1 = point->y1;
		temp->y2 = point->y2;
	
		g_point.push_back(temp);
		//printf("%f %f %f %f\n", point->x1,  point->y1,point->x2, point->y2);
		point++;
	}
	for (int i = 0;i < 3;i++)
	{
		point--;
	}
	delete point;
	point =nullptr;
}
void PlayModule::pointProc()
{
	std::list<ROOM*>::iterator itr_room;
	std::list<Point2*>::iterator itr = g_point.begin();

	auto pconfig = m_pDataService->QueryRecord<MapDefine>(101);
	bool RedIn = false;
	bool BlueIn = false;
	Point2 *point = new Point2;
	for (itr_room = m_pDataBase->g_room.begin();itr_room != m_pDataBase->g_room.end();itr_room++)
	{
		std::list<PLAYER*>::iterator itr_blue_list = (*itr_room)->PLAYER_BLUE_LIST.begin();
		std::list<PLAYER*>::iterator itr_red_list= (*itr_room)->PLAYER_RED_LIST.begin();

	
		point = &(pconfig->PointVec[(*itr_room)->pointtaken]);
		if ((*itr_room)->pointtaken > 3)//据点三个都已经占据游戏结束
		{
			(*itr_room)->GAMEEND_FLAG = true;
			Msg_ENDGAME_S2C msg_end;
			msg_end.set_result(1);
			std::map<int, int>::iterator itr_player_map = (*itr_room)->PLAYER.begin();
			while (itr_player_map != (*itr_room)->PLAYER.end())
			{
				m_pNetServer->SendMsg(itr_player_map->first, MsgType::EndGame,msg_end);
				itr_player_map++;
			}
			continue;
		}
		while (itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end())
		{
			if ((*itr_blue_list)->LOCATIONX>point->x1&& (*itr_blue_list)->LOCATIONX<point->x2&& (*itr_blue_list)->LOCATIONY<point->y1&& (*itr_blue_list)->LOCATIONY>point->y2)
			{
				BlueIn = true;
				break;
			}
			itr_blue_list++;
		}
		while (itr_red_list != (*itr_room)->PLAYER_RED_LIST.end())
		{
			if ((*itr_red_list)->LOCATIONX>point->x1&& (*itr_red_list)->LOCATIONX < point->x2&&(*itr_red_list)->LOCATIONY<point->y1&& (*itr_red_list)->LOCATIONY > point->y2)
			{
				RedIn = true;
				break;
			}
			itr_red_list++;
		}
	
		
		Msg_POINTTAKING_S2C msg_taking;
		Msg_POINTTAKEN_S2C msg_taken;
		if (BlueIn && (RedIn == false))
		{
		

			if ((*itr_room)->pointowner == -1|| (*itr_room)->pointowner==0)
			{
			
				(*itr_room)->pointowner = 0;
				int temp = (*itr_room)->pointpercent;
				(*itr_room)->pointpercent = temp + 5;
				msg_taking.set_percent((*itr_room)->pointpercent);
				msg_taking.set_team(0);
				for (itr_blue_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();itr_blue_list++)
				{
					m_pNetServer->SendMsg((*itr_blue_list)->ID, MsgType::PointTaking, msg_taking);
				}
				for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();itr_red_list++)
				{
					m_pNetServer->SendMsg((*itr_red_list)->ID, MsgType::PointTaking, msg_taking);
				}
				
			}
			else if ((*itr_room)->pointowner == 1)
			{
				if ((*itr_room)->pointpercent <= 0)
					(*itr_room)->pointowner = 0;

				int temp = (*itr_room)->pointpercent;
				(*itr_room)->pointpercent = temp- 10;
				msg_taking.set_percent((*itr_room)->pointpercent);
				msg_taking.set_team(1);
				for (itr_blue_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();itr_blue_list++)
				{
					m_pNetServer->SendMsg((*itr_blue_list)->ID, MsgType::PointTaking, msg_taking);
				}
				for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();itr_red_list++)
				{
					m_pNetServer->SendMsg((*itr_red_list)->ID, MsgType::PointTaking, msg_taking);
				}
			}
			if ((*itr_room)->pointpercent >= 100)
			{
			
				std::list<PLAYER*>::iterator itr_blue_scores = (*itr_room)->PLAYER_BLUE_LIST.begin();
				while (itr_blue_scores != (*itr_room)->PLAYER_BLUE_LIST.end())
				{
					int temp = (*itr_blue_scores)->scores;
					(*itr_blue_scores)->scores = temp + 50;
					itr_blue_scores++;
				}
				
				(*itr_room)->bluescores++;
				(*itr_room)->pointtaken++;
				(*itr_room)->pointpercent = 0;
				(*itr_room)->pointowner = -1;
				msg_taken.set_team(0);
				msg_taken.set_pointnumber((*itr_room)->pointtaken);
				for (itr_blue_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();itr_blue_list++)
				{
					m_pNetServer->SendMsg((*itr_blue_list)->ID, MsgType::PointTaken, msg_taken);
				}
				for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();itr_red_list++)
				{
					m_pNetServer->SendMsg((*itr_red_list)->ID, MsgType::PointTaken, msg_taken);
				}
			}
		}
		else if (RedIn && (BlueIn == false))
		{
			if ((*itr_room)->pointowner == -1 || (*itr_room)->pointowner == 1)
			{
				(*itr_room)->pointowner = 1;
				int temp = (*itr_room)->pointpercent;
				(*itr_room)->pointpercent = temp + 5;
				msg_taking.set_percent((*itr_room)->pointpercent);
				msg_taking.set_team(1);
				for (itr_blue_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();itr_blue_list++)
				{
					m_pNetServer->SendMsg((*itr_blue_list)->ID, MsgType::PointTaking, msg_taking);
				}
				for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();itr_red_list++)
				{
					m_pNetServer->SendMsg((*itr_red_list)->ID, MsgType::PointTaking, msg_taking);
				}
			}
			else if ((*itr_room)->pointowner == 0)
			{
				if ((*itr_room)->pointpercent <= 0)
					(*itr_room)->pointowner = 1;

				int temp = (*itr_room)->pointpercent;
				(*itr_room)->pointpercent = temp - 10;
				msg_taking.set_percent((*itr_room)->pointpercent);
				msg_taking.set_team(0);
				for (itr_blue_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();itr_blue_list++)
				{
					m_pNetServer->SendMsg((*itr_blue_list)->ID, MsgType::PointTaking, msg_taking);
				}
				for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();itr_red_list++)
				{
					m_pNetServer->SendMsg((*itr_red_list)->ID, MsgType::PointTaking, msg_taking);
				}
			}
			if ((*itr_room)->pointpercent >= 100)
			{
				std::list<PLAYER*>::iterator itr_red_scores = (*itr_room)->PLAYER_RED_LIST.begin();
				while (itr_red_scores != (*itr_room)->PLAYER_RED_LIST.end())
				{
					int temp = (*itr_red_scores)->scores;
					(*itr_red_scores)->scores = temp + 50;
					itr_red_scores++;
				}
				(*itr_room)->redscores++;
				(*itr_room)->pointtaken++;
				(*itr_room)->pointpercent = 0;
				(*itr_room)->pointowner = -1;
				msg_taken.set_team(1);
				msg_taken.set_pointnumber((*itr_room)->pointtaken);
				for (itr_blue_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();itr_blue_list++)
				{
					m_pNetServer->SendMsg((*itr_blue_list)->ID, MsgType::PointTaken, msg_taken);
				}
				for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();itr_red_list++)
				{
					m_pNetServer->SendMsg((*itr_red_list)->ID, MsgType::PointTaken, msg_taken);
				}
			}
		}
		else if (RedIn&&BlueIn)
		{
		Msg_POINTDESPUTE_S2C msg_dispute;
		msg_dispute.set_result(1);
		for (itr_blue_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();itr_blue_list++)
		{
			m_pNetServer->SendMsg((*itr_blue_list)->ID, MsgType::PointDispute, msg_dispute);
		}
		for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();itr_red_list++)
		{
			m_pNetServer->SendMsg((*itr_red_list)->ID, MsgType::PointDispute, msg_dispute);
		}
			printf("争夺中。。。");

		}
		delete point;
		point = nullptr;
	}
}
void PlayModule::EndProc()
{
	std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
	while (itr_room != m_pDataBase->g_room.end())
	{
		std::list<PLAYER*>::iterator itr_blue = (*itr_room)->PLAYER_BLUE_LIST.begin();
		std::list<PLAYER*>::iterator itr_red = (*itr_room)->PLAYER_RED_LIST.begin();
		int blue_scores = 0;
		int red_scores = 0;
		while (itr_blue != (*itr_room)->PLAYER_BLUE_LIST.end())
		{
			blue_scores = blue_scores + (*itr_blue)->scores;
			++itr_blue;
		}
		while (itr_red != (*itr_room)->PLAYER_RED_LIST.end())
		{
			red_scores = red_scores + (*itr_red)->scores;
			++itr_red;
		}
		if ((blue_scores > 40 || red_scores > 40)&& (*itr_room)->GAMEEND_FLAG==false)
		{
			std::map<int, int >::iterator itr_map= (*itr_room)->PLAYER.begin();
			Msg_ENDGAME_S2C msg_e;
			msg_e.set_result(1);
			msg_e.set_team(blue_scores > red_scores ? 0 : 1);
			printf("RRRR%dRRRR", blue_scores);
			printf("BBBB%dBBBB", red_scores);
			printf("___%d____", msg_e.team());
			while (itr_map != (*itr_room)->PLAYER.end())
			{
				m_pNetServer->SendMsg(itr_map->first, MsgType::EndGame, msg_e);
				++itr_map;
			}
			(*itr_room)->GAMEEND_FLAG = true;
		}
			++itr_room;
			
	}
}
void PlayModule::OnUserCreateRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_CreateRoom_C2S pMsg;
	
	pMsg.ParseFromArray(msg + 4, nLen);
	if (m_pDataBase->g_player.size() > 0)
	{
		
		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		bool flag = true;
		while (itr_room != m_pDataBase->g_room.end())
		{

			if ((*itr_room)->ID == pMsg.id())//如果存在该id号的房间
			{
				flag = false;//则创建房间失败
			}
			++itr_room;
		}


		if (flag == false)//房间已经被创建
		{
			printf("已经有相同的房间了！\n");
		}
		else
		{
			std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
			while (itr != m_pDataBase->g_player.end())
			{

				if ((*itr)->ID == nClientID)//通过id查找该玩家
				{
					//初始化数据结构
					ROOM *room = new ROOM();
					room->ID = pMsg.id();
					room->PLAYER[nClientID] = 0;
					//std::map<std::string, int>::iterator itr = room->PYAER_BLUE.find(pMsg->szName);
					room->PYAER_BLUE[(*itr)->USERNAME] = 0;
					room->MasterID = nClientID;
					room->MasterUsername = (*itr)->USERNAME;
					room->GAMEEND_FLAG = false;
					PLAYER *player = new PLAYER();
					player->ID = nClientID;
					player->TEAM = 0;
					player->USERNAME = (*itr)->USERNAME;
					player->MASTER = 1;
					player->HP = 100;
					player->scores = 0;
					player->DEAD = 0;
					room->PLAYER_BLUE_LIST.push_back(player);
					cout << "玩家" << (*itr)->USERNAME << "创建了房间" << pMsg.id() << endl;
				
					m_pDataBase->g_room.push_back(room);
				}
				++itr;
			}
		

		}
	}
	
	
}
void PlayModule::OnUserEnterRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_EnterRoom_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);
	
	if (m_pDataBase->g_player.size() > 0)
	{
		int count = 0;
		bool number[12] = { true };
		int match = 0;
		bool room_exist = false;
		std::list<ROOM*>::iterator itr = m_pDataBase->g_room.begin();
		std::list<PLAYER*>::iterator itr1 = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_room.end())
		{
			if ((*itr)->ID == pMsg.id())
			{
				room_exist = true;
				break;
			}
			++itr;
		}
		if (room_exist)
		{
			if ((*itr)->PLAYER.size() < 12) {//有空余位置
				int team;
				if ((*itr)->PYAER_BLUE.size() < 6)
					team = 0;
				else
					team = 1;//计算队伍值
				
				while (itr1 != m_pDataBase->g_player.end())
						{

							if ((*itr1)->ID == nClientID)//通过id查找该玩家
							{
								std::map<std::string, int>::iterator itr2 = (*itr)->PYAER_BLUE.find((*itr1)->USERNAME);
								std::map<std::string, int>::iterator itr3 = (*itr)->PYAER_RED.find((*itr1)->USERNAME);
								std::map<int, int>::iterator itr_player_map = (*itr)->PLAYER.find(nClientID);
								int temp1 = (*itr)->PLAYER.size();
								(*itr)->PLAYER[(nClientID)] = temp1;
								//初始化房间内的list
								if (team == 0)
								{
									if (itr2 == (*itr)->PYAER_BLUE.end() && itr3 == (*itr)->PYAER_RED.end())
									{
										int temp0 = (*itr)->PYAER_BLUE.size();
										(*itr)->PYAER_BLUE[(*itr1)->USERNAME] = temp0;
										int temp = (*itr)->PLAYER.size();
										(*itr)->PLAYER[(*itr1)->ID] = temp;
										PLAYER *player = new PLAYER();
										player->ID = nClientID;
										player->TEAM = 0;
										player->USERNAME = (*itr1)->USERNAME;
										player->HP = 100;
										player->scores = 0;
										(*itr)->PLAYER_BLUE_LIST.push_back(player);
										printf("蓝色阵营加入玩家\n");

										break;
									}
									else
									{


										break;
									}
								}
								else if (team == 1)
								{
									if (itr2 == (*itr)->PYAER_BLUE.end() && itr3 == (*itr)->PYAER_RED.end())
									{
										int temp0 = (*itr)->PYAER_RED.size();
										(*itr)->PYAER_RED[(*itr1)->USERNAME] = temp0;
										int temp = (*itr)->PLAYER.size();
										(*itr)->PLAYER[(*itr1)->ID] = temp;
										PLAYER *player = new PLAYER();
										player->ID = nClientID;
										player->TEAM = 0;
										player->USERNAME = (*itr1)->USERNAME;
										player->HP = 100;
										player->scores = 0;
										(*itr)->PLAYER_RED_LIST.push_back(player);


										break;

									}
									else
									{


										break;
									}
								}
								break;

							}
							itr1++;
						}
						
			}
			else//房间已满
			{

			}

		}
		
		else //不存在房间
		{
			printf("不存在房间");

		}
	}
	


}
void PlayModule::OnUserChangeState(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) 
{
	Msg_ChangeState_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);
	if (m_pDataBase->g_room.size() > 0 && m_pDataBase->g_player.size() > 0)
	{
		Msg_ChangeState_S2C msg_c;
		std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{
			if ((*itr)->ID == nClientID)
			{
				break;
			}
			++itr;
		}
		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		while(itr_room != m_pDataBase->g_room.end())
		{
			if ((*itr_room)->ID == (*itr)->ROOM)
			{
				break;
			}
			++itr_room;
		}
		if (pMsg.has_ready())
		{
			(*itr)->READY = pMsg.ready();
	
			
		}
		if (pMsg.has_type())
		{
			if (pMsg.type() == 1007 || pMsg.type() == 1008)//如果请求的是高级坦克
			{
				if ((*itr)->scores > 100)//
				{
					(*itr)->TYPE = pMsg.type();
					if ((*itr)->TEAM == 0)
					{
						std::list<PLAYER*>::iterator itr_blue=(*itr_room)->PLAYER_BLUE_LIST.begin();
						while (itr_blue != (*itr_room)->PLAYER_BLUE_LIST.end())
						{
							if ((*itr_blue)->ID == nClientID)
							{
								(*itr_blue)->TYPE = pMsg.type();
							}
							++itr_blue;
						}
					}
					else if ((*itr)->TEAM == 1)
						{
							std::list<PLAYER*>::iterator itr_red=(*itr_room)->PLAYER_RED_LIST.begin();
							while (itr_red!= (*itr_room)->PLAYER_RED_LIST.end())
							{
								if ((*itr_red)->ID == nClientID)
								{
									(*itr_red)->TYPE = pMsg.type();
								}
								++itr_red;
							}
						}
					
					
				}
				else
				{
					printf("玩家想使用高级坦克但是积分不够！\n");
				}
			}
			else//请求的是普通坦克
			{
				(*itr)->TYPE = pMsg.type();
				if ((*itr)->TEAM == 0)
				{
					std::list<PLAYER*>::iterator itr_blue = (*itr_room)->PLAYER_BLUE_LIST.begin();
					while (itr_blue != (*itr_room)->PLAYER_BLUE_LIST.end())
					{
						if ((*itr_blue)->ID == nClientID)
						{
							(*itr_blue)->TYPE = pMsg.type();
						}
						++itr_blue;
					}
				}
				else if ((*itr)->TEAM == 1)
				{
					std::list<PLAYER*>::iterator itr_red = (*itr_room)->PLAYER_RED_LIST.begin();
					while (itr_red != (*itr_room)->PLAYER_RED_LIST.end())
					{
						if ((*itr_red)->ID == nClientID)
						{
							(*itr_red)->TYPE = pMsg.type();
						}
						++itr_red;
					}
				}
			}
		}
		if (pMsg.has_number1() && pMsg.has_number2())
		{
		
			 if (pMsg.number2() <= 6)
			{
				
				std::list<PLAYER*>::iterator itr_red;
				for (itr_red = (*itr_room)->PLAYER_RED_LIST.begin();itr_red != (*itr_room)->PLAYER_RED_LIST.end();)
				{
					if ((*itr_red)->ID == nClientID)
					{
						PLAYER *player = new PLAYER();
						player->ID = (*itr_red)->ID;
						player->USERNAME = (*itr_red)->USERNAME;
						player->ROOM = (*itr_red)->ROOM;
						player->MASTER = (*itr_red)->MASTER;
						player->TYPE = (*itr_red)->TYPE;
						player->TEAM = 0;
						player->NUMBER = pMsg.number2();
						player->HP = 100;
						player->scores = 0;
						player->DEAD = 0;
						(*itr_room)->PLAYER_BLUE_LIST.push_back(player);
						(*itr_room)->PLAYER_RED_LIST.erase(itr_red++);
					}
					else
					{
						itr_red++;
					}
				
				}
				
			}
			else if ( pMsg.number2() > 6)
			{
				std::list<PLAYER*>::iterator itr_blue;
				for (itr_blue = (*itr_room)->PLAYER_BLUE_LIST.begin();itr_blue != (*itr_room)->PLAYER_BLUE_LIST.end();)
				{
					if ((*itr_blue)->ID == nClientID)
					{
						PLAYER *player = new PLAYER();
						player->ID = (*itr_blue)->ID;
						player->USERNAME = (*itr_blue)->USERNAME;
						player->ROOM = (*itr_blue)->ROOM;
						player->MASTER = (*itr_blue)->MASTER;
						player->TYPE = (*itr_blue)->TYPE;
						player->TEAM = 1;
						player->NUMBER = pMsg.number2();
						player->HP = 100;
						player->scores = 0;
						player->DEAD = 0;
						printf("红队加入新成员\n");
						(*itr_room)->PLAYER_RED_LIST.push_back(player);	
						(*itr_room)->PLAYER_BLUE_LIST.erase(itr_blue++);
					}
					else
					{
						itr_blue++;
					}
					
				}
				
				
			}
			else
			{

			}
		}
	}

}
void PlayModule::OnUserExpel(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_Expel_C2S pMsg;

	pMsg.ParseFromArray(msg + 4, nLen);

	if (m_pDataBase->g_room.size() > 0 && m_pDataBase->g_player.size() > 0)
	{
		int ret = -1;
		bool IsMaster = false;
	
		
		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		while (itr_room != m_pDataBase->g_room.end())//查询发消息这是否为房主
		{
			if ((*itr_room)->ID == pMsg.id())
			{
				std::list<PLAYER*>::iterator itr_blue_list;
				std::list<PLAYER*>::iterator itr_red_list;
	
				for (itr_blue_list = (*itr_room)->PLAYER_BLUE_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();itr_blue_list++)
				{
					if ((*itr_blue_list)->ID == nClientID)
					{
						
						if ((*itr_blue_list)->MASTER == 1)
						{
							IsMaster = 1;

							ret = 0;
						}
						
					}
					

				}
				for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();itr_red_list++)
				{
					if ((*itr_red_list)->ID == nClientID)
					{
					
						if ((*itr_red_list)->MASTER == 1)
						{
							IsMaster = 1;

							ret = 0;
						}
					
						
					}
				
				}

				break;
			}

		}
		if (IsMaster)
		{
		
			itr_room = m_pDataBase->g_room.begin();
			while (itr_room != m_pDataBase->g_room.end())
			{
				if ((*itr_room)->ID == pMsg.id())
				{
					std::list<PLAYER*>::iterator itr_blue_list;
					std::list<PLAYER*>::iterator itr_red_list;
					std::map<int, int>::iterator itr_player_map = (*itr_room)->PLAYER.find(pMsg.id());
					for (itr_blue_list = (*itr_room)->PLAYER_BLUE_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();)
					{
						if ((*itr_blue_list)->ID ==pMsg.userid())
						{
							std::map<string, int>::iterator itr_blue_map = (*itr_room)->PYAER_BLUE.find((*itr_blue_list)->USERNAME);
							(*itr_room)->PLAYER_BLUE_LIST.erase(itr_blue_list++);
							while (itr_blue_map != (*itr_room)->PYAER_BLUE.end())
							{
								itr_blue_map++;
								itr_blue_map->second--;
							}
							while (itr_player_map != (*itr_room)->PLAYER.end())
							{
								itr_player_map++;
								itr_player_map->second--;
							}
							ret = 0;
						}
						else
						{
							itr_blue_list++;
						}

					}
					for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();)
					{
						if ((*itr_red_list)->ID ==pMsg.userid())
						{
							std::map<string, int>::iterator itr_red_map = (*itr_room)->PYAER_RED.find((*itr_red_list)->USERNAME);
							(*itr_room)->PLAYER_RED_LIST.erase(itr_red_list++);
							while (itr_red_map != (*itr_room)->PYAER_RED.end())
							{
								itr_red_map++;
								itr_red_map->second--;
							}
							while (itr_player_map != (*itr_room)->PLAYER.end())
							{
								itr_player_map++;
								itr_player_map->second--;
							}
							ret = 0;
						}
						else
						{
							itr_red_list++;
						}
					}

					break;
				}
				++itr_room;
			}
		}
		else
		{
			printf("玩家不是房主\n");
		}
		if (ret == 0) {//更新状态成功
			printf("踢人成功\n");
		}

		else //更新状态失败
		{
			printf("踢人失败\n");
		}
	}
	
}
void PlayModule::OnUserStartGame(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_StartGame_C2S pMsg;
	
	pMsg.ParseFromArray(msg + 4, nLen);
	if (m_pDataBase->g_room.size() > 0 && m_pDataBase->g_player.size() > 0)
	{
		int count = 0;
		bool flag = true;
		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ROOM == pMsg.id())//查找房间内所有玩家
			{
				count++;
				if ((*itr)->READY != 1)
					flag = false;//有玩家没有准备
			}
			++itr;
		}

		while (itr_room != m_pDataBase->g_room.end())
		{
			if ((*itr_room)->ID == pMsg.id())
			{

				break;
			}
			++itr_room;
		}
		std::list<PLAYER*>::iterator itr_blue = (*itr_room)->PLAYER_BLUE_LIST.begin();
		std::list<PLAYER*>::iterator itr_red = (*itr_room)->PLAYER_RED_LIST.begin();
		if  ((*itr_room)->PLAYER_BLUE_LIST.empty() == true || (*itr_room)->PLAYER_RED_LIST.empty() == true || flag == false)//玩家人数不足或有人未准备
		{// 
			printf("玩家人数不足或有人未准备");

		}
		else {
			//开始游戏成功
			
			(*itr_room)->GAMESTART_FLAG = true;
			printf("&&&&&&&&&房间%d开始游戏！&&&&&&\n", pMsg.id());
		
			Msg_USERINFO_S2C *user_info;
			MSG_USERINFOLISTOS_S2C user_info_list;
			while (itr_blue != (*itr_room)->PLAYER_BLUE_LIST.end())
			{
				user_info = user_info_list.add_userinfo();
				user_info->set_hp((*itr_blue)->HP);
				user_info->set_id((*itr_blue)->ID);
				user_info->set_username((*itr_blue)->USERNAME);
				user_info->set_type((*itr_blue)->TYPE);
				user_info->set_team((*itr_blue)->TEAM);
				++itr_blue;
			}
			while (itr_red != (*itr_room)->PLAYER_RED_LIST.end())
			{
				user_info = user_info_list.add_userinfo();
				user_info->set_hp((*itr_red)->HP);
				user_info->set_id((*itr_red)->ID);
				user_info->set_username((*itr_red)->USERNAME);
				user_info->set_type((*itr_red)->TYPE);
				user_info->set_team((*itr_red)->TEAM);
				++itr_red;
			}
			
			
			std::map<int, int>::iterator itr_map = (*itr_room)->PLAYER.begin();
		
			while (itr_map != (*itr_room)->PLAYER.end())
			{

				m_pNetServer->SendMsg(itr_map->first, MsgType::UserInfoOS, user_info_list);
				++itr_map;
			}
		}
	}
	
}
void PlayModule::OnUserExitRoom(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen) {
	Msg_ExitRoom_C2S pMsg;
	int ret = -1;

	pMsg.ParseFromArray(msg + 4, nLen);
	if (m_pDataBase->g_room.size() > 0 && m_pDataBase->g_player.size() > 0)
	{
	
		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		while (itr_room != m_pDataBase->g_room.end())
		{
			if ((*itr_room)->ID == pMsg.id())
			{
				std::list<PLAYER*>::iterator itr_blue_list;
				std::list<PLAYER*>::iterator itr_red_list ;
				std::map<int, int>::iterator itr_player_map = (*itr_room)->PLAYER.find(nClientID);
				for(itr_blue_list = (*itr_room)->PLAYER_BLUE_LIST.begin();itr_blue_list != (*itr_room)->PLAYER_BLUE_LIST.end();)
				{
					if ((*itr_blue_list)->ID == nClientID)
					{
						std::map<string, int>::iterator itr_blue_map = (*itr_room)->PYAER_BLUE.find((*itr_blue_list)->USERNAME);
						(*itr_room)->PLAYER_BLUE_LIST.erase(itr_blue_list++);
						while (itr_blue_map != (*itr_room)->PYAER_BLUE.end())
						{
							itr_blue_map++;
							itr_blue_map->second--;
						}
						while (itr_player_map != (*itr_room)->PLAYER.end())
						{
							itr_player_map++;
							itr_player_map->second--;
						}
						ret = 0;
					}
					else
					{
						itr_blue_list++;
					}
					
				}
				for (itr_red_list = (*itr_room)->PLAYER_RED_LIST.begin();itr_red_list != (*itr_room)->PLAYER_RED_LIST.end();)
				{
					if ((*itr_red_list)->ID == nClientID)
					{
						std::map<string, int>::iterator itr_red_map = (*itr_room)->PYAER_RED.find((*itr_red_list)->USERNAME);
						(*itr_room)->PLAYER_RED_LIST.erase(itr_red_list++);
						while (itr_red_map != (*itr_room)->PYAER_RED.end())
						{
							itr_red_map++;
							itr_red_map->second--;
						}
						while (itr_player_map != (*itr_room)->PLAYER.end())
						{
							itr_player_map++;
							itr_player_map->second--;
						}
						ret = 0;
					}
					else
					{
						itr_red_list++;
					}
				}

				break;
			}
			++itr_room;
		}

		if (ret == 0) {//退出房间成功

		}

		else //退出房间失败
		{


		}
	}


	

	
}
void PlayModule::RoomInfoRequest(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen)
{
	Msg_ROOMINFOREQUEST_C2S pMsg;

	pMsg.ParseFromArray(msg + 4, nLen);

	if (m_pDataBase->g_player.size() > 0)
	{
		Msg_ROOMLIST_S2C msg_room_list;
		Msg_ROOMINFO_S2C *msg_roominfo;
		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		if (m_pDataBase->g_room.empty())
		{

			msg_room_list.set_result(-1);
		}
		else
		{
			while (itr_room != m_pDataBase->g_room.end())
			{


				msg_roominfo = msg_room_list.add_room();
				msg_roominfo->set_masterid((*itr_room)->MasterID);
				msg_roominfo->set_masterusername((*itr_room)->MasterUsername);
				msg_roominfo->set_roomid((*itr_room)->ID);

				++itr_room;
			}
			msg_room_list.set_result(0);
		}
		m_pNetServer->SendMsg(nClientID, MsgType::RoomList, msg_room_list);
	}
	

}
void PlayModule::OnInfoAc(const uint64_t nClientID, const uint32_t nMsgID, const char* msg, const uint32_t nLen)
{
	MSG_INFOAC_C2S pMsg;
	pMsg.ParseFromArray(msg + 4, nLen);
	if (m_pDataBase->g_room.size() > 0 && m_pDataBase->g_player.size() > 0)
	{
	
		std::list<ROOM*>::iterator itr_room = m_pDataBase->g_room.begin();
		std::list<PLAYER*>::iterator itr = m_pDataBase->g_player.begin();
		while (itr != m_pDataBase->g_player.end())
		{

			if ((*itr)->ID== nClientID)
			{
				break;
			}
			++itr;
		}

		while (itr_room != m_pDataBase->g_room.end())
		{
			if ((*itr_room)->ID == (*itr)->ROOM)
			{

				break;
			}
			++itr_room;
		}
		std::list<PLAYER*>::iterator itr_blue = (*itr_room)->PLAYER_BLUE_LIST.begin();
		std::list<PLAYER*>::iterator itr_red = (*itr_room)->PLAYER_RED_LIST.begin();
		
	
			//开始游戏成功


			MSG_VECOS_S2C* uservec;
			MSG_VECOSLIST_S2C veclist;
			float xos = 10.0;
			while (itr_blue != (*itr_room)->PLAYER_BLUE_LIST.end())
			{
				xos = xos + 5;
				uservec = veclist.add_vec();
				uservec->set_id((*itr_blue)->ID);
				uservec->set_direction(2);
				uservec->set_ingrass(2);
				uservec->set_x(xos);
				uservec->set_y(-10.0);
				printf("---------blue%d---------", (*itr_blue)->ID);
				++itr_blue;
			}
			xos = 10.0;
			while (itr_red != (*itr_room)->PLAYER_RED_LIST.end())
			{
				xos = xos + 5;
				uservec = veclist.add_vec();
				uservec->set_id((*itr_red)->ID);
				uservec->set_direction(1);
				uservec->set_ingrass(2);
				uservec->set_x(xos);
				uservec->set_y(-15.0);
				printf("---------red1---------", (*itr_red)->ID);
				++itr_red;
			}

			std::map<int, int>::iterator itr_map = (*itr_room)->PLAYER.begin();

			while (itr_map != (*itr_room)->PLAYER.end())
			{

				m_pNetServer->SendMsg(itr_map->first, MsgType::VecOsList, veclist);
				++itr_map;
			}
		}
	
}
void PlayModule::OnClientLeave(uint64_t nClientID)
{
	std::cout << "ClientDisConnected Play:" << nClientID << std::endl;
}
