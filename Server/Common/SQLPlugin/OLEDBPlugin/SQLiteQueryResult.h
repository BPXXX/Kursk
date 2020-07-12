#pragma once
#include "../Common/QueryResult.h"
#include "./sqlite/sqlite3.h"

class SQLiteQueryResult :
    public QueryResult
{
public:
	SQLiteQueryResult(sqlite3_stmt *stmt, uint64_t row_count, uint32_t field_count);
    ~SQLiteQueryResult(void);

    virtual bool Read()override;
    virtual void SetToBegin()override;
    virtual std::string field_name(int n)override;
    virtual uint32_t length(int32_t n)override;

    virtual uint32_t get_data(int n, char *data)override;
    virtual int32_t get_int32(int n)override;
    virtual int64_t get_int64(int n)override;
    virtual float get_float(int n)override;
    virtual double get_double(int n)override;
    virtual std::string get_string(int n)override;

private:
    sqlite3_stmt *_sqlite_stmt;
};
