# Assignment 5
## Ransomeware 1
### Ghidra
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/29b3059a-23b2-4e34-9b2a-d76aee18c111)

### Decryptor
This decryptor just XORs every byte from the encrypted file with the hex equivelant of the char `4`. The contents are then printed on screen aswell as saved to a textfile.
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

## Ransomware 2
### Ghidra
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/d3a55b6c-7868-444c-a2e6-31fd3e040e91)

### Decryptor
This malware was a little more tricky with its encryption algorithm, it used a rotating cypher of sorts. I found a pretty nice way of just using modulo on the index of an array containing the different possible hex codes used, in this case it was `leet`.
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

## Ransomware 3
### Ghidra
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/123a8306-8a84-4387-bc6d-5ac5c99167ff)

### Decryptor
This malware had a similar encryption algorithm to ransomware 2 but rotated on on a secret key of `R3V3R53`, which was found in the program. They did however change the key at runtime, but it was just by subtracting 1 from the stored key.
~~~python3
def decryptfile(inputfile, outputfile):
    with open(inputfile, 'rb') as f:
        encrypted_data = f.read()
    code = [0x52, 0x33, 0x56, 0x33, 0x52, 0x35, 0x33]
    decrypted_data = bytes([byte ^ code[i % 7] for i, byte in enumerate(encrypted_data)])

    print(decrypted_data.decode('utf-8'))

    with open(outputfile, 'wb') as f:
        f.write(decrypted_data)

inputf = "secret.txt.pay_up"
outputf = "secret.txt"

decryptfile(inputf, outputf)
~~~
### Secret File Contents
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/fede7a52-ab39-4a5a-8236-70fef1733861)
