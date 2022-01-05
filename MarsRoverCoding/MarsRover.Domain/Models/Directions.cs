using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Domain.Models
{
    /// <summary>
    /// Directions
    /// </summary>
    public class Directions
    {
        public string FacingDirection { get; set; }
        public string Instruction { get; set; }
        public string NewDirection { get; set; }

        public Directions(string FacingDirection, string Instruction, string NewDirection)
        {
            this.FacingDirection = FacingDirection;
            this.Instruction = Instruction;
            this.NewDirection = NewDirection;
        }
    }
}
