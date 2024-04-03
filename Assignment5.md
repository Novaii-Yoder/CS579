# Assignment 5
## Ransomeware 1
### Ghidra
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/29b3059a-23b2-4e34-9b2a-d76aee18c111)
### Decryptor
~~~python3
def decryptfile(inputfile, outputfile):
  with open(inputfile, 'rb') as f:
    encrypted_data = f.read()

  decrypted_data = bytes([byte ^ 0x34 for byte in encrypted_data])

  print(decrypted_data.decode('utf-8'))

  with open(outputfile, 'wb') as f:
    f.write(decrypted_data)

inputf = "secret.txt.pay_up"
outputf = "secret.txt"

decryptfile(inputf, outputf)  
~~~
### Secret File Contents
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/4c343a6e-53d8-4685-bdb6-1a6073d1605a)

# Ransomware 2
### Ghidra
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/d3a55b6c-7868-444c-a2e6-31fd3e040e91)
### Decryptor
~~~python3
def decryptfile(inputfile, outputfile):
    with open(inputfile, 'rb') as f:
        encrypted_data = f.read()
    leet = [0x31, 0x33, 0x33, 0x37]
    decrypted_data = bytes([byte ^ leet[i % 4] for i, byte in enumerate(encrypted_data)])

    print(decrypted_data.decode('utf-8'))

    with open(outputfile, 'wb') as f:
        f.write(decrypted_data)

inputf = "secret.txt.pay_up"
outputf = "secret.txt"

decryptfile(inputf, outputf)

~~~
### Secret File Contents
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/fb000002-dcbb-43fb-879d-f5966e8cda7d)
