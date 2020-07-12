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
#include "stdafx.h"
#include "Plugin.h"
#include <string>

DEF_string(i, "./", "string");
DEF_string(o, "./Out", "string");

CPlugin::CPlugin(void)
{
}


CPlugin::~CPlugin(void)
{
}

const char *CPlugin::Desc()
{
	return "";
}

void CPlugin::ParseArgv(int argc, char* argv[])
{
	flag::init(argc, argv);
}

bool CPlugin::Run()
{
	std::vector<fastring> vecFiles;
	char szPath[_MAX_PATH];
	FLG_i = _fullpath(szPath, FLG_i.c_str(), _MAX_PATH);
	path::get_files(FLG_i, vecFiles, "*.*");

	for (int i = 0; i < vecFiles.size(); i++)
	{
		std::cout << vecFiles[i] << std::endl;
	}

	return true;
}
