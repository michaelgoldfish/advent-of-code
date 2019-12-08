using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /*  --- Day 7: Amplification Circuit ---
        
        Based on the navigational maps, you're going to need to send more power to your ship's thrusters to reach Santa in time.
        To do this, you'll need to configure a series of amplifiers already installed on the ship.

        There are five amplifiers connected in series; each one receives an input signal and produces an output signal.
        They are connected such that the first amplifier's output leads to the second amplifier's input, the second amplifier's output leads to the third amplifier's input, and so on.
        The first amplifier's input value is 0, and the last amplifier's output leads to your ship's thrusters.

            O-------O  O-------O  O-------O  O-------O  O-------O
        0 ->| Amp A |->| Amp B |->| Amp C |->| Amp D |->| Amp E |-> (to thrusters)
            O-------O  O-------O  O-------O  O-------O  O-------O

        The Elves have sent you some Amplifier Controller Software (your puzzle input), a program that should run on your existing Intcode computer.
        Each amplifier will need to run a copy of the program.

        When a copy of the program starts running on an amplifier, it will first use an input instruction to ask the amplifier for its current phase setting (an integer from 0 to 4).
        Each phase setting is used exactly once, but the Elves can't remember which amplifier needs which phase setting.

        The program will then call another input instruction to get the amplifier's input signal, compute the correct output signal, and supply it back to the amplifier with an output instruction.
        (If the amplifier has not yet received an input signal, it waits until one arrives.)

        Your job is to find the largest output signal that can be sent to the thrusters by trying every possible combination of phase settings on the amplifiers.
        Make sure that memory is not shared or reused between copies of the program.

        For example, suppose you want to try the phase setting sequence 3,1,2,4,0, which would mean setting amplifier A to phase setting 3, amplifier B to setting 1, C to 2, D to 4, and E to 0.
        Then, you could determine the output signal that gets sent from amplifier E to the thrusters with the following steps:

        -Start the copy of the amplifier controller software that will run on amplifier A. At its first input instruction, provide it the amplifier's phase setting, 3.
         At its second input instruction, provide it the input signal, 0. After some calculations, it will use an output instruction to indicate the amplifier's output signal.
        -Start the software for amplifier B. Provide it the phase setting (1) and then whatever output signal was produced from amplifier A.
         It will then produce a new output signal destined for amplifier C.
        -Start the software for amplifier C, provide the phase setting (2) and the value from amplifier B, then collect its output signal.
        -Run amplifier D's software, provide the phase setting (4) and input value, and collect its output signal.
        -Run amplifier E's software, provide the phase setting (0) and input value, and collect its output signal.

        The final output signal from amplifier E would be sent to the thrusters.
        However, this phase setting sequence may not have been the best one; another sequence might have sent a higher signal to the thrusters.

        Here are some example programs:

        Max thruster signal 43210 (from phase setting sequence 4,3,2,1,0):

        3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0
        Max thruster signal 54321 (from phase setting sequence 0,1,2,3,4):

        3,23,3,24,1002,24,10,24,1002,23,-1,23,
        101,5,23,23,1,24,23,23,4,23,99,0,0
        Max thruster signal 65210 (from phase setting sequence 1,0,4,3,2):

        3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,
        1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0

        Try every combination of phase settings on the amplifiers. What is the highest signal that can be sent to the thrusters?
        Answer: 225056
    */

    /*  --- Part Two ---
       
        It's no good - in this configuration, the amplifiers can't generate a large enough output signal to produce the thrust you'll need.
        The Elves quickly talk you through rewiring the amplifiers into a feedback loop:

          O-------O  O-------O  O-------O  O-------O  O-------O
        0 -+->| Amp A |->| Amp B |->| Amp C |->| Amp D |->| Amp E |-.
        |  O-------O  O-------O  O-------O  O-------O  O-------O |
        |                                                        |
        '--------------------------------------------------------+
                                                                |
                                                                v
                                                         (to thrusters)

        Most of the amplifiers are connected as they were before; amplifier A's output is connected to amplifier B's input, and so on.
        However, the output from amplifier E is now connected into amplifier A's input. This creates the feedback loop: the signal will be sent through the amplifiers many times.

        In feedback loop mode, the amplifiers need totally different phase settings: integers from 5 to 9, again each used exactly once.
        These settings will cause the Amplifier Controller Software to repeatedly take input and produce output many times before halting.
        Provide each amplifier its phase setting at its first input instruction; all further input/output instructions are for signals.

        Don't restart the Amplifier Controller Software on any amplifier during this process. Each one should continue receiving and sending signals until it halts.

        All signals sent or received in this process will be between pairs of amplifiers except the very first signal and the very last signal.
        To start the process, a 0 signal is sent to amplifier A's input exactly once.

        Eventually, the software on the amplifiers will halt after they have processed the final loop.
        When this happens, the last output signal from amplifier E is sent to the thrusters.
        Your job is to find the largest output signal that can be sent to the thrusters using the new phase settings and feedback loop arrangement.

        Here are some example programs:

        Max thruster signal 139629729 (from phase setting sequence 9,8,7,6,5):

        3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,
        27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5
        Max thruster signal 18216 (from phase setting sequence 9,7,8,5,6):

        3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,
        -5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,
        53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10

        Try every combination of the new phase settings on the amplifier feedback loop. What is the highest signal that can be sent to the thrusters?
        Answer: 14260332
    */

    class Day7
    {
        private static Tuple<int, int, List<int>> RunIntcodeProgram(List<int> intcode, int phaseSetting, int previousOutput, int instructionPointer)    // Runs the given intcode program, using phaseSetting and previousOutput as its input values
        {
            // Returned values: program output, instruction pointer, edited intcode up to where it was returned

            // Step count changes based on the opcode ran
            int stepCount = 4;      // Step count for opcodes within the list. After the first opcode operation is done, this many places are skipped to find the next opcode

            int inputCount = 0;     // Tracks the number of inputs the code has asked for (for opcode 3)

            for (int index = instructionPointer; index < intcode.Count; index += stepCount)     // Loop starts at instructionPointer instead of 0 to allow saving where in the code the pointer is
            {
                int indexValue = (int)intcode[index];

                if (indexValue == 99)   // Opcode for ending the program
                {
                    return Tuple.Create(previousOutput, -1, intcode);           // Return output and the current state of the intcode       
                }

                // Check the parameter modes and opcode within indexValue. Mode 0 = Parameter mode (parameters are addresses), Mode 1 = Immediate mode (parameters are values)

                int opcode = indexValue % 100;              // Stores the rightmost two digits
                int mode1 = (indexValue / 100) % 10;        // Stores the third digit from the right; the hundreds digit
                int mode2 = (indexValue / 1000) % 10;       // Stores the fourth digit from the right; the thousands digit
                int mode3 = (indexValue / 10000) % 10;      // Stores the fifth digit from the right; the ten-thousands digit

                int input1;                             // Value of the first input
                int input2;                             // Value of the second input (if applicable)

                if (opcode == 1 || opcode == 2 || opcode == 5 || opcode == 6 || opcode == 7 || opcode == 8)        // Opcodes for addition & multiplication, as well as opcodes 5-8 from part 2
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

                    if (opcode == 1)                // Opcode for addition
                    {
                        intcode[intcode[index + 3]] = input1 + input2;   // Set the value at the third parameter to the calculated value
                        stepCount = 4;
                    }

                    else if (opcode == 2)           // Opcode for multiplication
                    {
                        intcode[intcode[index + 3]] = input1 * input2;   // Set the value at the third parameter to the calculated value
                        stepCount = 4;
                    }

                    else if (opcode == 5)           // Opcode for jump-if-true
                    {
                        if (input1 != 0)
                        {
                            index = input2;         // Sets the instruction pointer to the value of the second parameter
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
                            index = input2;         // Sets the instruction pointer to the value of the second parameter
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
                            intcode[intcode[index + 3]] = 1;    // Set the value at the third parameter to 1
                        }
                        else
                        {
                            intcode[intcode[index + 3]] = 0;    // Set the value at the third parameter to 1
                        }
                        stepCount = 4;
                    }

                    else if (opcode == 8)           // Opcode for equals
                    {
                        if (input1 == input2)
                        {
                            intcode[intcode[index + 3]] = 1;    // Set the value at the third parameter to 1
                        }
                        else
                        {
                            intcode[intcode[index + 3]] = 0;    // Set the value at the third parameter to 1
                        }
                        stepCount = 4;
                    }
                }

                else if (opcode == 3)   // Opcode for inputting a value. Takes phaseSetting as input first, then previousOutput second
                {
                    inputCount++;
                    int input;

                    if (inputCount == 1 && instructionPointer == 0)     // Only uses phaseSetting as input once for each Amp, even if this function is ran multiple times
                    {
                        input = phaseSetting;
                    }
                    else
                    {
                        input = previousOutput;
                    }

                    intcode[intcode[index + 1]] = input;    // Set the value at the second parameter to the inputted value

                    stepCount = 2;
                }

                else if (opcode == 4)   // Opcode for outputting a value
                {
                    int output4;

                    if (mode1 == 0)
                    {
                        output4 = intcode[intcode[index + 1]];
                    }
                    else    // mode1 == 1
                    {
                        output4 = intcode[index + 1];
                    }

                    //Increase index by 2, then return
                    index += 2;
                    return Tuple.Create(output4, index, intcode);      // Returns the output, the 
                }

                else
                {
                    Console.WriteLine("Opcode {0} has been found at index {1} but does not have an operation designated to it. Known opcodes are 1-8 and 99.", indexValue, index);
                }
            }

            return Tuple.Create(previousOutput, -80, intcode);           // Should never get ran, as function returns after reaching opcode 4 or 99
        }

        public static List<int> InitializeIntcodeProgram()          // Initializes the intcode from the input file
        {
            string inputString = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day7Input.txt").ReadToEnd();    // Stores text file as a string
            string[] inputStringArray = inputString.Split(',');     // Splits the input into substrings separated by the ',' delimiter
            List<int> intcode = new List<int>();                   // Stores the intcode (puzzle input) as a list of integers

            // Take each element from inputStringArray and store it as an integer in the intcode list
            foreach (string element in inputStringArray)
            {
                intcode.Add(Convert.ToInt32(element));
            }

            return intcode;
        }

        public static void OutputSolution()
        {
            List<int> intcode = InitializeIntcodeProgram();
            const int bottom = 5;               // Lowest number in the sequence
            const int top = 9;                  // Highest number in the sequence
            const int inputSignal = 0;          // First input for A (set outputE to this since A takes it as input)

            int highestSignal = 0;              // Tracks the highest signal that has been found

            //Run the intcode program five times given a five - digit phase setting sequence
            for (int a = bottom; a <= top; a++)
            {
                for (int b = bottom; b <= top; b++)
                {
                    if (b != a)
                    {
                        for (int c = bottom; c <= top; c++)
                        {
                            if (c != b && c != a)
                            {
                                for (int d = bottom; d <= top; d++)
                                {
                                    if (d != c && d != b && d != a)
                                    {
                                        for (int e = bottom; e <= top; e++)
                                        {
                                            if (e != d && e != c && e != b && e != a)
                                            {
                                                // Loop the amplifiers until amplifier E reaches opcode 99, then compare if the output size is larger

                                                Tuple<int, int, List<int>> tupleA;  // Tuple to store A's output
                                                int outputA = 0;                    // A's output; input for B
                                                int pointerA = 0;                   // Instruction Pointer for A
                                                List<int> intcodeA = new List<int>(intcode);       // intcode for amplifier A

                                                Tuple<int, int, List<int>> tupleB;  // Tuple to store B's output
                                                int outputB = 0;                    // B's output; input for C
                                                int pointerB = 0;                   // Instruction Pointer for B
                                                List<int> intcodeB = new List<int>(intcode);       // intcode for amplifier B

                                                Tuple<int, int, List<int>> tupleC;  // Tuple to store C's output
                                                int outputC = 0;                    // C's output; input for D
                                                int pointerC = 0;                   // Instruction Pointer for C
                                                List<int> intcodeC = new List<int>(intcode);       // intcode for amplifier C

                                                Tuple<int, int, List<int>> tupleD;  // Tuple to store D's output
                                                int outputD = 0;                    // D's output; input for E
                                                int pointerD = 0;                   // Instruction Pointer for D
                                                List<int> intcodeD = new List<int>(intcode);       // intcode for amplifier D

                                                Tuple<int, int, List<int>> tupleE;  // Tuple to store E's output
                                                int outputE = inputSignal;          // E's output; input for D
                                                int pointerE = 0;                   // Instruction Pointer for E
                                                List<int> intcodeE = new List<int>(intcode);       // intcode for amplifier E

                                                do
                                                {
                                                    tupleA = RunIntcodeProgram(intcodeA, a, outputE, pointerA);
                                                    outputA = tupleA.Item1;
                                                    pointerA = tupleA.Item2;
                                                    intcodeA = tupleA.Item3;

                                                    tupleB = RunIntcodeProgram(intcodeB, b, outputA, pointerB);
                                                    outputB = tupleB.Item1;
                                                    pointerB = tupleB.Item2;
                                                    intcodeB = tupleB.Item3;

                                                    tupleC = RunIntcodeProgram(intcodeC, c, outputB, pointerC);
                                                    outputC = tupleC.Item1;
                                                    pointerC = tupleC.Item2;
                                                    intcodeC = tupleC.Item3;

                                                    tupleD = RunIntcodeProgram(intcodeD, d, outputC, pointerD);
                                                    outputD = tupleD.Item1;
                                                    pointerD = tupleD.Item2;
                                                    intcodeD = tupleD.Item3;

                                                    tupleE = RunIntcodeProgram(intcodeE, e, outputD, pointerE);
                                                    outputE = tupleE.Item1;
                                                    pointerE = tupleE.Item2;
                                                    intcodeE = tupleE.Item3;
                                                }
                                                while (pointerE != -1);

                                                if (outputE > highestSignal)
                                                {
                                                    highestSignal = outputE;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Highest signal is: {0}", highestSignal);
        }
    }
}
