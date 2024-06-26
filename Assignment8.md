# Assignment 8
### NjRAT
NjRAT is a remote access trojan used to control infected machines. It has many versions and names, NjRat is a variant of jRAT which is also called Bladabindi. The malware has access to almost everything if it is run, it can controll cameras, microphones, kill processes, manipulate files, and manipulate the registry. It also can copy itself into different parts of the computer to make sure it is always ran. [(source)](https://www.cynet.com/attack-techniques-hands-on/njrat-report-bladabindi/)
NjRAT first appeared in 2012 and is still around today, in 2013 the source code was leaked and is why it became so popular among cybercriminals[Ahmed Nosir](https://medium.com/@egycondor/njrat-technical-insights-and-strategic-hunting-approaches-b0ae4c8a4f74). It seems NjRAT is common by actors in the middle-east [cynet](https://www.cynet.com/attack-techniques-hands-on/njrat-report-bladabindi/)[mcafee](https://www.mcafee.com/blogs/other-blogs/mcafee-labs/trail-njrat/), and was created by a hacker group called Sparclyheason. The malware was at its peak in 2014 where many copies of this malware were targeting machines in the Middle East, McAfee has a cool graph and map to take a look at[mcafee](https://www.mcafee.com/blogs/other-blogs/mcafee-labs/trail-njrat/). 

### RegShot
I took a RegShot before and a few minutes after running NjRAT, I also included directory changes. After comparing the changes between the two snapshots I was left with an absorbently long file of changes, deletions, and additions. Sifting through them and finding what NjRAT had changed and what Windows or other programs had changed was quite difficult. I did come across the binary BAM files for NjRAT which is how I know that NjRAT was actually run, but figureing out what NjRAT changed specifically is not very easy to do. I know that NjRAT creates a connection to an outside port based on FakeNet results, and according to the research I've done I should be expecting NjRAT to clone itself and pontentially gain access to everything.

After sifting through the RegShot I came across these changes that seem to be affecting the firewall rules:
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/8ab7c017-4f3a-414d-8333-d7e8bb4b878b)

It also seems that it is specifically changing the rules for a file called `windows.exe` and looking for more changes related to this file we find:
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/61000c67-183a-4504-a0c4-bd197a3516b7)

A prefetch file is a file windows creates on the first time you run a program to try and speed up future startups. This seems like an indicator that NjRAT created and ran these files because there was no other programs other than RegShot, and FakeNet running at the time. We can see this suspicious `windows.exe` file and a `NJQ8.exe` file which is created by NjRAT:
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/85502917-6cec-4e25-8583-5290cefd2e03)

We can see the malware creating a file in the `mui cache` which is the multilingual user inferface cache, but I have a feeling thats not why NjRAT is using it.

#### Idicators of Compromise
The easiest way to see if a system has been compromised is by seeing if the file `windows.exe` exists in the temp folder. Another option is to look for the `NJQ8.exe` file, but I wasn't able to find as much on this file, but even after the NjRAT has been terminated the file still persists so it would be a good indicator of compromise.

### FakeNet
FakeNet was used to provide a fake internet environment for NjRAT to run and view the connections the malware tried to make. There was only one connection it tried to make and it was to `zaaptoo.zapto.org`, I was unable to find a DNS lookup for the `zaaptoo` sub domain but was able to find and ping `zapto.org` which belongs to the IP `158.247.7.206`. After doing some searching on zapto.org I discovered that it is a dynamic DNS, and is also likely where the attacker resides. It seems that the domain `zapto.org` is a normal looking domain, it hosts a number of sub-domains a number of which are blogs, or servers for projects and git repos[urlscan](https://urlscan.io/domain/zapto.org). But seeing as all of these blogs and sites are small and likely not going to be accessed by any non-malicious software, I think using `zapto.org` as an indicator of compromise would not be a bad thing. If a machine in a network is trying to make contact with a machine at the DDNS of `zapto.org` they should be flag as high chance of being compromised.
