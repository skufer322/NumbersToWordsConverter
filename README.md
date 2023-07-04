# NumbersToWordsConverter
C# program which converts a currency (dollars) from numbers into words.

The main entry point of the programm is the **Main()** method of the class **InputLoop.cs**.

When running the program, a command prompt is opened, repeatedly asking the user for numbers to be converted into words (until the words "*exit*" or "*quit*" are entered as inputs). In case of an invalid input or an unexpected error, the program terminates with an appropriate error message and asks you to restart the program in case you want to try any further inputs.

*Examples*:

|**Input**|**Output**|
|--------------|---------------------------------------------------------------------------------------------------------------------------|
|0| zero dollars|
|1| one dollar|
|25,1| twenty-five dollars and ten cents|
|0,01| zero dollars and one cent|
|45 100| forty-five thousand one hundred dollars|
|999 999 999,99| nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents|
