#include "SQLService.h"
#include <stdarg.h>
#define _CRT_SECURE_NO_WARNINGS

SQLService::SQLService(IPluginManager *pluginManager)
	: m_pPluginManager(pluginManager)
{

}
SQLService::~SQLService(void)
{
}

bool SQLService::Init()
{
	return true;
}

bool SQLService::Update()
{
	int nResponCount = 0;
	do
	{
		SQL_Respon *pRespon = NULL;
		{
			std::lock_guard<std::mutex> lck(m_ResponLock);
			nResponCount = m_listRespon.size();
			if (nResponCount == 0)
				break;
			pRespon = m_listRespon.front();
			m_listRespon.pop_front();
		}
		if (pRespon->_callback)
		{
			pRespon->_callback(pRespon->_result, pRespon->errorCode);
		}
		nResponCount--;
	} while (nResponCount);
	return true;
}

bool SQLService::Shut()
{
	return true;
}

int SQLService::Open(const char * constr)
{
    char *p = _strdup(constr);

    char *tmp = p;

    char *param;
    char *val;

    std::map<std::string, std::string> strmap;
	char *buf;
    tmp = strtok_s(tmp, "=", &buf);
    while(tmp)
    {
        param = tmp;
        tmp = strtok_s(0, ";", &buf);
        if(!tmp)
            break;
        val = tmp;
        strmap[param] = val;
        tmp = strtok_s(0, "=", &buf);
    }

    free(p); 
	
	std::thread t1(&SQLService::Run, this);//创建一个分支线程，回调到myThread函数里
	t1.detach();

    return OpenImpl(strmap);
}

int SQLService::ExcuteQueryf(IQueryResult **result, const char *cmd, ...)
{
    va_list arg_ptr;

    va_start(arg_ptr, cmd);
	vsprintf_s(_param, cmd, arg_ptr);

    va_end(arg_ptr);
    return ExcuteQuery(result, _param);
}

int SQLService::ExcuteQueryf(const char *cmd, ...)
{
    va_list arg_ptr;

    va_start(arg_ptr, cmd);
	vsprintf_s(_param, cmd, arg_ptr);

    va_end(arg_ptr);
    return ExcuteQuery(_param);
}


int SQLService::ExcuteBinaryf(const char *cmd, SQLParam *param, ...)
{
    va_list arg_ptr;

    va_start(arg_ptr, cmd);
	vsprintf_s(_param, cmd, arg_ptr);

    va_end(arg_ptr);
    return ExcuteBinary(_param, param);
}

void SQLService::ExcuteQueryAsync(const char *cmd, SQL_Callback callback, bool bHasResult/* = true*/)
{
	std::lock_guard<std::mutex> lck(m_RequestLock);

	SQL_Request *pRequest = new SQL_Request(cmd, callback, NULL, bHasResult);
	m_listRequest.push_back(pRequest);
}

void SQLService::ExcuteQueryAsyncf(const char *cmd, SQL_Callback callback, bool bHasResult/*t = true*/, ...)
{
    va_list arg_ptr;

    va_start(arg_ptr, cmd);
	vsprintf_s(_param, cmd, arg_ptr);

    va_end(arg_ptr);

    ExcuteQueryAsync(_param, callback, bHasResult);
}

void SQLService::ExcuteBinaryAsync(const char *cmd, SQLParam *param, SQL_Callback callback)
{
	std::lock_guard<std::mutex> lck(m_RequestLock);

	SQL_Request *pRequest = new SQL_Request(cmd, callback, param, false);
	m_listRequest.push_back(pRequest);
}

void SQLService::ExcuteBinaryAsyncf(const char *cmd, SQLParam *param, SQL_Callback callback, ...)
{
    va_list arg_ptr;

    va_start(arg_ptr, cmd);
    vsprintf_s(_param, cmd, arg_ptr);

    va_end(arg_ptr);

    ExcuteBinaryAsync(_param, param, callback);
}

void SQLService::Run()
{
	while(true)
	{
		m_RequestLock.lock();
		if(m_listRequest.size() > 0)
		{
			SQL_Request *pRequest = m_listRequest.front();
			m_listRequest.pop_front();			
			m_RequestLock.unlock();

			SQL_Respon *pRespon = NULL;
			if(pRequest->_param)
			{
				int nRet = ExcuteBinary(pRequest->strCmd.c_str(), pRequest->_param);
				pRespon = new SQL_Respon(pRequest->_callback, NULL, nRet);
			}
			else if(pRequest->_bHasResult)
			{
				IQueryResult *pResult;
				int nRet = ExcuteQuery(&pResult, pRequest->strCmd.c_str());
				pRespon = new SQL_Respon(pRequest->_callback, pResult, nRet);
			}
			else
			{
				int nRet = ExcuteQuery(pRequest->strCmd.c_str());
				pRespon = new SQL_Respon(pRequest->_callback, NULL, nRet);
			}
			m_ResponLock.lock();
			m_listRespon.push_back(pRespon);
			m_ResponLock.unlock();
		}
		else
		{
			m_RequestLock.unlock();
		}
	}
}