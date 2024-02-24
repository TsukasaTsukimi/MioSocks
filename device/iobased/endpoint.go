package iobased

import (
	"context"
	"errors"
	"io"
	"sync"

	"gvisor.dev/gvisor/pkg/buffer"
	"gvisor.dev/gvisor/pkg/tcpip"
	"gvisor.dev/gvisor/pkg/tcpip/header"
	"gvisor.dev/gvisor/pkg/tcpip/link/channel"
	"gvisor.dev/gvisor/pkg/tcpip/stack"
)

const (
	// Queue length for outbound packet, arriving for read. Overflow
	// causes packet drops.
	defaultOutQueueLen = 1 << 10
)

type Endpoint struct {
	*channel.Endpoint
	rw     io.ReadWriter
	mtu    uint32
	offset int
	once   sync.Once
	wg     sync.WaitGroup
}

func NewEndpoint(rw io.ReadWriter, mtu uint32, offset int) (*Endpoint, error) {
	if mtu == 0 {
		return nil, errors.New("MTU size is zero")
	}

	if rw == nil {
		return nil, errors.New("ReadWriter interface is nil")
	}

	if offset < 0 {
		return nil, errors.New("offset must be non-negative")
	}

	return &Endpoint{
		Endpoint: channel.New(defaultOutQueueLen, mtu, ""),
		rw:       rw,
		mtu:      mtu,
		offset:   offset,
	}, nil
}

// dispatchLoop dispatches packets to upper layer.
func (e *Endpoint) dispatchLoop(cancel context.CancelFunc) {
	// Call cancel() to ensure (*Endpoint).outboundLoop(context.Context) exits
	// gracefully after (*Endpoint).dispatchLoop(context.CancelFunc) returns.
	defer cancel()

	offset, mtu := e.offset, int(e.mtu)

	for {
		data := make([]byte, offset+mtu)

		n, err := e.rw.Read(data)
		if err != nil {
			break
		}

		if n == 0 || n > mtu {
			continue
		}

		if !e.IsAttached() {
			continue /* unattached, drop packet */
		}

		pkt := stack.NewPacketBuffer(stack.PacketBufferOptions{
			Payload: buffer.MakeWithData(data[offset : offset+n]),
		})

		switch header.IPVersion(data[offset:]) {
		case header.IPv4Version:
			e.InjectInbound(header.IPv4ProtocolNumber, pkt)
		case header.IPv6Version:
			e.InjectInbound(header.IPv6ProtocolNumber, pkt)
		}
		pkt.DecRef()
	}
}

// outboundLoop reads outbound packets from channel, and then it calls
// writePacket to send those packets back to lower layer.
func (e *Endpoint) outboundLoop(ctx context.Context) {
	for {
		pkt := e.ReadContext(ctx)
		if pkt.IsNil() {
			break
		}
		e.writePacket(pkt)
	}
}

// writePacket writes outbound packets to the io.Writer.
func (e *Endpoint) writePacket(pkt stack.PacketBufferPtr) tcpip.Error {
	defer pkt.DecRef()

	buf := pkt.ToBuffer()
	defer buf.Release()
	if e.offset != 0 {
		v := buffer.NewViewWithData(make([]byte, e.offset))
		_ = buf.Prepend(v)
	}

	if _, err := e.rw.Write(buf.Flatten()); err != nil {
		return &tcpip.ErrInvalidEndpointState{}
	}
	return nil
}
