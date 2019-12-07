using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /*  --- Day 6: Universal Orbit Map ---
        You've landed at the Universal Orbit Map facility on Mercury.
        Because navigation in space often involves transferring between orbits, the orbit maps here are useful for finding efficient routes between, for example, you and Santa.
        You download a map of the local orbits (your puzzle input).

        Except for the universal Center of Mass (COM), every object in space is in orbit around exactly one other object. An orbit looks roughly like this:

                          \
                           \
                            |
                            |
        AAA--> o            o <--BBB
                            |
                            |
                           /
                          /

        In this diagram, the object BBB is in orbit around AAA. The path that BBB takes around AAA (drawn with lines) is only partly shown.
        In the map data, this orbital relationship is written AAA)BBB, which means "BBB is in orbit around AAA".

        Before you use your map data to plot a course, you need to make sure it wasn't corrupted during the download.
        To verify maps, the Universal Orbit Map facility uses orbit count checksums - the total number of direct orbits (like the one shown above) and indirect orbits.

        Whenever A orbits B and B orbits C, then A indirectly orbits C.
        This chain can be any number of objects long: if A orbits B, B orbits C, and C orbits D, then A indirectly orbits D.

        For example, suppose you have the following map:

        COM)B
        B)C
        C)D
        D)E
        E)F
        B)G
        G)H
        D)I
        E)J
        J)K
        K)L

        Visually, the above map of orbits looks like this:

                G - H       J - K - L
               /           /
        COM - B - C - D - E - F
                       \
                        I

        In this visual representation, when two objects are connected by a line, the one on the right directly orbits the one on the left.

        Here, we can count the total number of orbits as follows:

        D directly orbits C and indirectly orbits B and COM, a total of 3 orbits.
        L directly orbits K and indirectly orbits J, E, D, C, B, and COM, a total of 7 orbits.
        COM orbits nothing.
        The total number of direct and indirect orbits in this example is 42.

        What is the total number of direct and indirect orbits in your map data?
        Answer: 300598
    */

    class Day6
    {
        public static List<OrbitObject> InstantiateList()
        {
            List<OrbitObject> objectList = new List<OrbitObject>();     // List of orbiting objects

            string line;        // Stores a line from the text file
            System.IO.StreamReader file = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day6Input.txt");    // Allows to read the text file

            // Read the file line by line, performing the fuel calculation for each line and getting the sum
            while ((line = file.ReadLine()) != null)
            {
                string[] names = line.Split(')');       // Splits the line into two substrings separated by the ')' delimiter
                string orbitedName = names[0];          // Name of object being orbitted
                string orbitingName = names[1];         // Name of orbitting object

                OrbitObject orbitedObject = objectList.FirstOrDefault(o => o.name == orbitedName);          // Looks for the orbited object in the list
                OrbitObject orbitingObject = objectList.FirstOrDefault(o => o.name == orbitingName);        // Looks for the orbiting object in the list

                if (orbitedObject == null)      // Object being orbited is not in the list, instantiate and add it to the list
                {
                    orbitedObject = new OrbitObject(orbitedName);
                    objectList.Add(orbitedObject);
                }
                else { /* Do nothing, as the orbited object doesn't need to know what it gets orbited by */ }

                if (orbitingObject == null)     // Object that is orbiting is not in the list; instantiate and at it to list with reference to the orbited object
                {
                    orbitingObject = new OrbitObject(orbitingName, orbitedObject);
                    objectList.Add(orbitingObject);
                }
                else
                {
                    // Add reference to the orbited object
                    orbitingObject.orbiting = orbitedObject;
                }
            }

            return objectList;
        }                                 // Creates the list of objects based on the input file

        private static int CalculateOrbitCountOfObject(OrbitObject orbitObject)
        {
            int numOfOrbits = 0;
            OrbitObject currentObject = orbitObject;        // Keeps track of which object is being analyzed. Each loop changes currentObject to be the object that was previously being orbited
            
            while (!currentObject.name.Equals("COM"))
            {
                currentObject = currentObject.orbiting;
                numOfOrbits++;
            }

            return numOfOrbits;
        }           // Calculates the number of direct and indirect orbits of one object

        private static int CalculateOrbitCountOfList(List<OrbitObject> objectList)
        {
            int numOfOrbits = 0;

            foreach (OrbitObject obj in objectList)
            {
                numOfOrbits += Day6.CalculateOrbitCountOfObject(obj);
            }

            return numOfOrbits;
        }        // Calculates the total number of direct and indirect orbits of a list of objects

        public static void OutputSolution()
        {
            List<OrbitObject> objectList;

            objectList = Day6.InstantiateList();
            int numOfOrbits = Day6.CalculateOrbitCountOfList(objectList);

            Console.WriteLine("Total number of orbits is: {0}", numOfOrbits);
        }
    }
}
