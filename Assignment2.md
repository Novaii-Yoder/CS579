# Summary
In short the malware I analyzed was a ransomware specifically KeyPass, it would encrypt your files and request money in return for the key te decrypt. This was discovered by finding strings in the binary. Using the hash I was able to find others who have also identified it as ransomware. 
# Type of Malware
* The malware targets Windows based machines
  1. CFF Explorer identifies it as a `Portable Executable 32`
  2. The use of DLL files in CFF Explorer, which is a Windows specific libraries

* The malware is some flavor of ransomware
  1. I found a ransom note asking for $300 dollars for the decryption key
  2. Bit coin logos can be found in CFF Explorer
  3. Running the executable in a closed system, we can see that our files are encrypted and replaced with the ransom note

* Potentially could also be dropper
  1. In the strings of the binary we see a connection to a website at `kronus.pp.ua` and `onus.pp.ua`, where the program is trying to load some page
  2. This could also just be tracking for the people who created the malware

# Hashes
MD5: `6999C944D1C98B2739D015448C99A291`
SHA-1: `D9BEB50B51C30C02326EA761B5F1AB158C73B12C`

Looking up these hashes on VirusTotal does indeed confirm my findings of this file being a Windows ransomware.

# Indicators of Compromise
The malware attempts to contact websites at `kronus.pp.ua` and `onus.pp.ua`.
The malware also will change files to have the `.KEYPASS` extention as described in the ransom note
The ransom note also has two emails listed, `keypassdecrypt@india.com` and `BM-2cUMY51WfNRG8jGrWcMzTASeUGX84yX741@bitmessage.ch`, any corespondance to these would also be signs of compromise

# Clues About Origin
We have a few conflicting clues as to where the malware originated from:
  1. I found a connection to `kronus.pp.ua` and `onus.pp.ua` which are Ukrainian based sites
  2. There is a few grammar mistakes which could imply any foreign origin outside of dominatly English speaking countries
  3. The email has `@india.com` which likely means very little because anyone can obtain the domain india.com
  4. I found some strings indicating languages including `english-south african`, `english-trinidad y tobago`, `portuguese-brazilian`, and `spanish-dominican republic` 3 of which point towards the caribbean

# YARA rule
```
rule detectKeyPass 
{
    strings:
        $a = "KEYPASS"
        $b = "kronus.pp.ua" nocase
        $c = "onus.pp.ua" nocase
        $d = "keypassdecrypt@india.com" nocase

    condition:
        $a and ($b or $c) and $d

}
```
This YARA rule is quite specific, so it wont catch all ransomware but it is likely to catch all versions of this specific ransomware. Since this rule is quite specific, it is unlikely to have false positives, becuase this email is known to be malicious and the string `KEYPASS` and the connections to `___.pp.ua` are not common or likely to be found with the other rules. The only downfall would be if the creators change their contact email for the decrypt keys. 
