#include "ODBCQueryResult.h"

ODBCQueryResult::ODBCQueryResult(SQLHSTMT res, uint64_t row_count, uint32_t field_count)
: QueryResult(row_count, field_count)
, m_hstmt(res)
{

}

ODBCQueryResult::~ODBCQueryResult(void)
{
}

bool ODBCQueryResult::Read()
{
	SQLRETURN mResult = SQLFetch(m_hstmt);

	if ((mResult != SQL_SUCCESS) && (mResult != SQL_SUCCESS_WITH_INFO) && (mResult != SQL_NO_DATA)) 
	{
		return false;
	}

	if(mResult == SQL_NO_DATA)
	{
		return false;
	}

	return true;
}

void ODBCQueryResult::SetToBegin()
{
	SQLSetPos(m_hstmt, 0, SQL_POSITION, SQL_LOCK_NO_CHANGE);
}

std::string ODBCQueryResult::field_name(int n)
{
	SQLCHAR colName[129];
	union
	{
		unsigned  int   size;
		long            sizeLong;
	}colValueSizeUnion;
	union
	{
		SQLSMALLINT     size;
		long            sizeLong;
	} colNameSizeUnion;
	SQLSMALLINT       colType;
	SQLSMALLINT         scale;
	SQLSMALLINT         nullable;

	SQLDescribeColA(m_hstmt, n, colName, sizeof(colName), (SQLSMALLINT*)&colNameSizeUnion.size, &colType, (SQLULEN*)&colValueSizeUnion.size, &scale, &nullable);

	return std::string((const char*)colName);
}

uint32_t ODBCQueryResult::length(int n)
{
	SQLCHAR colName[129];
	union
	{
		unsigned  int   size;
		long            sizeLong;
	}colValueSizeUnion;
	union
	{
		SQLSMALLINT     size;
		long            sizeLong;
	} colNameSizeUnion;
	SQLSMALLINT       colType;
	SQLSMALLINT         scale;
	SQLSMALLINT         nullable;

	SQLDescribeColA(m_hstmt, n, colName, sizeof(colName), (SQLSMALLINT*)&colNameSizeUnion.size, &colType, (SQLULEN*)&colValueSizeUnion.size, &scale, &nullable);

	return colValueSizeUnion.size;
}

uint32_t ODBCQueryResult::get_data(int n, char *data)
{
    char *pBuf = NULL;

    uint32_t l = length(n);
	pBuf = new char[l];

	SQLLEN sql_data_len;
	SQLGetData(m_hstmt, n, SQL_INTEGER, data, l, &sql_data_len);

	return sql_data_len;
}

int32_t ODBCQueryResult::get_int32(int n)
{
	int32_t value;
	SQLLEN sql_data_len;
	SQLGetData(m_hstmt, n, SQL_INTEGER, &value, sizeof(value), &sql_data_len);
	return value;
}

int64_t ODBCQueryResult::get_int64(int n)
{
	int64_t value;
	SQLLEN sql_data_len;
	SQLGetData(m_hstmt, n, SQL_INTEGER, &value, sizeof(value), &sql_data_len);
	return value;
}

float ODBCQueryResult::get_float(int n)
{
	float value;
	SQLLEN sql_data_len;
	SQLGetData(m_hstmt, n, SQL_INTEGER, &value, sizeof(value), &sql_data_len);
	return value;
}

double ODBCQueryResult::get_double(int n)
{
	double value;
	SQLLEN sql_data_len;
	SQLGetData(m_hstmt, n, SQL_INTEGER, &value, sizeof(value), &sql_data_len);
	return value;
}

std::string ODBCQueryResult::get_string(int n)
{
	char buf[1024] = { 0 };
	SQLLEN sql_data_len;
	SQLGetData(m_hstmt, n, SQL_INTEGER, &buf, sizeof(buf), &sql_data_len);
	buf[sql_data_len] = 0;
	return buf;
}
