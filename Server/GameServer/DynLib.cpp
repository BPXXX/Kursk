#include "DynLib.h"

DynLib::DynLib(const char *name)
{
	m_strName = name;
#ifdef _DEBUG
	m_strName.append("_d");
#endif
	m_strName.append(".dll");
}

DynLib::~DynLib()
{
}

bool DynLib::Load()
{
    std::string strLibPath = "./";
    strLibPath += m_strName;
    mInst = LoadLibraryExA(strLibPath.c_str(), NULL, LOAD_WITH_ALTERED_SEARCH_PATH);
    return mInst != NULL;
}

bool DynLib::UnLoad()
{
	FreeLibrary(mInst);
    return true;
}

const char* DynLib::GetName()
{
	return m_strName.c_str();
}

void* DynLib::GetSymbol(const char* szProcName)
{
    return GetProcAddress(mInst, szProcName);
}
