#pragma once
#include <DataService/IDataService.h>
#include <IPluginManager.h>
#include <unordered_map>

class DataService :
	public IDataService
{
public:
	DataService(IPluginManager *pluginManager);
	~DataService();

	// 通过 IModule 继承
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;

private:
	IPluginManager *m_pluginManager;

	// 通过 IDataService 继承
	virtual IData* QueryRecord(const char * name, int id) override;
	virtual IData** QueryRecords(const char * name, int &count) override;

	// 通过 IDataService 继承
	virtual ITable * QueryTable(const char * name) override;

private:
	ITable* LoadTable(const char* name);

private:
	std::unordered_map<std::string, ITable*> m_mapTables;
};

