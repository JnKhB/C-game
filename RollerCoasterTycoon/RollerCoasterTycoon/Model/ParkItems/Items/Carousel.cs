using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.ParkItems.Items 
{
    /// <summary>
    /// Carousel class derived from Attraction class.
    /// </summary>
    public class Carousel : Attraction
    {
        public static new String Name { get { return "Carousel"; } }
        public static new Int32 Price { get { return 500;  } }
        public static new Int32 Height { get { return 4; } }
        public static new Int32 Width { get { return 4; } }

        public Carousel()
        {
            BuildTime = 25;
            OperationCost = 80;
            AdrenalinFact = 5;
            TimeOfUse = 15;
            StartMin = 0.4;
            MaintenanceCost = 40;
            Capacity = 12;
            MoodOrSatietyValue = 7;
            CostOfUse = 7;
        }

        public override Type GetItemType() 
        {
            return typeof(Carousel);
        }
    }
}
