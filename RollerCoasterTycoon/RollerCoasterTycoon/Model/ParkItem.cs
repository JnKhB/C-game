using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using RollerCoasterTycoon.Model.EventArgument;
namespace RollerCoasterTycoon.Model
{
    /// <summary>
    /// ParkItem is the base class of every items which you can put down during the game
    /// 
    /// </summary>
    public enum State { Operate, Wait, Build, None };
    public class ParkItem
    {

        #region Adattagok
        protected int Time;
        protected int UseTime;
        #endregion

        #region Properties
        /// <value>Gets and sets the Position of ParkItem - Upper left corner of the item.</value>
        public AmusementPark.Position Pos { get; set; }
        /// <value>Gets and sets the value of min. capacity when the ParkItem starts round. Value is between 0 and 1</value>
        public virtual double StartMin { get; set; }
        /// <value>Gets and sets the value of Adrenalin factor.</value>
        public virtual Int32 AdrenalinFact { get; protected set; }
        /// <value>Gets and sets the Name of the Park Item.</value>
        public static String Name { get; protected set; }
        /// <value>Gets and sets the Height of the Park Item.</value>
        public static Int32 Height { get; set; }
        /// <value>Gets and sets the Width of the Park Item.</value>
        public static Int32 Width { get; set; }
        /// <value>Gets and sets the Price of the Park Item.</value>
        public static Int32 Price { get; protected set; }
        /// <value>Gets and sets the Building time of the Park Item.</value>
        public Int32 BuildTime { get; protected set; }
        /// <value>Gets and sets the Maintenance cost of the Park Item.</value>
        public Int32 MaintenanceCost { get; set; }
        /// <value>Gets and sets the Capacity time of the Park Item.</value>
        public Int32 Capacity { get; protected set; }
        /// <value>Gets and sets the Time of one round of the Park Item.</value>
        public Int32 TimeOfUse { get; protected set; }
        /// <value>Gets and sets if the Park Item is reachable with sidewalks from the entrance</value>
        public bool Reachable { get; set; }
        /// <value>Gets and sets the waiting line of the Park Item.</value>
        public List<Visitor> Line { get; protected set; }
        /// <value>Gets and sets the current users of the Park Item.</value>
        public List<Visitor> Users { get; protected set; }
        /// <value>Gets and sets the cost of using a Park Item.</value>
        public Int32 CostOfUse { get; set; }
        /// <value>Gets and sets the state of building of the Park Item.</value>
        public State State { get; set; }
        /// <value>Gets and sets the Operation cost of the Park Item.</value>
        public Int32 OperationCost { get; protected set; }
        /// <value>Gets and sets the the mood or satiety value of the Park Item. Users mood or satiety value will change with this value after using a parkitem</value>
        public int MoodOrSatietyValue { get; set; }
        #endregion

        #region Események
        public event EventHandler<EndOfBuildingEventArgs> EndOfBuilding;
        public event EventHandler<NeedToPayEventArgs> NeedToPay;
        #endregion
        public ParkItem()
        {
            Time = 0;
            UseTime = 0;
            Reachable = false;
            State = State.Wait;
            Line = new List<Visitor>();
            Users = new List<Visitor>();
        }
        public virtual Type GetItemType()
        {
            return typeof(ParkItem);
        }
        /// <summary>
        /// Get out given visitor from the line of the Parkitem. 
        /// </summary>
        public bool GetOutFromLine(Visitor visitor)
        {
            for(int i = 0; i < Line.Count; i++)
            {
                if(Line[i] == visitor)
                {
                    Line.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Add visitor in the line of the ParkItem
        /// </summary>
        public void AddInLine(Visitor visitor)
        {
            Line.Add(visitor);
        }
        /// <summary>
        /// This function check if there are enough people in the line of the attraction. If yes, call Visitors pay and start method and send event to the AmusementPark
        /// </summary>
        private bool UseParkItem()
        {
            int numOfPeopleInLine = Line.Count;
            if (numOfPeopleInLine >= Math.Ceiling(StartMin * Capacity) && numOfPeopleInLine > 0)
            {
                if (numOfPeopleInLine <= Capacity)
                {
                    VisitorsPayAndStart(Line, CostOfUse, numOfPeopleInLine);
                    Line.Clear();
                }
                else
                {
                    VisitorsPayAndStart(Line, CostOfUse, Capacity);
                    Line.RemoveRange(0, Capacity);
                }
                On_NeedToPay();
                Time = 0;
                State = State.Operate;
                return true;
            }
            else
            {
                return false;
            }
        }
        protected virtual void EndUsingOfParkItem() { }
        /// <summary>
        /// It takes length amount of people from the beginning of the line of the ParkItem.
        /// Add into Users list, call Pay, and call StartUsingParkItem functions. 
        /// </summary>
        private void VisitorsPayAndStart(List<Visitor> line, int fee, int length)
        {
            for (int i = 0; i < length; i++)
            {
                Users.Add(line[i]);
                line[i].Pay(fee);
                line[i].StartUsingParkItem();
            }
        }
        /// <summary>
        /// If building of a ParkItem is finished it sends EndOfBuilding event args to the View.
        /// </summary>
        public void FinishBuilding()
        {
            if (Time >= BuildTime)
            {
                Time = 0;
                State = State.Wait;
                On_EndOfBuilding();
            }
        }
        /// <summary>
        /// At every tick in the game we increase Time by 1, and check if any round finished, any building finished, or if its time to use a parkitem
        /// </summary>
        public void TimerTicked(object sender, EventArgs eventArgs)
        {
            Time++;
            EndUsingOfParkItem();
            if (State == State.Build)
            {
                FinishBuilding();
            }
            if (State == State.Operate)
            {
                UseTime++;
                EndUsingOfParkItem();
            }
            if(State == State.Wait)
            {
                UseParkItem();
            }
        }
        public void On_EndOfBuilding()
        {
            if (EndOfBuilding != null)
            {
                EndOfBuilding(this, new EndOfBuildingEventArgs(this));
            }
        }
        public void On_NeedToPay()
        {
            if (NeedToPay != null)
            {
                NeedToPay(this, new NeedToPayEventArgs(OperationCost));
            }
        }
    }
}
