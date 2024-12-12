<a href="https://github.com/belowaverage-org/ImmediateAccess/releases">
  <img src="https://raw.githubusercontent.com/belowaverage-org/ImmediateAccess/master/ImmediateAccessAssets/banner.svg">
</a>

# What is Immediate Access?
Immediate Access is a replacement for Microsoft’s Direct Access “Always On VPN” technology. This service behaves in the same manner as Microsoft’s Direct Access, but instead of connecting via IPHTTPS or Teredo, the service will start a VPN connection of your choice. (Must be a Windows VPN profile: IPSec, SSTP, L2TP, PPTP…)

This service — along with starting a VPN connection — will automatically close the VPN connection if the computer is on the corporate network.


# How does Immediate Access work?
In a nutshell, this service will check if the current computer is on the corporate network by attempting to reach an internal only “probe”. The probe is a web server that responds via HTTPS with any content (a blank page is best). The HTTPS site needs to present a trusted certificate to the client, or the Immediate Access service will not except the probe attempt. If the probe attempt is successfull, the Immediate Access service will sleep until the next “network change event” (aka: an IP address changes) or until the Health Check Interval lapses.

After an event or the Health Check Interval lapses, the Immediate Access service will re-check for a probe connection.

If the Immediate Access service cannot reach the probe, the service will connect to the GPO specified VPN profiles. If Immediate Access can once again reach the probe, the service will disconnect the GPO specified VPN profiles.
