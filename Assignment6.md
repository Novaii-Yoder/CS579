# Assignment 6



### Crackme 1 Solution (link/to/download/location):

To solve this crackme I had to create a



### Crackme 2 Solution (link/to/download/location):

To solve this crackme I had to create a keygen that produces a serial number that gets past the crackme.
My solution is:

```
#!/usr/bin/env python3

print("This is the answer!")
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



