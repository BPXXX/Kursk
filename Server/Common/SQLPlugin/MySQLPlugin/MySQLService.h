/********************************************************************
	purpose:	数据库服务接口实现类
*********************************************************************/
#pragma once

#include "../Common/SQLService.h"

struct st_mysql;

class MySQLService :
	public SQLService
{
public:
	MySQLService(IPluginManager *pluginManager);
	virtual ~MySQLService(void);

public:
    int ExcuteQuery(IQueryResult **result, const char *cmd);
    int ExcuteQuery(const char *cmd);
    int ExcuteBinary(const char *cmd, SQLParam *param);

protected:
    int OpenImpl(std::map<std::string, std::string> &strmap);

private:
	st_mysql *_connection;
};
