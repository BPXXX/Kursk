// stdafx.h: 标准系统包含文件的包含文件，
// 或是经常使用但不常更改的
// 项目特定的包含文件
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // 从 Windows 头文件中排除极少使用的内容
// Windows 头文件
#include <windows.h>

#include <base/def.h>
#include <base/atomic.h>
#include <base/random.h>
#include <base/lru_map.h>
#include <base/fast.h>
#include <base/fastream.h>
#include <base/fastring.h>
#include <base/str.h>
#include <base/flag.h>
#include <base/log.h>
#include <base/unitest.h>
#include <base/time.h>
#include <base/thread.h>
#include <base/co.h>
#include <base/json.h>
#include <base/rpc.h>
#include <base/hash.h>
#include <base/rpc.h>
#include <base/path.h>
#include <base/fs.h>
#include <base/os.h>
#include <base/hash/md5.h>

// 在此处引用程序需要的其他标头
