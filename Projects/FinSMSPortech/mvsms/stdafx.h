// stdafx.h : �i�b�����Y�ɤ��]�t�зǪ��t�� Include �ɡA
// �άO�g�`�ϥΫo�ܤ��ܧ�
// �M�ױM�� Include �ɮ�
//
#pragma once

// �p�G�z�������u����������x�A�Эק�U�C�w�q�C
// �Ѧ� MSDN ���o���P���x�����Ȫ��̷s��T�C
#ifndef WINVER				// ���\�ϥ� Windows XP (�t) �H�᪩�����S�w�\��C
#define WINVER 0x0501		// �N���ܧ󬰰w�� Windows ��L�������A��ȡC
#endif

#ifndef _WIN32_WINNT		// ���\�ϥ� Windows XP (�t) �H�᪩�����S�w�\��C
#define _WIN32_WINNT 0x0501	// �N���ܧ󬰰w�� Windows ��L�������A��ȡC
#endif

#ifndef _WIN32_WINDOWS		// ���\�ϥ� Windows 98 (�t) �H�᪩�����S�w�\��C
#define _WIN32_WINDOWS 0x0410 // �N���ܧ󬰰w�� Windows Me (�t) �H�᪩�����A��ȡC
#endif

#ifndef _WIN32_IE			// ���\�ϥ� IE 6.0 (�t) �H�᪩�����S�w�\��C
#define _WIN32_IE 0x0600	// �N���ܧ󬰰w�� IE ��L�������A��ȡC
#endif

#define WIN32_LEAN_AND_MEAN		// �q Windows ���Y�ư����`�ϥΪ�����
// Windows ���Y��:
#include <windows.h>

// C RunTime ���Y��
#include <stdlib.h>
#include <malloc.h>
#include <memory.h>
#include <tchar.h>
#include <ctype.h>
#include <stdio.h>

#include <winsock2.h>
#pragma comment(lib, "WS2_32")	// �챵��WS2_32.lib

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
