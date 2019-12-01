using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    // Part 1: https://adventofcode.com/2019/day/1
    // Answer: 3263354

    class Day1
    {
        private static int calculateFuel(int mass)    // Mass function: take the mass, divide by three, round down, subtract two
        {
            double massOverThree = mass / 3d;                       // Divide mass by 3
            int fuel = Convert.ToInt32(Math.Floor(massOverThree));  // Round down to nearest integer
            fuel = fuel - 2;                                        // Subtract by two
            return fuel;
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
            Console.ReadLine();
        }
    }
}
