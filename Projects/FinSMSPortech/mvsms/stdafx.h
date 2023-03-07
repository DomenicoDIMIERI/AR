// stdafx.h : 可在此標頭檔中包含標準的系統 Include 檔，
// 或是經常使用卻很少變更的
// 專案專用 Include 檔案
//
#pragma once

// 如果您有必須優先選取的平台，請修改下列定義。
// 參考 MSDN 取得不同平台對應值的最新資訊。
#ifndef WINVER				// 允許使用 Windows XP (含) 以後版本的特定功能。
#define WINVER 0x0501		// 將它變更為針對 Windows 其他版本的適當值。
#endif

#ifndef _WIN32_WINNT		// 允許使用 Windows XP (含) 以後版本的特定功能。
#define _WIN32_WINNT 0x0501	// 將它變更為針對 Windows 其他版本的適當值。
#endif

#ifndef _WIN32_WINDOWS		// 允許使用 Windows 98 (含) 以後版本的特定功能。
#define _WIN32_WINDOWS 0x0410 // 將它變更為針對 Windows Me (含) 以後版本的適當值。
#endif

#ifndef _WIN32_IE			// 允許使用 IE 6.0 (含) 以後版本的特定功能。
#define _WIN32_IE 0x0600	// 將它變更為針對 IE 其他版本的適當值。
#endif

#define WIN32_LEAN_AND_MEAN		// 從 Windows 標頭排除不常使用的成員
// Windows 標頭檔:
#include <windows.h>

// C RunTime 標頭檔
#include <stdlib.h>
#include <malloc.h>
#include <memory.h>
#include <tchar.h>
#include <ctype.h>
#include <stdio.h>

#include <winsock2.h>
#pragma comment(lib, "WS2_32")	// 鏈接到WS2_32.lib

#include <time.h>
#include <windowsx.h>
#include <commdlg.h>
#include <fcntl.h>
#include <sys/stat.h>

#pragma comment(lib, "shlwapi.lib")
#include <shlwapi.h>
#include <string>

#include "sms_data.h"


#define INIPATH  INIFile


#if (1)
#define LIST_PRINT log_print
#define ERR_PRINT err_print
#define M_ERROR err_print
#define M_PRINT log_print
#define M_LOG log_print
#else
#define LIST_PRINT null_print
#define ERR_PRINT null_print
#define M_ERROR err_print
#define M_PRINT list_print
#define M_LOG list_print
#endif




#define C_CHAR_TAB		9
#define C_CHAR_SPACE	0x20

//#define MAX_SMS 30000


extern TCHAR x_count[10];
extern HWND hEdit,hEdit2;
extern DWORD threadID;
extern HANDLE hThread;

extern char INIFile[MAX_PATH];

extern int g_total_sms;
extern int g_total_mobile;

extern int thread_flag;
//extern int total;
//extern int mgok;

extern int key_thread;
extern int tx_ct;
//extern int log_id;
