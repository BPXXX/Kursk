/********************************************************************
	purpose:	数据库服务接口
*********************************************************************/
#pragma once

#include "IModule.h"
#include "IQueryResult.h"
#include <functional>

typedef std::function<void(IQueryResult *result, int nRetCode)> SQL_Callback;

struct SQLParam
{
    SQLParam() {};
    SQLParam(void *p, uint32_t l)
    {
        data = p;
        length = l;
    }
    void *data;
    uint32_t length;
};

class ISQLService : public IModule
{
public:
    virtual int Open(const char * constr) = 0;
    virtual int ExcuteQuery(IQueryResult **result, const char *cmd) = 0;
    virtual int ExcuteQueryf(IQueryResult **result, const char *cmd, ...) = 0;
	virtual int ExcuteQuery(const char *cmd) = 0;
	virtual int ExcuteQueryf(const char *cmd, ...) = 0;
    virtual int ExcuteBinary(const char *cmd, SQLParam *param) = 0;
    virtual int ExcuteBinaryf(const char *cmd, SQLParam *param, ...) = 0;
	
    virtual void ExcuteQueryAsync(const char *cmd, SQL_Callback callback = 0, bool bHasResult = true) = 0;
    virtual void ExcuteQueryAsyncf(const char *cmd, SQL_Callback callback = 0, bool bHasResult = true, ...) = 0;
    virtual void ExcuteBinaryAsync(const char *cmd, SQLParam *param, SQL_Callback callback = 0) = 0;
    virtual void ExcuteBinaryAsyncf(const char *cmd, SQLParam *param, SQL_Callback callback = 0, ...) = 0;
};
