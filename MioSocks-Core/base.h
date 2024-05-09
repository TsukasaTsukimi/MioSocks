#pragma once

#include <iostream>
#include <thread>
#include <winsock2.h>
#include "windivert.h"

#pragma comment(lib,"WinDivert.lib")
#pragma comment(lib,"Ws2_32.lib")

struct NetTuple
{
	UINT32 SrcAddr;
	UINT16 SrcPort;
	UINT32 DstAddr;
	UINT16 DstPort;
	char ProcessName[MAX_PATH];
};

extern NetTuple M[65536];