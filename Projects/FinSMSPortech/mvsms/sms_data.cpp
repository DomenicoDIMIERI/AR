#include "stdafx.h"


S_SMS_PACK sSmsPack[MAX_SMS];
S_MOBILE_PACK sMobilePack[MAX_MOBILE];

P_MOBILE_PACK psMobilePack(int i)
{
	return &sMobilePack[i];
}

P_SMS_PACK psSmsPack(int i)
{
	return &sSmsPack[i];
}
