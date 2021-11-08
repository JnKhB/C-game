using System;
using System.Collections.Generic;
using System.Text;

namespace RollerCoasterTycoon.Model.ParkItems
{
    /// <summary>
    /// Sidewalk class derived from ParkItem
    /// </summary>
    public class Sidewalk : ParkItem
    {
        public bool IsDirty { get; set; }
        public bool HasBin { get; set; }
        public static new String Name { get { return "SideWalk"; } }
        public static new Int32 Height { get { return 1; } }
        public static new Int32 Width { get { return 1; } }
        public static new Int32 Price { get { return 10; } }
        public static Int32 BinPrice { get { return 20; } }
        public Sidewalk()
        {
            IsDirty = false;
            HasBin = false;
            BuildTime = 0;
        }
        public override Type GetItemType()
        {
            return typeof(Sidewalk);
        }
    }
}
