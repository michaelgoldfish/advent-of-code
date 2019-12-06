using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /*  --- Day 5: Sunny with a Chance of Asteroids ---
       
        You're starting to sweat as the ship makes its way toward Mercury.
        The Elves suggest that you get the air conditioner working by upgrading your ship computer to support the Thermal Environment Supervision Terminal.

        The Thermal Environment Supervision Terminal (TEST) starts by running a diagnostic program (your puzzle input).
        The TEST diagnostic program will run on your existing Intcode computer after a few modifications:

        First, you'll need to add two new instructions:

        -Opcode 3 takes a single integer as input and saves it to the position given by its only parameter. For example, the instruction 3,50 would take an input value and store it at address 50.
        -Opcode 4 outputs the value of its only parameter. For example, the instruction 4,50 would output the value at address 50.

        Programs that use these instructions will come with documentation that explains what should be connected to the input and output.
        The program 3,0,4,0,99 outputs whatever it gets as input, then halts.

        Second, you'll need to add support for parameter modes:

        Each parameter of an instruction is handled based on its parameter mode.
        Right now, your ship computer already understands parameter mode 0, position mode, which causes the parameter to be interpreted as a position
        - if the parameter is 50, its value is the value stored at address 50 in memory. Until now, all parameters have been in position mode.

        Now, your ship computer will also need to handle parameters in mode 1, immediate mode.
        In immediate mode, a parameter is interpreted as a value - if the parameter is 50, its value is simply 50.

        Parameter modes are stored in the same value as the instruction's opcode.
        The opcode is a two-digit number based only on the ones and tens digit of the value, that is, the opcode is the rightmost two digits of the first value in an instruction.
        Parameter modes are single digits, one per parameter, read right-to-left from the opcode:

        the first parameter's mode is in the hundreds digit,
        the second parameter's mode is in the thousands digit,
        the third parameter's mode is in the ten-thousands digit, and so on.
        Any missing modes are 0.

        For example, consider the program 1002,4,3,4,33.

        The first instruction, 1002,4,3,4, is a multiply instruction - the rightmost two digits of the first value, 02, indicate opcode 2, multiplication.
        Then, going right to left, the parameter modes are 0 (hundreds digit), 1 (thousands digit), and 0 (ten-thousands digit, not present and therefore zero):

        ABCDE
         1002

        DE - two-digit opcode,      02 == opcode 2
         C - mode of 1st parameter,  0 == position mode
         B - mode of 2nd parameter,  1 == immediate mode
         A - mode of 3rd parameter,  0 == position mode,
                                          omitted due to being a leading zero

        This instruction multiplies its first two parameters. The first parameter, 4 in position mode, works like it did before
        - its value is the value stored at address 4 (33). The second parameter, 3 in immediate mode, simply has value 3.
        The result of this operation, 33 * 3 = 99, is written according to the third parameter, 4 in position mode, which also works like it did before - 99 is written to address 4.

        Parameters that an instruction writes to will never be in immediate mode.

        Finally, some notes:

        -It is important to remember that the instruction pointer should increase by the number of values in the instruction after the instruction finishes.
         Because of the new instructions, this amount is no longer always 4.
        -Integers can be negative: 1101,100,-1,4,0 is a valid program (find 100 + -1, store the result in position 4).
        -The TEST diagnostic program will start by requesting from the user the ID of the system to test by running an input instruction - provide it 1, the ID for the ship's air conditioner unit.

        It will then perform a series of diagnostic tests confirming that various parts of the Intcode computer, like parameter modes, function correctly.
        For each test, it will run an output instruction indicating how far the result of the test was from the expected value, where 0 means the test was successful.
        Non-zero outputs mean that a function is not working correctly; check the instructions that were run before the output instruction to see which one failed.

        Finally, the program will output a diagnostic code and immediately halt.
        This final output isn't an error; an output followed immediately by a halt means the program finished.
        If all outputs were zero except the diagnostic code, the diagnostic program ran successfully.

        After providing 1 to the only input instruction and passing all the tests, what diagnostic code does the program produce?
        Answer: 14522484
    */

    class Day5
    {

        private static List<int> RunIntcodeProgram(List<int> intcode)    // Takes a list of integers and runs the intcode, returning the list with updated integer values
        {
            // Step count changes based on the opcode ran
            int stepCount = 4;    // Step count for opcodes within the list. After the first opcode operation is done, this many places are skipped to find the next opcode

            for (int index = 0; index < intcode.Count; index += stepCount)
            {
                int indexValue = (int)intcode[index];

                if (indexValue == 99)   // Opcode for ending the program
                {
                    return intcode;     // Program has finished
                }

                // Check the parameter modes and opcode within indexValue. Mode 0 = Parameter mode (parameters are addresses), Mode 1 = Immediate mode (parameters are values)

                int opcode = indexValue % 100;              // Stores the rightmost two digits
                int mode1 = (indexValue / 100) % 10;        // Stores the third digit from the right; the hundreds digit
                int mode2 = (indexValue / 1000) % 10;       // Stores the fourth digit from the right; the thousands digit
                int mode3 = (indexValue / 10000) % 10;      // Stores the fifth digit from the right; the ten-thousands digit

                int input1;                             // Value of the first input
                int input2;                             // Value of the second input (if applicable)
                int outputValue = 0;                    // Value of the output address

                if (opcode == 1 || opcode == 2)        // Opcodes for addition & multiplication. Both have three parameters after the opcode
                {
                    if (mode1 == 0)
                    {
                        input1 = intcode[intcode[index + 1]];   // Value at the address of parameter 1
                    }
                    else    // mode1 == 1
                    {
                        input1 = intcode[index + 1];            // Value of parameter 1
                    }

                    if (mode2 == 0)
                    {
                        input2 = intcode[intcode[index + 2]];   // Value at the address of parameter 2
                    }
                    else    // mode2 == 1
                    {
                        input2 = intcode[index + 2];            // Value of parameter 2
                    }

                    //if (mode3 == 0)     // mode3 should always be 0 according to the problem. "Parameters that an instruction writes to will never be in immediate mode."
                    //{
                    if (opcode == 1)                // Opcode for addition
                    {
                        outputValue = input1 + input2;
                    }
                    else    // indexValue == 2          // Opcode for multiplication
                    {
                        outputValue = input1 * input2;
                    }
                    //}

                    intcode[intcode[index + 3]] = outputValue;   // Set the value at the third parameter to the calculated value
                    stepCount = 4; // Increase the stepcount by 4 to move past the three parameters to the next opcode
                }
                else if (opcode == 3)   // Opcode for inputting a value
                {
                    Console.WriteLine("Please enter the ID of the system to test (it's 1):");
                    string input = Console.ReadLine();

                    intcode[intcode[index + 1]] = Convert.ToInt32(input);    // Set the value at the second parameter to the inputted value

                    stepCount = 2;
                }
                else if (opcode == 4)   // Opcode for outputting a value
                {
                    int output;

                    if (mode1 == 0)
                    {
                        output = intcode[intcode[index + 1]];
                    }
                    else    // mode1 == 1
                    {
                        output = intcode[index + 1];
                    }
                    Console.WriteLine("Test result code: {0}", output);

                    stepCount = 2;
                }
                else
                {
                    Console.WriteLine("Opcode {0} has been found at index {1} but does not have an operation designated to it. Known opcodes are 1, 2, 3, 4, and 99.", indexValue, index);
                    return intcode; // End the function as something went wrong.
                }
            }

            return intcode;
        }

        public static void OutputSolution()
        {
            string inputString = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day5Input.txt").ReadToEnd();    // Stores text file as a string
            string[] inputStringArray = inputString.Split(',');     // Splits the input into substrings separated by the ',' delimiter
            List<int> intcode = new List<int>();                   // Stores the intcode (puzzle input) as a list of integers

            // Take each element from inputStringArray and store it as an integer in the intcode list
            foreach (string element in inputStringArray)
            {
                intcode.Add(Convert.ToInt32(element));
            }

            RunIntcodeProgram(intcode);
        }
    }
}
