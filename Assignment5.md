# Assignment 5
## Ransomeware1
### Ghidra
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/29b3059a-23b2-4e34-9b2a-d76aee18c111)
### Decryptor
~~~python3
def decryptfile(inputfile, outputfile):
  with open(inputfile, 'rb') as f:
    encrypted_data = f.read()

  decrypted_data = bytes([byte ^ 0x34 for byte in decrypted_data])

  print(decrypted_data.decode('utf-8'))

  with open(outputfile, 'wb') as f:
    f.write(decrypted_data)

inputf = "secret.txt.pay_up"
outputf = "secret.txt"

decryptfile(inputf, outputf)  
~~~
### Secret File Contents
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/4c343a6e-53d8-4685-bdb6-1a6073d1605a)

