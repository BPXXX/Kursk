#pragma once
#include "../Common/QueryResult.h"

#include <windows.h>
#include "sql.h"
#include "sqlext.h"

class ODBCQueryResult :
    public QueryResult
{
public:
	ODBCQueryResult(SQLHSTMT res, uint64_t row_count, uint32_t field_count);
    ~ODBCQueryResult(void);

    bool Read();
    void SetToBegin();
    std::string field_name(int n);
    uint32_t length(int32_t n);

    uint32_t get_data(int n, char *data);
    int32_t get_int32(int n);
    int64_t get_int64(int n);
    float get_float(int n);
    double get_double(int n);
    std::string get_string(int n);
private:
	SQLHSTMT m_hstmt;
};
