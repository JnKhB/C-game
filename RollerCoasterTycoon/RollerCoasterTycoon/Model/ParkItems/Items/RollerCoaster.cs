using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.ParkItems.Items
{
    /// <summary>
    /// RollerCoaster class derived from Attraction class.
    /// </summary>
    public class RollerCoaster : Attraction
    {
        public static new String Name { get { return "RollerCoaster"; } }
        public static new Int32 Price { get { return 1000; } }
        public static new Int32 Height { get { return 7; } }
        public static new Int32 Width { get { return 7; } }

        public RollerCoaster()
        {
            BuildTime = 15;
            OperationCost = 20;
            AdrenalinFact = 5;
            TimeOfUse = 18;
            StartMin = 0.34;
            MaintenanceCost = 90;
            Capacity = 25;
            MoodOrSatietyValue = 18;
            CostOfUse = 32;
        }

        public override Type GetItemType()
        {
            return typeof(RollerCoaster);
        }
    }
}
