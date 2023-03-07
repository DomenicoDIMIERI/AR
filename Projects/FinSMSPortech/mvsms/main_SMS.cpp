// MV_SMS.cpp : 定義應用程式的進入點。
//
#include "stdafx.h"
#include "mv_SMS.h"

#define MAX_LOADSTRING 100

// 全域變數:

HINSTANCE hInst;                                // 目前執行個體
TCHAR szWindowClass[MAX_LOADSTRING];            // 主視窗類別名稱

OPENFILENAME ofn;
TCHAR szTitle[128] = "SMS";
TCHAR szTitle_org[] = "SMS [%s]";
TCHAR szFile[MAX_PATH];
TCHAR szFileTitle[MAX_PATH];

DWORD threadID = 0;
HANDLE hThread = 0;
HWND hEdit,hEdit2;

TCHAR x_count[10];

int open_flag = 0;
int thread_flag = 0;
//int mgct = 0;

int g_total_sms = 0;

//int total;
int g_total_mobile = 0;

char fpath[50];
char INIFile[MAX_PATH];


// 這個程式碼模組中所包含之函式的向前宣告:

ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    IP_Dlg(HWND, UINT, WPARAM, LPARAM);
void check_File();
int file_Open(HWND hEdit);
//int read_File();
void err_print(const char *inMsg, ...);
void list_print(const char *inMsg, ...);
void open_thread();

static int read_phoneNumber(FILE *fd, char *phone, int idx, int r)
{
    //WCHAR wc, wsz[MAX_PHONE_NUMBER+1];
	//char *c = (char*)&wc;
    /*int i = 0;

    while (r >= 2)
    {*/
		/*c[0] = fgetc(fd);

        r--;

        c[1] = fgetc(fd);
        r--;

        if (!wc)
        {
            M_ERROR("PhoneNumber[%d] read null char, Error !!\n", idx);
            return 0;
        }
        else if (wc == '\t')
            break;
        else if (wc > 'z' || wc <= ' ')
            continue;

        if (i >= MAX_PHONE_NUMBER)
        {
            M_ERROR("PhoneNumber[%d] read string too long, Error !!\n", idx);
        }

        wsz[i] = wc;
        i++;
        wsz[i] = 0;*/

 //   }

	//wsz[i+1] = 0;
 //   wcstombs_s(0, phone, MAX_PHONE_NUMBER, wsz, MAX_PHONE_NUMBER); // to ascii.
	int i = 0;
	int loop = 1;
	do {
		int ch = fgetc(fd);
		if (ch < 0) {
			M_ERROR("PhoneNumber[%d] read beyound file end, Error !!\n", idx);
			loop = 0;
			break;
		}
		switch ((char)ch) {
		case '\t':
		case '\r':
		case '\n':
			phone[i] = '\0';
			loop = 0;
			break;
		default:
			if ((ch<(int)('0')) || (ch>(int)('9'))) {
				M_ERROR("PhoneNumber[%d] bad phone number, Error !!\n", idx);
				loop = 0;
				break;
			}
			phone[i] = (char)ch;
		}
		i += 1;		
	} while (loop);
    
	return i;
}



static int read_SMStext(FILE *fd, WCHAR *pwsms, int idx, int r)
{
    /*WCHAR wc, wsz[MAX_SMS_TEXT+1];
    char *c = (char*)&wc;
    int i = 0;

    while (r >= 2)
    {
        c[0] = fgetc(fd);
        r--;

		c[1] = fgetc(fd);
		r--;

        if (!wc)
        {
            M_ERROR("SMStext[%d] read null char, Error !!\r\n", idx);
            return 0;
        }
        else if (wc == '\r')
            break;

        if (i >= MAX_SMS_TEXT)
        {
            M_ERROR("SMStext[%d] read string too long, Error !!\r\n", idx);
        }

        wsz[i] = wc;
        i++;
        wsz[i] = 0;

    }

	wsz[i+1] = 0;
    wcscpy_s(pwsms, MAX_SMS_TEXT, wsz);*/

	int i = 0;
	int loop = 1;
	do {
		int ch = fgetc(fd);
		if (ch < 0) {
			M_ERROR("SMS Text[%d] read beyound file end, Error !!\n", idx);
			loop = 0;
			break;
		}
		switch ((char)ch) {
		case '\r':
		case '\n':
			pwsms[i] = '\0';
			loop = 0;
			break;
		default:
			pwsms[i] = (char)ch;
		}
		i += 1;
	} while (loop);

    return i;
}

static int read_File(void)
{
	P_SMS_PACK sms = 0;// = psSmsPack(mgid);

    FILE *fd;
    int i, j = 0, flen, r;
    errno_t nErr;

    g_total_sms = 0;
    nErr = fopen_s(&fd, fpath, "rb");
    if (nErr)
    {
        LIST_PRINT("Open File Failed !!\r\n");
        return 0 ;
    }


	fseek(fd, 0, SEEK_END); 	// set pointer to end of file
 	flen = ftell(fd); 		// offset in bytes from file's beginning
 	if (flen < 2) goto _ex;
 	fseek(fd, 0, SEEK_SET); 	// restore original position

    i = 0;
    r = flen;

    for (i = 0; i < MAX_SMS; i++)
    {
        sms = psSmsPack(i);
        memset(sms, 0, sizeof(S_SMS_PACK));

        r = read_phoneNumber(fd, sms->phone_str, i, r);
        if (r < 2) break;

        r = read_SMStext(fd, sms->wsms, i, flen);
        if (r < 2) break;

        sms->nTry = 3;
        g_total_sms++;
    }

    M_LOG("Read file: SMS Total = %d.\r\n", g_total_sms);

_ex:
    if (fd) fclose(fd);
    return g_total_sms;
}


int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
    int zlen;
    zlen = GetModuleFileName( 0, INIFile, MAX_PATH-6 );
    PathRenameExtension(INIFile, ".ini");

        WSADATA wsaData;
        WORD sockVersion = MAKEWORD(2, 2);// 初始化

        if(WSAStartup(sockVersion, &wsaData) != 0)
        {
            return 0;
        }

    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // TODO: 在此置入程式碼。
    MSG msg;
    HACCEL hAccelTable;

    // 初始化全域字串
    LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInstance, IDC_MV_SMS, szWindowClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);

    // 執行應用程式初始設定:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }

    hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_MV_SMS));

    // 主訊息迴圈:
    while (GetMessage(&msg, NULL, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int) msg.wParam;
}

ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEX wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = WndProc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_MV_SMS));
    wcex.hCursor        = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground  = (HBRUSH)(COLOR_WINDOW+1);
    wcex.lpszMenuName   = MAKEINTRESOURCE(IDC_MV_SMS);
    wcex.lpszClassName  = szWindowClass;
    wcex.hIconSm        = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassEx(&wcex);
}

BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
    HWND hWnd;

    hInst = hInstance; // 將執行個體控制代碼儲存在全域變數中

    hWnd = CreateWindow(szWindowClass,
                        szTitle,
                        WS_OVERLAPPEDWINDOW,
                        CW_USEDEFAULT,
                        0,
                        500,    // 寬度
                        300,    // 高度
                        NULL,
                        NULL,
                        hInstance,
                        NULL);

    if (!hWnd)
    {
        return FALSE;
    }

    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);

    return TRUE;
}


LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    //LRESULT hr;
    int wmId, wmEvent, id;
    static HMENU hMenu;

    switch (message)
    {
    case WM_CREATE:

        hMenu = GetMenu(hWnd);

        hEdit = CreateWindow( "EDIT", NULL,
                              WS_CHILD | ES_READONLY | WS_BORDER | WS_VSCROLL
                              | ES_AUTOVSCROLL | ES_MULTILINE | WS_VISIBLE,
                              0, 0, 0, 0, hWnd, (HMENU)ID_EDIT, hInst, NULL);

        SendMessage(hEdit, EM_SETLIMITTEXT, 90000*80, 0);

        hEdit2 = CreateWindow( "EDIT", NULL,
                               WS_CHILD | ES_NOHIDESEL | WS_BORDER | ES_AUTOHSCROLL
                               | ES_AUTOVSCROLL | ES_MULTILINE | WS_VISIBLE,
                               0, 0, 0, 0, hWnd, (HMENU)ID_EDIT2, hInst, NULL) ;
        check_File();
        open_thread();
        break;

    case WM_SIZE:
        MoveWindow(hEdit, 0, 0, LOWORD(lParam), HIWORD(lParam) - 25, TRUE);
        MoveWindow(hEdit2, 0, HIWORD(lParam) - 25, LOWORD(lParam) , 25, TRUE);
        break;

    case WM_COMMAND:

        wmId    = LOWORD(wParam);
        wmEvent = HIWORD(wParam);

        switch (wmId)
        {
        case IDM_FILE:
            ERR_PRINT("");
            SendMessage(hEdit, VK_CLEAR, 0, 0);
            id = file_Open(hEdit);
            if (id && (!thread_flag))
            {
                id = read_File();
                if (id)
                    open_flag = 1;
                else
                    open_flag = 0;

            }
            else
                open_flag = 0;
            break;

        case IDM_MESSAGE:

            id = IDYES;//MessageBox(hWnd, "Are you sure to send SMS？", "Send SMS Message", MB_YESNO | MB_ICONQUESTION);

            if (id == IDYES)
            {
                SendMessage(hEdit, VK_CLEAR, 0, 0);

                if (open_flag && (!thread_flag))
                {
                    thread_flag = 1;
                }
                else
                    ERR_PRINT("Send Message Failed ...");
            }
            break;

        case IDM_EXIT:
            WSACleanup();
            SendMessage(hWnd, WM_CLOSE, 0, 0);
            break;

        case IDM_ABOUT:
            DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
            break;
        }

    case WM_SETFOCUS:
        break;

    case WM_CLOSE:

        id = IDYES;//MessageBox( hWnd, "Are you sure to exit of the program ？", "Exit Message", MB_YESNO | MB_ICONQUESTION);

        if (id == IDYES)
        {
            key_thread = 1;
            //memset(tx_flag, 1, sizeof(tx_flag));
            DestroyWindow(hEdit);
            DestroyWindow(hEdit2);
            DestroyWindow(hWnd);
        }
        break;

    case WM_DESTROY:
        PostQuitMessage(0);
        break;

    default:

        return DefWindowProc(hWnd, message, wParam, lParam);
    }

    return 0;
}

// [關於] 方塊的訊息處理常式。
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);

    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}

UINT WritePrivateProfileInt(PCHAR p_Sec, PCHAR p_Key, int iNum, PCSTR p_File )
{
    DWORD hr;
    TCHAR zTmp[20];

    wsprintf(zTmp, "%i", iNum );
    hr =  WritePrivateProfileString( p_Sec, p_Key, zTmp, p_File );
    return hr;
}

void check_File()
{
    int fd;
    struct stat buf;

    fd = stat(INIPATH, &buf);
    if(fd != 0)
    {
        wsprintf(x_count,"%d",1);
        WritePrivateProfileInt("info", "Total", 1 , INIPATH);
        WritePrivateProfileString("VOIP", x_count, ("192.168.0.100"), INIPATH);
        WritePrivateProfileString("PORT", x_count, ("23"), INIPATH);
        WritePrivateProfileString("USER", x_count, ("voip"), INIPATH);
        WritePrivateProfileString("PASS", x_count, ("1234"), INIPATH);
    }

    g_total_mobile = GetPrivateProfileInt("info", "Total", 0, INIPATH);
}

int file_Open(HWND hEdit)
{
    int bOK;
    DWORD dwSize = 0L;
    HWND hMain;

    memset((void*)&ofn, 0, sizeof(ofn));
    ofn.lStructSize = sizeof(OPENFILENAME);
    ofn.hwndOwner = hEdit;
    ofn.lpstrFilter = TEXT("text(*.txt)\0*.txt\0All files(*.*)\0*.*\0\0");
    ofn.lpstrFile = szFile;
    ofn.lpstrFileTitle =szFileTitle;
    ofn.nMaxFile = MAX_PATH;
    ofn.nMaxFileTitle = MAX_PATH;
    ofn.Flags = OFN_FILEMUSTEXIST | OFN_HIDEREADONLY;

    bOK = GetOpenFileName(&ofn);
    if(!bOK)
    {
        DWORD err = GetLastError();
        hMain = GetParent(hEdit);
        SetWindowText(hMain, TEXT("SMS"));
        return 0;
    }
    wsprintf(fpath, "%s", szFile);
    wsprintf(szTitle, szTitle_org, szFileTitle);
    hMain = GetParent(hEdit);
    SetWindowText(hMain, szTitle);
    return 1;
}

void null_print(const char *inMsg, ...)
{
}

void list_print(const char *inMsg, ...)
{
    char zMsg[1024];

    zMsg[0] = 0;
    va_list ap;
    va_start(ap, inMsg);
    wvsprintf(zMsg, inMsg, ap);
    va_end(ap);
    SendMessage(hEdit, EM_GETSEL, 0, 0);
    SendMessage(hEdit, EM_SETSEL, 0, 0);
    SendMessage(hEdit, EM_REPLACESEL, 0, (LPARAM)zMsg);

}


void log_print(const char *inMsg, ...)
{
    char zMsg[1024];

    zMsg[0] = 0;
    va_list ap;
    va_start(ap, inMsg);
    wvsprintf(zMsg, inMsg, ap);
    va_end(ap);
    //SendMessage(hEdit, EM_GETSEL, 0, 0);
    SendMessage(hEdit, EM_SETSEL, -1, -1);
    SendMessage(hEdit, EM_REPLACESEL, 0, (LPARAM)zMsg);

}

void err_print(const char *inMsg, ...)
{
    char zMsg[256];

    zMsg[0] = 0;
    va_list ap;
    va_start(ap, inMsg);
    wvsprintf(zMsg, inMsg, ap);
    va_end(ap);
    SendMessage(hEdit2, WM_SETTEXT, 0, (LPARAM)zMsg);
}

void open_thread()
{
    hThread = CreateThread( NULL, 0, (LPTHREAD_START_ROUTINE)creat_TxRx, 0, 0, (LPDWORD)&threadID);
    CloseHandle(hThread);
}

