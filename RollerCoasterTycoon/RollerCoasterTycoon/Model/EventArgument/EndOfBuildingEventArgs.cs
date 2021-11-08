using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.EventArgument
{
   public class EndOfBuildingEventArgs : EventArgs
    {
        public ParkItem Attraction;
        public EndOfBuildingEventArgs(ParkItem Attraction)
        {
            this.Attraction = Attraction; 
        }
    }
}
