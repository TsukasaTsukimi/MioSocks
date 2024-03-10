package windivert

import (
	"fmt"

	"github.com/TsukasaTsukimi/MioSocks/tun/core/device"
	"github.com/TsukasaTsukimi/MioSocks/tun/core/device/iobased"

	"github.com/imgk/divert-go"
)

type DIVERT struct {
	*iobased.Endpoint

	handle *divert.Handle
	name   string
}

func Open(filter string) (_ device.Device, err error) {
	defer func() {
		if r := recover(); r != nil {
			err = fmt.Errorf("open tun: %v", r)
		}
	}()

	handle, err := divert.Open(filter, divert.LayerNetwork, divert.PriorityDefault, divert.FlagDefault)
	if err != nil {
		err = fmt.Errorf("open handle error: %w", err)
		return nil, err
	}

	d := &DIVERT{
		handle: handle,
		name:   "windivert",
	}

	ep, err := iobased.New(d, 65575, 0)
	if err != nil {
		return nil, fmt.Errorf("create endpoint: %w", err)
	}
	d.Endpoint = ep

	return d, nil
}

func (d *DIVERT) Read(packet []byte) (int, error) {
	address := divert.Address{}
	n, err := d.handle.Recv(packet, &address)
	return int(n), err
}

func (d *DIVERT) Write(packet []byte) (int, error) {
	address := divert.Address{}
	n, err := d.handle.Send(packet, &address)
	return int(n), err
}

func (d *DIVERT) Name() string {
	name := d.name
	return name
}

func (d *DIVERT) Close() error {
	defer d.Endpoint.Close()
	return d.handle.Close()
}
