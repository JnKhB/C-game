using System;
using System.Collections.Generic;
using System.Text;
using RollerCoasterTycoon.Model.EventArgument;

namespace RollerCoasterTycoon.Model.ParkItems
{
    /// <summary>
    /// Attraction class derived From ParkItem class. 
    /// The class contains all methods which Attractions have.
    /// ParkItem Properties are set in derived class from Attractions
    /// </summary>
    public class Attraction : ParkItem
    {
        
        public override Int32 AdrenalinFact { get; protected set; }
        public override double StartMin { get; set; }
        public Attraction()
        {
            State = State.Build;
        }
        public override Type GetItemType()
        {
            return typeof(Attraction);
        }
        /// <summary>
        /// Add given visitor to the line of Visitors who are waiting in the queue for the attraction 
        /// </summary>
        public void VisitorWantsToUse(Visitor visitor)
        {
            Line.Add(visitor);
        }
        /// <summary>
        /// When using a parkitem finishes all user mood and destination need to change. 
        /// </summary>
        protected override void EndUsingOfParkItem()
        {
            if(UseTime == TimeOfUse)
            {
                for (int i = 0; i < Users.Count; i++)
                {
                    Users[i].ChangeMood(MoodOrSatietyValue);
                    Users[i].ChooseDest();
                }
                Users.Clear();
                UseTime = 0;
                State = State.Wait;
            }
        }

    }
}
