#ifndef DYNLIB_H
#define DYNLIB_H

#include <stdio.h>
#include <iostream>

#ifdef _WIN32
#    define DYNLIB_HANDLE hInstance
#    define DYNLIB_LOAD( a ) LoadLibraryExA( a, NULL, LOAD_WITH_ALTERED_SEARCH_PATH )
#    define DYNLIB_GETSYM( a, b ) GetProcAddress( a, b )
#    define DYNLIB_UNLOAD( a ) FreeLibrary( a )

struct HINSTANCE__;
typedef struct HINSTANCE__* hInstance;

#elif __linux__
#include <dlfcn.h>
#define DYNLIB_HANDLE void*
#define DYNLIB_LOAD( a ) dlopen( a, RTLD_LAZY | RTLD_GLOBAL)
#define DYNLIB_GETSYM( a, b ) dlsym( a, b )
#define DYNLIB_UNLOAD( a ) dlclose( a )

#elif APPLE
#include <dlfcn.h>
#define DYNLIB_HANDLE void*
#define DYNLIB_LOAD( a ) dlopen( a, RTLD_LOCAL|RTLD_LAZY)
#define DYNLIB_GETSYM( a, b ) dlsym( a, b )
#define DYNLIB_UNLOAD( a ) dlclose( a )

#endif

class DynLib
{

public:

	DynLib(const std::string& strName)
    {
        mbMain = false;
        mstrName = strName;
#ifdef _DEBUG
        mstrName.append("_d");
#endif

#if _WIN32
        mstrName.append(".dll");
#elif __linux__
        mstrName.append(".so");
#elif APPLE
        mstrName.append(".so");
#endif

        printf("LoadPlugin:%s\n", mstrName.c_str());
    }

    ~DynLib()
    {

    }

    bool Load();

    bool UnLoad();

    /// Get the name of the library
    const std::string& GetName(void) const
    {
        return mstrName;
    }

    const bool GetMain(void) const
    {
        return mbMain;
    }

    void* GetSymbol(const char* szProcName);

protected:

    std::string mstrName;
    bool mbMain;

    DYNLIB_HANDLE mInst;
};

#endif
