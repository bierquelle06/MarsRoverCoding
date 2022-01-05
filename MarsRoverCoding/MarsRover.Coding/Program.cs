using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Coding
{
    class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Test Input:");

            var inputArray = new string[] { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM" };

            Console.WriteLine(string.Join("\n", inputArray));

            //The first line of input is the upper-right coordinates of the	plateau, the lower-left	coordinates	are	assumed	to be 0,0.
            var coordinatesOfPlateau = inputArray.ToList()[0];

            inputArray = inputArray.Where(w => w != coordinatesOfPlateau).ToArray();
            //The rest of the input	is information pertaining to the rovers	that have been deployed.

            //Each rover has two lines of input. The first line gives the rover's position, and	the second line is a series of instructions telling the rover how to explore the plateau.
            List<List<string>> eachTwoLines = Logic.Extensions.Utils.ChunkBy<string>(inputArray.ToList(), 2);

            //The position is made up of two integers and a letter separated by spaces,	corresponding to the x and y co-ordinates and the rover's orientation.

            //Each rover will be finished sequentially,	which means	that the second	rover won't	start to move until	the	first one has finished moving.

            Console.WriteLine("");
            Console.WriteLine("Expected	Output:");

            foreach (var perItemLine in eachTwoLines)
            {
                //Add to Start Coordinates Of Plateau
                perItemLine.Insert(0, coordinatesOfPlateau);

                var result = NavigateSurface(perItemLine.ToArray());

                //Show Result
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Navigate Surface
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NavigateSurface(string[] input)
        {
            var _navigationService = new Logic.Services.NavigationService();

            return _navigationService.Navigate(input);
        }


        
    }
}
