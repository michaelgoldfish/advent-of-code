﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{

    /*  --- Day 4: Secure Container ---
        You arrive at the Venus fuel depot only to discover it's protected by a password. The Elves had written the password on a sticky note, but someone threw it out.

        However, they do remember a few key facts about the password:

        It is a six-digit number.
        The value is within the range given in your puzzle input.
        Two adjacent digits are the same (like 22 in 122345).
        Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).

        Other than the range rule, the following are true:

        111111 meets these criteria (double 11, never decreases).
        223450 does not meet these criteria (decreasing pair of digits 50).
        123789 does not meet these criteria (no double).
        How many different passwords within the range given in your puzzle input meet these criteria?

        Your puzzle input is 387638-919123.
        Answer: 466
    */

    /*  --- Part Two ---
        An Elf just remembered one more important detail: the two adjacent matching digits are not part of a larger group of matching digits.

        Given this additional criterion, but still ignoring the range rule, the following are now true:

        112233 meets these criteria because the digits never decrease and all repeated digits are exactly two digits long.
        123444 no longer meets the criteria (the repeated 44 is part of a larger group of 444).
        111122 meets the criteria (even though 1 is repeated more than twice, it still contains a double 22).
        How many different passwords within the range given in your puzzle input meet all of the criteria?
        Answer: 292
    */

    class Day4
    {
        const int rangeBottom = 387638;     // Bottom range of possible passwords
        const int rangeTop = 919123;        // Top range of possible values
        const int requiredLength = 6;       // Length of password

        private static bool FitsCriteria(int password)
        {
            string passwordAsString = password.ToString();
            char[] passwordChars = passwordAsString.ToCharArray();

            // Check that number is 6 digits
            int length = passwordAsString.Length;
            if (length != requiredLength)                        // Password is too long or too short
            {
                return false;
            }

            // Check that number is within specified range
            if (password < rangeBottom || password > rangeTop)  // Password is above or beyond the specified range of values
            {
                return false;
            }

            // Check that two adjacent digits are the same and are not part of a larger group of matching digits
            bool hasTwoAdjacent = false;                        // Tracks whether an adjacent match has been found
            for (int n = 0; n < passwordChars.Length - 1; n++)  // Goes through each digit of the password except the last
            {
                if (passwordChars[n] == passwordChars[n + 1])           //Check that the two digits surrounding the pair are not the same
                {   
                    if (n == 0 || passwordChars[n - 1] != passwordChars[n])    // Check that either n is the first digit , or that n - 1 is different from n
                    {
                        if (n + 1 == passwordChars.Length - 1 || passwordChars[n + 1] != passwordChars[n + 2])    // Check that either n + 1 is the last digit, or that n + 1 is different from n + 2
                        {
                            hasTwoAdjacent = true;
                            break;
                        }
                    }
                }
            }
            if (!hasTwoAdjacent)                                // Password does not contain two adjacent matching digits
            {
                return false;
            }

            // Check that digits never decrease from left to right
            for (int n = 0; n < passwordChars.Length - 1; n++)
            {
                if (passwordChars[n + 1] < passwordChars[n])                                  // A digit decreased from left to right
                {
                    return false;
                }
            }

            return true;            // All the checks have been successful
        }

        public static void OutputSolution()
        {
            int numOfPasswords = 0;             // Number of different passwords within the range that meet the criteria
            for (int i = rangeBottom; i <= rangeTop; i++)
            {
                if (Day4.FitsCriteria(i))
                {
                    numOfPasswords++;
                }
            }

            Console.WriteLine("The number of different passwords is: {0}", numOfPasswords);
        }
    }
}
