#pragma once
#include "../Common/QueryResult.h"
#ifdef _WINDOWS
#include "winsock2.h"
typedef int	socklen_t;
#else
#include "sys/socket.h"
#include "sys/ioctl.h"
#include "netinet/in.h"
#include "netdb.h"
#include "fcntl.h"
#include "errno.h"
#include "unistd.h"
#include "sys/stat.h"
#include "sys/types.h"
#include "arpa/inet.h"
typedef int	SOCKET;

//#pragma region define win32 const variable in linux
#define INVALID_SOCKET	-1
#define SOCKET_ERROR	-1
//#pragma endregion
#endif

#include <mysql.h>

class MysqlQueryResult :
    public QueryResult
{
public:
    MysqlQueryResult(MYSQL_RES *res, uint64_t row_count, uint32_t field_count);
    ~MysqlQueryResult(void);

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
    MYSQL_ROW _row;
    MYSQL_ROW_OFFSET _begin;
    MYSQL_RES *_mysql_res;
};
