using System;
using System.Collections.Generic;
using System.Text;

namespace RollerCoasterTycoon.Model
{
    /// <summary>
    /// The Janitor class is derived from the Person class. AmusementPark objects create Janitor objects.
    /// </summary>
    public class Janitor : Person
    {

        #region Properties
        /// <value>
        /// Gets the Wage of a Janitor.
        /// </value>
        public static int Wage { get { return 100; } }
        /// <value>
        /// Gets the state of the Janitor. It can be "Working" or "Fired".
        /// </value>
        public string State { get; set; }
        /// <value>
        /// It is true, if the Janitor was stopped for move and is not changing positions.
        /// </value>
        public bool MidMove { get; private set; }

        #endregion

        #region Public methods
        /// <summary>
        /// Constructor for Janitor class.
        /// </summary>
        /// <param name="park">An AmusementPark object.</param>
        public Janitor(AmusementPark park) : base(park)
        {
            Pos = new AmusementPark.Position(park.Entrance.X, park.Entrance.Y);
            State = "Working";
            MidMove = false;
        }
        /// <summary>
        /// Change the janitor's position to newPos. Choose a new destination, then it starts moving again.
        /// </summary>
        /// <param name="newPos">A Position</param>
        public void Move(AmusementPark.Position newPos)
        {
            Pos = newPos;
            ChooseDest();
            Start();
        }
        /// <summary>
        /// Sets MidMove to true.
        /// </summary>
        public void Stop()
        {
            MidMove = true;
        }
        /// <summary>
        /// Sets MidMove to false.
        /// </summary>
        public void Start()
        {
            MidMove = false;
        }

        /// <summary>
        /// Choose the next destination. If there is reachable dirty sidewalk in the park, the next destination is that, or a random step.
        /// </summary>
        public override void ChooseDest()
        {
            route.Clear();
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
                {               // x tengely
                    if (i >= 0 && i < n && j >= 0 && j < n && (park.ParkArea[i, j] != null && park.ParkArea[i, j].GetItemType() == typeof(ParkItems.Sidewalk)))
                    {

                        found = (park.ParkArea[i, j] as ParkItems.Sidewalk).IsDirty;
                        DestinationPosition = new AmusementPark.Position(i, j);

                        if (dest[i, j] == n * n + 1)
                        {
                            dest[i, j] = dest[pos.X, pos.Y] + 1;
                            parents[i, j] = pos;
                            q.Enqueue(new AmusementPark.Position(i, j));
                        }

                    }
                    i += 2;
                }

                i = pos.X;
                j = pos.Y - 1;
                while (j <= pos.Y + 1 && !found)
                {               // y tengely
                    if (i >= 0 && i < n && j >= 0 && j < n && (park.ParkArea[i, j] != null && park.ParkArea[i, j].GetItemType() == typeof(ParkItems.Sidewalk)))
                    {

                        found = (park.ParkArea[i, j] as ParkItems.Sidewalk).IsDirty;
                        DestinationPosition = new AmusementPark.Position(i, j);

                        if (dest[i, j] == n * n + 1)
                        {
                            dest[i, j] = dest[pos.X, pos.Y] + 1;
                            parents[i, j] = pos;
                            q.Enqueue(new AmusementPark.Position(i, j));
                        }

                    }
                    j += 2;
                }

            }
            if (!found)
            {                                                // ha nics koszos útszakasz random lépeget vagy egy helyben áll
                Random rand = new Random();
                List<AmusementPark.Position> positions = new List<AmusementPark.Position>();
                int j;
                int i;

                j = 0;
                for (i = -1; i <= 1; i += 2) positions.Add(new AmusementPark.Position(Pos.X + i, Pos.Y + j));
                i = 0;
                for (j = -1; j <= 1; j += 2) positions.Add(new AmusementPark.Position(Pos.X + i, Pos.Y + j));
                positions.Add(new AmusementPark.Position(Pos.X, Pos.Y));

                int r = rand.Next(positions.Count);
                AmusementPark.Position possible_pos = positions[r];
                positions.RemoveAt(r);
                while (positions.Count > 0
                    && (possible_pos.X < 0 || possible_pos.X >= n || possible_pos.Y < 0 || possible_pos.Y >= n
                    || park.ParkArea[possible_pos.X, possible_pos.Y] == null
                    || park.GetItemType(possible_pos.X, possible_pos.Y) != typeof(ParkItems.Sidewalk)
                    || (possible_pos == park.Entrance && positions.Count > 0)))
                {
                    r = rand.Next(positions.Count);
                    possible_pos = positions[r];
                    positions.RemoveAt(r);
                }
                if (possible_pos.X < 0 || possible_pos.X >= n || possible_pos.Y < 0 || possible_pos.Y >= n || park.ParkArea[possible_pos.X, possible_pos.Y] == null
                    || park.GetItemType(possible_pos.X, possible_pos.Y) != typeof(ParkItems.Sidewalk) || (possible_pos == park.Entrance && positions.Count > 0))
                {
                    DestinationPosition = Pos;
                }
                else
                    DestinationPosition = possible_pos;
            }

            route.Push(DestinationPosition);
            if (DestinationPosition != Pos)
            {
                pos = DestinationPosition;
                while (parents[pos.X, pos.Y] != Pos)
                {
                    pos = parents[pos.X, pos.Y];
                    route.Push(pos);
                }
            }
        }
        /// <summary>
        /// Sets the state to "Fired" and the next destination to the exit.
        /// </summary>
        public void StopWorking()
        {
            State = "Fired";
            DestinationPosition = park.Entrance;
            CalculateRoute();
        }
        /// <summary>
        /// The janitor leaves the park.
        /// </summary>
        public override void GoHome()
        {
            park.JanitorFired(this);
            base.GoHome();
        }

        #endregion

        #region Protected methods
        protected override void ToDoOnTimerTick()
        {

        }
        /// <summary>
        /// Returns if the janitor can make a step. It calls the same method from the base class and checks MidMove.
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldMove()
        {
            return base.ShouldMove() && !MidMove;
        }
        /// <summary>
        /// If the janitor steps on the entrance and its state is "Fired", it leaves the park.
        /// If not, the janitor cleans.
        /// </summary>
        protected override void ArriveAtDestination()
        {
            if (DestinationPosition == park.Entrance && State == "Fired")
            {
                GoHome();
                return;
            }

            if (park.ParkArea[DestinationPosition.X, DestinationPosition.Y] != null && (park.ParkArea[DestinationPosition.X, DestinationPosition.Y] as ParkItems.Sidewalk).IsDirty)
            {
                park.CleanSideWalk(DestinationPosition);
            }
            ChooseDest();
        }



        #endregion

    }
}
