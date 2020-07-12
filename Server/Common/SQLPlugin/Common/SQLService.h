#pragma once

#include <SQLService/ISQLService.h>
#include <IPluginManager.h>

#include <map>
#include <list>
#include <functional>
#include <mutex>


class SQL_Request
{
public:
	SQL_Request(const char *cmd, SQL_Callback callback, SQLParam *param, bool bHasResult)
	{
		strCmd = cmd;
		_callback = callback;
		_param = param;
		_bHasResult = bHasResult;
	}
	std::string strCmd;
	SQL_Callback _callback;
	SQLParam *_param;
	bool _bHasResult ;
};

class SQL_Respon
{
public:
	SQL_Respon(SQL_Callback callback, IQueryResult *result, bool retCode)
	{
		_result = result;
		_callback = callback;
		errorCode = retCode;
	}
	SQL_Callback _callback;
	IQueryResult *_result;
	int errorCode;
};

class SQLService : public ISQLService
{
public:
	SQLService(IPluginManager *pluginManager);
	~SQLService(void);

	// Í¨¹ý ISQLService ¼Ì³Ð
	virtual bool Init() override;
	virtual bool Update() override;
	virtual bool Shut() override;

    virtual int Open(const char * constr) override;

    virtual int ExcuteQuery(IQueryResult **result, const char *cmd) = 0;
    virtual int ExcuteQueryf(IQueryResult **result, const char *cmd, ...) override;
    virtual int ExcuteQuery(const char *cmd) = 0;
    virtual int ExcuteQueryf(const char *cmd, ...) override;
    
    virtual int ExcuteBinary(const char *cmd, SQLParam *param) = 0;
    virtual int ExcuteBinaryf(const char *cmd, SQLParam *param, ...) override;
		
    virtual void ExcuteQueryAsync(const char *cmd, SQL_Callback callback = 0, bool bHasResult = true) override;
    virtual void ExcuteQueryAsyncf(const char *cmd, SQL_Callback callback = 0, bool bHasResult = true, ...) override;
    virtual void ExcuteBinaryAsync(const char *cmd, SQLParam *param, SQL_Callback callback = 0);
    virtual void ExcuteBinaryAsyncf(const char *cmd, SQLParam *param, SQL_Callback callback = 0, ...) override;

	void Run();

protected:
	SQLService(void) {};
    virtual int OpenImpl(std::map<std::string, std::string> &strmap) = 0;
    char _param[1024];

private:
	std::mutex m_RequestLock;
	std::list<SQL_Request*> m_listRequest;

	std::mutex m_ResponLock;
	std::list<SQL_Respon*> m_listRespon;
	IPluginManager *m_pPluginManager;
};
