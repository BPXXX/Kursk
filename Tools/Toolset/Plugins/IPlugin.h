#pragma once

class IPlugin
{
public:
	virtual const char *Desc() = 0;
	virtual void ParseArgv(int nVar, char* argv[]) = 0;
	virtual bool Run() = 0;
};
