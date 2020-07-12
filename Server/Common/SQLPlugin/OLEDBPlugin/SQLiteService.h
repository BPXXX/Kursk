/********************************************************************
	purpose:	数据库服务接口实现类
*********************************************************************/
#pragma once

#include "../Common/SQLService.h"

struct sqlite3;

class SQLiteService :
	public SQLService
{
public:
	SQLiteService(IPluginManager *pluginManager);
	virtual ~SQLiteService(void);

public:
    int ExcuteQuery(IQueryResult **result, const char *cmd);
    int ExcuteQuery(const char *cmd);
    int ExcuteBinary(const char *cmd, SQLParam *param);

protected:
    int OpenImpl(std::map<std::string, std::string> &strmap);

private:
    sqlite3 *_db;
};
