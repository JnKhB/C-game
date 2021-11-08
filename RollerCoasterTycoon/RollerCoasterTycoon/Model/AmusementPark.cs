using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RollerCoasterTycoon.Model.EventArgument;
using RollerCoasterTycoon.Model.ParkItems;
using RollerCoasterTycoon.Model.ParkItems.Items;

namespace RollerCoasterTycoon.Model
{
    /// <summary>
    /// The AmusementPark class contains and uses ParkItem, Visitor and Janitor objects.
    /// The game is working through this class.
    /// </summary>
    public partial class AmusementPark
    {

        #region Events

        public event EventHandler TimerTicked;
        public event EventHandler GameOverEvent;
        public event EventHandler BalanceChanged;
        public event EventHandler<EndOfBuildingEventArgs> EndOfBuilding;
        public event EventHandler<SidewalkDirtinessChangedEventArgs> SidewalkDirtinessChanged;

        #endregion


        #region Adattagok

        private int balance;
        private Janitor chosenJanitor;

        #endregion

        #region Properties  
        /// <value>
        /// Gets the parks area in a two-dimensional array.
        /// </value>
        public ParkItem[,] ParkArea { get; private set; }
        /// <value>
        /// Gets the list of all Restaurants build in the park.
        /// </value>
        public List<Restaurant> Restaurants { get; private set; }
        // <value>
        /// Gets the list of all Attractions build in the park.
        /// </value>
        public List<Attraction> Attractions { get; private set; }
        /// <value>
        /// Gets the list of all Visitors that are currently in the park.
        /// </value>
        public List<Visitor> Visitors { get; private set; }
        /// <value>
        /// Gets the Janitors list that are currently in the park. Even those, who are "Fired" but hasn't reached the exit yet.
        /// </value>
        public List<Janitor> Janitors { get; private set; }
        /// <value>
        /// Gets if the park is open for visitors.
        /// </value>
        public bool IsOpen { get; private set; }
        /// <value>
        /// Gets if the game has ended.
        /// </value>
        public bool IsGameOver { get; private set; }
        /// <value>
        /// Gets the entrance fee
        /// </value>
        public int EntranceFee { get; private set; }
        /// <value>
        /// Gets the balance for the game. (It is the amount of money, that the player has.)
        /// </value>
        public int Balance
        {
            get { return balance; }
            private set
            {
                balance = value;
                if (balance < 0)
                {   // ha elfogy a pénze vége a játéknak, vagy mi történik? Gondolom érdemes ellenőrizni a pénzét minden költés után
                    GameOver();
                }
                BalanceChanged?.Invoke(this, null);
            }
        }
        /// <value>
        /// Gets the time that passed since starting the current game.
        /// </value>
        public int Time { get; private set; }
        /// <value>
        /// Gets the park's size.
        /// </value>
        public int Size { get; private set; }
        /// <value>
        /// Gets the position of the entrance.
        /// </value>
        public Position Entrance { get; private set; }          // bejárat helye a vidámparkban


        #endregion

        #region Public methods
        /// <summary>
        /// Constructor for the AmusementPark class.
        /// </summary>
        public AmusementPark()
        {
            Size = 50;
            ParkArea = new ParkItem[Size, Size];
            Restaurants = new List<Restaurant>();
            Attractions = new List<Attraction>();
            Visitors = new List<Visitor>();
            Janitors = new List<Janitor>();
            Entrance = new Position(0, 0);

            NewGame();
        }
        /// <summary>
        /// Opens the park.
        /// </summary>
        public void OpenPark()
        {
            IsOpen = true;
        }
        /// <summary>
        /// Start a new game by resetting all properties.
        /// </summary>
        public void NewGame()
        {

            while (Visitors.Count != 0)
            {
                Visitors[0].GoHome();
            }
            while (Janitors.Count != 0)
            {
                Janitors[0].GoHome();
            }
            foreach (Attraction a in Attractions)
            {
                this.TimerTicked -= a.TimerTicked;
                a.NeedToPay -= PayFees;
            }
            foreach (Restaurant r in Restaurants)
            {
                this.TimerTicked -= r.TimerTicked;
                r.NeedToPay -= PayFees;
            }


            IsGameOver = false;
            IsOpen = false;
            EntranceFee = 0;
            Balance = 5000;
            Time = 0;
            ParkArea = new ParkItem[Size, Size];
            ParkArea[Entrance.X, Entrance.Y] = new Sidewalk();

            Restaurants.Clear();
            Attractions.Clear();
        }
        /// <summary>
        /// If it is possible, the BuildItem method builds the given <paramref name="item"/> item to the given positon <paramref name="x"/> <paramref name="y"/>.
        /// It can be built if, the balance is more than the item's cost and it has enough place.
        /// If the item is built, the Reachable property is calculated and set.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="item">A string that specifies the item to be built.</param>
        /// <returns>True, if the building was successful, false, if not.</returns>
        public bool BuildItem(int x, int y, string item)
        {
            if (x == Entrance.X && y == Entrance.Y) return false;                   // a bejáratra ne lehessen semmit se rakni

            if (item == "Bin")
            {                                                                // KUKA
                if (ParkArea[x, y] == null)
                {
                    if (Balance < Sidewalk.Price + Sidewalk.BinPrice) return false;
                    ParkItem p_item = new Sidewalk
                    {
                        Pos = new Position(x, y)
                    };
                    ParkArea[x, y] = p_item;
                    Balance -= Sidewalk.Price;
                    SetReachable(x, y, Sidewalk.Width, Sidewalk.Height, true);
                }
                else if (ParkArea[x, y].GetItemType() != typeof(Sidewalk)) return false;
                if (Balance < Sidewalk.BinPrice) return false;
                (ParkArea[x, y] as Sidewalk).HasBin = true;
                Balance -= Sidewalk.BinPrice;
                return true;
            }

            bool freeSpace;
            int i;
            int j;
            Type type;
            Int32 height;
            Int32 width;
            ParkItem parkItem = null;

            if (Enum.IsDefined(typeof(PlantType), item))
            {                                      // NÖVÉNYEK
                type = typeof(Plant);

                height = (Int32)type.GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                width = (Int32)type.GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                freeSpace = CheckFreeSpace(x, y, width, height, CheckFreePlantSpace);
            }
            else
            {
                if (item == "Sidewalk") type = typeof(Sidewalk);
                else type = Type.GetType("RollerCoasterTycoon.Model.ParkItems.Items." + item);
                if (type == null || !type.IsSubclassOf(typeof(ParkItem))) return false;

                height = (Int32)type.GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                width = (Int32)type.GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                freeSpace = CheckFreeSpace(x, y, width, height, CheckFreeItemSpace);
            }

            Int32 price = (Int32)type.GetProperty("Price", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
            if (Balance < price) return false;               // nincs elég pénz megépíteni


            if (freeSpace)
            {

                if (type == typeof(Plant))
                {
                    parkItem = new Plant();
                    (parkItem as Plant).PlantType = Enum.Parse<PlantType>(item);                          // beállítja az enum megfelelő értékét

                }
                else
                {
                    parkItem = CreateItem(type);           // csak akkor hozzuk létre, ha le is tudjuk helyezni
                }

                for (i = x; i < x + width; i++)
                {               // az összes mezőre beállítani, amit elfoglal
                    for (j = y; j < y + height; j++)
                    {
                        ParkArea[i, j] = parkItem;

                    }
                }
                SetReachable(x, y, width, height, (ParkArea[x, y].GetItemType() == typeof(Sidewalk)));
                parkItem.Pos = new Position(x, y);
                Balance -= price;
                return true;
            }

            return false; // sikertelen lehelyezés
        }
        /// <summary>
        /// If the IsGameOver property is true, it does nothing.
        /// If the park is open, new visitors arrive randomly every two seconds.
        /// In every twelve minutes, the Balance is reduced by the maintanance costs and wages of the janitors.
        /// In every minute the plants and dirty sidewalks modify the mood of the visitors.
        /// </summary>
        public void TimerTickedInView()
        {
            if (IsGameOver) return;

            Time++;
            if (Time % (12 * 60) == 0)
            {           // eltelt a játékban egy fél nap

                PayOnTime();                        // állandó költségek
                PayJanitors();
            }
            if (IsOpen)
            {                           // csak akkor, ha nyitva van a park

                if (Time % 2 == 0)
                {                   // 2 másodpercenként jöhet új vendég
                    var rand = new Random();
                    int newVisitors = rand.Next(2);     // 0-2 közötti látogató érkezik
                    for (int i = 0; i < newVisitors; i++)
                    {
                        Visitor newVisitor = new Visitor(this);
                        Visitors.Add(newVisitor);
                        if (newVisitor.WillingToPay(EntranceFee))
                        {
                            newVisitor.Pay(EntranceFee);
                            newVisitor.ChooseDest();
                        }
                        else newVisitor.GoHome();
                    }
                }

                if (Time % 60 == 0)
                {               // játékbeli óránként változtatja a hangulatot a növényzet és a szemét
                    PlantsModifyMood();
                    DirtySidewalksModifyMood();
                }
            }
            TimerTicked?.Invoke(this, null);
        }
        /// <summary>
        /// Returns the items type from the given position.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>The type of the given item.</returns>
        public Type GetItemType(int x, int y)
        {
            return ParkArea[x, y].GetItemType();
        }
        /// <summary>
        /// Changes the StartMin to <paramref name="newMin"/> a new value of the ParkItem at the given position.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="newMin">Double value for new StartMin</param>
        /// <returns>Returns true if the change was successful.</returns>
        public bool ChangeStartMin(int x, int y, double newMin)
        {
            if (newMin < 0 || newMin > 1) return false;

            if (!ParkArea[x, y].GetItemType().IsSubclassOf(typeof(Attraction))) return false;

            (ParkArea[x, y] as Attraction).StartMin = newMin;
            return true;
        }
        /// <summary>
        /// Changes the CostOfUse to <paramref name="newCostOfUse"/> a new value of the ParkItem at the given position.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="newCostOfUse">Integer value for new CostOfUse</param>
        /// <returns>Returns true if the change was successful.</returns>
        public bool ChangeCostOfUse(int x, int y, int newCostOfUse)
        {
            if (newCostOfUse < 0) return false;
            ParkArea[x, y].CostOfUse = newCostOfUse;
            return true;
        }
        /// <summary>
        /// Changes the entrance fee of the park.
        /// </summary>
        /// <param name="newFee">Integer value for new EntranceFee</param>
        /// <returns>True if the change was successful.</returns>
        public bool ChangeEntranceFee(int newFee)
        {            // az osztálydiagramban void volt (bool ellenőrzés miatt?) 
            if (newFee >= 0)
            {
                EntranceFee = newFee;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if there is a bin at the given position, false otherwise.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>True, if there is a bin, false, if not.</returns>
        public bool IsBin(int x, int y)
        {
            return IsSidewalk(x, y) && (ParkArea[x, y] as Sidewalk).HasBin;
        }
        /// <summary>
        /// Returns true if there is a sidewalk at the given position, false otherwise.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>True, if there is a sidewalk, false, if not.</returns>
        public bool IsSidewalk(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size && ParkArea[x, y] != null && (ParkArea[x, y].GetItemType() == typeof(Sidewalk));
        }
        /// <summary>
        /// Returns the width of the ParkItem at the given position.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>An integer value for width.</returns>
        public int GetWidth(int x, int y)
        {
            return (Int32)(ParkArea[x, y].GetItemType()).GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
        }
        /// <summary>
        /// Returns the height of the ParkItem at the given position.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>An integer value for height.</returns>
        public int GetHeight(int x, int y)
        {
            return (Int32)(ParkArea[x, y].GetItemType()).GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
        }
        /// <summary>
        /// Returns the number of visitors at the given position.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>An integer for the number of visitors at the given position.</returns>
        public int GetVisitorNum(int x, int y)
        {
            int n = 0;
            foreach (Visitor v in Visitors)
            {
                if (v.Pos.X == x && v.Pos.Y == y) n++;
            }
            return n;
        }
        /// <summary>
        /// Returns the number of janitors at the given position.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>An integer for the number of janitors at the given position.</returns>
        public int GetJanitorNum(int x, int y)
        {
            int n = 0;
            foreach (Janitor j in Janitors)
            {
                if (j.Pos.X == x && j.Pos.Y == y) n++;
            }
            return n;
        }
        /// <summary>
        /// The visitor pays for the park. The given <paramref name="m"/> amount is added to the Balance.
        /// </summary>
        /// <param name="m">An integer</param>
        public void VisitorPays(int m)
        {
            Balance += m;
        }
        /// <summary>
        /// Chooses an Attraction from the Attractions list for the visitor.
        /// </summary>
        /// <param name="v">The Visitor for whom the park chooses.</param>
        /// <returns>The chosen attraction or null if it was not successful.</returns>
        public Attraction ChooseAttraction(Visitor v)
        {
            var rand = new Random();

            if (Attractions.Count == 0) return null;
            int i = rand.Next(Attractions.Count);
            Attraction attr = Attractions[i];
            int j = 0;                                  // ha nem talál olyat, amihez oda tud menni, és fizetni is hajlandó érte nullt adjunk vissza
            while ((!attr.Reachable || attr.State == State.Build || !v.WantsToUse(attr.AdrenalinFact, attr.CostOfUse)) && j < 10)
            {
                i = rand.Next(Attractions.Count);
                attr = Attractions[i];
                j++;
            }
            if (j == 10)
            {
                return null;
            }
            return attr;

        }
        /// <summary>
        /// Chooses an Restaurant from the Restaurants list for the visitor.
        /// </summary>
        /// <param name="v">The Visitor for whom the park chooses.</param>
        /// <returns>The chosen restaurant or null if it was not successful.</returns>
        public Restaurant ChooseRestaurant(Visitor v)
        {
            var rand = new Random();

            if (Restaurants.Count == 0) return null;
            int i = rand.Next(Restaurants.Count);
            Restaurant rest = Restaurants[i];
            int j = 0;
            while ((!rest.Reachable || !v.WillingToPay(rest.CostOfUse)) && j < 10)
            {
                i = rand.Next(Restaurants.Count);
                rest = Restaurants[i];
                j++;
            }
            if (j == 10)
            {
                return null;
            }
            return rest;

        }
        /// <summary>
        /// Removes the given Visitor from the Visitors list.
        /// </summary>
        /// <param name="v">Visitor</param>
        public void VisitorLeaves(Visitor v)
        {
            Visitors.Remove(v);
        }
        /// <summary>
        /// Removes the given Janitros from the Janitors list.
        /// </summary>
        /// <param name="v">Janitor</param>
        public void JanitorFired(Janitor j)
        {
            Janitors.Remove(j);
        }
        /// <summary>
        /// Stops the Janitor at the given position.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>True if a janitor was successfully stopped, or else it returns false.</returns>
        public bool StopJanitor(int x, int y)
        {

            if (Janitors.Count == 0) return false;

            Position pos = new Position(x, y);
            Janitor j = FindJanitor(pos);
            if (j != null && j.State == "Fired") return false;
            if (j != null)
            {
                j.Stop();
                chosenJanitor = j;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Starts the janitor that was stopped.
        /// </summary>
        /// <returns>Returns true, if a janitor was stopped before and now started. If no janitor was stopped it returns false.</returns>
        public bool StartJanitor()
        {

            if (chosenJanitor != null)
            {
                chosenJanitor.Start();
                chosenJanitor = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the stopped janitor to the given position.
        /// </summary>
        /// <param name="to_x">x coordinate</param>
        /// <param name="to_y">y coordniate</param>
        /// <returns>Returns true, if moving a janitor was successfull, false, if not.</returns>
        public bool MoveJanitor(int to_x, int to_y)
        {               // sikeres volt-e a mozgatás

            if (to_x < 0 || to_x >= Size || to_y < 0 || to_y >= Size
                || ParkArea[to_x, to_y] == null || ParkArea[to_x, to_y].GetItemType() != typeof(Sidewalk)) return false;

            Position to = new Position(to_x, to_y);

            if (chosenJanitor != null)
            {
                chosenJanitor.Move(to);
                chosenJanitor = null;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Changes the number of the working janitors by hiring new ones or firing some of them.
        /// </summary>
        /// <param name="n">New number of janitors.</param>
        public void ChangeJanitorNum(int n)
        {
            if (n < 0) return;
            if (n < CountWorkingJanitors())
            {
                int i = Janitors.Count - 1;
                while (i > 0 && CountWorkingJanitors() != n)
                {
                    if (Janitors[i].State == "Working")
                        Janitors[i].StopWorking();
                    i--;
                }
            }
            else if (n > CountWorkingJanitors())
            {
                while (CountWorkingJanitors() != n)
                {
                    Janitor j = new Janitor(this);
                    j.ChooseDest();
                    Janitors.Add(j);
                }
            }
        }
        /// <summary>
        /// Returns the number of janitors whose state is "Working".
        /// </summary>
        /// <returns>The number of working janitors.</returns>
        public int CountWorkingJanitors()
        {
            int n = 0;
            foreach (Janitor janitor in Janitors)
            {
                if (janitor.State == "Working") n++;
            }
            return n;
        }
        /// <summary>
        /// Checks if there is a bin closer than two fields to the given position.
        /// </summary>
        /// <param name="pos">Position</param>
        /// <returns>True, if there is a bin next to the position, otherwise returns false.</returns>
        public bool CloseBin(Position pos)
        {                        // pozíció 2 sugarú környezetében van-e kuka
            bool hasCloseBin = false;
            int r = 2;
            int i = pos.X - r;
            int j;
            while (i < pos.X + r && !hasCloseBin)
            {
                j = pos.Y - r;
                while (j < pos.Y + r && !hasCloseBin)
                {
                    if (i >= 0 && i < Size && j >= 0 && j < Size) hasCloseBin = ParkArea[i, j] != null && ParkArea[i, j].GetItemType() == typeof(Sidewalk) && (ParkArea[i, j] as Sidewalk).HasBin;
                    j++;
                }
                i++;
            }
            return hasCloseBin;
        }
        /// <summary>
        /// Makes the sidewalk at the given <paramref name="position"/>position dirty.
        /// </summary>
        /// <param name="position">Position</param>
        public void TrashOnSidewalk(Position position)
        {
            (ParkArea[position.X, position.Y] as Sidewalk).IsDirty = true;
            SidewalkDirtinessChanged?.Invoke(this, new SidewalkDirtinessChangedEventArgs(position, true));
        }
        /// <summary>
        /// Makes the sidewalk at the given <paramref name="position"/> position not dirty.
        /// </summary>
        /// <param name="position">Position</param>
        public void CleanSideWalk(Position position)
        {
            (ParkArea[position.X, position.Y] as Sidewalk).IsDirty = false;
            SidewalkDirtinessChanged?.Invoke(this, new SidewalkDirtinessChangedEventArgs(position, false));
        }


        #region Private methods
        /// <summary>
        /// Sets the IsGameOver property to true and emits an event.
        /// </summary>
        private void GameOver()
        {
            IsGameOver = true;
            GameOverEvent?.Invoke(this, null);
        }
        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="type">Type of the new item.</param>
        /// <returns>The new item.</returns>
        private ParkItem CreateItem(Type type)
        {

            ParkItem newItem = (ParkItem)Activator.CreateInstance(type);
            if (type.IsSubclassOf(typeof(Restaurant)) || type.IsSubclassOf(typeof(Attraction)))
            {
                if (type.IsSubclassOf(typeof(Restaurant)))
                {
                    Restaurants.Add(newItem as Restaurant);
                }
                else if (type.IsSubclassOf(typeof(Attraction)))
                {
                    newItem.EndOfBuilding += new EventHandler<EndOfBuildingEventArgs>(EndOfBuildingEventHandler);
                    Attractions.Add(newItem as Attraction);
                }
                newItem.NeedToPay += new EventHandler<NeedToPayEventArgs>(PayFees);
                this.TimerTicked += new EventHandler(newItem.TimerTicked);

            }
            return newItem;
        }

        /// <summary>
        /// Calculates if the given area is reachable from the entrance.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="width">An integer, the width of the area</param>
        /// <param name="height">An integer, the height of the area</param>
        /// <param name="isSideWalk">A bool, which is true if the area is a sidewalk, false if not.</param>
        private void SetReachable(int x, int y, int width, int height, bool isSideWalk)
        {

            int i = x - 1;
            int j;
            bool isReachable = false;

            // elérhető-e a bejárattól (átlósan nem lehet lépni)

            while (i <= x + width && !isReachable)
            {                    // szomszédos mezők függőlegesen
                j = y;
                while (j < y + height && !isReachable)
                {
                    if (!(i == x && j == y) && i >= 0 && i < Size && j >= 0 && j < Size)
                    {                                                  // nem az az elem, amit épp leraktunk, és a területen belül van
                        isReachable = (i == Entrance.X && j == Entrance.Y || (ParkArea[i, j] is Sidewalk) && ParkArea[i, j].Reachable);     // ha a bejárat mellé tettük vagy olyan út mellé, amiről tudjuk, hogy elérhető
                    }
                    j++;
                }
                i += width + 1;
            }

            j = y - 1;
            while (j <= y + height && !isReachable)
            {                    // szomszédos mezők vízszintesen
                i = x;
                while (i < x + width && !isReachable)
                {
                    if (!(i == x && j == y) && i >= 0 && i < Size && j >= 0 && j < Size)
                    {                                                  // nem az az elem, amit épp leraktunk, és a területen belül van
                        isReachable = (i == Entrance.X && j == Entrance.Y || (ParkArea[i, j] is Sidewalk) && ParkArea[i, j].Reachable);     // ha a bejárat mellé tettük vagy olyan út mellé, amiről tudjuk, hogy elérhető
                    }
                    i++;
                }
                j += height + 1;
            }
            ParkArea[x, y].Reachable = isReachable;


            if (isSideWalk && ParkArea[x, y].Reachable)
            {
                SetReachableSidewalk(x, y);
            }
        }

        /// <summary>
        /// A recursive method to set the Reachable property after a sidewalk was built.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        private void SetReachableSidewalk(int x, int y)
        {
            int i;
            int j;
            // négy oldalát megnézni
            j = y;
            for (i = x - 1; i <= x + 1; i += 2)
            {
                if (!(i == x && j == y) && i >= 0 && i < Size && j >= 0 && j < Size && ParkArea[i, j] != null && !ParkArea[i, j].Reachable)
                {

                    ParkArea[i, j].Reachable = true;
                    if (ParkArea[i, j].GetItemType() == typeof(Sidewalk)) SetReachableSidewalk(i, j);
                }
            }

            i = x;
            for (j = y - 1; j <= y + 1; j += 2)
            {
                if (!(i == x && j == y) && i >= 0 && i < Size && j >= 0 && j < Size && ParkArea[i, j] != null && !ParkArea[i, j].Reachable)
                {

                    ParkArea[i, j].Reachable = true;
                    if (ParkArea[i, j].GetItemType() == typeof(Sidewalk)) SetReachableSidewalk(i, j);
                }
            }

        }
        /// <summary>
        /// Checks if there is enough space for the given area.
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="width">An integer, the width of the area</param>
        /// <param name="height">An integer, the height of the area</param>
        /// <param name="checkSpace">A Function which will decide if the space if free or not.</param>
        /// <returns></returns>
        private bool CheckFreeSpace(int x, int y, int width, int height, Func<Position, bool> checkSpace)
        {
            if (x < 0 || y < 0 || x + width - 1 >= Size || y + height >= Size) return false;
            int i = x;
            int j = y;
            while (j < Size && j < y + height)
            {
                if (!checkSpace(new Position(i, j))) return false;
                if (i == x + width - 1)
                {
                    i = x;
                    j++;
                }
                else
                {
                    i++;
                }
            }
            return true;
        }
        /// <summary>
        /// Function to check free space at given <paramref name="p"/> position for almost all ParkItems.
        /// </summary>
        /// <param name="p">Position</param>
        /// <returns>Return true if there is free space, false if not.</returns>
        private bool CheckFreeItemSpace(Position p)
        {
            if (!(p.X >= 0 && p.X < Size && p.Y >= 0 && p.Y < Size)) return false;
            return ParkArea[p.X, p.Y] == null;
        }
        /// <summary>
        /// Function to check free space at given <paramref name="p"/> position for Plants.
        /// Plants can be placed on Grass.
        /// </summary>
        /// <param name="p">Position</param>
        /// <returns>Return true if there is free space, false if not.</returns>
        private bool CheckFreePlantSpace(Position p)
        {
            if (!(p.X >= 0 && p.X < Size && p.Y >= 0 && p.Y < Size)) return false;
            return ParkArea[p.X, p.Y] == null || (ParkArea[p.X, p.Y].GetItemType() == typeof(Plant) && (ParkArea[p.X, p.Y] as Plant).PlantType == PlantType.Grass);
        }

        /// <summary>
        /// For every visitors, the visitor's mood changes if there is a plant nearby.
        /// Their mood changes by the plant's MoodChanger property.
        /// </summary>
        private void PlantsModifyMood()
        {
            int modArea = 4;
            foreach (Visitor visitor in Visitors)
            {             // a max 4 távolságra lévő növények javítják a hangulatot
                for (int i = visitor.Pos.X - modArea; i < visitor.Pos.X + modArea; i++)
                {
                    for (int j = visitor.Pos.Y - modArea; j < visitor.Pos.Y + modArea; j++)
                    {
                        if (i >= 0 && i < Size && j >= 0 && j < Size && ParkArea[i, j] != null && ParkArea[i, j].GetItemType() == typeof(Plant))
                        {
                            visitor.ChangeMood((ParkArea[i, j] as Plant).MoodChanger);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Every dirty sidewalk decreases all visitors mood by two.
        /// </summary>
        private void DirtySidewalksModifyMood()
        {
            int changer = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (ParkArea[i, j] != null && ParkArea[i, j].GetItemType() == typeof(Sidewalk) && (ParkArea[i, j] as Sidewalk).IsDirty) changer += 2;           // minden szemetes járdaszakasz 2-t von le a hangulatból
                }
            }

            foreach (Visitor visitor in Visitors)
            {
                visitor.ChangeMood(-changer);
            }
        }

        /// <summary>
        /// The balance is decreased by the MaintenanceCosts of ParkItems in the Attractions and Restaurants lists.
        /// </summary>
        private void PayOnTime()
        {

            foreach (Restaurant rest in Restaurants)
            {
                Balance -= rest.MaintenanceCost;
            }
            foreach (Attraction attr in Attractions)
            {
                Balance -= attr.MaintenanceCost;
            }

        }
        /// <summary>
        /// The Balance is reduced by the janitors' wages.
        /// </summary>
        private void PayJanitors()
        {
            Balance -= CountWorkingJanitors() * Janitor.Wage;
        }
        /// <summary>
        /// Returns the janitor at the given <paramref name="pos"/> position.
        /// </summary>
        /// <param name="pos">Position</param>
        /// <returns>The janitors at the position, or null if there are no janitors.</returns>
        private Janitor FindJanitor(Position pos)
        {

            if (Janitors.Count == 0) return null;
            bool found = false;
            int i = 0;
            while (i < Janitors.Count && !found)
            {
                found = Janitors[i].Pos == pos;
                i++;
            }
            if (found) return Janitors[i - 1];
            return null;
        }

        #endregion


        #endregion

        #region EventHandlers
        /// <summary>
        /// Pay the amount that is in the <paramref name="e"/>e parameter.
        /// </summary>
        /// <param name="sender">The ParkItem that needs to be paid.</param>
        /// <param name="e">NeedToPayEventArgs</param>
        public void PayFees(object sender, NeedToPayEventArgs e)
        {
            Balance -= e.Fee;
        }
        /// <summary>
        /// Emits an event if an Attraction finished building.
        /// </summary>
        /// <param name="sender">The ParkItem that finished building</param>
        /// <param name="e">EndOfBuildingEventArgs</param>
        private void EndOfBuildingEventHandler(object sender, EndOfBuildingEventArgs e)
        {

            EndOfBuilding?.Invoke(this, e);

        }

        #endregion







    }
}
