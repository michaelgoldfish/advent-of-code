using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /*  --- Day 3: Crossed Wires ---
        The gravity assist was successful, and you're well on your way to the Venus refuelling station.
        During the rush back on Earth, the fuel management system wasn't completely installed, so that's next on the priority list.

        Opening the front panel reveals a jumble of wires. Specifically, two wires are connected to a central port and extend outward on a grid.
        You trace the path each wire takes as it leaves the central port, one wire per line of text (your puzzle input).

        The wires twist and turn, but the two wires occasionally cross paths. To fix the circuit, you need to find the intersection point closest to the central port.
        Because the wires are on a grid, use the Manhattan distance for this measurement.
        While the wires do technically cross right at the central port where they both start, this point does not count, nor does a wire count as crossing with itself.

        For example, if the first wire's path is R8,U5,L5,D3, then starting from the central port (o), it goes right 8, up 5, left 5, and finally down 3:

        ...........
        ...........
        ...........
        ....+----+.
        ....|....|.
        ....|....|.
        ....|....|.
        .........|.
        .o-------+.
        ...........
        Then, if the second wire's path is U7,R6,D4,L4, it goes up 7, right 6, down 4, and left 4:

        ...........
        .+-----+...
        .|.....|...
        .|..+--X-+.
        .|..|..|.|.
        .|.-X--+.|.
        .|..|....|.
        .|.......|.
        .o-------+.
        ...........
        These wires cross at two locations (marked X), but the lower-left one is closer to the central port: its distance is 3 + 3 = 6.

        Here are a few more examples:

        R75,D30,R83,U83,L12,D49,R71,U7,L72
        U62,R66,U55,R34,D71,R55,D58,R83 = distance 159
        R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
        U98,R91,D20,R16,D67,R40,U7,R15,U6,R7 = distance 135

        What is the Manhattan distance from the central port to the closest intersection?
        Answer: 207
    */

    /*  --- Part Two ---
        It turns out that this circuit is very timing-sensitive; you actually need to minimize the signal delay.

        To do this, calculate the number of steps each wire takes to reach each intersection;
        choose the intersection where the sum of both wires' steps is lowest.
        If a wire visits a position on the grid multiple times, use the steps value from the first time it visits that position when calculating the total value of a specific intersection.

        The number of steps a wire takes is the total number of grid squares the wire has entered to get to that location, including the intersection being considered.
        Again consider the example from above:

        ...........
        .+-----+...
        .|.....|...
        .|..+--X-+.
        .|..|..|.|.
        .|.-X--+.|.
        .|..|....|.
        .|.......|.
        .o-------+.
        ...........
        In the above example,
        the intersection closest to the central port is reached after 8+5+5+2 = 20 steps by the first wire and 7+6+4+3 = 20 steps by the second wire for a total of 20+20 = 40 steps.

        However, the top-right intersection is better: the first wire takes only 8+5+2 = 15 and the second wire takes only 7+6+2 = 15, a total of 15+15 = 30 steps.

        Here are the best steps for the extra examples from above:

        R75,D30,R83,U83,L12,D49,R71,U7,L72
        U62,R66,U55,R34,D71,R55,D58,R83 = 610 steps
        R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
        U98,R91,D20,R16,D67,R40,U7,R15,U6,R7 = 410 steps

        What is the fewest combined steps the wires must take to reach an intersection?
        Answer: 21196
    */

    class Day3
    {
        readonly private static int[] origin = { 0, 0 };                // Origin of the path as an xy coordinate

        private static List<int[]> ConvertPathToValues(string path)     // Converts path string to a list of individual location values for easier comparison between wires
        {
            List<int[]> positionList = new List<int[]>();   // Stores every position that the wire has been on along with the # of steps to get to that position (3rd value)
            int stepCount = 0;                              // Stores the number of steps that have been taken
            int[] currentPosition = { origin[0], origin[1] };                 // Keeps track of current position when going through the path
            string[] pathArray = path.Split(',');           // Separates each path in string by the comma delimeter

            // Perform convertion to positions for each command of the path
            for (int i = 0; i < pathArray.Length; i++)
            {
                char[] pathUnitArray = pathArray[i].ToCharArray();      // Specific travel unit being analyzed, eg. "R8", as a char array to separate the direction and distance parts
                char direction = pathUnitArray[0];                      // Direction of travel, eg. "R"
                int distance;                                           // Distance traveled, e.g. "8"

                // Store distance as an integer regardless of # of digits
                string distanceAsString = "";
                for(int x = 1; x < pathUnitArray.Length; x++)   // Add the rest of the digits together
                {
                    distanceAsString += pathUnitArray[x];
                }
                distance = Convert.ToInt32(distanceAsString);   // Convert the distance string into an integer

                int localStartPosition = currentPosition[0];    // Stores the start position of the below if statements

                if (direction.Equals('R'))          // Code to travel right
                {
                    for (int n = localStartPosition; n < localStartPosition + distance; n++)
                    {
                        currentPosition[0] += 1;            // Moves position right one unit
                        stepCount++;                        // Increases the step count by one
                        positionList.Add(new int[] { currentPosition[0], currentPosition[1], stepCount });  // Adds the new position to the list with the step count
                    }
                }
                else if (direction.Equals('L'))     // Code to travel left
                {
                    for (int n = localStartPosition; n > localStartPosition - distance; n--)
                    {
                        currentPosition[0] -= 1;            // Moves position left one unit
                        stepCount++;                        // Increases the step count by one
                        positionList.Add(new int[] { currentPosition[0], currentPosition[1], stepCount });  // Adds the new position to the list with the step count
                    }
                }
                else if (direction.Equals('U'))     // Code to travel upward
                {
                    for (int n = localStartPosition; n < localStartPosition + distance; n++)
                    {
                        currentPosition[1] += 1;            // Moves position up one unit
                        stepCount++;                        // Increases the step count by one
                        positionList.Add(new int[] { currentPosition[0], currentPosition[1], stepCount });  // Adds the new position to the list with the step count
                    }
                }
                else if (direction.Equals('D'))     // Code to travel downward
                {  
                    for (int n = localStartPosition; n > localStartPosition - distance; n--)
                    {
                        currentPosition[1] -= 1;            // Moves position down one unit
                        stepCount++;                        // Increases the step count by one
                        positionList.Add(new int[] { currentPosition[0], currentPosition[1], stepCount });  // Adds the new position to the list with the step count
                    }
                }
                else
                {
                    // Invalid path
                    Console.WriteLine("Invalid path, direction \"{0}\" is not specified. Valid directions are R, L, U, D.", direction);
                }
            }

            return positionList;
        }

        private static List<int[]> findPointsOfIntersection(List<int[]> wire1, List<int[]> wire2)       // Returns a list of coordinates where the two wires overlap
        {
            List<int[]> pointsOfIntersection = new List<int[]>();   // Stores the coordinates where the wires intersect

            for (int i = 0; i < wire1.Count; i++)
            {
                for (int j = 0; j < wire2.Count; j++)
                {
                    if (wire1[i][0] == wire2[j][0] && wire1[i][1] == wire2[j][1])   // Checks if the two coordinates are equal
                    {
                        int[] intersection = { wire1[i][0], wire1[i][1], 0 };      // Gives the coordinate being added the same position as the intersection
                        intersection[2] = wire1[i][2] + wire2[j][2];            // Stores the total step count between the two wires as a third coordinate value

                        pointsOfIntersection.Add(intersection); // Adds the coordinate to the list with the summed step count
                    }
                }
            }

            return pointsOfIntersection;
        }

        private static int findShortestMahnattan(List<int[]> pointsOfIntersection, int[] coordinate)    // Calculates the shortest manhattan distance between the points of intersection and the specified coordinate (eg. the origin)
        {
            int shortestDistance = Math.Abs(pointsOfIntersection[0][0] - coordinate[0]) + Math.Abs(pointsOfIntersection[0][1] - coordinate[1]);   // Stores the shortest distance found. Default is set to first value in the list

            for (int i = 1; i < pointsOfIntersection.Count; i++)    // Searches for a shorter distance through the rest of the list
            {
                int distance = Math.Abs(pointsOfIntersection[i][0] - coordinate[0]) + Math.Abs(pointsOfIntersection[i][1] - coordinate[1]);
                if (distance < shortestDistance)    // Distance found is shorter than current shortest distance
                {
                    shortestDistance = distance;    // Replace shortest distance with the current distance found
                }
            }

            return shortestDistance;
        }

        private static int findShortestStepCount(List<int[]> pointsOfIntersection)                      // Finds the shortest number of steps within the intersection list
        {
            int shortestStepCount = pointsOfIntersection[0][2];     // Tracks the shortest step count found. Default value is the step count of the first point of intersection

            for (int i = 1; i < pointsOfIntersection.Count; i++)    // Checks if the current step count is shorter than the current shortest and replaces it if it is
            {
                if (pointsOfIntersection[i][2] < shortestStepCount)
                {
                    shortestStepCount = pointsOfIntersection[i][2];
                }
            }

            return shortestStepCount;
        }

        public static void OutputSolution()  
        {
            System.IO.StreamReader file = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day3Input.txt");    // Allows to read the text file
            string line;
            int currentLine = 1;    // Track which line of input is being read
            string wire1Path = "";  // Stores the path of the first wire
            string wire2Path = "";  // Stores the path of the second wire

            while ((line = file.ReadLine()) != null)    // Splits the input file into two strings, one for each wire path
            {
                if (currentLine == 1)
                {
                    wire1Path = line;
                }
                else if (currentLine == 2)
                {
                    wire2Path = line;
                }
                else
                {
                    Console.WriteLine("Only two wires are currently supported.");
                }
                currentLine++;
            }

            List<int[]> wire1Values = ConvertPathToValues(wire1Path);   // Converts the input strings into a list of positions that the wires are on
            List<int[]> wire2Values = ConvertPathToValues(wire2Path);

            List<int[]> pointsOfIntersection = findPointsOfIntersection(wire1Values, wire2Values);  // Stores the positions where the wires overlap along with the step counts to reach there

            //int shortestManhattan = findShortestMahnattan(pointsOfIntersection, origin);  // Finds the shortest manhattan value to the origin point
            //Console.WriteLine("Manhattan distance of closest intersection is {0}", shortestManhattan);

            int shortestStepCount = findShortestStepCount(pointsOfIntersection);            // Find the shortest step count among the intersections      
            Console.WriteLine("Fewest combined steps is: {0} steps", shortestStepCount);
        }
    }
}
