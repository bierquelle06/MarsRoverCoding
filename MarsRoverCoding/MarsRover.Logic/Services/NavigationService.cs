using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Domain.Models;
using MarsRover.Models.Domain;

namespace MarsRover.Logic.Services
{
    public class NavigationService
    {
        /// <summary>
        /// Navigate Mars Rover using set of instructions
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public virtual string Navigate(IList<string> instructions)
        {
            var parsedInstructionsTuple = ValidateAndParseInsutrctions(instructions);

            var isValid = parsedInstructionsTuple.Item1;
            if (!isValid)
                return string.Empty;

            var parsedInstructions = parsedInstructionsTuple.Item2;

            var xLength = parsedInstructions.Plateau.XTotalLength;
            var yLength = parsedInstructions.Plateau.YTotalLength;
            var xStartingPosition = parsedInstructions.StartingPosition.XStartingPosition;
            var yStartingPosition = parsedInstructions.StartingPosition.YStartingPosition;
            var facingDirection = parsedInstructions.StartingPosition.DirectionFacing;

            var position = new Position
            {
                XPosition = xStartingPosition,
                YPosition = yStartingPosition,
                FacingDirection = facingDirection
            };

            foreach (var instruction in parsedInstructions.MovementInstructions)
            {
                position = Move(instruction, xLength, yLength, position);
            }

            return $"{position.XPosition} {position.YPosition} {position.FacingDirection}";
        }

        /// <summary>
        /// Lookup Directions
        /// </summary>
        /// <returns></returns>
        protected virtual List<Directions> LookupDirections()
        {
            List<Directions> result = new List<Directions>();

            result.Add(new Directions(FacingDirection: "N", Instruction: "L", NewDirection: "W")); //North, Left, West
            result.Add(new Directions(FacingDirection: "W", Instruction: "L", NewDirection: "S")); //West, Left, South
            result.Add(new Directions(FacingDirection: "S", Instruction: "L", NewDirection: "E")); //South, Left, East
            result.Add(new Directions(FacingDirection: "E", Instruction: "L", NewDirection: "N")); //East, Left, North
            result.Add(new Directions(FacingDirection: "N", Instruction: "R", NewDirection: "E")); //North, Right, East
            result.Add(new Directions(FacingDirection: "E", Instruction: "R", NewDirection: "S")); //East, Rigth, South
            result.Add(new Directions(FacingDirection: "S", Instruction: "R", NewDirection: "W")); //South, Right, West
            result.Add(new Directions(FacingDirection: "W", Instruction: "R", NewDirection: "N")); //West, Rigth, North

            return result;
        }

        /// <summary>
        /// Turn or move Mars Rover based on the current instruction
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="currentPosition"></param>
        /// <returns></returns>
        protected virtual Position Move(char instruction, int xLength, int yLength, Position currentPosition)
        {
            if (instruction != 'M')
            {
                var _facingDirection = currentPosition.FacingDirection.ToCharArray().FirstOrDefault().ToString();
                var _instruction = instruction.ToString();

                var lookupValue = LookupDirections().FirstOrDefault(lookup => lookup.FacingDirection == _facingDirection && lookup.Instruction == _instruction);

                currentPosition.FacingDirection = lookupValue.NewDirection.ToString();

                return currentPosition;
            }

            var movementAxis = "";
            if (currentPosition.FacingDirection == "E" || currentPosition.FacingDirection == "W")
                movementAxis = "X";
            else
                movementAxis = "Y";

            var xPosition = currentPosition.XPosition;
            var yPosition = currentPosition.YPosition;

            if (movementAxis == "X")
            {
                if (currentPosition.FacingDirection == "W")
                    xPosition = currentPosition.XPosition - 1;
                else
                    xPosition = currentPosition.XPosition + 1;
            }
            else
            {
                if (currentPosition.FacingDirection == "S")
                    yPosition = currentPosition.YPosition - 1;
                else
                    yPosition = currentPosition.YPosition + 1;
            }

            return new Position {
                FacingDirection = currentPosition.FacingDirection, 
                XPosition = xPosition, 
                YPosition = yPosition 
            };
        }

        /// <summary>
        /// Helper method to parse and validate Mars Rover inputs.
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns>Boolean based on validation of instructions and tuple of instructions for navigation.</returns>
        protected virtual Tuple<bool, ParsedInstructions> ValidateAndParseInsutrctions(IList<string> instructions)
        {
            if (!instructions.Any() || instructions.Count != 3)
                return new Tuple<bool, ParsedInstructions>(false, null);

            var plateau = instructions[0].Split(' ');
            if(plateau.Length != 2)
                return new Tuple<bool, ParsedInstructions>(false, null);

            var startingPosition = instructions[1].Split(' ');
            var movementInstructions = instructions[2].ToArray();

            var invalidInputsList = new List<bool>
            {
                int.TryParse(plateau[0], out var xTotalLength),
                int.TryParse(plateau[1], out var yTotalLength),
                int.TryParse(startingPosition[0], out var xStartingPosition),
                int.TryParse(startingPosition[1], out var yStartingPosition)
            };

            if (plateau.Length != 2 || startingPosition.Length != 3 || invalidInputsList.Contains(false))
                return new Tuple<bool, ParsedInstructions>(false, null);

            return new Tuple<bool, ParsedInstructions>(true, new ParsedInstructions
            {
                Plateau = (xTotalLength, yTotalLength),
                StartingPosition = (xStartingPosition, yStartingPosition, startingPosition[2].FirstOrDefault().ToString()),
                MovementInstructions = movementInstructions
            });
        }
    }
}
