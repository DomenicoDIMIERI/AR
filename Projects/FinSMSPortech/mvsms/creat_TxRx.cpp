#include "stdafx.h"
#include "mv_SMS.h"

//int mgok;
int key_thread;
//int log_id = 0;
/*
int Smessage(void)
{
	P_SMS_PACK sms;

    if (log_id >= g_total_sms)
        goto _err;//log_id = 0;

	sms = psSmsPack(log_id);

    if (!sms->ok_message)
        return log_id;
    else
        log_id ++;
_err:
    return -1;
}
*/



int is_all_Mobile_free(void)
{
	P_MOBILE_PACK mob;
	int i;

    for (i = 0; i < g_total_mobile; i++)
    {
    	mob = psMobilePack(i);
    	if ( mob->tx_flag )
			return 0;
	}

	return 1;
}

int is_all_SMS_free(void)
{
    P_SMS_PACK sms;
	int i;

    for (i = 0; i < g_total_sms; i++)
    {
    	sms = psSmsPack(i);
    	if (!sms->bSending)
    		return 1;
	}

	return 0;
}

int get_free_mobile(int iStart, int nTotal)
{
	P_MOBILE_PACK mob;
	int i = iStart;

	while(nTotal--)
	{
		if (i >= nTotal)
			i = 0;

		mob = psMobilePack(i);

		if (!mob->tx_flag && !mob->module_flag)
		{
			if (mob->bMv372)
				mob->nPort ^= 1;
			else
				mob->nPort = 0;

			return i;
		}

		i++;
	}

	return -1;
}

int get_unsend_message(int iStart, int nTotal)
{
	P_SMS_PACK sms;
	int i = iStart;
	int n = nTotal;

	while(n--)
	{
		if (i >= nTotal)
			i = 0;

		sms = psSmsPack(i);

		if ( !sms->ok_message && sms->nTry )
		{
			return i;
		}

		i++;
	}

	return -1;
}

void smstask(void)
{
    P_MOBILE_PACK mob;
    P_SMS_PACK sms;

    int iSms = -1;
    int iMobile = -1;

//   int i, n;

    while (1)
    {
        Sleep(10);

        iSms = get_unsend_message(iSms+1, g_total_sms);
        if (iSms < 0)
        {
            break;
        }

		sms = psSmsPack(iSms);

		if (sms->bSending)
			continue;

		do
		{
			iMobile = get_free_mobile(iMobile+1, g_total_mobile);
			Sleep(10);
		}
		while (iMobile < 0);


		sms->bSending = 1;
		sms->nTry--;

       	mob = psMobilePack(iMobile);
        mob->iSms = iSms;
        mob->tx_flag = 1;	// start tx task
    }

	while( !is_all_Mobile_free() ) 
		Sleep(10);

	Sleep(10);
}


DWORD WINAPI creat_TxRx()
{
	int i;
	P_MOBILE_PACK mob;

    for (i = 0; i < g_total_mobile; i++)
    {
    	mob = psMobilePack(i);
    	memset( mob, 0, sizeof(S_MOBILE_PACK) );

        hThread = CreateThread( NULL, 0, (LPTHREAD_START_ROUTINE)sms_Rx, (LPVOID)i, 0, (LPDWORD) & threadID );
        CloseHandle(hThread);

        hThread = CreateThread( NULL, 0, (LPTHREAD_START_ROUTINE)sms_Tx, (LPVOID)i, 0, (LPDWORD) & threadID );
        CloseHandle(hThread);
    }

    while (1)
    {
        while (!thread_flag)
        	Sleep(100);

        M_LOG(" SMS Sender Start.\r\n", g_total_sms);
        smstask();

        Sleep(200);

        M_LOG("\r\n");
        M_LOG(" === SMS Sender Ending ===\r\n");

        thread_flag = 0;
    }

    return 0;
}