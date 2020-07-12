#pragma once
#include "IModule.h"

class ITemplateModule :
	public IModule
{
public:
	virtual void SayHellow() = 0;
};

