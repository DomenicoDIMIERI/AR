
#include "stdafx.h"
#include "mv_SMS.h"

typedef char *PINT8U;


char CTRL_Z = 26;//簡訊輸入結束
char CTRL_X = 24;//離開簡訊功能

static int szSend(int h, char *s)
{
    return send(h, s, (int)strlen(s), 0);
}

void get_time(int iSMS, int iModile)
{
//    char showbuf[256];
    SYSTEMTIME stime;

    //P_MOBILE_PACK mob = psMobilePack(txopt);
    P_SMS_PACK sms = psSmsPack(iSMS);

    GetLocalTime(&stime);

    M_LOG( "SMS[%d] %s %04d/%02d/%02d %02d:%02d:%02d.\r\n",
              iSMS+1, sms->phone_str, //iModile+1,
              stime.wYear, stime.wMonth, stime.wDay,
              stime.wHour , stime.wMinute , stime.wSecond );
}

UINT chr_utf8_to_uc2(PINT8U po, PINT8U pi)
{
    char uc1, uc2, b2htab[] = "0123456789ABCDEF";
    UINT l = 0;

    if (!pi[0])
    {
        po[0] = 0;
        return 0;
    }
    else if ((pi[0] & 0xf0) == 0xe0)
    {
        uc1 = ((pi[0] & 0xF) << 4) | ((pi[1] >> 2) & 0xF);
        uc2 = (pi[1] << 6)     | (pi[2] & 0x3f);
        l = 3;
    }
    else if ((pi[0] & 0xe0) == 0xc0)
    {
        uc1 = (pi[0] >> 2) & 0x7;
        uc2 = (pi[0] << 6) | (pi[1] & 0x3f);
        l = 2;
    }
    else
    {
        uc1 = 0;
        uc2 = pi[0];
        l = 1;
    }

    po[0] = b2htab[ (uc1 >> 4) & 0x0F];
    po[1] = b2htab[ uc1 & 0x0F];

    po[2] = b2htab[ (uc2 >> 4) & 0x0F];
    po[3] = b2htab[ uc2 & 0x0F];

    po[4] = 0;
    return l;
}

UINT str_utf8_to_uc2(PINT8U po, PINT8U pi)
{
    UINT r, o = 0, i = 0;

    while(pi[i])
    {
        r = chr_utf8_to_uc2(&po[o], &pi[i]);

        o += 4;
        i += r;
    }

    po[o] = 0;
    return o;   // len of uc2 hex string.
}

// =======================================================================
UINT char_uc2_to_hex16(PINT8U po, PINT8U pi)
{
    char b2htab[] = "0123456789ABCDEF";
    UINT l = 0;

    po[2] = b2htab[ (pi[0] >> 4) & 0x0F];
    po[3] = b2htab[ pi[0] & 0x0F];

    po[0] = b2htab[ (pi[1] >> 4) & 0x0F];
    po[1] = b2htab[ pi[1] & 0x0F];

    po[4] = 0;
    return l;
}

int str_uc2_to_hex16(PINT8U po, PINT8U pi)
{
    int i = 0;

    while (pi[i*2] || pi[i*2+1])
    {
        char_uc2_to_hex16(&po[i*4], &pi[i*2]);
        i++;
        if (i > 512)
        {
            LIST_PRINT("str_uc2_to_hex() overflow!\r\n");
            return 0;
        }
    }
    return i;
}

// =======================================================================
char char_uc2_to_ascii7(WCHAR wc)
{
	char c = wc & 0x7f;

	if (c >= ' ') return c;

	switch(c)
	{
	case 0:
	case '\r':
	case '\n':
		return c;
	}

    return ' ';
}

int str_uc2_to_ascii7(PINT8U po, WCHAR *pi)
{
    int i = 0;
    char c;

    while (i < 512)
    {
        c = char_uc2_to_ascii7(pi[i]);
        po[i] = c;
        i++;

        if (!c) break;
    }

    /*
        if (i > 512)
        {
            LIST_PRINT("str_uc2_to_ascii7() overflow!\r\n");
        }
        */

    po[i] = 0;
    return i;
}

// =======================================================================
static size_t make_pdu(PCHAR pd, size_t iMax, PCHAR da, PCHAR msg)//, int nType)
{
    size_t l, i;

    // 00 服務中心號碼 (SMSC 地址)
    // 11
    // 00 TP-MR(TP-Message-Reference)
    strcpy_s(pd, iMax ,"001100");

    l = strlen(da);
    sprintf_s(&pd[6], iMax,"%02X", l); // 0D = strlen(TP-DA);

    if (da[0] == '0')
        strcpy_s(&pd[8], iMax,"81"); // 81 = unknown code
    else
        strcpy_s(&pd[8], iMax,"91"); // 91 =  international  code

    // 683176116125F0 TP-DA 收信人手機號碼
    da[l] = 'F'; //
    for (i=0; i<l; i+=2)
    {
        pd[10+i] = da[i+1];
        pd[11+i] = da[i];
    }
    da[l] = 0; // restore

    // 00 TP-PID (TP-Protocol-Identifier)
    // 08 TP-DCS (TP-Data-Coding-Scheme)
    // 01 TP-VP (TP-Validy-Period)
    strcpy_s(&pd[10+i] , iMax,"000801");

    l = strlen(msg);
    sprintf_s(&pd[16+i],iMax, "%02X", l/2); //06 TP-UDL 短信長度
    sprintf_s(&pd[18+i],iMax, msg);

    return (l+i+18)/2-1;
}

static int rx_char(int sck, char* ch)
{
	char c;
	int rn;

	ch[0] = 0;
	rn = recv(sck, &c, 1, 0);
	if (rn == 1)
	{
		ch[0] = c;
		return 1;
	}

	return 0;
}


static int rx_line(int sck, char* d)
{

}

static int check_username(int sck, char *user)
{
	return 0;
}


static int check_password(int sck, char *user)
{
	return 0;
}

static int connect_mv(P_MOBILE_PACK mob, int opt, char *user, char *pass)
{
    int port;
    char ipaddr[64];
    //char ch;
	char sn[16];
    int mgid;
    int hr, sck = INVALID_SOCKET;

    sockaddr_in s_remote, *p_remote = &s_remote;

//    int rn;
	int b = 0;

    wsprintf(sn, "%d", opt);
    port = GetPrivateProfileInt("PORT", sn, 0, INIPATH);
    GetPrivateProfileString("VOIP", sn, "", ipaddr, 64, INIPATH);

    mgid = mob->iSms;

    sck = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (sck == INVALID_SOCKET)
    {
        M_ERROR("connect_mv(%d): open socket Failed ...\r\n", opt);
        goto _err;
    }

    p_remote->sin_family = AF_INET;
    p_remote->sin_port = htons(port);
    p_remote->sin_addr.S_un.S_addr = inet_addr(ipaddr);

    hr = connect( sck, (sockaddr*)p_remote, sizeof(sockaddr) );
    if (hr < 0)
    {
        M_ERROR("connect_mv(%d): Connect %s:%d fail ...\r\n", opt, ipaddr, port);
        goto _err;
    }

	hr = check_username(sck, user);
	if (!hr) goto _err;

	hr = check_password(sck, pass);
	if (!hr) goto _err;

    return 1;

_err:
    return 0;
}


BOOL waitStr(char* pi, int it, int nMobile)
{
	P_MOBILE_PACK mob = psMobilePack(nMobile);
    int t = it;// * 10;
    char *zkey = mob->szWaitStr;

    strcpy_s(mob->szWaitStr, MAX_AT_STRING, pi);

    while(t--)
    {
    	if(mob->bCompOK < 0)
    		break;

        if(mob->bCompOK)
        {
            zkey[0] = 0;
            mob->bCompOK = 0;

            //Sleep(100);
            return 1;
        }

        Sleep(100);
    }

    zkey[0] = 0;
    mob->bCompOK = 0;
    return 0;
}

int is_all_7bitChar(WCHAR *pi, int max)
{
	WCHAR wc;

	while(max--)
	{
		wc = pi[0];

		if (!wc)
			break;

		if (wc > 127)
			return 0;

		pi++;
	}

	return 1;
}

int unicode_message(P_MOBILE_PACK mob, int txopt, int mgid, char username[], char password[])
{
	P_SMS_PACK sms = psSmsPack(mgid);

	int nTry = 3;
	int sck = mob->hSocket;
	int hr, i = 0;
	size_t l;

	char uTmp[1024], ssTmp[1024], sz[1024];

	nTry = 3;
	do
	{
		hr = waitStr("username:", 20, txopt);
		nTry--;
	} while (hr < 1 && nTry);

	if (hr < 1)
	{
		goto _err;
	}

	sck = mob->hSocket;

	szSend(sck, username);

	hr = waitStr("password:", 20, txopt);
	if (hr < 1) goto _err;

	szSend(sck, password);

	hr = waitStr("command:", 20, txopt);
	if (hr < 1) goto _err;

	sprintf_s(sz, "state%d\r", mob->nPort + 1);
	szSend(sck, sz);

	hr = waitStr("free", 10, txopt);
	if (hr < 1)
	{
		mob->nPort ^= 1;
		sprintf_s(sz, "state%d\r", mob->nPort + 1);
		szSend(sck, sz);

		hr = waitStr("free", 10, txopt);
		if (hr < 1) goto _err;
	}

	sprintf_s(sz, "module%d\r", mob->nPort + 1);
	szSend(sck, sz);

	hr = waitStr("getting", 10, txopt);
	if (hr < 1) goto _err;

	if (sms->nTry)
		M_LOG("Send SMS[%d] %s @ Mobile[%d/%d], retry=%d. \r\n", mgid + 1, sms->phone_str, mob->nPort, txopt + 1, sms->nTry);
	else
		M_LOG("Send SMS[%d] %s @ Mobile[%d/%d], last try.\r\n", mgid + 1, sms->phone_str, mob->nPort, txopt + 1);

	szSend(sck, "ATE1\r");
	hr = waitStr("0\r", 20, txopt);
	if (hr < 1) goto _err;
	Sleep(10);

	szSend(sck, "AT\r");
	hr = waitStr("0\r", 20, txopt);
	if (hr < 1) goto _err;

	//  list_print("wsms[%d]\r\n",wcslen(wsms[mgid]));
	sms->wsms[160] = 0;
	if (is_all_7bitChar(sms->wsms, 160))
	{
		sms->nType = 7;

		szSend(sck, "AT+CMGF=1\r");
		hr = waitStr("0\r", 20, txopt);
		if (hr < 1) goto _err;

		szSend(sck, "AT+CMGS=\"");
		szSend(sck, sms->phone_str);
		szSend(sck, "\"\r");

		//hr = waitStr(">", 20, txopt);
		//if (hr < 1) goto _err;

		str_uc2_to_ascii7(ssTmp, sms->wsms);
	}
	else
	{
		sms->wsms[70] = 0;
		str_uc2_to_hex16(uTmp, (char*)sms->wsms);
		sms->nType = 16;

		szSend(sck, "AT+CMGF=0\r");
		hr = waitStr("0\r", 20, txopt);
		if (hr < 1) goto _err;

		l = make_pdu(ssTmp, 510, (PCHAR)sms->phone_str, uTmp);//, sms->nType);
		sprintf_s(sz, 510, "AT+CMGS=%d\r", l);
		szSend(sck, sz);
	}

	hr = waitStr(">", 20, txopt);
	if (hr < 1) goto _err;

	szSend(sck, ssTmp);
	Sleep(10);
	send(sck, &CTRL_Z, 1, 0);

	hr = waitStr("+CMGS:", 150, txopt);
	if (hr > 0) sms->ok_message = 1;

_err:
	char* err = NULL;
	switch (hr) {
	case EAGAIN:
	case EWOULDBLOCK: err = "The socket's file descriptor is marked O_NONBLOCK and the requested operation would block."; break;
	case EBADF: err = "The socket argument is not a valid file descriptor."; break;
	case ECONNRESET: err = "A connection was forcibly closed by a peer."; break;
	case EDESTADDRREQ: err = "The socket is not connection - mode and no peer address is set."; break;
	case EINTR: err = "A signal interrupted send() before any data was transmitted."; break;
	case EMSGSIZE: err = "The message is too large to be sent all at once, as the socket requires."; break;
	case ENOTCONN: err = "The socket is not connected or otherwise has not had the peer pre - specified."; break;
	case ENOTSOCK: err = "The socket argument does not refer to a socket."; break;
	case EOPNOTSUPP: err = "The socket argument is associated with a socket that does not support one or more of the values set in flags."; break;
	case EPIPE: err = "The socket is shut down for writing, or the socket is connection - mode and is no longer connected.In the latter case, and if the socket is of type SOCK_STREAM, the SIGPIPE signal is generated to the calling thread."; break;
	//The send() function may fail if:
	case EACCES: err = "The calling process does not have the appropriate privileges."; break; 
	case EIO: err = "An I / O error occurred while reading from or writing to the file system."; break;
	case ENETDOWN: err = "The local network interface used to reach the destination is down."; break;
	case ENETUNREACH: err = "No route to the network is present."; break;
	case ENOBUFS: err = "Insufficient resources were available in the system to perform the operation."; break;
	}

    send(sck, &CTRL_X, 1, 0);
    hr = waitStr("release", 10, txopt);

    Sleep(5);

    if (hr)
        szSend(sck, "logout\r");
    else
        szSend(sck, "\rlogout\r");

    waitStr("exit", 10, txopt);

    Sleep(5);

    if (!sms->ok_message)
    {
        if ( sms->nTry)
            M_LOG("Error: SMS[%d] %s send fail! retry=%d (%s).\r\n", mgid + 1, sms->phone_str, sms->nTry, err);
        else
            M_LOG("Error: SMS[%d] %s send fail! give up! (%s)\r\n", mgid + 1, sms->phone_str, err);//, sms->nTry);

        return 0;
    }

    get_time(mgid, txopt);
    return 1;
}


DWORD WINAPI sms_Tx(int nMoible)
{
	P_MOBILE_PACK mob = psMobilePack(nMoible);

    int iSms;// = mob->iSms;
	P_SMS_PACK sms;// = psSmsPack(iSms);

    char username[32], password[32], zNum[16];
    size_t j, k;


    wsprintf(zNum, "%d", nMoible+1);
    GetPrivateProfileString("USER", zNum, "", username, 32, INIPATH);
    k = strlen(username);
    username[k] = '\r';
    username[k+1] = 0;


    GetPrivateProfileString("PASS", zNum, "", password, 32, INIPATH);
    j = strlen(password);
    password[j] = '\r';
    password[j+1] = 0;

    while (1)
    {
        while (!mob->tx_flag)
        {
            Sleep(10);
            if (key_thread) goto _err;
        }

//        mob->send_flag = 1;

        iSms = mob->iSms;

        unicode_message(mob, nMoible, iSms, username, password);

		sms = psSmsPack(iSms);
		sms->bSending = 0;

        //mob->send_flag = 0;
        mob->connect_ok = 0;
        mob->tx_flag = 0;
    }

_err:
	M_PRINT("sms_Tx(): loop break !!\n");

    //mob->tx_flag = 0;

    if (mob->hSocket != INVALID_SOCKET)
    {
        closesocket(mob->hSocket);
        Sleep(10);
        mob->hSocket = INVALID_SOCKET;
    }

    return 0;
}
