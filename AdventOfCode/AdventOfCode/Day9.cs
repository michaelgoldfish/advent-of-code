using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /*  --- Day 9: Sensor Boost ---
       
        You've just said goodbye to the rebooted rover and left Mars when you receive a faint distress signal coming from the asteroid belt.
        It must be the Ceres monitoring station!

        In order to lock on to the signal, you'll need to boost your sensors.
        The Elves send up the latest BOOST program - Basic Operation Of System Test.

        While BOOST (your puzzle input) is capable of boosting your sensors, for tenuous safety reasons,
        it refuses to do so until the computer it runs on passes some checks to demonstrate it is a complete Intcode computer.

        Your existing Intcode computer is missing one key feature: it needs support for parameters in relative mode.

        Parameters in mode 2, relative mode, behave very similarly to parameters in position mode: the parameter is interpreted as a position.
        Like position mode, parameters in relative mode can be read from or written to.

        The important difference is that relative mode parameters don't count from address 0.
        Instead, they count from a value called the relative base. The relative base starts at 0.

        The address a relative mode parameter refers to is itself plus the current relative base.
        When the relative base is 0, relative mode parameters and position mode parameters with the same value refer to the same address.

        For example, given a relative base of 50, a relative mode parameter of -7 refers to memory address 50 + -7 = 43.

        The relative base is modified with the relative base offset instruction:

        Opcode 9 adjusts the relative base by the value of its only parameter. The relative base increases (or decreases, if the value is negative) by the value of the parameter.
        For example, if the relative base is 2000, then after the instruction 109,19, the relative base would be 2019.
        If the next instruction were 204,-34, then the value at address 1985 would be output.

        Your Intcode computer will also need a few other capabilities:

        -The computer's available memory should be much larger than the initial program.
         Memory beyond the initial program starts with the value 0 and can be read or written like any other memory.
         (It is invalid to try to access memory at a negative address, though.)
        -The computer should have support for large numbers. Some instructions near the beginning of the BOOST program will verify this capability.

        Here are some example programs that use these features:

        109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99 takes no input and produces a copy of itself as output.
        1102,34915192,34915192,7,4,7,99,0 should output a 16-digit number.
        104,1125899906842624,99 should output the large number in the middle.

        The BOOST program will ask for a single input; run it in test mode by providing it the value 1.
        It will perform a series of checks on each opcode, output any opcodes (and the associated parameter modes) that seem to be functioning incorrectly,
        and finally output a BOOST keycode.

        Once your Intcode computer is fully functional, the BOOST program should report no malfunctioning opcodes when run in test mode;
        it should only output a single value, the BOOST keycode. What BOOST keycode does it produce?
        Answer: 3280416268
        */

    /*  --- Part Two ---
     
        You now have a complete Intcode computer.

        Finally, you can lock on to the Ceres distress signal! You just need to boost your sensors using the BOOST program.

        The program runs in sensor boost mode by providing the input instruction the value 2. Once run, it will boost the sensors automatically, but it might take a few seconds to complete the operation on slower hardware. In sensor boost mode, the program will output a single value: the coordinates of the distress signal.

        Run the BOOST program in sensor boost mode. What are the coordinates of the distress signal?
        Answer: 
    */
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

                // Adjust input1 based on parameter mode (0), immediate mode (1) and relative mode (2)
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
                else    // (mode1 == 2)
                {
                    int inputAddress = (int)intcode[index + 1] + relativeBase;
                    intcode = ExpandIntcodeMemory(intcode, inputAddress);

                    input1 = intcode[inputAddress];
                }

                if (opcode == 1 || opcode == 2 || opcode == 5 | opcode == 6 || opcode == 7 || opcode == 8)  // Sets input 2 only for opcodes that use input2
                {
                    if (mode2 == 0)
                    {
                        int inputAddress = (int)intcode[index + 2];
                        intcode = ExpandIntcodeMemory(intcode, inputAddress);

                        input2 = intcode[inputAddress];   // Value at the address of parameter 2
                    }
                    else if (mode2 == 1)
                    {
                        input2 = intcode[index + 2];            // Value of parameter 2
                    }
                    else    //(mode2 == 2)
                    {
                        int inputAddress = (int)intcode[index + 2] + relativeBase;
                        intcode = ExpandIntcodeMemory(intcode, inputAddress);

                        input2 = intcode[inputAddress];
                    }
                }

                // Set output address based on mode
                if (opcode == 1 || opcode == 2 || opcode == 7 || opcode == 8)    // 3 is intcode[index + 1] so it's done locally, 4-6 have no outputAddress
                {
                    if (mode3 == 0 || mode3 == 1)
                    {
                        outputAddress = (int)intcode[index + 3];
                    }
                    else    // (mode3 == 2)
                    {
                        outputAddress = (int)intcode[index + 3] + relativeBase;
                    }
                    intcode = ExpandIntcodeMemory(intcode, outputAddress);
                }

                if (opcode == 1)                // Opcode for addition
                {
                    intcode[outputAddress] = input1 + input2;   // Set the value at the third parameter to the calculated value

                    stepCount = 4;
                }

                else if (opcode == 2)           // Opcode for multiplication
                {

                    intcode[outputAddress] = input1 * input2;   // Set the value at the third parameter to the calculated value

                    stepCount = 4;
                }

                else if (opcode == 3)   // Opcode for inputting a value
                {
                    Console.WriteLine("Please enter an ID. For test mode type 1.");
                    string input = Console.ReadLine();

                    if (mode1 == 0 || mode1 == 1)
                    {
                        outputAddress = (int)intcode[index + 1];
                    }
                    else    // (mode1 == 2)
                    {
                        outputAddress = (int)intcode[index + 1] + relativeBase;
                    }

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
                    relativeBase += (int)input1;

                    stepCount = 2;
                }

                else
                {
                    Console.WriteLine("Opcode {0} has been found at index {1} but does not have an operation designated to it. Known opcodes are 1-9 and 99.", indexValue, index);
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

        public static void OutputSolution()
        {
            string inputString = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day9Input.txt").ReadToEnd();    // Stores text file as a string
            string[] inputStringArray = inputString.Split(',');     // Splits the input into substrings separated by the ',' delimiter
            List<long> intcode = new List<long>();                  // Stores the intcode (puzzle input) as a list of integers

            // Take each element from inputStringArray and store it as an integer in the intcode list
            foreach (string element in inputStringArray)
            {
                intcode.Add(Convert.ToInt64(element));
            }

            RunIntcodeProgram(intcode);
            // intcode to test 209: 109,1,203,11,209,8,204,1,99,10,0,42,0
            // Returns the input given if correct. Found on the advent of code subreddit, this was used to diagnose and fix the lack of relative mode in opcode 9
        }
    }
}
