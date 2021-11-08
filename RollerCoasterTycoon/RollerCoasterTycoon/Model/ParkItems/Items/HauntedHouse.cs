using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.ParkItems.Items 
{
    /// <summary>
    /// HauntedHouse class derived from Attraction class.
    /// </summary>
    public class HauntedHouse : Attraction
    {
        public static new String Name { get { return "HauntedHouse"; } }
        public static new Int32 Price { get { return 900; } }
        public static new Int32 Height { get { return 5; } }
        public static new Int32 Width { get { return 5; } }

        public HauntedHouse()
        {
            BuildTime = 23;
            OperationCost = 10;
            AdrenalinFact = 10;
            TimeOfUse = 17;
            StartMin = 0.3;
            MaintenanceCost = 80;
            Capacity = 10;
            MoodOrSatietyValue = 15;
            CostOfUse = 8;
        }

        public override Type GetItemType() 
        {
            return typeof(HauntedHouse);
        }
    }
}
