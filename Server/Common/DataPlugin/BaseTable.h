#pragma once

#include <unordered_map>
#include "json.h"

class BaseTable : public ITable
{
public:
	BaseTable(const char *name)
	{
		strName = name;
	}
	virtual IData* QueryRecord(int id) override
	{
		auto itr = m_mapData.find(id);
		if (itr != m_mapData.end())
		{
			return itr->second;
		}

		return nullptr;
	}

	virtual IData** QueryRecords(int &count) override
	{
		count = m_records.size();
		if (count == 0)
			return nullptr;
		else
			return &m_records[0];
	}

	bool Load()
	{
		std::string path = "Config/Bin/" + strName;
		FILE *fp;
		fopen_s(&fp, path.c_str(), "rb");
		auto length = _filelength(_fileno(fp));
		m_pData = new char[length];
		fread(m_pData, length, 1, fp);

		// 读取字符串字典的数量
		m_nDataOffset = 0;
		int count = *(int*)(m_pData + m_nDataOffset);
		m_nDataOffset += sizeof(count);

		// 初始化保存字符串的容器
		m_vecTexts.resize(count);

		// 读取所有的字符串
		for (int i = 0; i < count; i++)
		{
			unsigned short usCount = *(unsigned short*)(m_pData + m_nDataOffset);
			// 每次都把此位置设置位0 方便给上一个字符串末尾设置为0
			*(unsigned short*)(m_pData + m_nDataOffset) = 0;
			m_nDataOffset += sizeof(usCount);
			m_vecTexts[i] = m_pData + m_nDataOffset;
			m_nDataOffset += usCount;
		}

		int rowNum = *(int*)(m_pData + m_nDataOffset);
		*(unsigned short*)(m_pData + m_nDataOffset) = 0;
		m_nDataOffset += sizeof(rowNum);

		unsigned char colNum = *(unsigned char *)(m_pData + m_nDataOffset);
		m_nDataOffset += sizeof(colNum);

		m_pRecordIdxs = (int*)(m_pData + m_nDataOffset);

		m_nRecordIdx = 0;

		m_records.reserve(rowNum);
		for (int row = 0; row < rowNum; row++)
		{
			int id;
			auto config = ReadRow(id);
			AddRecord(id, (IData*)config);
		}

		return true;
	}
	virtual IData* ReadRow(int &id) = 0;
	void UnLoad()
	{
		delete[] m_pData;
	}

protected:
	void ReadValue(int & value)
	{
		value = atoi(m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]]);
	}
	void ReadValue(float &value)
	{
		value = atof(m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]]);
	}
	void ReadValue(long &value)
	{
		value = atoll(m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]]);
	}
	void ReadValue(const char* &value)
	{
		value = m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]];
	}
	void ReadValue(std::vector<std::string> &vecData)
	{
		std::string value = m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]];
		int nPos = 0;
		int nIndex = value.find_first_of("|", nPos);
		while (nIndex != -1)
		{
			vecData.push_back(value.substr(nPos, nIndex-nPos));
			nPos = nIndex+1;
			nIndex = value.find_first_of("|", nPos);
		}
		if (value.size() > 0)
		{
			vecData.push_back(value.substr(nPos));
		}
	}
	void ReadValue(std::vector<int> &vecValue)
	{
		std::string value = m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]];
		int nPos = 0;
		int nIndex = value.find_first_of("|", nPos);
		while (nIndex != -1)
		{
			vecValue.push_back(atoi(value.substr(nPos, nIndex - nPos).c_str()));
			nPos = nIndex + 1;
			nIndex = value.find_first_of("|", nPos);
		}
		if (value.size() > 0)
		{
			vecValue.push_back(atoi(value.substr(nPos).c_str()));
		}
		//char *next_token = 0;
		//char *token = (char*)m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]];
		//token = strtok_s(token, "|", &next_token);
		//while (token != 0)
		//{
		//	vecValue.push_back(atoi(token));
		//	token = strtok_s(NULL, "|", &next_token);
		//}
	}
	void ReadValue(std::vector<float> &vecValue)
	{
		char *next_token = 0;
		char *token = (char*)m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]];
		token = strtok_s(token, "|", &next_token);
		while (token != 0)
		{
			vecValue.push_back((float)atof(token));
			token = strtok_s(NULL, "|", &next_token);
		}
	}
	void ReadValue(std::vector<long> &vecValue)
	{
		char *next_token = 0;
		char *token = (char*)m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]];
		token = strtok_s(token, "|", &next_token);
		while (token != 0)
		{
			vecValue.push_back(atol(token));
			token = strtok_s(NULL, "|", &next_token);
		}
	}
	void ReadValue(std::vector<const char*> &vecValue)
	{
		char *next_token = 0;
		char *token = (char*)m_vecTexts[m_pRecordIdxs[m_nRecordIdx++]];
		token = strtok_s(token, "|", &next_token);
		while (token != 0)
		{
			vecValue.push_back(token);
			token = strtok_s(NULL, "|", &next_token);
		}
	}
	void ReadValue(const json::Value &jvalue, const std::string &prop,int &value)
	{
		value = jvalue[prop].ToInt();
	}
	void ReadValue(const json::Value &jvalue, const std::string &prop, float &value)
	{
		value = jvalue[prop].ToFloat();
	}
	void AddRecord(int id, IData *pData)
	{
		m_mapData[id] = pData;
		m_records.push_back(pData);
	}

protected:
	int m_nDataOffset;
	int m_nRecordIdx;
	int *m_pRecordIdxs;
	std::vector<const char*> m_vecTexts;


protected:
	std::string strName;
	char *m_pData;
	std::vector<IData*> m_records;
	std::unordered_map<int, IData*> m_mapData;
};