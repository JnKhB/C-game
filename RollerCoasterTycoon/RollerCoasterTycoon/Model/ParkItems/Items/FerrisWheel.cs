using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.ParkItems.Items 
{
    /// <summary>
    /// FerrisWheel class derived from Attraction class.
    /// </summary>
    public class FerrisWheel : Attraction
    {
        public static new String Name { get { return "FerrisWheel"; } }
        public static new Int32 Price { get { return 600; } }
        public static new Int32 Height { get { return 6; } }
        public static new Int32 Width { get { return 6; } }

        public FerrisWheel()
        {
            BuildTime = 20;
            OperationCost = 20;
            AdrenalinFact = 4;
            StartMin = 0.23;
            TimeOfUse = 12;
            MaintenanceCost = 50;
            Capacity = 20;
            MoodOrSatietyValue = 12;
            CostOfUse = 20;
        }

        public override Type GetItemType() 
        {
            return typeof(FerrisWheel);
        }
    }
}
