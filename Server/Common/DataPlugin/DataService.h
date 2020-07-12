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

	// ͨ�� IModule �̳�
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;

private:
	IPluginManager *m_pluginManager;

	// ͨ�� IDataService �̳�
	virtual IData* QueryRecord(const char * name, int id) override;
	virtual IData** QueryRecords(const char * name, int &count) override;

	// ͨ�� IDataService �̳�
	virtual ITable * QueryTable(const char * name) override;

private:
	ITable* LoadTable(const char* name);

private:
	std::unordered_map<std::string, ITable*> m_mapTables;
};

