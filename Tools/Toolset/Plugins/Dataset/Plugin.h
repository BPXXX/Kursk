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


	struct FileInfo
	{
		fastring name;
		fastring type;
		fastring value;
		fastring alias;
		fastring initValue;
		fastring comment;
		fastring meta;
	};

	struct ObjectType
	{
		fastring name;
		bool IsEnum;
		std::vector<FileInfo*> field;
		std::vector<FileInfo*> fieldSort;
	};

	struct TableFileInfo
	{
		fastring name;
		fastring type;
		fastring basetype;
		fastring split;
		fastring comment;
		bool list = false;
		ObjectType *ot = nullptr;
	};

private:

	struct TableInfo
	{
		fastring fileName;
		fastring name;
		fastring package;
		bool isVertical;
		int64 modifyTime;
		int64 fileSize;
		fastring md5;
		std::map<fastring, ObjectType*> objectType;
		std::vector<TableFileInfo*> field;
		std::vector<std::vector<fastring>> records;
		bool isHasChange;
	};

private:
	bool LoadCache();
	bool SaveCache();
	bool BuildBinaryCache();
	bool BuildCSVCache();
	bool ReplaceTextColor();
	bool ParseXlsx(const char *pPath, TableInfo* tableInfo);
	
private:
	bool ExportCSharp();
	bool ExportCPP();
	bool ExportCSV();
	bool ExportBinary();
	bool ReplaceAlias();

private:
	std::map<fastring, TableInfo*> m_mapTables;
	std::map<fastring, ObjectType*> m_mapObjectType;
};

