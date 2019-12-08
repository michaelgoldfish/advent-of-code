using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    /*  --- Day 8: Space Image Format ---
 
        The Elves' spirits are lifted when they realize you have an opportunity to reboot one of their Mars rovers, and so they are curious if you would spend a brief sojourn on Mars.
        You land your ship near the rover.

        When you reach the rover, you discover that it's already in the process of rebooting! It's just waiting for someone to enter a BIOS password.
        The Elf responsible for the rover takes a picture of the password (your puzzle input) and sends it to you via the Digital Sending Network.

        Unfortunately, images sent via the Digital Sending Network aren't encoded with any normal encoding; instead, they're encoded in a special Space Image Format.
        None of the Elves seem to remember why this is the case. They send you the instructions to decode it.

        Images are sent as a series of digits that each represent the color of a single pixel.
        The digits fill each row of the image left-to-right, then move downward to the next row, filling rows top-to-bottom until every pixel of the image is filled.

        Each image actually consists of a series of identically-sized layers that are filled in this way.
        So, the first digit corresponds to the top-left pixel of the first layer, the second digit corresponds to the pixel to the right of that on the same layer, and so on until the last digit, which corresponds to the bottom-right pixel of the last layer.

        For example, given an image 3 pixels wide and 2 pixels tall, the image data 123456789012 corresponds to the following image layers:

        Layer 1: 123
                 456

        Layer 2: 789
                 012

        The image you received is 25 pixels wide and 6 pixels tall.

        To make sure the image wasn't corrupted during transmission, the Elves would like you to find the layer that contains the fewest 0 digits.
        On that layer, what is the number of 1 digits multiplied by the number of 2 digits?
        Answer: 2064
    */

    /*  --- Part Two ---
       
        Now you're ready to decode the image. The image is rendered by stacking the layers and aligning the pixels with the same positions in each layer.
        The digits indicate the color of the corresponding pixel: 0 is black, 1 is white, and 2 is transparent.

        The layers are rendered with the first layer in front and the last layer in back.
        So, if a given position has a transparent pixel in the first and second layers, a black pixel in the third layer, and a white pixel in the fourth layer,
        the final image would have a black pixel at that position.

        For example, given an image 2 pixels wide and 2 pixels tall, the image data 0222112222120000 corresponds to the following image layers:

        Layer 1: 02
                 22

        Layer 2: 11
                 22

        Layer 3: 22
                 12

        Layer 4: 00
                 00

        Then, the full image can be found by determining the top visible pixel in each position:

        The top-left pixel is black because the top layer is 0.
        The top-right pixel is white because the top layer is 2 (transparent), but the second layer is 1.
        The bottom-left pixel is white because the top two layers are 2, but the third layer is 1.
        The bottom-right pixel is black because the only visible pixel in that position is 0 (from layer 4).
        So, the final image looks like this:

        01
        10

        What message is produced after decoding your image?
        Answer: KAUZA
    */

    class Day8
    {
        private static int[,] FindLayerWithFewest(List<int[,]> layerList, int n)            // Finds which layer in list has the fewest "n" digits
        {
        if (layerList.Count == 0)
        {
            return new int[0,0];    // Given list has no layers in it; return a useless array to spite them
        }

        // Set starting values to the values of the first layer
        int indexOfFewest = 0;
        int fewestDigits = FindDigitsOnLayer(layerList[indexOfFewest], n);

        for (int i = 1; i < layerList.Count; i++)   // Search for a layer with a smaller amount of n digits
        {
            int digitsFound = FindDigitsOnLayer(layerList[i], n);
            if ( digitsFound < fewestDigits)
            {
                indexOfFewest = i;
                fewestDigits = digitsFound;
            }
        }

        return layerList[indexOfFewest];
        }

        private static List<int[,]> GetLayersFromFile(int width, int height)                // Converts the text input into a list of image layers
        {
        string inputString = new System.IO.StreamReader(@"P:\Michael\Documents\GitHub\advent-of-code\AdventOfCode\AdventOfCode\Day8Input.txt").ReadToEnd();    // Stores text file as a string
        List<int> inputList = new List<int>();                   // Stores the picture data (puzzle input) as a list of integers
        List<int[,]> layerList = new List<int[,]>();            // Stores the different layers of the image. Each layer is stored as a 2-d fixed array

        // Take each element from inputStringArray and store it as an integer in the intcode list
        foreach (char element in inputString)
        {
            inputList.Add((int)Char.GetNumericValue(element));
        }

        int numDigitsPerLayer = width * height;
        int currentIndex = 0;                     // Tracks the index 

        for (int i = 0; i < inputList.Count; i += numDigitsPerLayer)    // Each loop is a new layer
        {

            int[,] imageLayer = new int[height,width];      // Instantiate a new layer

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    imageLayer[h,w] = inputList[currentIndex];
                    currentIndex++;
                }
            }

            layerList.Add(imageLayer);                      // Add layer to the list
        }

        return layerList;
        }

        private static int MultiplyDigitsOnLayer(int[,] layer, int i, int j)                // Multiplies the number of "i" digits by the number of "j" digits found on the layer
        {
        int numOfI = FindDigitsOnLayer(layer, i);
        int numOfJ = FindDigitsOnLayer(layer, j);

        return numOfI * numOfJ;
        }

        private static int FindDigitsOnLayer(int[,] layer, int n)                           // Finds the number of times a specific digit appears in the layer
        {
        // Return the number of digits "n" in the layer
        int height = layer.GetLength(0);    // # of rows
        int width = layer.GetLength(1);     // # of columns

        int numOfDigits = 0;                // Track the number of times digit n is found

        for (int h = 0; h < layer.GetLength(0); h++)
        {
            for (int w = 0; w < layer.GetLength(1); w++)
            {
                if (layer[h,w] == n)
                {
                    numOfDigits++;
                }
            }
        }

        return numOfDigits;
        }

        private static void PrintImage(List<int[,]> layersList)                             // Prints the image into the console
        {
            int height = layersList[0].GetLength(0);
            int width = layersList[0].GetLength(1);
            int[,] finalImage = layersList[0];

            // Put the layers together, with the top opaque layer being visible
            foreach (int[,] layer in layersList)
            {
                for (int h = 0; h < height; h++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        // Check if final image is transparent on this pixel
                        if (finalImage[h, w] == 2 && layer[h, w] != 2)
                        {
                            finalImage[h, w] = layer[h, w];
                        }
                    }
                }
            }

            // Print the image to the console, line by line
            for (int h = 0; h < height; h++)
            {
                string line = "";
                for (int w = 0; w < width; w++)
                {
                    line += finalImage[h, w];
                }
                Console.WriteLine(line);
            }
        }

        public static void OutputSolution()
        {       
            const int width = 25;
            const int height = 6;
            List<int[,]> layers = GetLayersFromFile(width, height);

            // Part 2 solution
            PrintImage(layers);

            // Part 1 solution
            int[,] layerWithFewest = FindLayerWithFewest(layers, 0);
            int output = MultiplyDigitsOnLayer(layerWithFewest, 1, 2);
            Console.WriteLine("\nMultiplication equals: {0}", output);
        }
    }
}
