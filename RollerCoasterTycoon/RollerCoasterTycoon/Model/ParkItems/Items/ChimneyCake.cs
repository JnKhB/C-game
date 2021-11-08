using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model.ParkItems.Items 
{
    /// <summary>
    /// ChimneyCake class derived from Resturant class.
    /// </summary>
    public class ChimneyCake : Restaurant
    {
        public static new String Name { get { return "Chimney Cake"; } }
        public static new Int32 Price { get { return 400; } }
        public static new Int32 Height { get { return 2; } }
        public static new Int32 Width { get { return 2; } }

        public ChimneyCake()
        {
            MaintenanceCost = 30;
            OperationCost = 80;
            MoodOrSatietyValue = 3;
            CostOfUse = 23;
            Capacity = 2;
            TimeOfUse = 6;
        }

        public override Type GetItemType() 
        {
            return typeof(ChimneyCake);
        }
    }
}
