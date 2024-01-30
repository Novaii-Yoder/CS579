# CS579 Reverse Engineering at NMSU
Contains reports on reverse engineering malware samples from "Practical Malware Analysis". 

### System Setup
As is standard when dealing with potentially malicious software system and network isolation are a good idea. I setup my environment using these practices to reduce the likelyhood of the malware getting out of a VM onto my personal computer, and even more so reducing the likelihood of it getting out via the network. 

My current environment is a Windows 11 virtual machine on a Ubuntu hypervisor, because we are working exclusively on windows based malware, we run the VM on a *NIX based system so that if somehow the malware were to get out, it would be less likely to cause problems. I also removed the VM's ability to access the internet by removing the ethernet device in the VM host, to create network isolation. 

I also disabled Windows defender becuase of the chance that it would either delete the malware I was working on and/or send a copy to Microsoft. 

#### Tools installed
VSCode
  Used to open either binaries directly or analyze the code after decompilation.
IDA Pro
  Used to decompile binaries
FlareVM
  Used to setup and run a reverse engineering virtual machine
