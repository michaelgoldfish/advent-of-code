using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class Day10
    {
        /*  --- Day 10: Monitoring Station ---
           
            You fly into the asteroid belt and reach the Ceres monitoring station.
            The Elves here have an emergency: they're having trouble tracking all of the asteroids and can't be sure they're safe.

            The Elves would like to build a new monitoring station in a nearby area of space;
            they hand you a map of all of the asteroids in that region (your puzzle input).

            The map indicates whether each position is empty (.) or contains an asteroid (#).
            The asteroids are much smaller than they appear on the map, and every asteroid is exactly in the center of its marked position.
            The asteroids can be described with X,Y coordinates where X is the distance from the left edge and Y is the distance from the top edge
            (so the top-left corner is 0,0 and the position immediately to its right is 1,0).

            Your job is to figure out which asteroid would be the best place to build a new monitoring station.
            A monitoring station can detect any asteroid to which it has direct line of sight - that is, there cannot be another asteroid exactly between them.
            This line of sight can be at any angle, not just lines aligned to the grid or diagonally.
            The best location is the asteroid that can detect the largest number of other asteroids.

            For example, consider the following map:

            .#..#
            .....
            #####
            ....#
            ...##

            The best location for a new monitoring station on this map is the highlighted asteroid at 3,4 because it can detect 8 asteroids, more than any other location.
            (The only asteroid it cannot detect is the one at 1,0; its view of this asteroid is blocked by the asteroid at 2,2.)
            All other asteroids are worse locations; they can detect 7 or fewer other asteroids.
            
            Here is the number of other asteroids a monitoring station on each asteroid could detect:

            .7..7
            .....
            67775
            ....7
            ...87

            Here is an asteroid (#) and some examples of the ways its line of sight might be blocked.
            If there were another asteroid at the location of a capital letter,
            the locations marked with the corresponding lowercase letter would be blocked and could not be detected:

            #.........
            ...A......
            ...B..a...
            .EDCG....a
            ..F.c.b...
            .....c....
            ..efd.c.gb
            .......c..
            ....f...c.
            ...e..d..c

            Here are some larger examples:

            Best is 5,8 with 33 other asteroids detected:
            ......#.#.
            #..#.#....
            ..#######.
            .#.#.###..
            .#..#.....
            ..#....#.#
            #..#....#.
            .##.#..###
            ##...#..#.
            .#....####

            Best is 1,2 with 35 other asteroids detected:
            #.#...#.#.
            .###....#.
            .#....#...
            ##.#.#.#.#
            ....#.#.#.
            .##..###.#
            ..#...##..
            ..##....##
            ......#...
            .####.###.

            Best is 6,3 with 41 other asteroids detected:
            .#..#..###
            ####.###.#
            ....###.#.
            ..###.##.#
            ##.##.#.#.
            ....###..#
            ..#.#..#.#
            #..#.#.###
            .##...##.#
            .....#.#..

            Best is 11,13 with 210 other asteroids detected:
            .#..##.###...#######
            ##.############..##.
            .#.######.########.#
            .###.#######.####.#.
            #####.##.#.##.###.##
            ..#####..#.#########
            ####################
            #.####....###.#.#.##
            ##.#################
            #####.##.###..####..
            ..######..##.#######
            ####.##.####...##..#
            .#####..#.######.###
            ##...#.##########...
            #.##########.#######
            .####.#.###.###.#.##
            ....##.##.###..#####
            .#.#.###########.###
            #.#.#.#####.####.###
            ###.##.####.##.#..##

            Find the best location for a new monitoring station. How many other asteroids can be detected from that location?
            Answer: 214 (at position [8,16]
        */

        /*  --- Part Two ---
        
            Once you give them the coordinates, the Elves quickly deploy an Instant Monitoring Station to the location and discover the worst: there are simply too many asteroids.

            The only solution is complete vaporization by giant laser.

            Fortunately, in addition to an asteroid scanner, the new monitoring station also comes equipped with a giant rotating laser perfect for vaporizing asteroids.
            The laser starts by pointing up and always rotates clockwise, vaporizing any asteroid it hits.

            If multiple asteroids are exactly in line with the station, the laser only has enough power to vaporize one of them before continuing its rotation.
            In other words, the same asteroids that can be detected can be vaporized, but if vaporizing one asteroid makes another one detectable,
            the newly-detected asteroid won't be vaporized until the laser has returned to the same position by rotating a full 360 degrees.

            For example, consider the following map, where the asteroid with the new monitoring station (and laser) is marked X:

            .#....#####...#..
            ##...##.#####..##
            ##...#...#.#####.
            ..#.....X...###..
            ..#.#.....#....##

            The first nine asteroids to get vaporized, in order, would be:

            .#....###24...#..
            ##...##.13#67..9#
            ##...#...5.8####.
            ..#.....X...###..
            ..#.#.....#....##

            Note that some asteroids (the ones behind the asteroids marked 1, 5, and 7) won't have a chance to be vaporized until the next full rotation.
            The laser continues rotating; the next nine to be vaporized are:

            .#....###.....#..
            ##...##...#.....#
            ##...#......1234.
            ..#.....X...5##..
            ..#.9.....8....76

            The next nine to be vaporized are then:

            .8....###.....#..
            56...9#...#.....#
            34...7...........
            ..2.....X....##..
            ..1..............

            Finally, the laser completes its first full rotation (1 through 3), a second rotation (4 through 8),
            and vaporizes the last asteroid (9) partway through its third rotation:

            ......234.....6..
            ......1...5.....7
            .................
            ........X....89..
            .................

            In the large example above (the one with the best monitoring station location at 11,13):

            -The 1st asteroid to be vaporized is at 11,12.
            -The 2nd asteroid to be vaporized is at 12,1.
            -The 3rd asteroid to be vaporized is at 12,2.
            -The 10th asteroid to be vaporized is at 12,8.
            -The 20th asteroid to be vaporized is at 16,0.
            -The 50th asteroid to be vaporized is at 16,9.
            -The 100th asteroid to be vaporized is at 10,16.
            -The 199th asteroid to be vaporized is at 9,6.
            -The 200th asteroid to be vaporized is at 8,2.
            -The 201st asteroid to be vaporized is at 10,9.
            -The 299th and final asteroid to be vaporized is at 11,1.

            The Elves are placing bets on which will be the 200th asteroid to be vaporized. Win the bet by determining which asteroid that will be; what do you get if you multiply its X coordinate by 100 and then add its Y coordinate? (For example, 8,2 becomes 802.)
            Answer: 
        */

        private static int[,] InstantiateMap()  
        {
            const int height = 20;
            const int width = 20;

            string line;        // Stores a line from the text file
            System.IO.StreamReader file = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day10Input.txt");    // Allows to read the text file
            int[,] map = new int[height, width];

            // Read the file line by line, adding a 0 in empty spaces and a 1 in spaces that have an asteroid
            int row = 0;

            while ((line = file.ReadLine()) != null)
            {
                int column = 0;
                foreach (char c in line)
                {
                    if (c.Equals('.'))
                    {
                        map[column, row] = 0;
                    }
                    else if (c.Equals('#'))
                    {
                        map[column, row] = 1;
                    }
                    column++;
                }
                row++;
            }

            return map;
        }

        private static int NumberOfVisibleAsteroids(int[,] map, int x, int y)   // Returns the number of asteroids that are visible from position [x,y] on the map
        {
            int numVisible = 0;     // Counts the number of asteroids that can be seen

            // Loop through each spot on the grid to compare to the home coordinate [x, y]
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    // Check if this spot is the same as the home coordinate. If so go to the next location
                    if (x == j && y == i)
                    {
                        continue;
                    }

                    // Check if this spot has an asteroid, proceed if it does
                    if (map[j, i] == 1)
                    {
                        bool isBlocked = false;     // Tracks if another asteroid is blocking this one

                        // Get the difference between home and this spot, difference (difference between the two locations)
                        int[] difference = { j - x, i - y };       // e.g. [3, 9] - [0, 0] = [3, 9] or [1, 2] - [5, 0] = [-4, 2]

                        // Find the reduced fraction p/q, "reduced"
                        int[] reduced = Reduce(difference);

                        // Do (p/q*)n from n = 1 until(p/q)*n = difference
                        int n = 1;
                        int[] RTimesN = { reduced[0] * n, reduced[1] * n };     // stores (p/q)*n
                        while (RTimesN[0] != difference[0] || RTimesN[1] != difference[1])
                        {
                            // For each value of n, check if home + (p/q)n has an asteroid. If it does it means there is an asteroid blocking this spot. break the loop and search the next spot.
                            int[] posOnPath = { x + RTimesN[0], y + RTimesN[1] };   // A position that is potentially blocking this spot's asteroid

                            if (map[posOnPath[0], posOnPath[1]] == 1)   // If there is an asteroid at this location, set isBlocked to true and exit the loop
                            {
                                isBlocked = true;
                                break;
                            }

                            n++;
                            RTimesN = new int[] { reduced[0] * n, reduced[1] * n };
                        }

                        if (!isBlocked)
                        {
                            numVisible++;
                        }
                    }
                }
            }

            return numVisible;
        }

        private static int[] Reduce(int[] fraction)   // Reduces a fraction as a size 2 array
        {
            int num = fraction[0];
            int den = fraction[1];
            bool numIsPositive = num > 0 ? true : false;
            bool denIsPositive = den > 0 ? true : false;
            int redNum;
            int redDen;

            // If numerator or denominator = 1, change the other to 1 and keep the sign. This isn't actually reduction but it fits the purpose for this problem.
            if (num == 0)
            {
                if (denIsPositive)
                {
                    den /= den;
                }
                else
                {
                    den /= den * -1;
                }
                int[] simplified = { num, den };
                return simplified;
            }
            else if (den == 0)
            {
                if (numIsPositive)
                {
                    num /= num;
                }
                else
                {
                    num /= num * -1;
                }
                int[] simplified = { num, den };
                return simplified;
            }

            // Track if either value was negative, and make it positive if it is
            if (!numIsPositive)
            {
                num *= -1;
            }
            if (!denIsPositive)
            {
                den *= -1;
            }

            int gcd = GCD(num, den);

            redNum = num / gcd;
            redDen = den / gcd;

            // Turn the numerator and/or denominator back to negative if it was negative earlier
            if (!numIsPositive)
            {
                redNum *= -1;
            }
            if (!denIsPositive)
            {
                redDen *= -1;
            }

            int[] reduced = { redNum, redDen };

            return reduced;
        }

        private static int GCD(int n, int d)    // Returns the greatest common divisor between n and d
        {
            if (n < d)
            {
                int temp = n;
                n = d;
                d = temp;
            }

            while (d > 0)
            {
                int temp = n % d;
                n = d;
                d = temp;
            }

            return n;
        }

        private static void HighestVisibilityAsteroid(int[,] map)
        {
            // Find the highest number of asteroids that can be seen from a single asteroid
            int mostAsteroidsSeen = 0;
            int[] mostAsteroidsPosition = { 0, 0 };
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[j, i] == 1)
                    {
                        int asteroids = NumberOfVisibleAsteroids(map, j, i);
                        if (asteroids > mostAsteroidsSeen)
                        {
                            mostAsteroidsSeen = asteroids;
                            int[] position = { j, i };
                            mostAsteroidsPosition = position;
                        }
                    }
                }
            }

            Console.WriteLine("The highest number of asteroids seen is {0} at position [{1},{2}]", mostAsteroidsSeen, mostAsteroidsPosition[0], mostAsteroidsPosition[1]);
        }

        private static int[] VaporizedAt(int[,] map, int[] laser, int n)   // Returns the location of the nth asteroid to get vaporized
        {
            int[] asteroidPosition = new int[2];

            //TODO vaporize asteroids clockwise

            // Possible idea: compare the asteroid's positions as fractions in order to figure out which ones get reached first.
            // Then on a tie do the same as part 1 to figure out which asteroid gets vaporized first. Maybe save the non-vaporized in a separate list to be looped through once the laser is pointing straight up again

            return asteroidPosition;
        }

        public static void OutputSolution()     // Laser position = [8,16]
        {
            int[,] map = InstantiateMap();
            int[] laser = { 8, 16 };
            const int asteroidBet = 200;

            int[] asteroidPos = VaporizedAt(map, laser, asteroidBet);
            int math = asteroidPos[0] * 100 + asteroidPos[1];

            Console.WriteLine("Arbitrary math solution: {0}", math);
        }
    }
}
