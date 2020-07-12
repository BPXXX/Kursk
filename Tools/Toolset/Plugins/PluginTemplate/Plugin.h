// **********************************************************************
//
// Copyright (c) 2017
// All Rights Reserved
//
// Author: Johance
// Email:421465201@qq.com
// Created:	2017/6/13   08:00
//
// **********************************************************************
#pragma once
#include "IPlugin.h"
#include <string>
#include <vector>
#include <map>

class CPlugin :
	public IPlugin
{
public:
	CPlugin(void);
	~CPlugin(void);

	virtual const char *Desc();
	virtual void ParseArgv(int argc, char* argv[]);
	virtual bool Run();

private:
	
private:

private:
};

