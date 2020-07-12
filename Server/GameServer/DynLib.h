#ifndef DYNLIB_H
#define DYNLIB_H

#include <windows.h>
#include <string>

class DynLib
{
public:
	DynLib(const char *name);
	~DynLib();
    bool Load();
    bool UnLoad();
	const char* GetName();
    void* GetSymbol(const char* szProcName);
protected:
    std::string m_strName;
	HMODULE mInst;
};

#endif
