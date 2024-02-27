# Assignment 4
Crackme Binary Reverse Engineering
In this report I go over the steps I took to crack and solve 6 crackmes, as well as my code for keygens. The crackmes can be found [here](https://github.com/tolvumadur/Reverse-Engineering-Class/blob/main/Spring24/Assignments/Assignment4.md).

# Crackme 1
This crackme was a simple single string compare that would output the right answer if the compare evaluated to zero. So, I followed the two inputs to the compare, one was just a simple input from the command line. The other was stored in the data section of the code as `picklecucumberl337`.
~~~
# keygen_easy_crackme_1.py
print("picklecucumberl337")
~~~

# Crackme 2
This crackme was very similar to the last, just a single string compare. After following the variables we find in the code the password `artificialtree`.
~~~
# keygen_easy_crackme_2.py
print("picklecucumberl337")
~~~

# Crackme 3
This crackme was a step up from the previous 2 and the password was now created at runtime and was not stored in one spot. We find that to get the message we want from the code we again need to match a string compare, but if we follow the variables we see that the input needs to match a string that has been concatenated a couple times. If we start at the beginning for the key, we see that it concats an empty string with `strawberry`, then that string is concatenated again with the string `kiwi`. Which leaves us with the key `strawberrykiwi`.
~~~
# keygen_easy_crackme_3.py
print("strawberrykiwi")
~~~

# Crackme 4 (ControlFlow1)
Crackme 4 is quite a big jump in difficulty so I started at the end and worked my way to the start. By looking at the function names(because whoever compiled the binary forgot to turn off the debug info) we can see a few functions, one of which is named `win`, which after a quick inspection does actually print the statement we want to see! So working backwards from here we need to find the function that calls win, and since all the funcitons are short this is easy to do, and leads us to the function `spock`. Spock contains a switch statement on the 15th index of a parameter (which is input, but we don't know this yet), and for us to reach the win function we need the 15th index to be `*`. Now we need to find the function that calls spock, which is `lizard`. Taking a look at lizard we see another switch statement, which we need index 1 of the parameter to be `6` in order to reach spock, thus reach the win function. 
Now we know that we must have the argument inputted into the function lizard to have '6' at index 1 and '*' at index 15, but what this argument is and what calls the lizard function we still don't know. Looking through the last few functions we can see that the function `scissors` calls it. This time there is no switch this time, but there is a fairly easy compare, it shows some char needs to be set to `A`, looking at the initialization of the char we see that it is the 
