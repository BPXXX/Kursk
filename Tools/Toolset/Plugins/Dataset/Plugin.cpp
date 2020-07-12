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
#include <map>
#include "MiniExcelReader.h"
#include <tinyxml2/tinyxml2.h>
#include <algorithm>

DEF_string(excel, "./Excel", "dir,dir");
DEF_string(csharp, "", "csharp code out dir");
DEF_string(cpp, "", "c++ code out dir");
DEF_string(csv, "", "csv out dir");
DEF_string(binary, "", "binary out dir");
DEF_string(cache, "./Cache", "cache dir");

void GetProps(const fastring &text, std::map<fastring, fastring> &props)
{
	auto params = str::split(text, " ");
	for (int i = 0; i < params.size() - 1; i += 2)
	{
		if (params[i + 1][0] == '"')
		{
			props[params[i].substr(0, params[i].size() - 1)] = params[i + 1].substr(1, params[i + 1].size() - 2);
		}
		else
		{
			props[params[i].substr(0, params[i].size() - 1)] = params[i + 1];
		}
	}
}

fastring GetCellValue(MiniExcelReader::Sheet *pSheel, int row, int col)
{
	auto pCell = pSheel->getCell(row, col);
	if (pCell == NULL)
		return "";

	return pCell->value.c_str();
}

fastring GetCSharpType(fastring type)
{
	bool bIsArray = false;
	if (type[0] == '[')
	{
		type = type.substr(2);
		bIsArray = true;
	}
	if (type == "int32")
	{
		type = "int";
	}
	else if (type == "int64")
	{
		type = "long";
	}
	if (bIsArray)
	{
		return "List<" + type + ">";
	}
	return type;
}
fastring GetCPPType(fastring type)
{
	bool bIsArray = false;
	if (type[0] == 'L')
	{
		int nIdx = type.find_first_of("List<");
		if (nIdx != -1)
		{
			int nIdx2 = type.find_last_of(">");
			type = type.substr(nIdx+5, nIdx2-5- nIdx);
			bIsArray = true;
		}
	}
	if (type == "int32")
	{
		type = "int";
	}
	else if (type == "int64")
	{
		type = "long";
	}
	else if (type == "string")
	{
		type = "const char*";
	}
	if (bIsArray)
	{
		return "std::vector<" + type + ">";
	}
	return type;
}

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
	char szPath[_MAX_PATH];
	if (FLG_csharp.size() > 0)
	{
		FLG_csharp = _fullpath(szPath, FLG_csharp.c_str(), _MAX_PATH);
		fs::mkdir(FLG_csharp, true);
	}
	if (FLG_cpp.size() > 0)
	{
		FLG_cpp = _fullpath(szPath, FLG_cpp.c_str(), _MAX_PATH);
		fs::mkdir(FLG_cpp, true);
	}
	if (FLG_csv.size() > 0)
	{
		FLG_csv = _fullpath(szPath, FLG_csv.c_str(), _MAX_PATH);
		fs::mkdir(FLG_csv, true);
	}
	if (FLG_binary.size() > 0)
	{
		FLG_binary = _fullpath(szPath, FLG_binary.c_str(), _MAX_PATH);
		fs::mkdir(FLG_binary, true);
	}
	if (FLG_cache.size() > 0)
	{
		FLG_cache = _fullpath(szPath, FLG_cache.c_str(), _MAX_PATH);
		fs::mkdir(FLG_cache, true);
	}

	auto dirs = str::split(FLG_excel, ",");
	for (auto i = 0; i < dirs.size(); i++)
	{
		dirs[i] = _fullpath(szPath, dirs[i].c_str(), _MAX_PATH);
	}

	std::vector<fastring> files;
	for (auto i = 0; i < dirs.size(); i++)
	{
		path::get_files(dirs[i].c_str(), files, "*.xlsm", true);
	}
	for (auto i = 0; i < dirs.size(); i++)
	{
		path::get_files(dirs[i].c_str(), files, "*.xlsx", true);
	}

	if (FLG_cache.size() > 0)
	{
		LoadCache();
	}

	for (auto i = 0; i < files.size(); i++)
	{
		auto &path = files[i];
		auto fileName = path::base(path);
		auto itr = m_mapTables.find(fileName);
		auto modifyTime = fs::mtime(path);
		auto fileSize = fs::fsize(path);
		TableInfo *table = 0;
		if (itr != m_mapTables.end())
		{
			table = itr->second;
			if (modifyTime == table->modifyTime && fileSize == table->fileSize)
				continue;
		}

		fs::file file;
		file.open(path.c_str(), 'r');
		auto content = file.read(fileSize);
		auto md5 = md5sum(content);
		if (table != 0)
		{
			if (md5 == table->md5)
			{
				table->modifyTime = modifyTime;
				continue;
			}
			delete table;
		}

		table = new TableInfo();
		table->isHasChange = true;
		table->fileName = fileName;
		table->fileSize = fileSize;
		table->modifyTime = modifyTime;
		table->md5 = md5;

		m_mapTables[fileName] = table;

		ParseXlsx(files[i].c_str(), table);
	}

	for (auto itr = m_mapTables.begin(); itr != m_mapTables.end(); ++itr)
	{
		auto table = itr->second;

		for (auto typeItr = table->objectType.begin(); typeItr != table->objectType.end(); ++typeItr)
		{
			if (m_mapObjectType.find(typeItr->first) != m_mapObjectType.end())
			{
				std::cout << "find same object type:" << typeItr->first << std::endl;
				return false;
			}
			m_mapObjectType[typeItr->first] = typeItr->second;
		}
	}

	for (auto itr = m_mapTables.begin(); itr != m_mapTables.end(); ++itr)
	{
		auto table = itr->second;
		for (auto field : table->field)
		{
			field->list = false;
			if (field->type[0] == 'L')
			{
				int nIdx = field->type.find_first_of("List<");
				if (nIdx != -1)
				{
					int nIdx2 = field->type.find_last_of(">");
					field->basetype = field->type.substr(nIdx + 5, nIdx2 - 5 - nIdx);
					field->list = true;
					auto oItr = m_mapObjectType.find(field->basetype);
					if (oItr != m_mapObjectType.end())
					{
						field->ot = oItr->second;
					}
				}
			}
			if (!field->list)
			{
				auto oItr = m_mapObjectType.find(field->type);
				if (oItr != m_mapObjectType.end())
				{
					field->ot = oItr->second;
				}
			}
		}
	}


	ReplaceAlias();
	BuildBinaryCache();
	BuildCSVCache();
	ReplaceTextColor();

	if (FLG_csharp.size() > 0)
	{
		ExportCSharp();
	}
	if (FLG_cpp.size() > 0)
	{
		ExportCPP();
	}

	if (FLG_csv.size() > 0)
	{
		ExportCSV();
	}

	if (FLG_binary.size() > 0)
	{
		ExportBinary();
	}

	if (FLG_cache.size() > 0)
	{
		SaveCache();
	}

	return true;
}

bool CPlugin::LoadCache()
{
	auto cacheFile = path::join(FLG_cache, "ExcelCache.xml");
	tinyxml2::XMLDocument doc;
	if (doc.LoadFile(cacheFile.c_str()) != tinyxml2::XMLError::XML_SUCCESS)
		return false;

	std::cout << "load cache file" << std::endl;
	auto rootNode = doc.RootElement();
	auto excelNode = rootNode->FirstChildElement();
	while (excelNode)
	{
		TableInfo* table = new TableInfo;
		table->isHasChange = false;
		table->name = excelNode->Attribute("Name");
		table->package = excelNode->Attribute("Package");
		table->isVertical = str::to_bool(excelNode->Attribute("Vertical"));
		table->fileName = excelNode->Attribute("FileName");
		table->fileSize = str::to_int64(excelNode->Attribute("FileSize"));
		table->modifyTime = str::to_int64(excelNode->Attribute("ModifyTime"));
		table->md5 = excelNode->Attribute("Md5"); 

		auto childNode = excelNode->FirstChildElement();
		while (childNode)
		{
			if(strcmp(childNode->Name(), "Field") == 0)
			{
				auto fieldNode = childNode;

				auto field = new TableFileInfo;
				field->name = fieldNode->Attribute("Name");
				field->type = fieldNode->Attribute("Type");
				
				field->split = fieldNode->Attribute("Split");
				field->comment = fieldNode->Attribute("Comment");
				table->field.push_back(field);
			}
			else if(strcmp(childNode->Name(), "ObjectType") == 0)
			{
				auto objectTypeNode = childNode;
				ObjectType *objectType = new ObjectType();
					objectType->name = objectTypeNode->Attribute("Name");
					objectType->IsEnum = false;

					auto fieldNode = objectTypeNode->FirstChildElement();
					while (fieldNode)
					{
						FileInfo* field = new FileInfo;
						field->name = fieldNode->Attribute("Name");
						field->type = fieldNode->Attribute("Type");
						field->value = fieldNode->Attribute("Value");

						if (field->value.size() > 0)
							objectType->IsEnum = true;

						field->alias = fieldNode->Attribute("Alias");
						field->initValue = fieldNode->Attribute("InitValue");
						field->comment = fieldNode->Attribute("Comment");
						field->meta = fieldNode->Attribute("Meta");

						objectType->field.push_back(field);
						fieldNode = fieldNode->NextSiblingElement();
					};

				table->objectType[objectType->name] = objectType;
			}
			childNode = childNode->NextSiblingElement();
		}

		m_mapTables[table->fileName] = table;

		excelNode = excelNode->NextSiblingElement();
	}

	return true;
}

bool CPlugin::SaveCache()
{
	std::cout << "save cache file" << std::endl;
	auto cacheFile = path::join(FLG_cache, "ExcelCache.xml");
	tinyxml2::XMLDocument doc(true, tinyxml2::COLLAPSE_WHITESPACE);
	tinyxml2::XMLElement* root = doc.NewElement("ExcelCache");
	doc.InsertEndChild(root);

	for(auto itrTable = m_mapTables.begin(); itrTable != m_mapTables.end(); ++itrTable)
	{
		auto excelNode = doc.NewElement("Excel");
		auto table = itrTable->second;

		excelNode->SetAttribute("Name", table->name.c_str());
		excelNode->SetAttribute("Package", table->package.c_str());
		excelNode->SetAttribute("Vertical", str::from(table->isVertical).c_str());
		excelNode->SetAttribute("FileName", table->fileName.c_str());
		excelNode->SetAttribute("FileSize", str::from(table->fileSize).c_str());
		excelNode->SetAttribute("ModifyTime", str::from(table->modifyTime).c_str());
		excelNode->SetAttribute("Md5", table->md5.c_str());

		for (int i = 0; i < table->field.size(); i++)
		{
			auto fieldNode = doc.NewElement("Field");
			auto field = table->field[i];
			fieldNode->SetAttribute("Name", field->name.c_str());
			fieldNode->SetAttribute("Type", field->type.c_str());
			fieldNode->SetAttribute("Split", field->split.c_str());
			fieldNode->SetAttribute("Comment", field->comment.c_str());

			excelNode->InsertEndChild(fieldNode);
		}

		for (auto itrType = table->objectType.begin(); itrType != table->objectType.end(); ++itrType)
		{
			auto typeNode = doc.NewElement("ObjectType");
			auto type = itrType->second;
			typeNode->SetAttribute("Name", type->name.c_str());

			for (auto itrField = type->field.begin(); itrField != type->field.end(); ++itrField)
			{
				auto fieldNode = doc.NewElement("Field");
				auto field = *itrField;

				fieldNode->SetAttribute("Name", field->name.c_str());
				fieldNode->SetAttribute("Type", field->type.c_str());
				fieldNode->SetAttribute("Value", field->value.c_str());
				fieldNode->SetAttribute("Alias", field->alias.c_str());
				fieldNode->SetAttribute("InitValue", field->initValue.c_str());
				fieldNode->SetAttribute("Meta", field->meta.c_str());
				fieldNode->SetAttribute("Comment", field->comment.c_str());

				typeNode->InsertEndChild(fieldNode);
			}

			excelNode->InsertEndChild(typeNode);
		}

		root->InsertEndChild(excelNode);
	}

	doc.SaveFile(cacheFile.c_str());

	return true;
}

bool CPlugin::BuildBinaryCache()
{
	std::cout << "build binary..." << std::endl;
	for (auto itr = m_mapTables.begin(); itr != m_mapTables.end(); ++itr)
	{
		auto table = itr->second;
		if (!table->isHasChange)
			continue;

		std::map<fastring, int> mapText;
		std::vector<fastring> vectorText;

		std::vector<std::vector<int>> vecRecordIdx;
		vecRecordIdx.reserve(table->records.size());
		for (int row = 0; row < table->records.size(); row++)
		{
			std::vector<int> vecColIdx;
			auto &cols = table->records[row];
			for (int col = 0; col < cols.size(); col++)
			{
				auto value = cols[col];
				auto itrText = mapText.find(value);
				if (itrText == mapText.end())
				{
					mapText[value] = vectorText.size();
					vecColIdx.push_back(vectorText.size());
					vectorText.push_back(value);
				}
				else
				{
					vecColIdx.push_back(itrText->second);
				}
			}
			vecRecordIdx.push_back(vecColIdx);
		}

		fs::file file;
		auto binpath = path::join(FLG_cache, "Bin");
		fs::mkdir(binpath, true);
		auto cacheFile = path::join(binpath, table->name + ".bin");

		if (!file.open(cacheFile.c_str(), 'w'))
			return false;


		int nLenght = vectorText.size();
		file.write(&nLenght, 4);
		for (int i = 0; i < vectorText.size(); i++)
		{
			unsigned short s = vectorText[i].size();
			file.write(&s, 2);
			file.write(vectorText[i]);
		}

		int rowNum = table->records.size();
		file.write(&rowNum, sizeof(rowNum));
		unsigned char colNum = table->field.size();
		file.write(&colNum, sizeof(colNum));

		for (int row = 0; row < rowNum; row++)
		{
			for (int col = 0; col < colNum; col++)
			{
				file.write(&vecRecordIdx[row][col], sizeof(int));
			}
		}
		file.close();
	}
}

bool FileInfoCompare(const CPlugin::FileInfo* left, const CPlugin::FileInfo* right)
{
	return left->alias.size() < right->alias.size();
}

bool CPlugin::ReplaceTextColor()
{
	auto colorFile = path::join(FLG_excel, "TextColors.xml");
	tinyxml2::XMLDocument doc;
	if (doc.LoadFile(colorFile.c_str()) != tinyxml2::XMLError::XML_SUCCESS)
	{
		std::cout << "no text color config !" << std::endl;
		return false;
	}

	std::cout << "load text color" << std::endl;
	auto rootNode = doc.RootElement();
	auto excelNode = rootNode->FirstChildElement();	
	std::map<std::string, std::string> mapColors;
	while (excelNode)
	{
		const char *name = excelNode->Attribute("Name");
		const char *code = excelNode->Attribute("Code");
		mapColors[name] = code;
		excelNode = excelNode->NextSiblingElement();
	}

	auto itr = m_mapTables.find("s_Language.xlsm");
	if (itr == m_mapTables.end())
	{
		return false;
	}
	auto table = itr->second;
	if (!table->isHasChange)
		return true;

	std::map<fastring, int> mapText;
	std::vector<fastring> vectorText;

	std::vector<std::vector<int>> vecRecordIdx;
	vecRecordIdx.reserve(table->records.size());
	for (int row = 0; row < table->records.size(); row++)
	{
		std::vector<int> vecColIdx;
		auto &cols = table->records[row];
		for (int col = 0; col < cols.size(); col++)
		{
			auto value = cols[col];
			auto itrText = mapText.find(value);
			if (itrText == mapText.end())
			{
				mapText[value] = vectorText.size();
				vecColIdx.push_back(vectorText.size());
				vectorText.push_back(value);
			}
			else
			{
				vecColIdx.push_back(itrText->second);
			}
		}
		vecRecordIdx.push_back(vecColIdx);
	}

	for (auto ot : m_mapObjectType)
	{
		ot.second->fieldSort = ot.second->field;
		sort(ot.second->fieldSort.begin(), ot.second->fieldSort.end(), FileInfoCompare);
	}

	for(auto colorPair =  mapColors.begin(); colorPair != mapColors.end(); ++colorPair)
	{
		std::string oldColor = std::string()+"<color="+ colorPair->first+">";
		std::string newColor = std::string() + "<color=" + colorPair->second + ">";
		for (int i = 0; i < vectorText.size(); i++)
		{
			vectorText[i].replace(oldColor.c_str(), newColor.c_str());
		}
	}

	fs::file file;
	auto binpath = path::join(FLG_cache, "Binary");
	fs::mkdir(binpath, true);
	auto cacheFile = path::join(binpath, table->name + ".bin");

	if (!file.open(cacheFile.c_str(), 'w'))
		return false;


	int nLenght = vectorText.size();
	file.write(&nLenght, 4);
	for (int i = 0; i < vectorText.size(); i++)
	{
		unsigned short s = vectorText[i].size();
		file.write(&s, 2);
		file.write(vectorText[i]);
	}

	int rowNum = table->records.size();
	file.write(&rowNum, sizeof(rowNum));
	char colNum = table->field.size();
	file.write(&colNum, sizeof(colNum));

	for (int row = 0; row < rowNum; row++)
	{
		for (int col = 0; col < colNum; col++)
		{
			file.write(&vecRecordIdx[row][col], sizeof(int));
		}
	}
	file.close();
	return true;
}

bool CPlugin::BuildCSVCache()
{
	std::cout << "build csv..." << std::endl;

	auto csvpath = path::join(FLG_cache, "CSV");
	fs::mkdir(csvpath, true);

	for (auto itr = m_mapTables.begin(); itr != m_mapTables.end(); ++itr)
	{
		auto table = itr->second;
		if (!table->isHasChange)
			continue;

		fastring strCsv;
		fastring name;
		fastring type;
		for (int i = 0; i < table->field.size(); i++)
		{
			name.append("\"");
			name.append(table->field[i]->name);
			name.append("\"");
			type.append("\"");
			type.append(table->field[i]->type);
			type.append("\"");
			if (i + 1 != table->field.size())
			{
				name.append(",");
				type.append(",");
			}
		}

		strCsv.append(name).append("\r\n");
		strCsv.append(type).append("\r\n");

		for (int row = 0; row < table->records.size(); row++)
		{
			auto &cols = table->records[row];
			for (int col = 0; col < cols.size(); col++)
			{
				strCsv.append("\"");
				auto value = str::replace(cols[col], "\"", "\"\"");
				strCsv.append(value);
				strCsv.append("\"");
				if (col + 1 != cols.size())
				{
					strCsv.append(",");
				}
			}
			if (row + 1 != table->records.size())
			{
				strCsv.append("\r\n");
			}
		}

		auto cacheFile = path::join(csvpath, table->name + ".csv");
		fs::file file;
		if(!file.open(cacheFile.c_str(), 'w'))
			return false;
		file.write(strCsv);
		file.close();
	}
}

bool CPlugin::ParseXlsx(const char *pPath, TableInfo *table)
{
	auto t = now::ms();
	MiniExcelReader::ExcelFile *excelFile = new MiniExcelReader::ExcelFile();

	if (!excelFile->open(pPath, false))
	{
		std::cout << "open failure:" << pPath << std::endl;
		return false;
	}

	// 解析类型
	auto typeSheet = excelFile->getSheet("@Types");
	excelFile->loadSheet(*typeSheet);

	fastring sheetPropsStr = typeSheet->getCell(1, 1)->value.c_str();

	std::map<fastring, fastring> params;
	GetProps(sheetPropsStr, params);

	fastring tableName = params["TableName"];
	table->name = tableName;
	table->package = params["Package"];
	table->isVertical = params["Vertical"] == "true";
	{
		auto &range = typeSheet->getDimension();
		for (int i = 4; i <= range.lastRow; i++)
		{
			FileInfo *fieldInfo = new FileInfo;
			fastring objectName = GetCellValue(typeSheet, i, 1);
			if (objectName.size() == 0)
				break;
			auto type = table->objectType[objectName];
			if (type == 0)
			{
				type = table->objectType[objectName] = new ObjectType;
				type->name = objectName;
				type->IsEnum = false;
			}
				
			fieldInfo->name = GetCellValue(typeSheet, i, 2);
			fieldInfo->type = GetCSharpType(GetCellValue(typeSheet, i, 3));
			fieldInfo->value = GetCellValue(typeSheet, i, 4);
			fieldInfo->alias = GetCellValue(typeSheet, i, 5);
			fieldInfo->initValue = GetCellValue(typeSheet, i, 6);
			fieldInfo->meta = GetCellValue(typeSheet, i, 7);
			fieldInfo->comment = GetCellValue(typeSheet, i, 8);
			str::replace(fieldInfo->comment, "\n", "");

			if (fieldInfo->value.size() > 0)
			{
				type->IsEnum = true;
			}
			type->field.push_back(fieldInfo);
		}
	}

	{
		auto &sheets = excelFile->sheets();

		for (auto i = 0; i < sheets.size(); i++)
		{
			auto &sheet = sheets[i];
			std::string name = sheet.getName();
			if (name[0] == '#' || name[0] == '@')
				continue;

			excelFile->loadSheet(sheet);

			auto &range = sheet.getDimension();

			std::vector<int> vecIdx;
			if (table->isVertical)
			{
				for (int row = 2; row <= range.lastRow; row++)
				{
					TableFileInfo *fieldInfo = new TableFileInfo();
					fieldInfo->name = GetCellValue(&sheet, row, 1);
					if (fieldInfo->name.size() == 0)
						break;
					if (fieldInfo->name[0] == '#' || fieldInfo->name[0] == '*')
						break;
					vecIdx.push_back(row);
					fieldInfo->type = GetCSharpType(GetCellValue(&sheet, row, 2));
					fieldInfo->split = GetCellValue(&sheet, row, 3);
					fieldInfo->comment = GetCellValue(&sheet, row, 4) + GetCellValue(&sheet, row, 6);
					str::replace(fieldInfo->comment, "\n", "");
					if (fieldInfo->type.size() > 0 && fieldInfo->type[0] == '[')
					{
						std::map<fastring, fastring> props;
						GetProps(fieldInfo->split, props);
						fieldInfo->split = props["ListSpliter"];
					}
					table->field.push_back(fieldInfo);
				}
				std::vector<fastring> cols;

				for (int row = 0; row < vecIdx.size(); row++)
				{
					auto value = GetCellValue(&sheet, vecIdx[row], 5);
					cols.push_back(value);
				}

				if (cols[0].size() > 0)
					table->records.push_back(cols);
			}
			else
			{
				for (int col = 1; col <= range.lastCol; col++)
				{
					TableFileInfo *fieldInfo = new TableFileInfo();
					fieldInfo->name = GetCellValue(&sheet, 1, col);
					if (fieldInfo->name.size() == 0)
						break;
					if (fieldInfo->name[0] == '#' || fieldInfo->name[0] == '*')
						continue;
					vecIdx.push_back(col);
					fieldInfo->type = GetCSharpType(GetCellValue(&sheet, 2, col));
					fieldInfo->split = GetCellValue(&sheet, 3, col);
					fieldInfo->comment = GetCellValue(&sheet, 4, col);
					str::replace(fieldInfo->comment, "\n", "");
					if (fieldInfo->type.size() > 0 && fieldInfo->type[0] == '[')
					{
						std::map<fastring, fastring> props;
						if (fieldInfo->split.size() == 0)
						{
							fieldInfo->split = "|";
						}
						else
						{
							GetProps(fieldInfo->split, props);
							fieldInfo->split = props["ListSpliter"];
						}
					}
					table->field.push_back(fieldInfo);
				}

				table->records.reserve(range.lastRow - 4);
				for (int row = 5; row <= range.lastRow; row++)
				{
					std::vector<fastring> cols;
					cols.reserve(vecIdx.size());
					for (int col = 0; col < vecIdx.size(); col++)
					{
						auto value = GetCellValue(&sheet, row, vecIdx[col]);
						cols.push_back(value);
					}
					if (cols[0].size() > 0)
						table->records.push_back(cols);
				}
			}

			std::cout << name << ":" << range.lastRow << "," << range.lastCol << "," << now::ms() - t << "ms" << std::endl;
			break;
		}
	}

	return true;
}

bool CPlugin::ExportCSharp()
{
	std::cout << "export csharp..." << std::endl;
	// 导出全局的类和枚举
	{
		fastring strDBDefine;
		strDBDefine = ""
			"using System;\r\n"
			"using System.Collections;\r\n"
			"using System.Collections.Generic;\r\n"
			"using UnityEngine;\r\n"
			"\r\n"
			"namespace Data\r\n"
			"{\r\n";

		for (auto itr = m_mapObjectType.begin(); itr != m_mapObjectType.end(); itr++)
		{
			auto type = itr->second;
			if (!type->IsEnum)
				continue;

			strDBDefine += "\tpublic enum ";
			strDBDefine += type->name;
			strDBDefine += "\r\n\t{\r\n";

			for (auto fieldItr = type->field.begin(); fieldItr != type->field.end(); ++fieldItr)
			{
				auto field = *fieldItr;
				strDBDefine += "\t\t";
				strDBDefine += field->name;
				strDBDefine += "\t=\t";
				if (field->value.size() > 0)
				{
					strDBDefine += field->value;
				}
				else
				{
					strDBDefine += "0";
				}
				strDBDefine += ",";
				if (field->alias.size() > 0 || field->comment.size() > 0)
				{
					strDBDefine += "//";
					strDBDefine += field->alias + field->comment;
				}
				strDBDefine += "\r\n";
			}
			strDBDefine += "\t};\r\n\r\n";
		}

		for (auto itr = m_mapObjectType.begin(); itr != m_mapObjectType.end(); itr++)
		{
			auto type = itr->second;
			if (type->IsEnum)
				continue;

			strDBDefine += "\tpublic class ";
			strDBDefine += type->name;
			strDBDefine += "\r\n\t{\r\n";

			for (auto fieldItr = type->field.begin(); fieldItr != type->field.end(); ++fieldItr)
			{
				auto &info = *fieldItr;
				strDBDefine += "\t\tpublic ";
				strDBDefine += GetCSharpType(info->type);
				strDBDefine += "\t";
				strDBDefine += info->name;
				strDBDefine += ";";
				if (info->alias.size() > 0 || info->comment.size() > 0)
				{
					strDBDefine += "//";
					strDBDefine += info->alias + info->comment;
				}
				strDBDefine += "\r\n";
			}
			strDBDefine += "\t};\r\n\r\n";
		}

		strDBDefine += "\r\n}\r\n\r\n";

		//Utility_ConvertUtf8ToGBK(strDBDefine);

		FILE *fp = fopen((FLG_csharp + "\\Global.cs").c_str(), "wb");
		fwrite(strDBDefine.c_str(), strDBDefine.size(), 1, fp);
		fclose(fp);
	}
	// 导出单张表的数据内容
	{

		for (auto it = m_mapTables.begin(); it != m_mapTables.end(); it++)
		{
			auto table = it->second;
			if (table->field.size() == 0)
				continue;

			fastring strDBDefine;
			strDBDefine = ""
				"using System;\r\n"
				"using System.Collections;\r\n"
				"using System.Collections.Generic;\r\n"
				"using UnityEngine;\r\n"
				"\r\n"
				"namespace Data\r\n"
				"{\r\n";

			strDBDefine += "    public class ";
			strDBDefine += table->name;
			strDBDefine += "Define";
			strDBDefine += "\r\n    {\r\n";

			for (auto i = 0; i < table->field.size(); i++)
			{
				auto field = table->field[i];
				if (field->comment.size() > 0)
				{
					strDBDefine += "        /// <summary> \r\n";
					strDBDefine += "        /// ";
					strDBDefine += field->comment;
					strDBDefine += "\r\n";
					strDBDefine += "        /// </summary>\r\n";
				}
				strDBDefine += "        ";
				if (i == 0)
				{
					strDBDefine += "[PrimaryKey] [AutoIncrement] ";
				}
				strDBDefine += "public ";
				auto csType = GetCSharpType(field->type);
				strDBDefine += csType;
				strDBDefine += " ";
				strDBDefine += field->name;
				if (i == 0)
				{
					strDBDefine += "  { get; set; }";
				}				
				else
				{
					//if (csType == "string")
					//{
					//	strDBDefine += " = string.Empty";
					//}
					//else if (csType == "int")
					//{
					//	strDBDefine += " = 0";
					//}
					//else if (csType == "float")
					//{
					//	strDBDefine += " = 0";
					//}
					//else
					//{
					//	strDBDefine += " = ";
					//	//if (csType.size() > 5 && csType.substr(0, 5) == "List<")
					//	//{
					//	//	strDBDefine += null;
					//	//}

					//}
					strDBDefine += ";";
				}
				strDBDefine += "\r\n";
				strDBDefine += "\r\n";
			}

			strDBDefine += "    }\r\n";

			strDBDefine += "}";

			FILE *fp = fopen((FLG_csharp + "\\" + table->name +"Config.cs").c_str(), "wb");
			fwrite(strDBDefine.c_str(), strDBDefine.size(), 1, fp);
			fclose(fp);
		}
	}
	return true;
}

bool CPlugin::ExportCPP()
{
	std::cout << "export cpp..." << std::endl;
	// 导出全局的类和枚举
	{
		fastring strDBDefine;
		strDBDefine = "#pragma once\n\n\n";

		for (auto itr = m_mapObjectType.begin(); itr != m_mapObjectType.end(); itr++)
		{
			auto type = itr->second;
			if (!type->IsEnum)
				continue;

			strDBDefine += "enum ";
			strDBDefine += type->name;
			strDBDefine += "\r\n{\r\n";

			for (auto fieldItr = type->field.begin(); fieldItr != type->field.end(); ++fieldItr)
			{
				auto field = *fieldItr;
				strDBDefine += "\t";
				strDBDefine += field->name;
				strDBDefine += "\t=\t";
				if (field->value.size() > 0)
				{
					strDBDefine += field->value;
				}
				else
				{
					strDBDefine += "0";
				}
				strDBDefine += ",";
				if (field->alias.size() > 0 || field->comment.size() > 0)
				{
					strDBDefine += "//";
					strDBDefine += field->alias + field->comment;
				}
				strDBDefine += "\r\n";
			}
			strDBDefine += "};\r\n\r\n";
		}

		for (auto itr = m_mapObjectType.begin(); itr != m_mapObjectType.end(); itr++)
		{
			auto type = itr->second;
			if (type->IsEnum)
				continue;

			strDBDefine += "class ";
			strDBDefine += type->name;
			strDBDefine += "\r\n{\r\npublic:\r\n";

			for (auto fieldItr = type->field.begin(); fieldItr != type->field.end(); ++fieldItr)
			{
				auto &info = *fieldItr;
				strDBDefine += "\t";
				strDBDefine += GetCPPType(info->type);
				strDBDefine += "\t";
				strDBDefine += info->name;
				strDBDefine += ";";
				if (info->alias.size() > 0 || info->comment.size() > 0)
				{
					strDBDefine += "//";
					strDBDefine += info->alias + info->comment;
				}
				strDBDefine += "\r\n";
			}
			strDBDefine += "};\r\n\r\n";
		}

		//Utility_ConvertUtf8ToGBK(strDBDefine);

		FILE *fp = fopen((FLG_cpp + "\\Global.h").c_str(), "wb");
		fwrite(strDBDefine.c_str(), strDBDefine.size(), 1, fp);
		fclose(fp);
	}

	// 导出单张表的加载器
	{
		for (auto it = m_mapTables.begin(); it != m_mapTables.end(); it++)
		{
			auto table = it->second;
			if (table->field.size() == 0)
				continue;

			fastring strDBDefine;
			strDBDefine = "#pragma once\r\n"
				"#include <vector>\r\n"
				"#include \"Global.h\"\r\n\r\n";

			strDBDefine += "class ";
			strDBDefine += table->name;
			strDBDefine += "Define";
			strDBDefine += "\r\n{\r\n";
			strDBDefine += "public:\r\n";

			for (auto i = 0; i < table->field.size(); i++)
			{
				auto field = table->field[i];
				if (field->comment.size() > 0)
				{
					strDBDefine += " \t/// <summary> \r\n";
					strDBDefine += " \t/// ";
					strDBDefine += field->comment;
					strDBDefine += "\r\n";
					strDBDefine += "\t/// </summary>\r\n";
				}
				auto csType = GetCPPType(field->type);
				strDBDefine += "\t";
				strDBDefine += csType;
				strDBDefine += " ";
				strDBDefine += field->name;

				strDBDefine += ";\r\n";
			}
			
			strDBDefine += "};";

			FILE *fp = fopen((FLG_cpp+ "\\" + table->name + "Config.h").c_str(), "wb");
			fwrite(strDBDefine.c_str(), strDBDefine.size(), 1, fp);
			fclose(fp);
		}
	}
	// 导出表的加载器
	{
		fastring strDBDefine;
		fastring strSwitchTable;
		strSwitchTable = "ITable* DataService::LoadTable(const char* name)"
			"\r\n{\r\n\tBaseTable *pTable = nullptr;\r\n";

		strDBDefine = "#pragma once\r\n"
			"#include <vector>\r\n\r\n";

		int nCount = 0;
		for (auto it = m_mapTables.begin(); it != m_mapTables.end(); it++)
		{
			auto table = it->second;
			if (table->field.size() == 0)
				continue;

			strSwitchTable += "\r\n\t";
			if (nCount > 0)
			{
				strSwitchTable += "else ";
			}
			strSwitchTable += "if(strcmp(\"class ";
			strSwitchTable += table->name;
			strSwitchTable += "Define\", name)==0)\r\n";
			strSwitchTable += "\t{\r\n\t\tpTable = new ";
			strSwitchTable += table->name;
			strSwitchTable += "Table();\r\n\t}";

			strDBDefine += "#include \"";
			strDBDefine += table->name;
			strDBDefine += "Config.h";
			strDBDefine += "\"\r\n";

			strDBDefine += "class ";
			strDBDefine += table->name;
			strDBDefine += "Table : public BaseTable\r\n";
			strDBDefine += "{\r\npublic:\r\n";
			strDBDefine += "\t";
			strDBDefine += table->name;
			strDBDefine += "Table() :BaseTable(\"";
			strDBDefine += table->name;
			strDBDefine += ".bin\") {}";
			
			strDBDefine += "\r\n\tvirtual IData* ReadRow(int &id) override\r\n";
			strDBDefine += "\t{\r\n";
			strDBDefine += "\t\tauto config = new ";
			strDBDefine += table->name;
			strDBDefine += "Define(); \r\n";
			strDBDefine += "\t\tint enumTmp;\r\n\r\n";
			strDBDefine += "\t\tconst char* data;\r\n\r\n";
			
			for (auto i = 0; i < table->field.size(); i++)
			{
				auto field = table->field[i];
				if (field->ot)
				{
					if (field->ot->IsEnum)
					{
						strDBDefine += "\t\tReadValue(enumTmp);\r\n";
						strDBDefine += "\t\tconfig->";
						strDBDefine += field->name;
						strDBDefine += "=(";
						strDBDefine += field->type;
						strDBDefine += ")enumTmp;\r\n";
					}
					else
					{
						if (!field->list)
						{
							strDBDefine += "\t\tReadValue(data);\r\n";
							strDBDefine += "\t\tauto ";
							strDBDefine += field->name;
							strDBDefine += " = json::Deserialize(data);\r\n";
							for (auto fileMember : field->ot->field)
							{
								strDBDefine += "\t\tReadValue(";
								strDBDefine += field->name;
								strDBDefine += ", \"";
								strDBDefine += fileMember->name;
								strDBDefine += "\", config->";
								strDBDefine += field->name;
								strDBDefine += ".";
								strDBDefine += fileMember->name;
								strDBDefine += ");\r\n";
							}
						}
						else
						{
							
							strDBDefine += "\t\tstd::vector<std::string> vec";
							strDBDefine += field->name; 
							strDBDefine += ";\r\n";
							strDBDefine += "\t\tReadValue(vec";
							strDBDefine += field->name;
							strDBDefine += ");\r\n";
							strDBDefine +="\t\tfor (auto value : vec";
							strDBDefine += field->name;
							strDBDefine += ")\r\n\t\t{\r\n";
							strDBDefine += "\t\t\tauto ";
							strDBDefine += field->name;
							strDBDefine += " = json::Deserialize(value);\r\n";

							fastring value = field->name + "value";
							strDBDefine += "\t\t\t";
							strDBDefine += field->basetype;
							strDBDefine += " ";
							strDBDefine += value;
							strDBDefine += ";\r\n";

							for (auto fileMember : field->ot->field)
							{
								strDBDefine += "\t\t\tReadValue(";
								strDBDefine += field->name;
								strDBDefine += ", \"";
								strDBDefine += fileMember->name;
								strDBDefine += "\", ";
								strDBDefine += value;
								strDBDefine += ".";
								strDBDefine += fileMember->name;
								strDBDefine += ");\r\n";
							}
							strDBDefine += "\t\t\tconfig->";
							strDBDefine += field->name;
							strDBDefine += ".push_back(";
							strDBDefine += value;
							strDBDefine += ");\r\n\t\t}\r\n";
						}
					}
				}
				else
				{
					auto csType = GetCPPType(field->type);
					strDBDefine += "\t\tReadValue(config->";
					strDBDefine += field->name;
					strDBDefine += ");\r\n";
				}
			}
			strDBDefine += "\t\tid=config->";
			strDBDefine += table->field[0]->name;
			strDBDefine += ";\r\n";
			strDBDefine += "\t\treturn (IData*)config;\r\n";
			strDBDefine += "\t}\r\n";
			strDBDefine += "};\r\n";
			nCount++;
		}

		strSwitchTable += "\r\n\tpTable->Load();";
		strSwitchTable += "\r\n\treturn pTable;";
		strSwitchTable += "\r\n}";

		strDBDefine.append(strSwitchTable);

		FILE *fp = fopen((FLG_cpp + "\\" + "TableLoader.h").c_str(), "wb");
		fwrite(strDBDefine.c_str(), strDBDefine.size(), 1, fp);
		fclose(fp);
	}
	return true;
}

bool CPlugin::ExportCSV()
{
	std::cout << "copy csv..." << std::endl;
	auto csvpath = path::join(FLG_cache, "CSV");
	for (auto itr = m_mapTables.begin(); itr != m_mapTables.end(); ++itr)
	{
		auto table = itr->second;
		auto to = FLG_csv + "/" + table->name + +".csv";
		auto from = path::join(csvpath, table->name + ".csv");
		fs::copy(from.c_str(), to.c_str());
	}
	return  true;
}

bool CPlugin::ExportBinary()
{
	std::cout << "copy binary..." << std::endl;
	auto binpath = path::join(FLG_cache, "Bin");
	for (auto itr = m_mapTables.begin(); itr != m_mapTables.end(); ++itr)
	{
		auto table = itr->second;
		auto to = FLG_binary + "/" + table->name + ".bin";
		auto from = path::join(binpath, table->name + ".bin");
		fs::copy(from.c_str(), to.c_str());
	}
	return true;
}

bool CPlugin::ReplaceAlias()
{
	std::cout << "replace alias" << std::endl;
	for (auto itr = m_mapTables.begin(); itr != m_mapTables.end(); ++itr)
	{
		auto table = itr->second;
		if (!table->isHasChange)
			continue;

		for (int row = 0; row < table->records.size(); row++)
		{
			auto &cols = table->records[row];
			for (int col = 0; col < cols.size(); col++)
			{
				auto value = cols[col];
				if (value.size() == 0)
				{
					continue;
				}
				auto field = table->field[col];
				if (field->ot == nullptr)
					continue;

				auto type = field->ot;
				if (type->IsEnum)
				{
					if (field->list)
					{
						for (int i = 0; i < type->fieldSort.size(); i++)
						{
							auto fieldType = type->fieldSort[i];
							if (fieldType->alias.size() > 0)
							{
								value = str::replace(value, fieldType->alias.c_str(), fieldType->value.c_str());
							}
						}
					}
					else
					{
						for (int i = 0; i < type->fieldSort.size(); i++)
						{
							auto fieldType = type->fieldSort[i];
							if (fieldType->alias == value)
							{
								value = fieldType->value;
								break;
							}
						}
					}
				}
				else
				{
					for (int i = 0; i < type->fieldSort.size(); i++)
					{
						auto fieldType = type->fieldSort[i];
						if (fieldType->alias.size() > 0)
						{
							value = str::replace(value, fieldType->alias.c_str(), fieldType->name.c_str());
						}
					}
					if (field->list)
					{
						value = str::replace(value, "|", "}|{\"");
					}
					value = str::replace(value, " ", ",\"");
					value = str::replace(value, ":", "\":");
					if (value[value.size() - 1] == '"')
					{
						value.resize(value.size() - 1);
					}
					value = "{\"" + value + "}";
				}
				cols[col] = value;
			}
		}
	}

	return true;
}