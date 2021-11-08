using System;
using System.Collections.Generic;
using System.Text;

namespace RollerCoasterTycoon.Model.ParkItems
{
    public enum PlantType { Grass, Bush, Tree, None };
    /// <summary>
    /// Plant class derived from ParkItem.  
    /// </summary>
    public class Plant : ParkItem
    {
        public Int32 MoodChanger { get; protected set; }
        public PlantType PlantType { get; set; }
        public static new Int32 Height { get { return 2; } }
        public static new Int32 Width { get { return 2; } }
        public static new Int32 Price { get { return 50; } }
        public Plant()
        {
            PlantType = PlantType.None;
            BuildTime = 0;
            MoodChanger = 5;
        }
        public override Type GetItemType()
        {
            return typeof(Plant);
        }
    }
}
