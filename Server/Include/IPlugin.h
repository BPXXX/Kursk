#pragma once
#include "IModule.h"
#include <unordered_map>

class IPlugin
{
public:
    virtual void Install() = 0;
	virtual void Uninstall() = 0;
};
