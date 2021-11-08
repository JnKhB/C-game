using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.ParkItems.Items
{
    /// <summary>
    /// Buffet class derived from Resturant class.
    /// </summary>
    public class Buffet : Restaurant
    {
        public static new String Name { get { return "Buffet"; } }
        public static new Int32 Price { get { return 300; } }
        public static new Int32 Height { get { return 3; } }
        public static new Int32 Width { get { return 3; } }

        public Buffet()
        {
            MaintenanceCost = 20;
            TimeOfUse = 4;
            OperationCost = 50;
            CostOfUse = 8;
            Capacity = 3;
            MoodOrSatietyValue = 5;
        }

        public override Type GetItemType()
        {
            return typeof(Buffet);
        }
    }
}
