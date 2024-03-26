# Assignment 6



## Crackme 1 Solution ([link/to/download/location](https://crackmes.dreamhosters.com/users/seveb/crackme05/download/crackme05.tar.gz)):

To solve this crackme I had to create a keygen that creates a usable username and serial number, which is reliant on the username.

My keygen:
~~~python
import string
import random

length = random.randint(8,12)
chars = string.ascii_letters + string.digits
arr = [random.choice(chars) for _ in range(length)]
pas = []

for i in range(length):
    temp = 0
    if (i%2 == 0):
        temp = int(ord(str.lower(arr[i])))
    else:
        temp = int(ord(str.upper(arr[i])))
    pas.append(str(temp))

serial = ""
for x in pas:
    serial += str(x)
serial = serial[(len(arr)-8)*2:(len(arr)-8)*2 +8]
username = ""
for x in arr:
    username += str(x)

print(username)
print(serial)
~~~

## Crackme 2 Solution ([link/to/download/location](https://crackmes.dreamhosters.com/users/adamziaja/crackme1/download/crackme1.tar.gz)):

To solve this crackme I had to create a keygen that produces a serial number that gets past the crackme.
My solution is:

```python3
import string
import random

def getchar(ch):
    tmp = ch
    while tmp == ch:
        tmp = random.choice([chr(i) for i in range(86, 90)] + [chr(i) for i in range(97, 122)])
    return tmp

arr = [0] * 19
arr[4] = arr[9] = arr[14] = '-'
arr[10] = arr[8] = random.choice(string.ascii_letters + string.digits)
arr[13] = arr[5] = random.choice(string.ascii_letters + string.digits)
arr[1] = arr[2] = getchar('*') // excludes given char, which is an illegal char anyways
arr[16] = arr[17] = getchar('*')

serial = ""
for x in arr:
    serial += str(x)

print(serial)
```


### How I did it using Ghidra:

1. I opened the crackme in Ghidra
2. There was a very readable and helpful usage block as well as a --help flag for how to run the program.
3. I then found that the program had 4 functions that checked the serial number entered. We want to get through all of them without the `bomb` function being called. 
4. The first function called `rock` loops through every index of the serial number and does a number of checks on them.
    In the end, the `rock` function just makes sure every index in the serial number is a number, letter, or '-'.
5. The second function `paper` has a few XORs and requires a few indexes to be specific values: 
    index[10] XOR index[8] <= 9
    index[13] XOR index[5] <= 9
    index[3] and index[15] == index[10] XOR index[8]
    index[0] and index[18] == index[13] XOR index[5] 
    There is a ton of combinations and possibilities, but it will be simpler to test and build a keygen that has the pairs of index be the same character so that the indexes that have to be equal to the XOR can just be zeros aswell.
6. The third function `scissors` again has just a few more conditions:
    index[2] + index[1] >= 171 and index[17] + index[16] >= 171
    index[2] + index[1] != index[17] + index[16]
7. The final condition in the `cracker` function is:
    index[14] + index[4] + index[9] == 135
    which is only possible when all three indexs are 45 which is '-'
8. To test my rules I tried the serial `0yY0-B00A-A00B-0jY0`, and everything came up cherry.

## Crackme 5 Solution ([link/to/download/location](http://crackmes.cf/users/seveb/crackme04/download/crackme04.tar.gz)):

To solve this crackme I had to create a keygen that produces a serial number that gets past the crackme.
My solution is:
patch the binary


### How I did it using Ghidra:
1. After opening the binary I saw that there was 2 end function:
    `theEnd` function which is the one where we complete the crackme and the `theOtherEnd` which is where we lose and also where the crackme deletes itself. This is an issue for testing my keygen, so I went into the binary with Ghidra and replaced the function call to delete the file with a bunch of NOPs.
        Side note: since the win function was right below this function and there is no return it actually executes the `theEnd` at the end of the `theOtherEnd` now. Which would work in reality as is to break the crackme.
2. I also noticed that the program finds local time and pid of the process and if they are equal to specific values it deletes itself. But this is another thing we can patch out by changing the JZ commands with JNZ, which means it will only delete itself if you run it at the specified time and it by chance has the right pid.



