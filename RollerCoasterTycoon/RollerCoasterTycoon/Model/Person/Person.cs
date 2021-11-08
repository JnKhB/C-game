using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RollerCoasterTycoon.Model.EventArgument;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model
{
    /// <summary>
    /// The Person class is a base class for the Visitor and Janitor classes
    /// It contains methods and properties used in both of those classes.
    /// </summary>
    public class Person
    {

        #region Fields

        protected AmusementPark park;
        protected Stack<AmusementPark.Position> route;
        protected AmusementPark.Position pos;

        #endregion

        #region Properties
        /// <value>
        /// Gets the person's position in the park
        /// </value>
        public AmusementPark.Position Pos
        {
            get { return pos; }
            protected set
            {
                pos = value;
            }
        }
        /// <value>
        /// Gets the ParkItem in the park, which is the person's current destination
        /// </value>
        public ParkItem Destination { get; protected set; }
        /// <value>
        /// Gets the destination's position. This is where the person will stop walking.
        /// </value>
        public AmusementPark.Position DestinationPosition { get; set; }
        /// <value>
        /// Gets the destination's name. 
        /// </value>
        public string DestinationString { get; protected set; }

        #endregion

        #region Public methods
        /// <summary>
        /// Construcor for Person class. Sets the park property and eventhandlers for the person.
        /// </summary>
        /// <param name="park">An AmusementPark object</param>
        public Person(AmusementPark park)
        {
            this.park = park;
            route = new Stack<AmusementPark.Position>();
            DestinationPosition = new AmusementPark.Position(-1, -1);

            park.TimerTicked += new EventHandler(TimerTicked);
        }
        /// <summary>
        /// Virtual method for choosing destination. It does nothing.
        /// </summary>
        public virtual void ChooseDest() { }
        /// <summary>
        /// Virtual method, it removes the eventhandler for the park's TimerTicked event.
        /// </summary>
        public virtual void GoHome()
        {
            park.TimerTicked -= TimerTicked;
        }

        #endregion



        #region Protected methods
        /// <summary>
        /// Calculates the route from Pos to the chosen destination.
        /// </summary>
        protected void CalculateRoute()
        {
            route.Clear();

            if (DestinationPosition == Pos)
            {
                route.Push(DestinationPosition);
                return;
            }

            int n = park.Size;
            AmusementPark.Position[,] parents = new AmusementPark.Position[n, n];
            int[,] dest = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    parents[i, j] = new AmusementPark.Position(-1, -1);
                    dest[i, j] = n * n + 1;
                }
            }
            dest[Pos.X, Pos.Y] = 0;
            Queue<AmusementPark.Position> q = new Queue<AmusementPark.Position>();
            q.Enqueue(Pos);
            AmusementPark.Position pos;

            bool found = false;
            while (q.Count > 0 && !found)
            {
                pos = q.Dequeue();
                int i; 
                int j;

                // megnézzük a körülötte lévő mezőket
                i = pos.X - 1;
                j = pos.Y;
                while (i <= pos.X + 1 && !found)
                {                          // x tengelyen szomszédos
                    if (i >= 0 && i < n && j >= 0 && j < n)
                    {

                        found = IsDestinationFound(i, j) && park.ParkArea[pos.X,pos.Y].GetItemType() == typeof(Sidewalk);

                        if (park.ParkArea[i, j] != null && park.ParkArea[i, j].GetItemType() == typeof(Sidewalk) || found)
                        {

                            if (dest[i, j] == n * n + 1)
                            {
                                dest[i, j] = dest[pos.X, pos.Y] + 1;
                                parents[i, j] = pos;
                                q.Enqueue(new AmusementPark.Position(i, j));
                            }

                        }

                    }
                    i += 2;
                }
                i = pos.X;
                j = pos.Y - 1;
                while (j <= pos.Y + 1 && !found)
                {                          // y tengelyen szomszédos
                    if (i >= 0 && i < n && j >= 0 && j < n)
                    {

                        found = IsDestinationFound(i, j);

                        if (park.ParkArea[i, j] != null && park.ParkArea[i, j].GetItemType() == typeof(Sidewalk) || found)
                        {

                            if (dest[i, j] == n * n + 1)
                            {
                                dest[i, j] = dest[pos.X, pos.Y] + 1;
                                parents[i, j] = pos;
                                q.Enqueue(new AmusementPark.Position(i, j));
                            }

                        }

                    }
                    j += 2;
                }

            }

            route.Push(DestinationPosition);
            pos = DestinationPosition;
            while (parents[pos.X, pos.Y] != Pos)
            {

                pos = parents[pos.X, pos.Y];
                route.Push(pos);
            }
        }
        /// <summary>
        /// Decides if the destination is at the (x,y) position in the park.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>True, if the destination is the (x,y) position in the park. False in every other case.</returns>
        protected bool IsDestinationFound(int x, int y)
        {
            if (Destination == null)
            {
                if (DestinationPosition.X == x && DestinationPosition.Y == y) return true;
                return false;
            }
            int height = (Int32)Destination.GetItemType().GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
            int width = (Int32)Destination.GetItemType().GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);

            for (int i = DestinationPosition.X; i < DestinationPosition.X + width; i++)
            {                  // ha a cél területét elértük, akkor nem kell tovább keresni
                for (int j = DestinationPosition.Y; j < DestinationPosition.Y + height; j++)
                {
                    if (i == x && j == y)
                    {
                        DestinationPosition = new AmusementPark.Position(i, j);                     // nem a bal felső sarkához megy, hanem ahhoz a pozícióhoz, amit találtunk
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Eventhandler for the park's TimerTicked event. Changes the position if needed.
        /// </summary>
        protected virtual void TimerTicked(object sender, EventArgs e)
        {
            MoveTowardDestination();

            ToDoOnTimerTick();
        }
        /// <summary>
        /// Virtual method. In this class, it does nothing.
        /// </summary>
        protected virtual void ToDoOnTimerTick() { }
        /// <summary>
        /// Changes the position in order to move toward the chosen destiantion.
        /// </summary>
        protected void MoveTowardDestination()
        {
            if (ShouldMove())
            {

                Pos = route.Pop();
                if (route.Count == 0)
                {
                    ArriveAtDestination();
                }
            }
        }
        /// <summary>
        /// Decides if the person should move at the moment
        /// </summary>
        protected virtual bool ShouldMove()
        {
            return DestinationPosition != new AmusementPark.Position(-1, -1);
        }
        /// <summary>
        /// Virtual method. In this class, it does nothing.
        /// </summary>
        protected virtual void ArriveAtDestination()
        {

        }

        #endregion
    }
}
