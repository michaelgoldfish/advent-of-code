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
     


    class Day3
    {
        readonly private static int[] origin = { 0, 0 };                // Origin of the path as an xy coordinate

        private static List<int[]> ConvertPathToValues(string path)     // Converts path string to a list of individual location values for easier comparison between wires
        {
            List<int[]> positionList = new List<int[]>();   // Stores every position that the wire has been on
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
                        positionList.Add(new int[] { currentPosition[0], currentPosition[1] });  // Adds the new position to the list
                    }
                }
                else if (direction.Equals('L'))     // Code to travel left
                {
                    for (int n = localStartPosition; n > localStartPosition - distance; n--)
                    {
                        currentPosition[0] -= 1;            // Moves position left one unit
                        positionList.Add(new int[] { currentPosition[0], currentPosition[1] });  // Adds the new position to the list
                    }
                }
                else if (direction.Equals('U'))     // Code to travel upward
                {
                    for (int n = localStartPosition; n < localStartPosition + distance; n++)
                    {
                        currentPosition[1] += 1;            // Moves position up one unit
                        positionList.Add(new int[] { currentPosition[0], currentPosition[1] });  // Adds the new position to the list
                    }
                }
                else if (direction.Equals('D'))     // Code to travel downward
                {  
                    for (int n = localStartPosition; n > localStartPosition - distance; n--)
                    {
                        currentPosition[1] -= 1;            // Moves position down one unit
                        positionList.Add(new int[] { currentPosition[0], currentPosition[1] });  // Adds the new position to the list
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
                        pointsOfIntersection.Add(wire1[i]); // Adds the coordinate to the list (either coordinate can be added since they have the same value)
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

            List<int[]> pointsOfIntersection = findPointsOfIntersection(wire1Values, wire2Values);  // Stores the positions where the wires overlap

            int shortestManhattan = findShortestMahnattan(pointsOfIntersection, origin);    // Finds the shortest manhattan value to the origin point
            Console.WriteLine("Manhattan distance of closest intersection is {0}", shortestManhattan);

            //List<int[]> testList = new List<int[]>();
            //testList.Add(new int[] { 5, 5 });
            //int manhattanTest = findShortestMahnattan(testList, origin);
        }
    }
}
