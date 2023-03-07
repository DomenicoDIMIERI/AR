
#include "stdafx.h"
#include "mv_SMS.h"

#define C_XON    17
#define C_XOFF   19

int rx_flag[MAX_SMS];
int gbCompOK[MAX_SMS];
int rx_connect_ok[MAX_SMS];

//SOCKET g_rx_sck[MAX_SMS] = {INVALID_SOCKET};
int hequ(char* d, char* h)
{
    char c;
    while (c = h[0])
    {
        if (d[0] != c)
            return 0;
        h++;
        d++;
    }

    return 1;
}

DWORD WINAPI sms_Rx(int nMoible)
{
    P_MOBILE_PACK mob = psMobilePack(nMoible);
    int port;
    char ipaddr[64];
    char Tmp[1024], zNum[32];
    char *pk;

    //int rxopt = nMoible;
    int mgid;
    int hr, sck = INVALID_SOCKET;

    mob->hSocket = sck;

    wsprintf(zNum, "%d", nMoible + 1);
    port = GetPrivateProfileInt("PORT", zNum, 0, INIPATH);

    GetPrivateProfileString("VOIP", zNum, "", ipaddr, 63, INIPATH);
    mgid = mob->iSms;

    int nRecv, b = 0;
    char ch, cs[2];

    cs[1] = 0;

    pk = mob->szWaitStr;

    while (1)
    {
        if (mob->module_flag)
            goto _err;

        while ( strcmp(pk, "username:") )
        {
            Sleep(100);
            if (mob->rx_flag)
                goto _err;
        };

        sck = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
        if (sck == INVALID_SOCKET)
        {
            LIST_PRINT("[r%d] socket System Failed ...\r\n", nMoible);
            goto _err;
        }

        mob->hSocket = sck;

        // 填寫遠端位址資訊
        sockaddr_in servAddr;
        servAddr.sin_family = AF_INET;
        servAddr.sin_port = htons(port);
        servAddr.sin_addr.S_un.S_addr = inet_addr(ipaddr);

        hr = connect( sck, (sockaddr*) & servAddr, sizeof(servAddr) );
		char* err = NULL;
		switch (hr) {
		case EADDRNOTAVAIL: err = "The specified address is not available from the local machine."; break;
		case EAFNOSUPPORT: err = "The specified address is not a valid address for the address family of the specified socket."; break;
		case EALREADY: err = "A connection request is already in progress for the specified socket."; break;
		case EBADF: err = "The socket argument is not a valid file descriptor."; break;
		case ECONNREFUSED: err = "The target address was not listening for connections or refused the connection request."; break;
		case EINPROGRESS: err = "O_NONBLOCK is set for the file descriptor for the socket and the connection cannot be immediately established; the connection shall be established asynchronously."; break;
		case EINTR: err = "The attempt to establish a connection was interrupted by delivery of a signal that was caught; the connection shall be established asynchronously."; break;
		case EISCONN: err = "The specified socket is connection - mode and is already connected."; break;
		case ENETUNREACH: err = "No route to the network is present."; break;
		case ENOTSOCK: err = "The socket argument does not refer to a socket."; break;
		case EPROTOTYPE: err = "The specified address has a different type than the socket bound to the specified peer address."; break;
		case ETIMEDOUT: err = "The attempt to connect timed out before a connection was made."; break;
		//If the address family of the socket is AF_UNIX, then connect() shall fail if:
		case EIO: err = "An I / O error occurred while reading from or writing to the file system."; break;
		case ELOOP: err = "A loop exists in symbolic links encountered during resolution of the pathname in address."; break;
		//case ELOOP: err = "More than{ SYMLOOP_MAX } symbolic links were encountered during resolution of the pathname in address."; break;
		case ENAMETOOLONG: err = "A component of a pathname exceeded{ NAME_MAX } characters, or an entire pathname exceeded{ PATH_MAX } characters."; break;
		//case ENAMETOOLONG: err = "Pathname resolution of a symbolic link produced an intermediate result whose length exceeds{ PATH_MAX }."; break;
		case ENOENT: err = "A component of the pathname does not name an existing file or the pathname is an empty string."; break;
		case ENOTDIR: err = "A component of the path prefix of the pathname in address is not a directory."; break;
		//The connect() function may fail if:
		case EACCES: err = "Search permission is denied for a component of the path prefix; or write access to the named socket is denied."; break;
		case EADDRINUSE: err = "Attempt to establish a connection that uses addresses that are already in use."; break;
		case ECONNRESET: err = "Remote host reset the connection request."; break;
		case EHOSTUNREACH: err = "The destination host cannot be reached(probably because the host is down or a remote router cannot reach it)."; break;
		case EINVAL: err = "The address_len argument is not a valid length for the address family; or invalid address family in the sockaddr structure."; break;
		case ENETDOWN: err = "The local network interface used to reach the destination is down."; break;
		case ENOBUFS: err = "No buffer space is available."; break;
		case EOPNOTSUPP: err = "The socket is listening and cannot be connected."; break;
		}

        if (err != NULL)
        {
			LIST_PRINT("[r%d] Connect %s:%d  fail ... %s\r\n", nMoible, ipaddr, port, err);
			goto _err;
        }

        Tmp[0] = 0;

        mob->connect_ok = 1;

        while (!mob->rx_flag)
        {
            //while ( !pk[0] )
            //  Sleep(10);

            nRecv = recv(sck, &ch, 1, 0);
            if (nRecv < 1 || mob->rx_flag) // 讀取到錯誤
                goto _err;

            else if (ch == C_XON || ch == C_XOFF || !ch )
                continue;

            cs[0] = ch;
            // M_LOG("%s", cs); // $$$$$ Debug Only

            if (ch == '\n')
            {
                if (nMoible == 1)
                {
                    M_LOG("[%d]", nMoible);
                }

                b = 0;
                Tmp[b] = 0;
                continue;
            }

            Tmp[b] = ch;
            b++;
            Tmp[b] = 0;

            if (ch == 26 || ch == '\r' || ch == '>' || ch == ':' )
            {
                if ( !pk[0] ) continue;

                if ( ch == ':' )
                {
                	if  ( hequ(Tmp, "username:") || hequ(Tmp, "password:") )
                	{
                		pk[0] = 0;
                		mob->bCompOK = 1;
                	}
                	else
                		continue;
            	}

                if ( hequ(pk, Tmp) )
                {
                    pk[0] = 0;
                    mob->bCompOK = 1;
				}
                else if ( hequ(Tmp, "got!! ") ) //||
                {
                    pk[0] = 0;
                    mob->bCompOK = 1;
            	}
            	else if ( hequ(Tmp, "0\r") || hequ(Tmp, "+CMGS:") )
                {
                    pk[0] = 0;
                    mob->bCompOK = 1;
            	}
				else if ( hequ(Tmp, "module ") )
				{
				 	if ( strstr(Tmp, ": free") )
				 	{
				 		pk[0] = 0;
				 		mob->bCompOK = 1;
				 	}
				 	else if ( strstr(Tmp, ": none") )
				 	{
				 		pk[0] = 0;
				 		mob->bCompOK = -1;
				 	}
				}
				else if ( hequ(Tmp, "bad command!!!") || hequ(Tmp, "+CMS ERROR") || hequ(Tmp, "+CME ERROR") || hequ(Tmp, "4\r") )
				{
                    pk[0] = 0;
                    mob->bCompOK = -1;
				}
				else if ( hequ(Tmp, " command: ") )
                {
					mob->bMv372 = ( strstr(Tmp, "module2") ) ? 1 : 0;
					pk[0] = 0;
					mob->bCompOK = 1;
                }
                else if ( hequ(Tmp, "exit...") )
                {
					pk[0] = 0;
					mob->bCompOK = 1;
                    goto _err;
                }

                b = 0;
            }
        }

_err:
        Sleep(10);
        pk[0] = 0;
        mob->bCompOK = -1;
        if (sck != INVALID_SOCKET && mob->hSocket != INVALID_SOCKET) //還有資料重新連結
        {
            closesocket(sck);
            sck = INVALID_SOCKET;
            mob->hSocket = sck;

        }

        if (mob->rx_flag)
        {
            break;
        }

    }

    return 0;
}
