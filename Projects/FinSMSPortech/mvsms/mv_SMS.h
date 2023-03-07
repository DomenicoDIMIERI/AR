#pragma once
#include "resource.h"

extern DWORD WINAPI creat_TxRx();
extern DWORD WINAPI sms_Rx(int opt);
extern DWORD WINAPI sms_Tx(int opt);

extern void log_print(const char *inMsg, ...);
extern void err_print(const char *inMsg, ...);
extern void list_print(const char *inMsg, ...);
extern void null_print(const char *inMsg, ...);

extern UINT WritePrivateProfileInt(PCHAR p_Sec, PCHAR p_Key, int iNum, PCSTR p_File );

