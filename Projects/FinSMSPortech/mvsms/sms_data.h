#ifndef __SMS_DATA_H__
#define __SMS_DATA_H__
//========================================================================

#define MAX_PASSWORD		(64-1)
#define MAX_PHONE_NUMBER	(64-1)
#define MAX_SMS_TEXT		(512-1)

#define MAX_SMS 		30000
#define MAX_MOBILE 		256
#define MAX_AT_STRING 	1024

typedef struct _SMS_PACK
{
	int ok_message;
	int nTry;
	int nErr;
	int nTryCount;
	int bSending;

	int nType;		// ucs2, 7bit or 8 bit ..

	char phone_str[MAX_PHONE_NUMBER+1];

	WCHAR wsms[MAX_SMS_TEXT+1];
}
S_SMS_PACK, *P_SMS_PACK;

typedef struct _MOBILE_PACK
{
	SOCKET hSocket;

	int bMv372;

	int nPort;

	int tx_flag;
	int rx_flag;

	int connect_ok;
	int module_flag;
	//int send_flag;
	int iSms;
	int bCompOK;

	char szWaitStr[MAX_SMS_TEXT+1];
}
S_MOBILE_PACK, *P_MOBILE_PACK;

extern P_SMS_PACK psSmsPack(int i);
extern P_MOBILE_PACK psMobilePack(int i);

//========================================================================
#endif
