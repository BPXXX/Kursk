/********************************************************************
	purpose:	数据库服务接口实现类
*********************************************************************/
#pragma once

#include "../Common/SQLService.h"

#include <windows.h>
#include "sql.h"
#include "sqlext.h"

class ODBCService :
	public SQLService
{
public:
	ODBCService(IPluginManager *pluginManager);
	virtual ~ODBCService(void);

public:
    int ExcuteQuery(IQueryResult **result, const char *cmd);
    int ExcuteQuery(const char *cmd);
    int ExcuteBinary(const char *cmd, SQLParam *param);

protected:
    int OpenImpl(std::map<std::string, std::string> &strmap);

private:
	std::string mSourceName;
	std::string mUserName;
	std::string mPwd;
	SQLHANDLE hEnv;
	SQLHANDLE hDbc;
	SQLHSTMT  hstmt;
};
