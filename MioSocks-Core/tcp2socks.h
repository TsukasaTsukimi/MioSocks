#pragma once

#include "base.h"
#include <ws2tcpip.h>
#include "socks5.h"

#pragma comment(lib,"Socks5.lib")

int Tcp2Socks_Process();