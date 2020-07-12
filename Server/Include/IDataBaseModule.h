#pragma once
#include "IModule.h"
#include<string>
#include <list>
#include<map>
#include <DataService/IDataService.h>
#include <DataService/automake/MapConfig.h>
#include <DataService/automake/ItemConfig.h>
#define MAX_NAME 200
using namespace std;

class Item
{
public:
	Item()
	{
		float x = 0.0;
		float y = 0.0;
		int kind = 0;
		bool taken = false;
	}
	float x;
	float y;
	int kind;
	bool taken;
};

class PLAYER
{
public:
	PLAYER() {
		ID = -1; USERNAME = "0"; ROOM = 0; NUMBER = 0; MASTER = 0; LOCATIONX = 0; LOCATIONY = 0; TEAM = 0; DIRECTION = 0;
		READY = 0; TYPE = 1;AMMOTYPE = 1;int HP = 100; DEAD = 0;item1 = 0;item2 = 0;scores = 0;
	}

	int ID;

	string USERNAME;
	int ROOM;
	int NUMBER;
	int MASTER;
	float LOCATIONX;
	float LOCATIONY;
	int TEAM;
	int DIRECTION;
	int READY;
	int TYPE;
	int AMMOTYPE;
	int HP;
	int DEAD;
	int item1;
	int item2;
	int scores;
};
class ROOM
{
public:ROOM()
{
	GAMEEND_FLAG = false;
	GAMESTART_FLAG = false;
	pointtaken = 0;
	NUMBER = 0;
	pointowner = -1;
	pointpercent = 0;
	bluescores = 0;
	redscores = 0;
	//PYAER_BLUE.insert(make_pair("head", 0));
	//PYAER_RED.insert(make_pair("head", 0));
	//PLAYER.insert(make_pair(-1, 0));
	//class PLAYER *player = new class PLAYER();
	//PLAYER_BLUE_LIST.push_back(player);
	//PLAYER_RED_LIST.push_back(player);
	//class Item *item = new class Item();
	//g_Item.push_back(item);
}
	   int ID;
	   int MasterID;
	   string MasterUsername;
	   std::map<std::string, int>PYAER_BLUE;
	   std::map<std::string, int>PYAER_RED;
	   std::list<PLAYER*>PLAYER_BLUE_LIST;
	   std::list<PLAYER*>PLAYER_RED_LIST;
	   std::map<int, int>PLAYER;
	   std::list<Item*>g_Item;
	   bool GAMESTART_FLAG;
	   bool GAMEEND_FLAG;
	   int pointtaken;
	   int pointowner;// 0 blue 1 red
	   int pointpercent;
	   int NUMBER;
	   int bluescores;
	   int redscores;
public:void InitItem()
{
	std::list<Item*>::iterator itr = g_Item.begin();
	for (int i = 0;i < 8;i++)
	{
		Item  *item = new Item();
		item->x = 0.0;
		item->y = 0.0;
		item->kind = 0;
		item->taken = false;
		g_Item.push_back(item);
	}
}
public:void GenerateItem()
{
	int width = 0;
	int length = 0;
	//auto mapconfig = m_pDataService->QueryRecord<MapDefine>(101);
	width = 50;
	length =50;
	std::list<Item*>::iterator itr;


	for (itr = g_Item.begin();itr != g_Item.end();itr++)
	{
		float x = (float)(rand() % width);
		float y = (float)(rand() % length);
		int type = rand() % 2 + 1;
		(*itr)->x = x;
		(*itr)->y = y;
		(*itr)->kind = type;
		(*itr)->taken = false;
		//printf("%f %f %d %s\n", x, y, type, (*itr)->taken ? "taken" : "untaken");
	}
}
private:IDataService *m_pDataService;
};

class IDataBaseModule :
	public IModule
{
public:
	virtual void SayHellow() = 0;
	std::list<PLAYER*>g_player;
	std::list<ROOM*>g_room;
};
