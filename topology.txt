#!/usr/bin/python
"""
This is the most simple example to showcase Containernet.
"""
from mininet.net import Containernet
from mininet.node import OVSSwitch, Controller, RemoteController
from mininet.cli import CLI
from mininet.link import TCLink
from mininet.log import info, setLogLevel
setLogLevel('info')

net = Containernet(controller=RemoteController)

info('*** Adding controller\n')
net.addController( 'c0',controller=RemoteController, ip='192.168.88.132', port=6653 )

info('*** Adding docker containers\n')
d1 = net.addDocker('d1', ip='10.0.0.1', dimage="ubuntu:trusty")
d2 = net.addDocker('d2', ip='10.0.0.2', dimage="ubuntu:trusty")
d3 = net.addDocker('d3', ip='10.0.0.3', dimage="ubuntu:trusty")
d4 = net.addDocker('d4', ip='10.0.0.4', dimage="ubuntu:trusty")
d5 = net.addDocker('d5', ip='10.0.0.5', dimage="ubuntu:trusty")
d6 = net.addDocker('d6', ip='10.0.0.6', dimage="ubuntu:trusty")

info('*** Adding switches\n')
s1 = net.addSwitch('s1', protocols="OpenFlow13" )
s2 = net.addSwitch('s2', protocols="OpenFlow13" )
s3 = net.addSwitch('s3', protocols="OpenFlow13" )
s4 = net.addSwitch('s4', protocols="OpenFlow13" )
s5 = net.addSwitch('s5', protocols="OpenFlow13" )
s6 = net.addSwitch('s6', protocols="OpenFlow13" )


info('*** Creating links\n')

net.addLink(d1, s1)
net.addLink(d2, s2)
net.addLink(d3, s3)
net.addLink(d4, s4)
net.addLink(d5, s5)
net.addLink(d6, s6)

net.addLink(s1, s2)
net.addLink(s2, s3)
net.addLink(s3, s4)
net.addLink(s4, s5)
net.addLink(s5, s6)
net.addLink(s6, s1)
net.addLink(s1, s3)
net.addLink(s3, s5)
net.addLink(s5, s1)

info('*** Starting network\n')
net.start()

info('*** Testing connectivity\n')
net.pingAll()
info('*** Running CLI\n')
CLI(net)

info('*** Stopping network')
net.stop()
