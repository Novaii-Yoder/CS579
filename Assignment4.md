# Assignment 4
Crackme Binary Reverse Engineering
In this report I go over the steps I took to crack and solve 6 crackmes, as well as my code for keygens. The crackmes can be found [here](https://github.com/tolvumadur/Reverse-Engineering-Class/blob/main/Spring24/Assignments/Assignment4.md).

# Crackme 1
This crackme was a simple single string compare that would output the right answer if the compare evaluated to zero. So, I followed the two inputs to the compare, one was just a simple input from the command line. The other was stored in the data section of the code as `picklecucumberl337`.
~~~python
# keygen_easy_crackme_1.py
print("picklecucumberl337")
~~~

# Crackme 2
This crackme was very similar to the last, just a single string compare. After following the variables we find in the code the password `artificialtree`.
~~~python
# keygen_easy_crackme_2.py
print("picklecucumberl337")
~~~

# Crackme 3
This crackme was a step up from the previous 2 and the password was now created at runtime and was not stored in one spot. We find that to get the message we want from the code we again need to match a string compare, but if we follow the variables we see that the input needs to match a string that has been concatenated a couple times. If we start at the beginning for the key, we see that it concats an empty string with `strawberry`, then that string is concatenated again with the string `kiwi`. Which leaves us with the key `strawberrykiwi`.
~~~python
# keygen_easy_crackme_3.py
print("strawberrykiwi")
~~~

# Crackme 4 (ControlFlow1)
Crackme 4 is quite a big jump in difficulty so I started at the end and worked my way to the start. By looking at the function names(because whoever compiled the binary forgot to turn off the debug info) we can see a few functions, one of which is named `win`, which after a quick inspection does actually print the statement we want to see! So working backwards from here we need to find the function that calls win, and since all the funcitons are short this is easy to do, and leads us to the function `spock`. Spock contains a switch statement on the 15th index of a parameter (which is input, but we don't know this yet), and for us to reach the win function we need the 15th index to be `*`. Now we need to find the function that calls spock, which is `lizard`. Taking a look at lizard we see another switch statement, which we need index 1 of the parameter to be `6` in order to reach spock, thus reach the win function. 

Now we know that we must have the argument inputted into the function lizard to have `6` at index 1 and `*` at index 15, but what this argument is and what calls the lizard function we still don't know. Looking through the last few functions we can see that the function `scissors` calls it. This time there is no switch this time, but there is a fairly easy compare, it shows some char needs to be set to `A`, looking at the initialization of the char we see that it is the first character of the argument inputted into the function. If we keep following the function calls we get led to `paper` which wants the and of some value and 1 to be 0 to call the scissors function. This value is the left shift of 1 by `(arg1[7] - '%') & 0b00111111` which seems complicated, but since we want this value to equate to 1 to call the scissors function we just want this to evaluate to 0. If this equation evaluates to 0 then the left shift wont do anything and will leave 1 as the final value. If we want the and of two numbers to be 0, the easiest way is to have one of the numbers to be zero, which we can do by having the 7th index of the argument to be `%`. 

We now want to find the function that calls paper and passes the argument where index 0 is `A`, index 1 is `6`, index 7 is `%`, and index 15 is `*`. We find that the function `rock` calls paper and that to get it to be called we need the 3rd index of the argument to be `2`. Finally, if we look through the main function we can see that it is the one that calls the rock function. And to get it to call that function we need our argument to have a length of 16 or more. Looking at the initialization of the argument we can also see that it is the first argument after calling the function on the command line. 

Finally, we have a key: 
Any 16+ character string where index 0 is `A`, index 1 is `6`, index 3 is `2`, index 7 is `%`, and index 15 is `*`. i.e. `A6XXXXX%XXXXXXX*` where X is any character.

~~~python
#keygen_controlflow1.py
import string
import random

def gen_ran_string(length):
  return ''.join(random.choice(string.ascii_letters + string.digits) for _ in range(length))

print(f"A6{gen_ran_string(1)}2{gen_ran_string(3)}%{gen_ran_string(7)}*{gen_ran_string(random.randint(0, 16))}")
~~~

# Crackme 5 (ControlFlow2)
I followed the same idea as controlflow1 but to save space, I will just put the restrictions on the password that I find.
- Must have 16 or more characters
- The 6th index must be `Y`
- The 8th index must be `#`
- The 10th index must be `A`
- the 13th index must be `6`
- the 11th index must be `*`
  
~~~python
#keygen_controlflow2.py
import string
import random

def gen_ran_string(length):
  return ''.join(random.choice(string.ascii_letters + string.digits) for _ in range(length))

print(f"{gen_ran_string(6)}Y{gen_ran_string(1)}#{gen_ran_string(1)}A*{gen_ran_string(1)}6{gen_ran_string(random.randint(2,16))}")
~~~

# Crackme 6 (ControlFlow3)
This crackme was a little more complex as far as the keygen, but finding the rules were just as easy as the other crackmes. I followed this control flow from the start of the program just looking for the next function call that wasn't recursive or back tracing. 
- Must have exactly 16 characters
- index[1] + index[3] - index[5] == index[6]
- index[6] ^ index[7] < 3
- index[10] == index[12]
- index[8] ^ index[7] !< 4
- index[8] != index[9]
- index[12] ^ index[8] ^ index[9] != index[10] < 3

~~~python
#keygen_controlflow3.py
import string
import random

def gen_ran_string(length):
  return ''.join(random.choice(string.ascii_letters + string.digits) for _ in range(length))
key = [gen_ran_string(1) for _ in range(16)]
while(1):
  key = [gen_ran_string(1) for _ in range(16)]
  key_asc = [ord(char) for char in key]
  if key_asc[1] + key_asc[3] - key_asc[5] != key_asc[6]:
    continue
  if key_asc[6] ^ key_asc[7] >= 3:
    continue
  if key_asc[10] != key_asc[12]:
    continue
  if key_asc[8] ^ key_asc[7] >= 3:
    continue
  if key_asc[8] == key_asc[9]:
    continue
  if key_asc[12] ^ key_asc[8] ^ key_asc[9] == key_asc[10] < 3:
    continue
  break

print(''.join(key))
~~~
