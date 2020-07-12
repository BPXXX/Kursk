#include "stdafx.h"
#include "DataService.h"
#include <io.h>

DataService::DataService(IPluginManager *pluginManager)
	: m_pluginManager(pluginManager)
{
}


DataService::~DataService()
{
}

bool DataService::Init()
{
	return true;
}

bool DataService::Update()
{
	return true;
}

bool DataService::Shut()
{
	return true;
}

IData * DataService::QueryRecord(const char * name, int id)
{
	return QueryTable(name)->QueryRecord(id);
}

IData** DataService::QueryRecords(const char * name, int &count)
{
	return QueryTable(name)->QueryRecords(count);
}

ITable * DataService::QueryTable(const char * name)
{
	std::string strName(name);
	auto itr = m_mapTables.find(strName);
	if (itr != m_mapTables.end())
	{
		return itr->second;
	}
	
	auto pTable = LoadTable(name);
	m_mapTables.insert(std::make_pair(strName, pTable));

	return pTable;
}

#include "BaseTable.h"
#include "DataService/automake/TableLoader.h"
//
//const ITable<IData>& DataService::QueryTable(const char * name)
//{
//	// TODO: 在此处插入 return 语句
//}
