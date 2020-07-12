#pragma once
#include <vector>

#include "AmmoConfig.h"
class AmmoTable : public BaseTable
{
public:
	AmmoTable() :BaseTable("Ammo.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new AmmoDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->ID);
		ReadValue(config->Damage);
		ReadValue(config->ArmorPiercing);
		id=config->ID;
		return (IData*)config;
	}
};
#include "AudioConfig3dConfig.h"
class AudioConfig3dTable : public BaseTable
{
public:
	AudioConfig3dTable() :BaseTable("AudioConfig3d.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new AudioConfig3dDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->ID);
		ReadValue(config->name);
		ReadValue(config->dopplerLevel);
		ReadValue(config->minDistance);
		ReadValue(config->maxDistance);
		ReadValue(config->spatialBlend);
		ReadValue(config->spread);
		ReadValue(config->curve);
		id=config->ID;
		return (IData*)config;
	}
};
#include "AudioGroupInfoConfig.h"
class AudioGroupInfoTable : public BaseTable
{
public:
	AudioGroupInfoTable() :BaseTable("AudioGroupInfo.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new AudioGroupInfoDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->ID);
		ReadValue(config->name);
		ReadValue(config->baseVolume);
		ReadValue(config->maxCount);
		ReadValue(config->maxCountAction);
		ReadValue(config->bgm);
		ReadValue(config->modifyGroup);
		ReadValue(config->modifyVolume);
		id=config->ID;
		return (IData*)config;
	}
};
#include "AudioInfoConfig.h"
class AudioInfoTable : public BaseTable
{
public:
	AudioInfoTable() :BaseTable("AudioInfo.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new AudioInfoDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->ID);
		ReadValue(config->name);
		ReadValue(config->group);
		ReadValue(config->mode);
		ReadValue(config->maxCount);
		ReadValue(config->maxCountAction);
		ReadValue(config->interval);
		ReadValue(config->volume);
		ReadValue(config->fadeTime);
		ReadValue(config->config3D);
		ReadValue(config->audios);
		id=config->ID;
		return (IData*)config;
	}
};
#include "ConfigConfig.h"
class ConfigTable : public BaseTable
{
public:
	ConfigTable() :BaseTable("Config.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new ConfigDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->id);
		ReadValue(config->serverIP);
		id=config->id;
		return (IData*)config;
	}
};
#include "MapConfig.h"
class MapTable : public BaseTable
{
public:
	MapTable() :BaseTable("Map.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new MapDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->ID);
		ReadValue(config->Width);
		ReadValue(config->Length);
		ReadValue(config->ClientDistance);
		ReadValue(config->MapTileSize);
		std::vector<std::string> vecPointVec;
		ReadValue(vecPointVec);
		for (auto value : vecPointVec)
		{
			auto PointVec = json::Deserialize(value);
			Point2 PointVecvalue;
			ReadValue(PointVec, "x1", PointVecvalue.x1);
			ReadValue(PointVec, "y1", PointVecvalue.y1);
			ReadValue(PointVec, "x2", PointVecvalue.x2);
			ReadValue(PointVec, "y2", PointVecvalue.y2);
			ReadValue(PointVec, "s", PointVecvalue.s);
			config->PointVec.push_back(PointVecvalue);
		}
		id=config->ID;
		return (IData*)config;
	}
};
#include "ItemConfig.h"
class ItemTable : public BaseTable
{
public:
	ItemTable() :BaseTable("Item.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new ItemDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->id);
		ReadValue(config->type);
		ReadValue(config->name);
		ReadValue(config->quality);
		id=config->id;
		return (IData*)config;
	}
};
#include "LanguageConfig.h"
class LanguageTable : public BaseTable
{
public:
	LanguageTable() :BaseTable("Language.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new LanguageDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->id);
		ReadValue(config->cn);
		ReadValue(config->en);
		ReadValue(config->arabic);
		ReadValue(config->tr);
		id=config->id;
		return (IData*)config;
	}
};
#include "LanguageSetConfig.h"
class LanguageSetTable : public BaseTable
{
public:
	LanguageSetTable() :BaseTable("LanguageSet.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new LanguageSetDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->ID);
		ReadValue(config->language);
		ReadValue(config->l_languageID);
		ReadValue(config->telephone);
		ReadValue(config->enumSwitch);
		ReadValue(config->translate);
		ReadValue(config->gameID);
		id=config->ID;
		return (IData*)config;
	}
};
#include "LoginConfigConfig.h"
class LoginConfigTable : public BaseTable
{
public:
	LoginConfigTable() :BaseTable("LoginConfig.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new LoginConfigDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->id);
		ReadValue(config->serverIP);
		ReadValue(config->health_lite);
		ReadValue(config->health_mid);
		ReadValue(config->health_heavy);
		ReadValue(config->load_lite);
		ReadValue(config->load_mid);
		ReadValue(config->load_heavy);
		id=config->id;
		return (IData*)config;
	}
};
#include "StaConfigConfig.h"
class StaConfigTable : public BaseTable
{
public:
	StaConfigTable() :BaseTable("StaConfig.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new StaConfigDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->sens_lite);
		ReadValue(config->sens_mid);
		ReadValue(config->sens_heavy);
		ReadValue(config->bullet_speed);
		ReadValue(config->heavy_cooldown);
		ReadValue(config->hide_alpha);
		ReadValue(config->expbullet_to_lite_front);
		ReadValue(config->expbullet_to_lite_side);
		ReadValue(config->expbullet_to_lite_back);
		ReadValue(config->expbullet_to_mid_front);
		ReadValue(config->expbullet_to_mid_side);
		ReadValue(config->expbullet_to_mid_back);
		ReadValue(config->expbullet_to_heavy_front);
		ReadValue(config->expbullet_to_heavy_side);
		ReadValue(config->expbullet_to_heavy_back);
		ReadValue(config->pierbullet_to_lite_front);
		ReadValue(config->pierbullet_to_lite_side);
		ReadValue(config->pierbullet_to_lite_back);
		ReadValue(config->pierbullet_to_mid_front);
		ReadValue(config->pierbullet_to_mid_side);
		ReadValue(config->pierbullet_to_mid_back);
		ReadValue(config->pierbullet_to_heavy_front);
		ReadValue(config->pierbullet_to_heavy_side);
		ReadValue(config->pierbullet_to_heavy_back);
		ReadValue(config->smogbullet_spread);
		ReadValue(config->smogbullet_time);
		ReadValue(config->smogbullet_cooldown);
		ReadValue(config->exitin_cooldown);
		ReadValue(config->fixtool_cooldown);
		ReadValue(config->extin_rate);
		ReadValue(config->fixtool_rate);
		ReadValue(config->smogbullet_rate);
		ReadValue(config->medp_rate);
		ReadValue(config->fired_ratio);
		ReadValue(config->fired_power);
		ReadValue(config->fired_time);
		ReadValue(config->shutdown_rate);
		ReadValue(config->shutdown_power);
		id=config->sens_lite;
		return (IData*)config;
	}
};
#include "TankConfig.h"
class TankTable : public BaseTable
{
public:
	TankTable() :BaseTable("Tank.bin") {}
	virtual IData* ReadRow(int &id) override
	{
		auto config = new TankDefine(); 
		int enumTmp;

		const char* data;

		ReadValue(config->ID);
		ReadValue(config->HP);
		ReadValue(config->Armor_front);
		ReadValue(config-> Armor_side);
		ReadValue(config-> Armor_back);
		ReadValue(config-> Mag);
		ReadValue(config->TankKind);
		ReadValue(config->TankSerial);
		ReadValue(config->Speed);
		ReadValue(config->MaxItem);
		ReadValue(config->ShootGap);
		ReadValue(config-> MoveGap);
		ReadValue(config->ReloadGap);
		id=config->ID;
		return (IData*)config;
	}
};
ITable* DataService::LoadTable(const char* name)
{
	BaseTable *pTable = nullptr;

	if(strcmp("class AmmoDefine", name)==0)
	{
		pTable = new AmmoTable();
	}
	else if(strcmp("class AudioConfig3dDefine", name)==0)
	{
		pTable = new AudioConfig3dTable();
	}
	else if(strcmp("class AudioGroupInfoDefine", name)==0)
	{
		pTable = new AudioGroupInfoTable();
	}
	else if(strcmp("class AudioInfoDefine", name)==0)
	{
		pTable = new AudioInfoTable();
	}
	else if(strcmp("class ConfigDefine", name)==0)
	{
		pTable = new ConfigTable();
	}
	else if(strcmp("class MapDefine", name)==0)
	{
		pTable = new MapTable();
	}
	else if(strcmp("class ItemDefine", name)==0)
	{
		pTable = new ItemTable();
	}
	else if(strcmp("class LanguageDefine", name)==0)
	{
		pTable = new LanguageTable();
	}
	else if(strcmp("class LanguageSetDefine", name)==0)
	{
		pTable = new LanguageSetTable();
	}
	else if(strcmp("class LoginConfigDefine", name)==0)
	{
		pTable = new LoginConfigTable();
	}
	else if(strcmp("class StaConfigDefine", name)==0)
	{
		pTable = new StaConfigTable();
	}
	else if(strcmp("class TankDefine", name)==0)
	{
		pTable = new TankTable();
	}
	pTable->Load();
	return pTable;
}