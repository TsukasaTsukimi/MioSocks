#include "SocketLayer.h"

#include <windows.h>
#include <psapi.h>
#include <shlwapi.h>
#include <stdio.h>
#include <stdlib.h>

#pragma comment(lib,"Shlwapi.lib")

#define INET6_ADDRSTRLEN    45

void Socket_Layer_Process()
{

	HANDLE handle;          // WinDivert handle
    HANDLE process;
    DWORD path_len;
	WINDIVERT_ADDRESS addr; // Packet address

    char path[MAX_PATH + 1];
    char local_str[INET6_ADDRSTRLEN + 1], remote_str[INET6_ADDRSTRLEN + 1];
    char* filename;

	// Open some filter
	handle = WinDivertOpen("tcp", WINDIVERT_LAYER_SOCKET, 1121, WINDIVERT_FLAG_SNIFF | WINDIVERT_FLAG_RECV_ONLY);
	if (handle == INVALID_HANDLE_VALUE)
	{
		// Handle error
		exit(1);
	}

	// Main capture-modify-inject loop:
	while (TRUE)
	{
		if (!WinDivertRecv(handle, NULL, 0, NULL, &addr))
		{
			// Handle recv error
			continue;
		}

        if (addr.IPv6 == 1)
            continue;
        if (addr.Event != WINDIVERT_EVENT_SOCKET_CONNECT)
            continue;
        if (addr.Socket.Protocol != IPPROTO_TCP)
            continue;

        /*printf(" pid=");
        printf("%u", addr.Socket.ProcessId);

        printf(" program=");*/
        process = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, FALSE,
            addr.Socket.ProcessId);
        path_len = 0;
        if (process != NULL)
        {
            path_len = GetProcessImageFileNameA(process, path, sizeof(path));
            CloseHandle(process);
        }
        if (path_len != 0)
        {
            filename = PathFindFileNameA(path);
            /*printf("%s", filename);*/
        }
        /*else if (addr.Socket.ProcessId == 4)
        {
            printf("Windows");
        }
        else
        {
            printf("???");
        }

        printf(" endpoint=");
        printf("%llu", addr.Socket.EndpointId);

        printf(" parent=");
        printf("%llu", addr.Socket.ParentEndpointId);*/

        WinDivertHelperFormatIPv6Address(addr.Socket.LocalAddr, local_str,
            sizeof(local_str));
        /*if (addr.Socket.LocalPort != 0 || strcmp(local_str, "::") != 0)
        {
            printf(" local=");
            printf("[%s]:%u", local_str, addr.Socket.LocalPort);
        }*/

        WinDivertHelperFormatIPv6Address(addr.Socket.RemoteAddr, remote_str,
            sizeof(remote_str));
        /*if (addr.Socket.RemotePort != 0 || strcmp(remote_str, "::") != 0)
        {
            printf(" remote=");
            printf("[%s]:%u", remote_str, addr.Socket.RemotePort);
        }*/
        if (path_len != 0)
        {
            printf("[Socket] %s %u:%u %u:%u\n", filename, addr.Socket.LocalAddr, addr.Socket.LocalPort, addr.Socket.RemoteAddr, addr.Socket.RemotePort);
            strcpy(M[addr.Socket.LocalPort].ProcessName, filename);
        }

        /*putchar('\n');*/
	}
}