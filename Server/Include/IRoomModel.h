#pragma once
#include "IModule.h"

class IRoomModel:
	public IModule
{
public:
	virtual void SayHellow() = 0;
};

