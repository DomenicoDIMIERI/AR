#include "stdafx.h"
#include "mv_SMS.h"

DWORD WINAPI show_time()
{
	int hr = 0;
	int min = 0;
	int sec = 0;
	char timebuf[10]; 
 
	while(1)
	{
		if(sec > 59)
		{
			sec = 0;
			min++;
			if(min > 59)
			{
				min = 0;
				hr++;
				if(hr >23)
					hr = 0;
			}
		}
		wsprintf(timebuf,"%02d:%02d:%02d", hr, min, sec);
		ERR_PRINT("%s",timebuf);
		sec ++;
		Sleep(1000);
		if(th_ct > total)
			break;
	}
 return 0;
}