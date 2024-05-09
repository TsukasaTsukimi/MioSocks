#include "base.h"
#include "tcp-proxy.h"
#include "tcp2socks.h"
#include "SocketLayer.h"

NetTuple M[65536];

int main()
{
	std::thread th1(Socket_Layer_Process);
	std::thread th2(Tcp2Socks_Process);
	th1.detach();
	th2.detach();
	TCP_Proxy_Process();
	return 0;
}