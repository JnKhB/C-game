using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.ParkItems.Items 
{
    /// <summary>
    /// Langos class derived from Restaurant class.
    /// </summary>
    public class Langos : Restaurant
    {
        public static new String Name { get { return "Langos"; } }
        public static new Int32 Price { get { return 500; } }
        public static new Int32 Height { get { return 4; } }
        public static new Int32 Width { get { return 4; } }

        public Langos()
        {
            MaintenanceCost = 40;
            OperationCost = 70;
            MoodOrSatietyValue = 8;
            CostOfUse = 30;
            Capacity = 5;
            TimeOfUse = 7;
        }

        public override Type GetItemType()
        {
            return typeof(Langos);
        }
    }
}
