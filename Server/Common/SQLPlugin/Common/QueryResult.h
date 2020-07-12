#pragma once
#include <SQLService/IQueryResult.h>

class QueryResult : public IQueryResult
{
public:
    QueryResult(uint64_t row_count, uint32_t field_count)
        :_row_count(row_count)
        ,_filed_count(field_count)
    {};

    ~QueryResult(void) {};

    uint64_t row_count()
    {
        return _row_count;
    }

    uint32_t field_count()
    {
        return _filed_count;
    }

protected:
    uint64_t _row_count;
    uint32_t _filed_count;
};
