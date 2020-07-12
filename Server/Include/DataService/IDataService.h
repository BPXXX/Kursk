#pragma once
#include "IModule.h"
#include "ITable.h"

class IDataService:
	public IModule
{
public:
	template<typename T>
	T* QueryRecord(int id)
	{
		return (T*)QueryRecord(typeid(T).name(), id);
	}
	template<typename T>
	T** QueryRecords(int &count)
	{
		return reinterpret_cast<T**>(QueryRecords(typeid(T).name(), count));
	}
	template<typename T>
	const Table<T>& QueryTable()
	{
		ITable *pTable = QueryTable(typeid(T).name());
		return Table<T>(pTable);
	}

protected:
	virtual IData* QueryRecord(const char *name, int id) = 0;
	virtual IData** QueryRecords(const char *name, int &count) = 0;
	virtual ITable* QueryTable(const char *name) = 0;
};

