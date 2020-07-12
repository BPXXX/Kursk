#pragma once
#include "IModule.h"

class IPlayModule :
	public IModule
{
public:
	virtual void SayHellow() = 0;
};

