# Assignment 3
# Solving the Crackme
I used Ghidra to solve the crackme, as it seemed to be the most intuitive to use, I'm sure IDA Pro has more options and capabilities, but Ghidra so far seems easier to use. I started by working backwards, taking a look at the print statement that prints the good answer, then trying to get all the up till that point coniditional statements to be true. So we see that there is a variable(var1) that has to evaluate to true. So, I followed the var1 to where it was assigned the output of a function, inside this function was a simple modulo on another variable, var2, and a comparison to 0, so we need var2 to be a mulitiple of the modulo for us to find a key that works. And if you follow var2 back to the function call, and even before that we see that it is simply the number inputted from the command line. So I just created a simple program that prints out 0 and pipes it into the crackme.

# Crackme in IDA Pro and Ghidra
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/03ab39dd-39df-415b-9edc-e8fc44f0e46c)
![image](https://github.com/Novaii-Yoder/CS579/assets/52936757/1712add8-57c2-4f23-a8cb-6e9bb2182f45)



# Assembly Review Questions
1. What is the difference between machine code and assembly?
Assembly is the lowest level language that isnt binary, it uses simple instructions like `mov`, `jmp`, and `add`. Whereas machine code is what assembly is compiled into, it is the actual binary of a compiled program.
2. If the ESP register is pointing to memory address 0x00000000001270A4 and I execute a pushq rax instruction, what address will rsp now be pointing to?
The RSP register wil
3. What is a stack frame?
A stack frame is the environment a function call lives in, it contains all the local variables aswell as the return addresses and stack pointers for the call above it.
4. What would you find in a data section?
The data section is where global and static variables are stored.
5. What is the heap used for?
The heap is used for dynamically allocated memory like variables. 
6. What is in the code section of a program's virtual memory space?
The code section is where the byte code of a program is stored.
7. What does the inc instruction do, and how many operands does it take?
It increase the value of a single register 
8. If I perform a div instruction, where would I find the remainder of the binary division (modulo)?
EDX is where the remainder of a div instruction gets put.
9. How does jz decide whether to jump or not?
The jz instruction checks the zero flag to decide if it should jump. If the zero flag is 0 it does jump, and if it is 0 the it doesn't jump.
10. How does jne decide whether to jump or not?
The jne instruction checks the zero flag to decide if it should jump. If the zero flag is 0 it doesn't jump, and if it is 1 it does jump.
11. What does a mov instruction do?
It moves the contents of one register into another.
12. What does the TF flag do, and why is it useful for debugging?
TF is th trap flag, and it is used to put the processor in single step mode, allowing us to use debuggers to step throught the execution.
13. Why would an attacker want to control the RIP register inside a program they want to take control of?
An attacker can execute any code that is loaded in the .data section of a program by controlling the RIP register, as the RIP register is the instruction pointer it dictates what instructions will be run next on the CPU.
14. What is the ax register and how does it relate to rax?
AX register is just the 16bit version of the RAX register(64 bit), when you store into the AX it will just replace the lower 16bit of the RAX register.  
15. What is the result of the instruction xor rax, rax and where is it stored?
XORing a register with itself just clears the register, it will evaluate to all zeros and save that into the register.
16. What does the leave instruction do in terms of registers to leave a stack frame?
The leave instruction loads the RBP register into RSP and pops the stack into RBP, this moves the stack frame back to the previous stack frame.
17. What pop instruction is retn equivalent to?
pop %RIP
pop %RSP
18. What is a stack overflow?
When the stack out grows whatever limit the operating system puts on it. 
19. What is a segmentation fault (a.k.a. a segfault)?
Segfaults occur when a program or function attempts to access a piece of memory outside of its frame or is not allowed to access.
20. What are the RSI and RDI registers for that gives them their name?
RSI - Register Source Index
RDI - Register Destination Index
These registers are used by the system as the source and destination when copying or moving bytes.
