package main

import (
	"log"
	"net"
	"os"
	"os/signal"
	"syscall"

	"github.com/TsukasaTsukimi/MioSocks/netstack"
	"github.com/TsukasaTsukimi/MioSocks/tun"
)

type myHandler struct{}

func (*myHandler) HandleTCPConn(info *netstack.ConnTuple, conn net.Conn) {
	log.Printf("tcp, src: %s, dst: %s", info.Src(), info.Dst())
	// do something...
}
func (*myHandler) HandleUDPConn(info *netstack.ConnTuple, conn net.PacketConn) {
	log.Printf("udp, src: %s, dst: %s", info.Src(), info.Dst())
	// do something...
}

func main() {
	nt := netstack.New(tun.TunConfig{
		Name: "tun2",
		Addr: "192.18.0.1/16",
		MTU:  tun.DefaultMTU,
	})
	if err := nt.Start(); err != nil {
		log.Fatal(err)
	}
	defer nt.Close()
	nt.RegisterConnHandler(&myHandler{})
	interrupt := make(chan os.Signal, 1)
	signal.Notify(interrupt, syscall.SIGINT)
	<-interrupt
}
