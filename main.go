package main

import (
	"github.com/TsukasaTsukimi/MioSocks/core"
	"github.com/TsukasaTsukimi/MioSocks/core/device/windivert"
	"github.com/TsukasaTsukimi/MioSocks/core/option"
	"github.com/xjasonlyu/tun2soe:\Github\shadow-main\shadow-main\pkgcks/v2/engine/mirror"
	"gvisor.dev/gvisor/pkg/tcpip/stack"
)

var (
	_defaultStack *stack.Stack
	opts          []option.Option
)

func main() {
	d := &windivert.Divert{}
	_defaultStock, err := core.CreateStack(&core.Config{
		LinkEndpoint:     d,
		TransportHandler: &mirror.Tunnel{},
		Options:          opts,
	})
	if err != nil {
		return
	}
}
