using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.ParkItems.Items
{
    /// <summary>
    /// Sweets class derived from Restaurant class.
    /// </summary>
    public class Sweets : Restaurant
    {
        public static new String Name { get { return "Sweets"; } }
        public static new Int32 Price { get { return 600; } }
        public static new Int32 Height { get { return 3; } }
        public static new Int32 Width { get { return 3; } }

        public Sweets()
        {
            MaintenanceCost = 20;
            OperationCost = 10;
            MoodOrSatietyValue = 4;
            CostOfUse = 4;
            Capacity = 4;
            TimeOfUse = 5;
        }

        public override Type GetItemType() 
        {
            return typeof(Sweets);
        }

    }
}
