# Assignment 7
In this assignment I reverse engineer a lab from a text book([Practical Malware Analysis](https://nostarch.com/malware) that is a likely DLL injection. The file is `Lab12-01.exe` and can be found [here](https://github.com/seanthegeek).
## Loader DLL Injection
After veiwing the program we can see a series of suspicious calls. The program first makes a call to `OpenProcess

![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/5d5f9f9d-7c8e-4fe2-bcc8-6d01b982c05c)
