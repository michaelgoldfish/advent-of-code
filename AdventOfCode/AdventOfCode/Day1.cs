using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /* --- Day 1: The Tyranny of the Rocket Equation ---
     
       Santa has become stranded at the edge of the Solar System while delivering presents to other planets!
       To accurately calculate his position in space, safely align his warp drive, and return to Earth in time to save Christmas, he needs you to bring him measurements from fifty stars.

       Collect stars by solving puzzles.Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first.
       Each puzzle grants one star. Good luck!

       The Elves quickly load you into a spacecraft and prepare to launch.

       At the first Go / No Go poll, every Elf is Go until the Fuel Counter-Upper.They haven't determined the amount of fuel required yet.

       Fuel required to launch a given module is based on its mass. Specifically, to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.

       For example:

           For a mass of 12, divide by 3 and round down to get 4, then subtract 2 to get 2.
           For a mass of 14, dividing by 3 and rounding down still yields 4, so the fuel required is also 2.
           For a mass of 1969, the fuel required is 654.
           For a mass of 100756, the fuel required is 33583.
           The Fuel Counter-Upper needs to know the total fuel requirement. To find it, individually calculate the fuel needed for the mass of each module (your puzzle input), then add together all the fuel values.

       What is the sum of the fuel requirements for all of the modules on your spacecraft?
       Answer: 3263354
    */

    /* --- Part Two ---
        During the second Go / No Go poll, the Elf in charge of the Rocket Equation Double-Checker stops the launch sequence.Apparently, you forgot to include additional fuel for the fuel you just added.

        Fuel itself requires fuel just like a module - take its mass, divide by three, round down, and subtract 2. However, that fuel also requires fuel, and that fuel requires fuel, and so on.Any mass that would require negative fuel should instead be treated as if it requires zero fuel; the remaining mass, if any, is instead handled by wishing really hard, which has no mass and is outside the scope of this calculation.

        So, for each module mass, calculate its fuel and add it to the total.
        Then, treat the fuel amount you just calculated as the input mass and repeat the process, continuing until a fuel requirement is zero or negative.For example:

            A module of mass 14 requires 2 fuel.This fuel requires no further fuel (2 divided by 3 and rounded down is 0, which would call for a negative fuel), so the total fuel required is still just 2.
            At first, a module of mass 1969 requires 654 fuel. Then, this fuel requires 216 more fuel (654 / 3 - 2). 216 then requires 70 more fuel, which requires 21 fuel, which requires 5 fuel, which requires no further fuel. So, the total fuel required for a module of mass 1969 is 654 + 216 + 70 + 21 + 5 = 966.
            The fuel required by a module of mass 100756 and its fuel is: 33583 + 11192 + 3728 + 1240 + 411 + 135 + 43 + 12 + 2 = 50346.

        What is the sum of the fuel requirements for all of the modules on your spacecraft when also taking into account the mass of the added fuel? (Calculate the fuel requirements for each module separately, then add them all up at the end.)
        Answer: 4892166
    */

    class Day1
    {
        private static int calculateFuel(int mass)    // Runs the fuel calculation, taking into account the extra fuel required for the mass of the fuel itself
        {
            // Fuel calculation: take the mass, divide by three, round down, subtract by two
            double massOverThree = mass / 3d;                       // Divide mass by 3
            int fuel = Convert.ToInt32(Math.Floor(massOverThree));  // Round down to nearest integer
            fuel = fuel - 2;                                        // Subtract by two

            if (fuel > 0)   // The fuel requires more fuel for itself
            {
                fuel += calculateFuel(fuel);                        // Add more fuel for the extra mass of the added fuel

                return fuel;
            }
            else    // The fuel requires no further fuel. The remaining mass is handled by "wishing really hard* according to the question
            {
                return 0;   
            }
        }

        public static void outputSolution()
        {
            int fuelTotal = 0;  // Stores the total amount of fuel
            string line;        // Stores a line from the text file
            System.IO.StreamReader file = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day1Input.txt");    // Allows to read the text file

            // Read the file line by line, performing the fuel calculation for each line and getting the sum
            while ((line = file.ReadLine()) != null)
            {
                int mass = Convert.ToInt32(line);
                fuelTotal += calculateFuel(mass);
            }

            Console.WriteLine("Fuel total is {0}", fuelTotal);
        }


    }
}
