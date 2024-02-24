package windivert

import "github.com/imgk/divert-go"

type Divert struct {
	Device
	Address *divert.Address
	Handle  *divert.Handle
}
