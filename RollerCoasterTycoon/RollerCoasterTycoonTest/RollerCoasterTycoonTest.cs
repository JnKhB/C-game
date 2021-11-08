using Microsoft.VisualStudio.TestTools.UnitTesting;
using RollerCoasterTycoon.Model;
using RollerCoasterTycoon.Model.ParkItems;
using RollerCoasterTycoon.Model.ParkItems.Items;
using System;
using static RollerCoasterTycoon.Model.AmusementPark;

namespace RollerCoasterTycoonTest {

    [TestClass]
    public class RollerCoasterTycoonTest {
        AmusementPark AmusementPark;
        FerrisWheel FerrisWheel;
        Int32 Balance;
        Position Position;
        Int32 CostOfUse;
        [TestInitialize]
        public void Initialize() {
            AmusementPark = new AmusementPark();
            FerrisWheel = new FerrisWheel();
            AmusementPark.NewGame();
            Balance = AmusementPark.Balance;
            Position = new Position(5, 5);
        }
        #region TestForAmusementParkMethods
        [TestMethod]
        public void TestOpenPark() {
            AmusementPark.OpenPark();
            Assert.AreEqual(AmusementPark.IsOpen, true);
        }
        [TestMethod]
        public void TestNewGame() {
            AmusementPark.NewGame();
            Assert.AreEqual(AmusementPark.IsOpen, false);
            AmusementPark.OpenPark();
            Assert.AreEqual(AmusementPark.IsOpen, true);
            Assert.AreEqual(AmusementPark.Restaurants.Count, 0);
            Assert.AreEqual(AmusementPark.Attractions.Count, 0);
            Assert.AreEqual(AmusementPark.Visitors.Count, 0);
            AmusementPark.NewGame();
            Assert.AreEqual(AmusementPark.IsOpen, false);
            Assert.AreEqual(AmusementPark.Restaurants.Count, 0);
            Assert.AreEqual(AmusementPark.Attractions.Count, 0);
            Assert.AreEqual(AmusementPark.Visitors.Count, 0);
            Assert.AreEqual(AmusementPark.ParkArea.Length, AmusementPark.Size * AmusementPark.Size);

        }
        [TestMethod]
        public void TestBuildItem() {

            AmusementPark.BuildItem(10, 10, FerrisWheel.Name);
            Assert.AreEqual(AmusementPark.Balance, Balance -= FerrisWheel.Price);
            //Jó helyre történõ lehelyezésnek a tesztelése továbbá a balance megfelelõ csökkenésének a tesztelése
            Assert.AreEqual(AmusementPark.BuildItem(1, 1, "RollerCoaster"), true);
            Assert.AreEqual(AmusementPark.Balance, Balance -= RollerCoaster.Price);
            Assert.AreEqual(AmusementPark.BuildItem(25, 25, "FerrisWheel"), true);
            Assert.AreEqual(AmusementPark.Balance, Balance -= FerrisWheel.Price);
            Assert.AreEqual(AmusementPark.BuildItem(15, 33, "Carousel"), true);
            Assert.AreEqual(AmusementPark.BuildItem(5, 40, "HauntedHouse"), true);
            Assert.AreEqual(AmusementPark.BuildItem(35, 22, "Langos"), true);
            Assert.AreEqual(AmusementPark.GetItemType(1, 1).ToString(), "RollerCoasterTycoon.Model.ParkItems.Items.RollerCoaster");
            Assert.AreEqual(AmusementPark.GetItemType(25, 25).ToString(), "RollerCoasterTycoon.Model.ParkItems.Items.FerrisWheel");
            Assert.AreEqual(AmusementPark.GetItemType(15, 33).ToString(), "RollerCoasterTycoon.Model.ParkItems.Items.Carousel");
            Assert.AreEqual(AmusementPark.GetItemType(5, 40).ToString(), "RollerCoasterTycoon.Model.ParkItems.Items.HauntedHouse");
            Assert.AreEqual(AmusementPark.GetItemType(35, 22).ToString(), "RollerCoasterTycoon.Model.ParkItems.Items.Langos");

            //Pályáról való kihelyezésnek a tesztelése
            Assert.AreEqual(AmusementPark.BuildItem(5, 46, "HauntedHouse"), false);
            Assert.AreEqual(AmusementPark.BuildItem(48, 46, "Sweets"), false);
            Assert.AreEqual(AmusementPark.BuildItem(3, 49, "ChimneyCake"), false);
            Assert.AreEqual(AmusementPark.BuildItem(47, 44, "Langos"), false);

            //Másik Item-re történõ lehelyezésnek a tesztelése
            Assert.AreEqual(AmusementPark.BuildItem(12, 12, "ChimneyCake"), false);
            Assert.AreEqual(AmusementPark.BuildItem(13, 13, "Buffet"), false);
            Assert.AreEqual(AmusementPark.BuildItem(14, 15, "Sidewalk"), false);

            //elemek lehelyezése utáni változások ellenõrzése, Létrehoztunk összesen 5 játékot, és egy éttermet
            Assert.AreEqual(AmusementPark.Restaurants.Count, 1);
            Assert.AreEqual(AmusementPark.Attractions.Count, 5);

            // Ha nincs elég pénz, sikertelen a vásárlás
            AmusementPark.NewGame();
            int i = 1;
            int j = 1;
            while (AmusementPark.Balance >= FerrisWheel.Price) {
                Assert.IsTrue(AmusementPark.BuildItem(i, j, "FerrisWheel"));
                i += FerrisWheel.Width;
                if (i >= AmusementPark.Size) {
                    j += FerrisWheel.Height;
                    i = 0;
                }
            }
            Assert.IsFalse(AmusementPark.BuildItem(i, j, "FerrisWheel"));

            //Fûre lehet más növényt "építeni"
            AmusementPark.NewGame();
            Assert.IsTrue(AmusementPark.BuildItem(1, 1, "Grass"));
            Assert.IsTrue(AmusementPark.ParkArea[2, 2].GetItemType() == typeof(Plant) && (AmusementPark.ParkArea[2, 2] as Plant).PlantType == PlantType.Grass);
            Assert.IsTrue(AmusementPark.BuildItem(2, 2, "Tree"));

            // Járda és kuka lehelyezése
            AmusementPark.NewGame();
            Assert.IsTrue(AmusementPark.BuildItem(0, 1, "Sidewalk"));
            Assert.IsTrue(AmusementPark.BuildItem(0, 2, "Bin"));
            Assert.IsTrue(AmusementPark.ParkArea[0, 2].GetItemType() == typeof(Sidewalk) && (AmusementPark.ParkArea[0, 2] as Sidewalk).HasBin);
            Assert.IsTrue(AmusementPark.ParkArea[0, 1].GetItemType() == typeof(Sidewalk) && !(AmusementPark.ParkArea[0, 1] as Sidewalk).HasBin);
            Assert.IsTrue(AmusementPark.BuildItem(0, 1, "Bin"));
            Assert.IsTrue(AmusementPark.ParkArea[0, 1].GetItemType() == typeof(Sidewalk) && (AmusementPark.ParkArea[0, 1] as Sidewalk).HasBin);

            // Elérhetõség
            AmusementPark.NewGame();
            i = AmusementPark.Entrance.X;
            j = AmusementPark.Entrance.Y;
            Assert.IsTrue(AmusementPark.BuildItem(i, j + 3, "Sweets"));
            Assert.IsFalse(AmusementPark.ParkArea[i, j + 3].Reachable);
            Assert.IsTrue(AmusementPark.BuildItem(i, j + 2, "Sidewalk"));
            Assert.IsFalse(AmusementPark.ParkArea[i, j + 2].Reachable);
            Assert.IsTrue(AmusementPark.BuildItem(i, j + 1, "Sidewalk"));
            Assert.IsTrue(AmusementPark.ParkArea[i, j + 1].Reachable);
            Assert.IsTrue(AmusementPark.ParkArea[i, j + 2].Reachable);
            Assert.IsTrue(AmusementPark.ParkArea[i, j + 3].Reachable);

            //Helytelen bemenet
            AmusementPark.NewGame();
            Assert.IsFalse(AmusementPark.BuildItem(2, 2, "sweets"));
            Assert.IsTrue(AmusementPark.BuildItem(2, 2, "Sweets"));

        }
        [TestMethod]
        public void TestTimerTickedInView() {
            Assert.AreEqual(AmusementPark.Time, 0);
            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");

            // Takarítók kifizetése
            InitJanitors(AmusementPark);
            for (int i = 0; i < 12 * 60 - 1; i++) {
                AmusementPark.TimerTickedInView();
            }
            int balanceBeforeJanitorsSalary = AmusementPark.Balance;
            AmusementPark.TimerTickedInView();
            balanceBeforeJanitorsSalary -= AmusementPark.ParkArea[16, 0].MaintenanceCost;
            balanceBeforeJanitorsSalary -= AmusementPark.CountWorkingJanitors() * Janitor.Wage;
            Assert.IsTrue(AmusementPark.Balance == balanceBeforeJanitorsSalary);

            InitVisitor(AmusementPark);
            Assert.IsTrue(AmusementPark.IsOpen);
            Assert.IsTrue(AmusementPark.Visitors.Count > 0);

            //Plants modify mood
            //while (AmusementPark.Time % 60 != 59) {
            //    AmusementPark.TimerTickedInView();
            //}
            //if (AmusementPark.Visitors.Count > 0) {
            //    Visitor v = AmusementPark.Visitors[0];
            //    int x = v.Pos.X - 3;
            //    int y = v.Pos.Y - 3;
            //    while( !(x == v.Pos.X + 3 && y == v.Pos.Y + 3) && 
            //        (x < 0 || y < 0 || x >= AmusementPark.Size || y >= AmusementPark.Size || AmusementPark.ParkArea[x,y] != null)) {
            //        x++;
            //        if (x == v.Pos.X + 3) {
            //            x = v.Pos.Y - 3;
            //            y++;
            //        }
            //    }
            //    if (AmusementPark.BuildItem(x, y, "Bush")) {
            //        v.ChangeMood(-20);
            //        int mood = v.Mood;
            //        AmusementPark.TimerTickedInView();
            //    }

            //}


            //Plants és dirty modify mood-ot hogyan?
            //Assert.AreEqual(AmusementPark.Balance, Balance);
            //AmusementPark.BuildItem(1, 1, "Bush");
            //AmusementPark.BuildItem(3, 1, "Bush");
            //AmusementPark.BuildItem(7, 1, "Bush");
            //AmusementPark.BuildItem(9, 1, "Bush");
            //AmusementPark.BuildItem(13, 1, "Bush");
            //AmusementPark.BuildItem(11, 3, "Bush");
            //AmusementPark.BuildItem(11, 7, "Bush");
            //AmusementPark.BuildItem(5, 3, "Bush");
            //int mood = AmusementPark.Visitors[3].Mood;
        }
        [TestMethod]
        public void TestGetItemType() {
            AmusementPark.BuildItem(10, 10, PlantType.Bush.ToString());
            AmusementPark.BuildItem(20, 22, Buffet.Name);
            AmusementPark.BuildItem(40, 40, FerrisWheel.Name);
            Assert.AreEqual(AmusementPark.GetItemType(20, 22).ToString(), "RollerCoasterTycoon.Model.ParkItems.Items.Buffet");
            Assert.AreEqual(AmusementPark.GetItemType(10, 10).ToString(), "RollerCoasterTycoon.Model.ParkItems.Plant");
            Assert.AreEqual(AmusementPark.GetItemType(40, 40).ToString(), "RollerCoasterTycoon.Model.ParkItems.Items.FerrisWheel");
        }
        [TestMethod]
        public void TestChangeStartMin() {
            //default érték
            AmusementPark.BuildItem(40, 40, FerrisWheel.Name);
            Assert.AreEqual(AmusementPark.ParkArea[40, 40].StartMin, 0.23);
            //Megváltoztatás utáni érték megfelelõ
            Assert.IsTrue(AmusementPark.ChangeStartMin(40, 40, 0.8));
            Assert.AreEqual(AmusementPark.ParkArea[40, 40].StartMin, 0.8);
            Assert.IsTrue(AmusementPark.ChangeStartMin(40, 40, 0.1));
            Assert.AreEqual(AmusementPark.ParkArea[40, 40].StartMin, 0.1);
            //sikertelen változtatás rossz érték miatt
            Assert.IsFalse(AmusementPark.ChangeStartMin(40, 40, 10));
            Assert.AreEqual(AmusementPark.ParkArea[40, 40].StartMin, 0.1);
            Assert.IsFalse(AmusementPark.ChangeStartMin(40, 40, -1));
            Assert.AreEqual(AmusementPark.ParkArea[40, 40].StartMin, 0.1);
        }
        [TestMethod]
        public void TestChangeCostOfUse() {
            //default érték       
            AmusementPark.BuildItem(20, 20, Langos.Name);
            Assert.AreEqual(AmusementPark.ParkArea[20, 20].CostOfUse, 30);
            //Megváltoztatás utáni érték megfelelõ
            Assert.IsTrue(AmusementPark.ChangeCostOfUse(20, 20, 150));
            Assert.AreEqual(AmusementPark.ParkArea[20, 20].CostOfUse, 150);
            Assert.IsTrue(AmusementPark.ChangeCostOfUse(20, 20, 5));
            Assert.AreEqual(AmusementPark.ParkArea[20, 20].CostOfUse, 5);
            //sikertelen változtatás rossz érték miatt
            Assert.IsFalse(AmusementPark.ChangeStartMin(20, 20, -5));
            Assert.AreEqual(AmusementPark.ParkArea[20, 20].CostOfUse, 5);
        }
        [TestMethod]
        public void TestChangeEntranceFee() {
            Assert.AreEqual(AmusementPark.EntranceFee, 0);
            Assert.IsTrue(AmusementPark.ChangeEntranceFee(20));
            Assert.AreEqual(AmusementPark.EntranceFee, 20);
            Assert.IsFalse(AmusementPark.ChangeEntranceFee(-20));
            Assert.AreEqual(AmusementPark.EntranceFee, 20);
        }
        [TestMethod]
        public void TestIsBin() {
            AmusementPark.BuildItem(10, 10, "Sidewalk");
            Assert.IsFalse(AmusementPark.IsBin(10, 10));
            AmusementPark.BuildItem(10, 13, "Sidewalk");
            AmusementPark.BuildItem(10, 13, "Bin");
            Assert.IsTrue(AmusementPark.IsBin(10, 13));
            AmusementPark.BuildItem(22, 22, "Bin");
            Assert.IsTrue(AmusementPark.IsBin(22, 22));
        }
        [TestMethod]
        public void TestIsSideWalk() {
            BuildWalkSides(AmusementPark);
            Assert.IsTrue(AmusementPark.IsSidewalk(14, 0));
            Assert.IsTrue(AmusementPark.IsSidewalk(5, 3));
            Assert.IsFalse(AmusementPark.IsSidewalk(30, 30));
            Assert.IsFalse(AmusementPark.IsSidewalk(16, 0));
            AmusementPark.BuildItem(20, 20, "Bin");
            Assert.IsTrue(AmusementPark.IsSidewalk(20, 20));
        }
        [TestMethod]
        public void TestGetWidthGetHeight() {
            AmusementPark.BuildItem(10, 10, "Langos");
            Assert.AreEqual(AmusementPark.GetHeight(10, 10), Langos.Height);
            Assert.AreEqual(AmusementPark.GetWidth(10, 10), Langos.Width);
            AmusementPark.BuildItem(20, 20, "ChimneyCake");
            Assert.AreEqual(AmusementPark.GetHeight(20, 20), ChimneyCake.Height);
            Assert.AreEqual(AmusementPark.GetWidth(20, 20), ChimneyCake.Width);
        }
        [TestMethod]
        public void TestVisitorsNum() {
            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            Assert.AreEqual(AmusementPark.GetVisitorNum(3, 0), 0);
            InitMoreVisitors(AmusementPark);

            foreach(Visitor v in AmusementPark.Visitors) {
                Position pos1 = v.Pos;
                int n = 1;
                foreach(Visitor _v in AmusementPark.Visitors) {
                    if (v != _v && _v.Pos.Equals(pos1)) n++;
                }
                Assert.AreEqual(AmusementPark.GetVisitorNum(pos1.X, pos1.Y), n);

            }

        }


        [TestMethod]
        public void TestVisitorPays() {
            AmusementPark.BuildItem(10, 10, FerrisWheel.Name);
            Assert.AreEqual(AmusementPark.Balance, Balance -= FerrisWheel.Price);
            AmusementPark.VisitorPays(50);
            Assert.AreEqual(AmusementPark.Balance, Balance += 50);
        }
        [TestMethod]
        public void TestChooseAttraction() {

            Visitor v = new Visitor(AmusementPark);
            
            BuildWalkSides(AmusementPark);
            Assert.IsNull(AmusementPark.ChooseAttraction(v));

            AmusementPark.BuildItem(12, 11, Langos.Name);
            Assert.IsTrue(AmusementPark.ParkArea[12, 11].Reachable);
            AmusementPark.BuildItem(16, 0, HauntedHouse.Name);
            Assert.AreEqual(AmusementPark.Attractions.Count, 1);
            Attraction a = AmusementPark.Attractions[0];
            
            // választ valamit 
            a.State = State.Wait;
            Assert.IsTrue(a.Reachable);
            Assert.IsTrue(a.State != State.Build);
            Attraction chosenAttr = AmusementPark.ChooseAttraction(v);
            if (chosenAttr != null) { 
                String stringOfAttraction1 = chosenAttr.ToString();
                Assert.IsTrue(stringOfAttraction1 == "RollerCoasterTycoon.Model.ParkItems.Items.HauntedHouse");
            } else {
                Assert.IsNull(chosenAttr);
            }

            AmusementPark.BuildItem(5, 6, Carousel.Name);
            Assert.IsTrue(AmusementPark.ParkArea[5, 6].Reachable);
            AmusementPark.ParkArea[5, 6].State = State.Wait;
            Assert.AreEqual(AmusementPark.Attractions.Count, 2);

            Visitor v2 = new Visitor(AmusementPark);
            chosenAttr = AmusementPark.ChooseAttraction(v2);
            if (chosenAttr != null) {
                string stringOfAttraction3 = chosenAttr.ToString();
                Assert.IsTrue(
                  stringOfAttraction3 == "RollerCoasterTycoon.Model.ParkItems.Items.HauntedHouse" ||
                  stringOfAttraction3 == "RollerCoasterTycoon.Model.ParkItems.Items.Carousel");
            }

        }
        [TestMethod]
        public void TestChooseRestaurant() {

            Visitor v1 = new Visitor(AmusementPark);
            Visitor v2 = new Visitor(AmusementPark);
            Visitor v3 = new Visitor(AmusementPark);

            //Nincs játék a pályán, de úthálózat van
            BuildWalkSides(AmusementPark);
            Assert.IsNull(AmusementPark.ChooseRestaurant(v1));
            Assert.IsNull(AmusementPark.ChooseRestaurant(v2));
            Assert.IsNull(AmusementPark.ChooseRestaurant(v3));

            //Építünk 1 játékot 1 éttermet
            AmusementPark.BuildItem(20, 20, HauntedHouse.Name);
            AmusementPark.BuildItem(16, 0, Langos.Name);
            AmusementPark.ParkArea[16, 0].State = State.Wait;
            Assert.IsTrue(AmusementPark.ParkArea[16, 0].Reachable);
            Assert.AreEqual(AmusementPark.Attractions.Count, 1);

            if (v1.WillingToPay(AmusementPark.ParkArea[16,0].CostOfUse))
                Assert.AreEqual(AmusementPark.ChooseRestaurant(v1).ToString(), "RollerCoasterTycoon.Model.ParkItems.Items.Langos");
            else {
                Assert.IsNull(AmusementPark.ChooseRestaurant(v1));
            }
        }
        [TestMethod]
        public void TestVisitorLeaves() {
            Visitor v1 = new Visitor(AmusementPark);
            Visitor v2 = new Visitor(AmusementPark);
            AmusementPark.Visitors.Add(v1);
            AmusementPark.Visitors.Add(v2);
            Assert.AreEqual(AmusementPark.Visitors.Count, 2);
            AmusementPark.VisitorLeaves(v1);
            Assert.AreEqual(AmusementPark.Visitors.Count, 1);
            AmusementPark.VisitorLeaves(v2);
            Assert.AreEqual(AmusementPark.Visitors.Count, 0);
        }

        [TestMethod]
        public void TestJanitorsMethods() {

            //ChangeJanitorNum és CountWorkingJanitors
            AmusementPark.ChangeJanitorNum(5);
            Assert.AreEqual(AmusementPark.Janitors.Count, 5);
            Assert.AreEqual(AmusementPark.CountWorkingJanitors(), 5);

            AmusementPark.ChangeJanitorNum(4);
            Assert.AreEqual(AmusementPark.Janitors.Count, 5);
            Assert.AreEqual(AmusementPark.CountWorkingJanitors(), 4);

            while (AmusementPark.Janitors.Count != 4) AmusementPark.TimerTickedInView();

            //JanitorFired
            AmusementPark.JanitorFired(AmusementPark.Janitors[3]);
            Assert.AreEqual(AmusementPark.Janitors.Count, 3);

            BuildWalkSides(AmusementPark);
            TicksOf25();

            //MoveJanitor - StopJanitor - StartJanitor
            AmusementPark.Janitors.Clear();
            AmusementPark.ChangeJanitorNum(1);
            Assert.IsTrue(AmusementPark.StopJanitor(AmusementPark.Janitors[0].Pos.X, AmusementPark.Janitors[0].Pos.Y));
            Assert.IsTrue(AmusementPark.MoveJanitor(12, 8));
            Assert.AreEqual(AmusementPark.Janitors[0].Pos.X, 12);
            Assert.AreEqual(AmusementPark.Janitors[0].Pos.Y, 8);

            Assert.IsTrue(AmusementPark.StopJanitor(AmusementPark.Janitors[0].Pos.X, AmusementPark.Janitors[0].Pos.Y));
            Assert.IsTrue(AmusementPark.MoveJanitor(10, 0));
            Assert.AreEqual(AmusementPark.Janitors[0].Pos.X, 10);
            Assert.AreEqual(AmusementPark.Janitors[0].Pos.Y, 0);

            Assert.IsTrue(AmusementPark.StopJanitor(AmusementPark.Janitors[0].Pos.X, AmusementPark.Janitors[0].Pos.Y));
            Assert.IsFalse(AmusementPark.MoveJanitor(-1, -1));
            Assert.IsFalse(AmusementPark.MoveJanitor(AmusementPark.Size, AmusementPark.Size));
            Assert.IsTrue(AmusementPark.StartJanitor());

            Assert.IsFalse(AmusementPark.StartJanitor());
            Assert.IsTrue(AmusementPark.StopJanitor(AmusementPark.Janitors[0].Pos.X, AmusementPark.Janitors[0].Pos.Y));
            Assert.IsTrue(AmusementPark.StartJanitor());

            AmusementPark.Janitors.Clear();
            Assert.IsFalse(AmusementPark.StopJanitor(1, 1));
            AmusementPark.ChangeJanitorNum(3);
            AmusementPark.StopJanitor(AmusementPark.Janitors[0].Pos.X, AmusementPark.Janitors[0].Pos.Y);
            AmusementPark.MoveJanitor(6, 0);
            AmusementPark.StopJanitor(AmusementPark.Janitors[1].Pos.X, AmusementPark.Janitors[1].Pos.Y);
            AmusementPark.MoveJanitor(6, 0);
            AmusementPark.StopJanitor(AmusementPark.Janitors[2].Pos.X, AmusementPark.Janitors[2].Pos.Y);
            AmusementPark.MoveJanitor(6, 0);
            AmusementPark.StopJanitor(6, 0);
            AmusementPark.MoveJanitor(7, 0);
            Position sixZero = new Position(6, 0);
            Position sevenZero = new Position(7, 0);
            Assert.AreEqual(sevenZero, AmusementPark.Janitors[0].Pos);
            Assert.AreEqual(sixZero, AmusementPark.Janitors[1].Pos);
            Assert.AreEqual(sixZero, AmusementPark.Janitors[2].Pos);

        }
        [TestMethod]
        public void TestCloseBin() {
            AmusementPark.BuildItem(2, 2, "Sidewalk");
            AmusementPark.BuildItem(2, 3, "Sidewalk");
            AmusementPark.BuildItem(2, 4, "Sidewalk");
            AmusementPark.BuildItem(2, 5, "Bin");

            AmusementPark.BuildItem(3, 6, "Sidewalk");
            AmusementPark.BuildItem(3, 7, "Sidewalk");
            Position position = new Position(2, 2);
            Assert.IsFalse(AmusementPark.CloseBin(position));
            position = new Position(2, 4);
            Assert.IsTrue(AmusementPark.CloseBin(position));
            position = new Position(3, 7);
            Assert.IsTrue(AmusementPark.CloseBin(position));
        }

        [TestMethod]
        public void TestTrashOnSidewalk() {
            AmusementPark.BuildItem(Position.X, Position.Y, "Sidewalk");
            Assert.IsFalse((AmusementPark.ParkArea[Position.X, Position.Y] as Sidewalk).IsDirty);
            AmusementPark.TrashOnSidewalk(Position);
            Assert.IsTrue((AmusementPark.ParkArea[Position.X, Position.Y] as Sidewalk).IsDirty);
        }
        [TestMethod]
        public void TestCleanSideWalk() {
            AmusementPark.BuildItem(Position.X, Position.Y, "Sidewalk");
            AmusementPark.TrashOnSidewalk(Position);
            Assert.IsTrue((AmusementPark.ParkArea[Position.X, Position.Y] as Sidewalk).IsDirty);
            AmusementPark.CleanSideWalk(Position);
            Assert.IsFalse((AmusementPark.ParkArea[Position.X, Position.Y] as Sidewalk).IsDirty);
        }
        #endregion

        #region TestJanitorPublicMethods
        [TestMethod]
        public void TestMove() {
            AmusementPark.ChangeJanitorNum(1);
            Janitor j = AmusementPark.Janitors[0];
            BuildWalkSides(AmusementPark);
            AmusementPark.OpenPark();
            TicksOf25();
            Position from = j.Pos;
            Position to = new Position(15, 0);
            Assert.AreEqual(j.Pos, from);
            j.Move(to);
            Assert.AreEqual(j.Pos, to);
            to = new Position(12, 9);
            j.Move(to);
            Assert.AreEqual(j.Pos, to);
        }
        [TestMethod]
        public void TestStopAndStart() {
            AmusementPark.ChangeJanitorNum(1);
            Janitor j = AmusementPark.Janitors[0];
            BuildWalkSides(AmusementPark);
            AmusementPark.OpenPark();
            TicksOf25();
            Assert.IsFalse(j.MidMove);
            j.Stop();
            Assert.IsTrue(j.MidMove);
            TicksOf25();
            Assert.IsTrue(j.MidMove);
            j.Start();
            Assert.IsFalse(j.MidMove);
        }
        [TestMethod]
        public void TestChooseDest() {
            AmusementPark.ChangeJanitorNum(1);
            BuildWalkSides(AmusementPark);
            AmusementPark.OpenPark();
            TicksOf25();
            Assert.IsFalse((AmusementPark.ParkArea[14, 0] as Sidewalk).IsDirty);
            (AmusementPark.ParkArea[14, 0] as Sidewalk).IsDirty = true;
            Assert.IsTrue((AmusementPark.ParkArea[14, 0] as Sidewalk).IsDirty);
            TicksOf25();
            Assert.IsFalse((AmusementPark.ParkArea[14, 0] as Sidewalk).IsDirty);

            Assert.IsFalse((AmusementPark.ParkArea[5, 5] as Sidewalk).IsDirty);
            (AmusementPark.ParkArea[5, 5] as Sidewalk).IsDirty = true;
            Assert.IsTrue((AmusementPark.ParkArea[5, 5] as Sidewalk).IsDirty);
            TicksOf25();
            Assert.IsFalse((AmusementPark.ParkArea[5, 5] as Sidewalk).IsDirty);

            AmusementPark.NewGame();
            BuildWalkSides(AmusementPark);
            Assert.IsFalse((AmusementPark.ParkArea[14, 0] as Sidewalk).IsDirty);
            (AmusementPark.ParkArea[14, 0] as Sidewalk).IsDirty = true;
            Assert.IsTrue((AmusementPark.ParkArea[14, 0] as Sidewalk).IsDirty);
            Janitor j = new Janitor(AmusementPark);
            j.ChooseDest();
            j.DestinationPosition = new Position(14, 0);
        }
        [TestMethod]
        public void TestGoHome() {
            AmusementPark.ChangeJanitorNum(5);
            Assert.AreEqual(AmusementPark.Janitors.Count, 5);
            AmusementPark.Janitors[2].GoHome();
            Assert.AreEqual(AmusementPark.Janitors.Count, 4);
            AmusementPark.Janitors[3].GoHome();
            Assert.AreEqual(AmusementPark.Janitors.Count, 3);
        }
        [TestMethod]
        public void TestStopWorking() {
            AmusementPark.ChangeJanitorNum(5);
            Assert.AreEqual(5, AmusementPark.CountWorkingJanitors());
            BuildWalkSides(AmusementPark);
            AmusementPark.OpenPark();
            (AmusementPark.ParkArea[14, 0] as Sidewalk).IsDirty = true;
            TicksOf25();

            //kiinduló pozíció, ha kirúgjuk, akkor el kezdjen indulni a kijárat felé
            int x = AmusementPark.Janitors[2].Pos.X;
            int y = AmusementPark.Janitors[2].Pos.Y;
            AmusementPark.Janitors[2].StopWorking();
            Assert.AreEqual(4, AmusementPark.CountWorkingJanitors());
            Assert.AreEqual(5, AmusementPark.Janitors.Count);
            Assert.AreEqual(AmusementPark.Janitors[2].State, "Fired");
            AmusementPark.TimerTickedInView();
            AmusementPark.TimerTickedInView();
            AmusementPark.TimerTickedInView();
            Assert.IsTrue(AmusementPark.Janitors[2].Pos.X != x ||
                          AmusementPark.Janitors[2].Pos.Y != y);

            //Ha kiér, akkor kikerül a takarítók közül
            TicksOf25();
            Assert.AreEqual(4, AmusementPark.Janitors.Count);
            Assert.AreEqual(4, AmusementPark.CountWorkingJanitors());
        }
        #endregion

        #region TestVisitorPublicMethods
        [TestMethod]
        public void TestVisitorChooseDest() {
            BuildWalkSides(AmusementPark);
            Visitor visitor = new Visitor(AmusementPark);
            Assert.IsTrue(visitor.Destination == null);
            visitor.ChooseDest();
            Assert.IsTrue(visitor.Destination == null && AmusementPark.Attractions.Count == 0 && AmusementPark.Restaurants.Count == 0);

            // Hangulat és éhség befolyása
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            AmusementPark.BuildItem(5, 6, "FerrisWheel");
            AmusementPark.ParkArea[16, 0].State = State.Wait;
            AmusementPark.ParkArea[5,6].State = State.Wait;
            InitVisitor(AmusementPark);
            Assert.IsTrue(AmusementPark.Visitors.Count >= 1);
            Assert.AreEqual(AmusementPark.Attractions.Count, 2);
            AmusementPark.Visitors[0].ChangeSatiety(-200);
            AmusementPark.Visitors[0].ChooseDest();
            Assert.IsTrue(AmusementPark.Visitors[0].Destination == null);
            AmusementPark.Visitors[0].ChangeSatiety(80);
            AmusementPark.Visitors[0].ChangeMood(-200);
            AmusementPark.Visitors[0].ChooseDest();
            Assert.IsNull(AmusementPark.Visitors[0].Destination);
            AmusementPark.Visitors[0].ChangeMood(60);
            AmusementPark.Visitors[0].ChooseDest();
            Assert.IsNotNull(AmusementPark.Visitors[0].Destination);

            AmusementPark.NewGame();
            BuildWalkSides(AmusementPark);
            Visitor v = new Visitor(AmusementPark);
            AmusementPark.Visitors.Add(v);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            AmusementPark.ParkArea[16, 0].State = State.Wait;
            v.ChooseDest();
            Assert.IsTrue(!(v.WantsToUse(AmusementPark.Attractions[0].AdrenalinFact, AmusementPark.Attractions[0].CostOfUse) && AmusementPark.Attractions[0].Reachable)
                || AmusementPark.Visitors[0].Destination.ToString() == "RollerCoasterTycoon.Model.ParkItems.Items.RollerCoaster");

            //Ha nem elérhetõ egy item, azt sosem választja ki
            AmusementPark.BuildItem(20, 30, "Carousel");
            AmusementPark.ParkArea[20, 30].State = State.Wait;
            InitVisitor(AmusementPark);
            bool choseCarousel = false;
            Position pos = new Position(20, 30);
            Attraction a;
            for (int i = 0; i < 20; i++) {
                a = AmusementPark.ChooseAttraction(AmusementPark.Visitors[0]);
                choseCarousel = choseCarousel || (a != null && pos.Equals(a.Pos));
            }
            Assert.IsFalse(choseCarousel);

            // ha nem tudja egyiket sem kifizetni, hazamegy
            AmusementPark.NewGame();
            BuildWalkSides(AmusementPark); 
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            AmusementPark.ParkArea[16, 0].CostOfUse = 0; 
            AmusementPark.BuildItem(5, 6, "FerrisWheel");
            AmusementPark.ParkArea[5, 6].CostOfUse = 0;
            AmusementPark.ParkArea[16, 0].State = State.Wait;
            AmusementPark.ParkArea[5, 6].State = State.Wait;
            Visitor visitor2 = new Visitor(AmusementPark);
            visitor2.ChooseDest();
            Assert.IsNotNull(visitor2.Destination);
            AmusementPark.ParkArea[16, 0].CostOfUse = visitor2.Money + 1;

            AmusementPark.ParkArea[5, 6].CostOfUse = visitor2.Money + 1;
            visitor2.ChooseDest();
            Assert.IsNull(visitor2.Destination);


            //Leesik az éhsége a hangulat alá, akkor éttermet választ
            Visitor visitor3 = new Visitor(AmusementPark);
            AmusementPark.BuildItem(0, 1, "Buffet");
            visitor3.ChangeSatiety(-30);
            visitor3.ChooseDest();
            Assert.IsTrue(!visitor3.WillingToPay(AmusementPark.ParkArea[0,1].CostOfUse) || visitor3.Destination.ToString() == "RollerCoasterTycoon.Model.ParkItems.Items.Buffet");

            //nem elérhetõ éttermet nem választ
            AmusementPark.BuildItem(40, 40, "Langos");
            bool chosenLangos = false;
            for(int i=0; i<20; i++) {
                visitor3.ChooseDest();
                chosenLangos = chosenLangos || visitor3.Destination.ToString() == "RollerCoasterTycoon.Model.ParkItems.Items.Langos";
            }
            Assert.IsFalse(chosenLangos);

        }

        [TestMethod]
        public void TestGoHomeVisitor() {
            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            InitMoreVisitors(AmusementPark);
            Visitor vis = AmusementPark.Visitors[0];
            Visitor vis1 = AmusementPark.Visitors[1];
            vis.ChooseDest();
            Assert.IsTrue(vis.Destination == null || vis.Destination.ToString() == "RollerCoasterTycoon.Model.ParkItems.Items.RollerCoaster");
            vis.GoHome();
            TicksOf25();
            for (int i = 0; i < AmusementPark.Visitors.Count; i++) {
                Assert.IsTrue(AmusementPark.Visitors[i] != vis);
            }
            vis1.GoHome();
            TicksOf25();
            for (int i = 0; i < AmusementPark.Visitors.Count; i++) {
                Assert.IsTrue(AmusementPark.Visitors[i] != vis1);
            }
        }

        [TestMethod]
        public void TestWillingToPay() {

            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            Assert.AreEqual(AmusementPark.Attractions.Count, 1);
            InitVisitor(AmusementPark);
            Visitor vis = AmusementPark.Visitors[0];

            AmusementPark.Visitors.Add(vis);
            Assert.IsFalse(vis.WillingToPay(vis.Money + 1));
            Assert.IsTrue(vis.WillingToPay(1));
        }
        [TestMethod]
        public void TestPay() {
            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            Assert.AreEqual(AmusementPark.Attractions.Count, 1);
            InitVisitor(AmusementPark);
            Visitor vis = AmusementPark.Visitors[0];
            int money = vis.Money;
            vis.Pay(10);
            Assert.AreEqual(vis.Money, money -= 10);
            vis.Pay(30);
            Assert.AreEqual(vis.Money, money -= 30);
        }
        [TestMethod]
        public void TestChangeMood() {
            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            InitVisitor(AmusementPark);
            Assert.AreEqual(AmusementPark.Attractions.Count, 1);
            Visitor vis = AmusementPark.Visitors[0];
            int mood = vis.Mood;
            vis.ChangeMood(-10);
            Assert.AreEqual(vis.Mood, mood -= 10);
            vis.ChangeMood(5);
            Assert.AreEqual(vis.Mood, mood += 5);

            vis.ChangeMood(300);
            Assert.AreEqual(100, vis.Mood);
            vis.ChangeMood(-500);
            Assert.AreEqual(0, vis.Mood);
        }
        [TestMethod]
        public void TestChangeSatiety() {

            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            InitVisitor(AmusementPark);
            Assert.AreEqual(AmusementPark.Attractions.Count, 1);
            Visitor vis = AmusementPark.Visitors[0];
            int satiety = vis.Satiety;
            vis.ChangeSatiety(-10);
            Assert.AreEqual(vis.Satiety, satiety -= 10);
            vis.ChangeSatiety(5);
            Assert.AreEqual(vis.Satiety, satiety += 5);
            
            vis.ChangeSatiety(80);
            Assert.AreEqual(vis.Satiety, 100);
            vis.ChangeSatiety(-400);
            Assert.AreEqual(0, vis.Satiety);

        }
        #endregion

        #region TestForParkItemPublicMethods

        [TestMethod]
        public void TestFinishBuilding() {
            AmusementPark.BuildItem(10, 10, "HauntedHouse");
            Assert.AreEqual(AmusementPark.ParkArea[10, 10].State, State.Build);
            for (int i = 0; i < 25; i++) {
                AmusementPark.TimerTickedInView();
                AmusementPark.ParkArea[10, 10].FinishBuilding();
            }
            Assert.IsTrue(AmusementPark.ParkArea[10, 10].State == State.Wait);

            AmusementPark.BuildItem(23, 22, "RollerCoaster");
            Assert.AreEqual(AmusementPark.ParkArea[23, 22].State, State.Build);
            for (int i = 0; i < 16; i++) {
                AmusementPark.TimerTickedInView();
                AmusementPark.ParkArea[23, 22].FinishBuilding();
            }
            Assert.IsTrue(AmusementPark.ParkArea[23, 22].State == State.Wait);
        }
        [TestMethod]
        public void TestTimerTicked() {

            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "RollerCoaster");
            AmusementPark.OpenPark();
            AmusementPark.TimerTickedInView();
            Assert.IsTrue(AmusementPark.Visitors.Count == 0);
            AmusementPark.TimerTickedInView();
            Assert.IsTrue(AmusementPark.Visitors.Count >= 0 && AmusementPark.Visitors.Count <= 2);
            TicksOf25();
            TicksOf25();
            Assert.IsTrue(AmusementPark.ParkArea[16, 0].State != State.Build);

            Assert.IsTrue(AmusementPark.ParkArea[16, 0].Users.Count >= 1 && AmusementPark.ParkArea[16, 0].State == State.Operate ||
                          AmusementPark.ParkArea[16, 0].Users.Count == 0 && AmusementPark.ParkArea[16, 0].State == State.Wait);

            for (int i = 0; i < 1000 && AmusementPark.ParkArea[16, 0].Users.Count < 1; i++) {
                AmusementPark.TimerTickedInView();
                if (AmusementPark.ParkArea[16, 0].Line.Count >= 9) {
                    Balance = AmusementPark.Balance;
                    AmusementPark.TimerTickedInView();
                    Assert.IsTrue(AmusementPark.ParkArea[16, 0].Line.Count == 0);
                    Assert.IsTrue(AmusementPark.ParkArea[16, 0].Users.Count == 9);
                    Assert.IsTrue(AmusementPark.ParkArea[16, 0].State == State.Operate);
                    Assert.AreEqual(Balance + AmusementPark.Attractions[0].CostOfUse * AmusementPark.Attractions[0].Users.Count - AmusementPark.Attractions[0].OperationCost, AmusementPark.Balance);
                }
            }
            for (int i = 0; i <= AmusementPark.ParkArea[16, 0].TimeOfUse; i++) {
                AmusementPark.TimerTickedInView();
                if (i >= AmusementPark.ParkArea[16, 0].TimeOfUse) {
                    Assert.IsTrue(AmusementPark.ParkArea[16, 0].State == State.Wait);
                    Assert.IsTrue(AmusementPark.ParkArea[16, 0].Users.Count == 0);
                }
            }
        }
        [TestMethod]
        public void TestGetOutFromLine() {
            BuildWalkSides(AmusementPark);
            AmusementPark.BuildItem(16, 0, "FerrisWheel");
            InitVisitor(AmusementPark);
            Visitor vis = AmusementPark.Visitors[0];
            AmusementPark.ParkArea[16, 0].Line.Add(vis);
            Assert.AreEqual(AmusementPark.ParkArea[16, 0].Line.Count, 1);
            AmusementPark.ParkArea[16, 0].GetOutFromLine(vis);
            Assert.AreEqual(AmusementPark.ParkArea[16, 0].Line.Count, 0);
        }

        #endregion

        #region Functions
        public void BuildWalkSides(AmusementPark Ap) {
            for (int i = 1; i <= 15; i++) {
                AmusementPark.BuildItem(i, 0, "Sidewalk");
            }
            for (int i = 1; i <= 5; i++) {
                Ap.BuildItem(5, i, "Sidewalk");
            }
            for (int i = 1; i <= 10; i++) {
                Ap.BuildItem(12, i, "Sidewalk");
            }
        }
        public void TicksOf25() {
            for (int i = 0; i < 25; i++) {
                AmusementPark.TimerTickedInView();
            }
        }
        public void InitVisitor(AmusementPark amusementPark) {
            amusementPark.OpenPark();
            amusementPark.Visitors.Clear();
            while (amusementPark.Visitors.Count < 1) {
                amusementPark.TimerTickedInView();
            }
        }
        public void InitMoreVisitors(AmusementPark amusementPark) {
            amusementPark.OpenPark();
            while (amusementPark.Visitors.Count < 4) {
                amusementPark.TimerTickedInView();
            }
        }
        public void InitJanitors(AmusementPark amusementPark) {
            amusementPark.ChangeJanitorNum(5);
        }
        #endregion
    }
}
