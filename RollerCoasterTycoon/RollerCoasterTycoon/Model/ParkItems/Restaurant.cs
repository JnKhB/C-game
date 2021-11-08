using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.EventArgument;

namespace RollerCoasterTycoon.Model.ParkItems
{
    /// <summary>
    /// Restaurant class derived From ParkItem class. 
    /// The class contains all methods which Restaurants have.
    /// ParkItem Properties are set in derived class from Restaurants
    /// </summary>
    public class Restaurant : ParkItem
    {
        public Restaurant()
        {
            TimeOfUse = 4;
            BuildTime = 0;
        }
        public override Type GetItemType()
        {
            return typeof(Restaurant);
        }
        /// <summary>
        /// Add given visitor to the line of Visitors who are waiting in the queue for the restaurant 
        /// </summary>
        public void VisitorWantsToEat(Visitor visitor)
        {
            Line.Add(visitor);
        }
        /// <summary>
        /// When using a parkitem finishes all user mood and destination need to change. 
        /// </summary>
        protected override void EndUsingOfParkItem()
        {
            if (UseTime == TimeOfUse)
            {
                for (int i = 0; i < Users.Count; i++)
                {
                    Users[i].ChangeSatiety(MoodOrSatietyValue);
                    Users[i].ChooseDest();
                }
                Users.Clear();
                UseTime = 0;
                State = State.Wait;
            }
        }
    }
}
