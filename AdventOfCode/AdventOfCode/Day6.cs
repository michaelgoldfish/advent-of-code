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

    /*  --- Part Two ---
       
        Now, you just need to figure out how many orbital transfers you (YOU) need to take to get to Santa (SAN).

        You start at the object YOU are orbiting; your destination is the object SAN is orbiting.
        An orbital transfer lets you move from any object to an object orbiting or orbited by that object.

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
        K)YOU
        I)SAN
        Visually, the above map of orbits looks like this:

                                  YOU
                                 /
                G - H       J - K - L
               /           /
        COM - B - C - D - E - F
                       \
                        I - SAN

        In this example, YOU are in orbit around K, and SAN is in orbit around I. To move from K to I, a minimum of 4 orbital transfers are required:

        K to J
        J to E
        E to D
        D to I
        Afterward, the map of orbits looks like this:

                G - H       J - K - L
               /           /
        COM - B - C - D - E - F
                       \
                        I - SAN
                         \
                          YOU

        What is the minimum number of orbital transfers required to move from the object YOU are orbiting to the object SAN is orbiting?
        (Between the objects they are orbiting - not between YOU and SAN.)
        Answer: 520
    */

    class Day6
    {   
        public static List<OrbitObject> InstantiateList()                                                           // Creates the list of objects based on the input file
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

                orbitedObject.orbitedBy.Add(orbitingObject);    // Orbited object knows that it's being orbited by the other object
            }

            return objectList;
        }                                 

        private static int OrbitalTransfersBetweenObjects(string startName, string endName, List<OrbitObject> objectList)   // Returns the number of orbital transfers between the objects startName and endName are orbiting
        {
            OrbitObject startObject = objectList.FirstOrDefault(o => o.name == startName).orbiting;     // Search starts from the object that startName (YOU) is orbiting
            if (startObject == null)
            {
                Console.WriteLine("Object \"{0}\" not found.", startName);
                return 0;
            }

            // Step 1: Search towards children to see if SAN is indirectly orbiting
            int[] searchOutcome = SearchObjectDownward(startObject, endName, 0);            // 0 is the current stepcount which = 0 at the start

            if (searchOutcome[1] == 1)      // endName was found, return # of transfers
            {
                return searchOutcome[0];
            }

            // Step 2: SAN was not found among the children. Go backwards until a parent is being orbited by multiple objects, then search those paths the same way.
            OrbitObject currentObject = startObject;
            int numOfTransfers = 0;
            do
            {
                numOfTransfers++;                           // Went from the current object to its parent object, so increase count by one
                string previousName = currentObject.name;   // Store name of child object to avoid searching the same branch of nodes again
                currentObject = currentObject.orbiting;     // Sets current object (child) to the parent object; the object that was being orbited

                if (currentObject.orbitedBy.Count > 1)      // The parent object has more than 1 object orbiting it; a new path to search through
                {
                    numOfTransfers++;
                    foreach (OrbitObject childObj in currentObject.orbitedBy)
                    {
                        if (childObj.name != previousName)
                        {
                            searchOutcome = SearchObjectDownward(childObj, endName, numOfTransfers);
                        }
                    }
                    if (searchOutcome[1] == 0)
                    {
                        numOfTransfers--;       // Remove the extra step added by the foreach above
                    }
                }
            }
            while (searchOutcome[1] == 0 && currentObject.name != "COM");   // Loop until endName is found (transfers[1] == 1), or until the universal Center of Mass is reached

            return searchOutcome[0];
        }

        private static int[] SearchObjectDownward(OrbitObject startObject, string endName, int currentTransfers)    // Searches for a specific object through its child objects (objects that are orbiting it)
        {
            int[] searchOutcome = { currentTransfers, 0 };         // This variable is used to track the number of transfers as well as whether endName was actually found or not.
                                                    // First value: # of transfers.         Second Value: 0 = endName not found, 1 = endName found

            // Check if startObject is the object endName is orbiting
            if (startObject.orbitedBy.Any(o => o.name == endName))
            {
                searchOutcome[1] = 1;
                return searchOutcome;
            }

            // Search each child to see if SAN is indirectly orbiting
            searchOutcome[0]++;

            foreach (OrbitObject childObj in startObject.orbitedBy)
            {
                if (childObj.orbitedBy.Count == 0)     // obj is not orbited by any objects, path ends
                {
                    // Stop forward search of obj
                }
                else    // obj is being orbited, search those orbits as well
                {
                    foreach (OrbitObject childOfChildObj in childObj.orbitedBy)
                    {
                        int[] childSearchOutcome = SearchObjectDownward(childObj, endName, searchOutcome[0]);

                        if (childSearchOutcome[1] == 1)
                        {
                            int[] finalOutcome = { childSearchOutcome[0], 1 };
                            return finalOutcome;
                        }
                    }
                }
            }
            // All outcomes led to a dead end
            return searchOutcome;
        }

        private static int CalculateOrbitCountOfObject(OrbitObject orbitObject)                                     // Calculates the number of direct and indirect orbits of one object
        {
            int numOfOrbits = 0;
            OrbitObject currentObject = orbitObject;        // Keeps track of which object is being analyzed. Each loop changes currentObject to be the object that was previously being orbited
            
            while (!currentObject.name.Equals("COM"))
            {
                currentObject = currentObject.orbiting;
                numOfOrbits++;
            }

            return numOfOrbits;
        }           

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
            string startName = "YOU";
            string endName = "SAN";

            objectList = Day6.InstantiateList();

            //OrbitObject testObject = objectList.FirstOrDefault(o => o.name == startName);
            //int[] LtoE = Day6.SearchObjectDownward(testObject, endName, 0);

            //Console.WriteLine("Minimum between {0} and the object {1} is orbiting is: {2}", startName, endName, LtoE[0]);

            int numOfTransfers = Day6.OrbitalTransfersBetweenObjects(startName, endName, objectList);

            Console.WriteLine("The number of orbital transfers between:\n\n- The object {0} is orbiting\n- The object {1} is orbiting\n\nEquals: {2}", startName, endName, numOfTransfers);
        }
    }
}
