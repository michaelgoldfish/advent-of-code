using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class Day9
    {
        private static void RunIntcodeProgram(List<long> intcode)    // Intcode program from day 5, with added relative mode, opcode 9, increased memory and large number support
        {
            // Step count changes based on the opcode ran
            int stepCount = 4;    // Step count for opcodes within the list. After the first opcode operation is done, this many places are skipped to find the next opcode
            int relativeBase = 0;   // Relative base for when in relative mode; addresses count from the relative base rather than from zero

            for (int index = 0; index < intcode.Count; index += stepCount)
            {
                int indexValue = (int)intcode[index];

                if (indexValue == 99)   // Opcode for ending the program
                {
                    return;     // Program has finished
                }

                // Check the parameter modes and opcode within indexValue.
                // Mode 0 = Parameter mode (parameters are addresses), Mode 1 = Immediate mode (parameters are values), Mode 2 = Relative mode (parameters are address + relativeBase)

                int opcode = indexValue % 100;              // Stores the rightmost two digits
                int mode1 = (indexValue / 100) % 10;        // Stores the third digit from the right; the hundreds digit
                int mode2 = (indexValue / 1000) % 10;       // Stores the fourth digit from the right; the thousands digit
                int mode3 = (indexValue / 10000) % 10;      // Stores the fifth digit from the right; the ten-thousands digit

                long input1 = 0;                            // Value of the first input
                long input2 = 0;                            // Value of the second input (if applicable)
                int outputAddress = 0;                      // Value of the output address (if applicable)
                
                // Adjust inputs based on parameter mode (0), immediate mode (1) and relative mode (2)
                if (mode1 == 0)
                {
                    int inputAddress = (int)intcode[index + 1];
                    intcode = ExpandIntcodeMemory(intcode, inputAddress);

                    input1 = intcode[inputAddress];   // Value at the address of parameter 1
                }
                else if (mode1 == 1)
                {
                    input1 = intcode[index + 1];            // Value of parameter 1
                }
                else    // mode1 == 2
                {
                    int inputAddress = (int)intcode[index + 1];
                    intcode = ExpandIntcodeMemory(intcode, inputAddress);

                    input1 = intcode[inputAddress + relativeBase];
                }

                if (opcode == 1 || opcode == 2 || opcode == 5 | opcode == 6 || opcode == 7 || opcode == 8)  // Sets input 2 only for opcodes that use input2
                {
                    if (mode2 == 0)
                    {
                        input2 = intcode[(int)intcode[index + 2]];   // Value at the address of parameter 2
                    }
                    else if (mode2 == 1)
                    {
                        input2 = intcode[index + 2];            // Value of parameter 2
                    }
                    else    //(mode2 == 2)
                    {
                        input2 = intcode[(int)intcode[index + 1] + relativeBase];
                    }
                }

                if (opcode == 1)                // Opcode for addition
                {
                    outputAddress = (int)intcode[index + 3];
                    intcode = ExpandIntcodeMemory(intcode, outputAddress);

                    intcode[outputAddress] = input1 + input2;   // Set the value at the third parameter to the calculated value

                    stepCount = 4;
                }

                else if (opcode == 2)           // Opcode for multiplication
                {
                    outputAddress = (int)intcode[index + 3];
                    intcode = ExpandIntcodeMemory(intcode, outputAddress);

                    intcode[outputAddress] = input1 * input2;   // Set the value at the third parameter to the calculated value

                    stepCount = 4;
                }

                else if (opcode == 3)   // Opcode for inputting a value
                {
                    Console.WriteLine("Please enter the ID of the mode to run. For test mode type 1.");
                    string input = Console.ReadLine();

                    outputAddress = (int)intcode[index + 1];
                    intcode = ExpandIntcodeMemory(intcode, outputAddress);

                    intcode[outputAddress] = Convert.ToInt64(input);    // Set the value at the second parameter to the inputted value

                    stepCount = 2;
                }

                else if (opcode == 4)   // Opcode for outputting a value
                {
                    Console.WriteLine("Test result code: {0}", input1);

                    stepCount = 2;
                }

                else if (opcode == 5)           // Opcode for jump-if-true
                {
                    if (input1 != 0)
                    {
                        index = (int)input2;         // Sets the instruction pointer to the value of the second parameter
                        stepCount = 0;
                    }
                    else
                    {
                        stepCount = 3;          // Nothing happened, skip to next instruction
                    }
                }

                else if (opcode == 6)           // Opcode for jump-if-false
                {
                    if (input1 == 0)
                    {
                        index = (int)input2;         // Sets the instruction pointer to the value of the second parameter
                        stepCount = 0;
                    }
                    else
                    {
                        stepCount = 3;          // Nothing happened, skip to next instruction
                    }
                }

                else if (opcode == 7)           // Opcode for less than
                {
                    outputAddress = (int)intcode[index + 3];
                    intcode = ExpandIntcodeMemory(intcode, outputAddress);

                    if (input1 < input2)
                    {
                        intcode[outputAddress] = 1;    // Set the value at the third parameter to 1
                    }
                    else
                    {
                        intcode[outputAddress] = 0;    // Set the value at the third parameter to 1
                    }
                    stepCount = 4;
                }

                else if (opcode == 8)           // Opcode for equals
                {
                    outputAddress = (int)intcode[index + 3];
                    intcode = ExpandIntcodeMemory(intcode, outputAddress);

                    if (input1 == input2)
                    {
                        intcode[outputAddress] = 1;    // Set the value at the third parameter to 1
                    }
                    else
                    {
                        intcode[outputAddress] = 0;    // Set the value at the third parameter to 1
                    }
                    stepCount = 4;
                }

                else if (opcode == 9)
                {
                    relativeBase += (int)intcode[index + 1];

                    stepCount = 2;
                }

                else
                {
                    Console.WriteLine("Opcode {0} has been found at index {1} but does not have an operation designated to it. Known opcodes are 1-8 and 99.", indexValue, index);
                    return;     // End the function as something went wrong.
                }
            }

            return;
        }

        private static List<long> ExpandIntcodeMemory(List<long> intcode, int address)  // Expands the list up to the address given
        {
            int size = intcode.Count;
            for (int i = 0; i <= address - size; i++)  // Adds zeros to the list up until (and including) the address value
            {
                intcode.Add(0);
            }

            return intcode;
        }

        public static void OutputSolution()     //*********************************************USING TEST INPUT**************************************
        {
            string inputString = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day9Test.txt").ReadToEnd();    // Stores text file as a string
            string[] inputStringArray = inputString.Split(',');     // Splits the input into substrings separated by the ',' delimiter
            List<long> intcode = new List<long>();                   // Stores the intcode (puzzle input) as a list of integers

            // Take each element from inputStringArray and store it as an integer in the intcode list
            foreach (string element in inputStringArray)
            {
                intcode.Add(Convert.ToInt64(element));
            }

            RunIntcodeProgram(intcode);
        }
    }
}
