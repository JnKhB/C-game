using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RollerCoasterTycoon.Model;
using RollerCoasterTycoon.Model.ParkItems;

namespace RollerCoasterTycoon.Model
{
    /// <summary>
    /// The Visitor class is derived from the Person class. AmusementPark objects create and use them.
    /// </summary>
    public class Visitor : Person
    {
        /// <summary>
        /// Visitors have three states. 
        /// Walking, when they are moving towards a destination.
        /// Waiting, when they have arrived at the chosen destination (a ParkItem) and are waiting in its line.
        /// UsingParkItem, when they are in the ParkItems Users list.
        /// </summary>
        private enum State { Walking, Waiting, UsingParkItem };

        #region Events

        #endregion

        #region Fields

        private int money;
        private int maxToPayAtOnce;
        private int satiety;
        private int mood;
        private string lineTo;
        private State visitorState;
        private int adrenalinePrefenece;

        #endregion


        #region Properties    

        /// <value>
        /// Gets a visitor's satiety, which is one of the two factors which describe the visitor's wellbeing.
        /// It is between 0 and MaxSatiety. If this value reaches 0, the visitor leaves the park.
        /// </value>
        public int Satiety
        {                         // jóllakottság
            get { return satiety; }
            private set
            {
                if (value > MaxSatiety)
                    satiety = MaxSatiety;
                else
                    satiety = value;
                if (Satiety <= 0)
                {
                    satiety = 0;
                    WantToGoHome();
                }
            }
        }


        /// <value>
        /// Gets a visitor's mood, which is one of the two factors which describe the visitor's wellbeing.
        /// It is between 0 and MaxMood. If this value reaches 0, the visitor leaves the park.
        /// </value>
        public int Mood
        {                       // hangulat
            get { return mood; }
            private set
            {
                if (value > MaxMood)
                    mood = MaxMood;
                else
                    mood = value;
                if (mood <= 0)
                {
                    mood = 0;
                    WantToGoHome();
                }
            }
        }
        /// <value>
        /// Gets the maximum value for mood.
        /// </value>
        public int MaxMood { get; private set; }
        /// <value>
        /// Gets the maximum value for satiety.
        /// </value>
        public int MaxSatiety { get; private set; }
        /// <value>
        /// Gets the visitor's money. 
        /// </value>
        public int Money
        {
            get { return money; }
            private set
            {
                money = value;
            }
        }

        /// <value>
        /// Gets the visitor's adrenalin preference. 
        /// </value>
        public int AdrenalinPreference
        {
            get { return adrenalinePrefenece; }
            private set { adrenalinePrefenece = value; }
        }

        /// <value>
        /// It's true, if the visitor has trash, and false if not.
        /// </value>
        public bool HasTrash { get; private set; }
        /// <value>
        /// Gets the time in which the visitor will throw trash in the park.
        /// </value>
        public int ThrowTrashTime { get; private set; }


        #endregion

        #region Public methods
        /// <summary>
        /// Constructor for Visitor class.
        /// Sets the values for MaxMood, MaxSatiety. And sets Satiety and Mood the same as their max values.
        /// Sets the position to the park's entrance.
        /// Sets the adrenalin preference randomly between 2 and 10, and sets the money randomly between 10 and 200
        /// </summary>
        /// <param name="park"></param>
        public Visitor(AmusementPark park) : base(park)
        {
            MaxMood = 100;
            MaxSatiety = 100;
            Satiety = MaxSatiety;
            Mood = MaxMood;

            Pos = new AmusementPark.Position(park.Entrance.X, park.Entrance.Y);

            var rand = new Random();
            Money = rand.Next(1, 20) * 10;                                             // 10 és 200 közötti pénzzel indul
            maxToPayAtOnce = rand.Next(10, Money) ;                                    // egyszerre max ennyi pénzt hajlandó kifizetni

            HasTrash = false;

            AdrenalinPreference = rand.Next(2, 10);

            visitorState = State.Waiting;
        }

        /// <summary>
        /// Chooses the next destination. 
        /// If the Satiety is lower than the Mood a Resturant, otherwise an Attraction. If a ParkItem is not found, the next destination will be the exit.
        /// </summary>
        public override void ChooseDest()
        {

            // ha csökkennek az értékei hazamegy
            if (Mood <= 0 || Satiety <= 0 || Money <= 0)
            {
                visitorState = State.Walking;
                DestinationString = "-> Exit";
                Destination = null;
                DestinationPosition = park.Entrance;
                lineTo = "";

            }
            else
            {

                if (lineTo == "restaurant")
                {              // ha étteremből jön ki, lehet, hogy van nála szemét
                    Random rand = new Random();
                    HasTrash = rand.NextDouble() > 0.5;
                    if (HasTrash)
                    {
                        ThrowTrashTime = rand.Next(5, 20);              // 5 és 20 mp között fogja valamikor eldobni / kidobni a szemetet
                    }
                }


                if (Satiety < Mood)
                {
                    Destination = park.ChooseRestaurant(this);
                    lineTo = "restaurant";
                }
                else
                {
                    Destination = park.ChooseAttraction(this);
                    lineTo = "attraction";
                }
                if (Destination == null)
                {
                    DestinationPosition = park.Entrance;
                    DestinationString = "-> Exit";
                }
                else
                {
                    if ((DestinationPosition.X == -1 || DestinationPosition.Y == -1) ||
                        park.ParkArea[DestinationPosition.X, DestinationPosition.Y] != Destination)
                    {                       // ha megint ugyanoda megy, ezeken nem módosítunk
                        DestinationPosition = Destination.Pos;
                    }
                    string name = (string)Destination.GetItemType().GetProperty("Name", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                    DestinationString = "-> " + name;
                }
            }

            visitorState = State.Walking;
            CalculateRoute();
        }

        /// <summary>
        /// If the exit is reached as a destination, the visitors leaves the park.
        /// </summary>
        public override void GoHome()
        {
            park.VisitorLeaves(this);
            base.GoHome();
        }
        /// <summary>
        /// Decides and returns if the wisitor is willing to pay m amount of money.
        /// </summary>
        /// <param name="m">The amount the visitor needs to pay</param>
        /// <returns>The visitors willingness to pay.</returns>
        public bool WillingToPay(int m)
        {
            if (m > Money) return false;                                // nem tudja kifizetni
            if (m < maxToPayAtOnce) return true;                    // még nem haladja meg azt a határt, amin belül fizet
            Random rand = new Random();
            double willingness = maxToPayAtOnce / (m * 1.5);
            if (rand.NextDouble() < willingness) return true;           // több, mint amit ki szeret fizetni, de most épp hajlandó
            return false;                                               // most meg nem
        }
        /// <summary>
        /// Calculates if the visitor wants to use a ParkItem. It depends on its cost and adrenaline factor 
        /// </summary>
        /// <param name="adr_fact">An integer value between 0 and 10</param>
        /// <param name="cost">A non-negative integer.</param>
        /// <returns>The visitors decision to use the parkitem.</returns>
        public bool WantsToUse(int adr_fact, int cost)
        {
            int adr_diff = Math.Abs(adr_fact - AdrenalinPreference);

            Random rand = new Random();
            bool wouldUse = (double)adr_diff / 10.0 < rand.NextDouble();        // az adrenalin érték alapján hajlandó-e felülni?
            if (!wouldUse) return false;

            int tmp = maxToPayAtOnce;
            maxToPayAtOnce += (int)((maxToPayAtOnce / 2) * 1.0 / adr_diff);      // minél közelebb van a pref. adr., annál többet hajlandó fizetni
            wouldUse = WillingToPay(cost);
            maxToPayAtOnce = tmp;
            return wouldUse;
        }
        /// <summary>
        /// The visitor pays. The money is reduced by the given amount.
        /// </summary>
        /// <param name="f">A non-negative integer. The amount the visitor has to pay.</param>
        public void Pay(int f)
        {
            Money -= f;
            park.VisitorPays(f);
        }
        /// <summary>
        /// Changes the mood by m.
        /// </summary>
        /// <param name="m">An integer number.</param>
        public void ChangeMood(int m)
        {
            Mood += m;
        }
        /// <summary>
        /// Changes the satiety by m.
        /// </summary>
        /// <param name="m">An integer number.</param>
        public void ChangeSatiety(int m)
        {
            Satiety += m;
        }
        /// <summary>
        /// Changes the visitor's state to UsingParkItem.
        /// </summary>
        public void StartUsingParkItem()
        {
            visitorState = State.UsingParkItem;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// If the visitor is walking or waiting in line, its destination will be changed to the exit.
        /// </summary>
        private void WantToGoHome()
        {
            if (visitorState != State.UsingParkItem)
            {

                if (visitorState == State.Waiting)
                {
                    Destination.GetOutFromLine(this);   // ki kell állnia a sorból
                    visitorState = State.Walking;

                }
                if (visitorState == State.Walking && DestinationPosition != park.Entrance)
                {
                    DestinationPosition = park.Entrance;
                    DestinationString = "-> Exit";
                    Destination = null;
                }
                CalculateRoute();

            }

        }

        /// <summary>
        /// Reduces the ThrowTrashTime. If it reaches 0, the visitor will throw the trash on the sidewalk.
        /// If the visitor finds a bin, HasTrash will be changed to false.
        /// </summary>
        private void ThrowTrash()
        {
            ThrowTrashTime--;

            if (HasTrash)
            {
                if (park.CloseBin(Pos))
                {
                    HasTrash = false;
                }
                else if (ThrowTrashTime <= 0 && park.ParkArea[Pos.X, Pos.Y] != null && park.GetItemType(Pos.X, Pos.Y) == typeof(Sidewalk))
                {
                    park.TrashOnSidewalk(Pos);
                    HasTrash = false;
                }
            }
        }

        #endregion

        #region Protected methods
        /// <summary>
        /// The visitor should move, if the state is Walking.
        /// </summary>
        protected override bool ShouldMove()
        {
            return visitorState == State.Walking;
        }
        /// <summary>
        /// ArriveAtDestination is called when the visitor reaches the end of its route.
        /// If the destination was the exit, the visitor leaves the park. It not, the visitor starts using the ParkItem.
        /// </summary>
        protected override void ArriveAtDestination()
        {
            if (DestinationPosition == park.Entrance)
            {
                GoHome();
                return;
            }

            if (Destination.GetItemType().IsSubclassOf(typeof(Restaurant)))
            {
                (Destination as Restaurant).VisitorWantsToEat(this);
            }
            else if (Destination.GetItemType().IsSubclassOf(typeof(Attraction)))
            {
                (Destination as Attraction).VisitorWantsToUse(this);
            }
            DestinationString = DestinationString[3..];
            visitorState = State.Waiting;
        }

        /// <summary>
        /// After every tick of the time, the Satiety and Mood values change.
        /// The ThrowTrash method is called, if the visitor's state is Walking.
        /// </summary>
        protected override void ToDoOnTimerTick()
        {
            int moodModifier = -1;
            if (Satiety <= MaxSatiety / 2)                     // ha nagyon éhes, jobban romlik a hangulata
                moodModifier *= 2;
            if (visitorState == State.Waiting)                 // ha sorban áll, jobban romlik a hangulata
                moodModifier *= 2;
            if (visitorState == State.UsingParkItem && lineTo == "attraction")        // ha játékon van, akkor nem romlik a hangulata           
                moodModifier = 0;
            ChangeMood(moodModifier);

            int satietyModifier = -1;
            if (visitorState == State.UsingParkItem && lineTo == "restaurant")
                satietyModifier = 0;
            ChangeSatiety(satietyModifier);

            if (visitorState == State.Walking) ThrowTrash();

        }

        #endregion

        #region EventHandlers



        #endregion
    }
}
