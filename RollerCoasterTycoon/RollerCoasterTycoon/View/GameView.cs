using RollerCoasterTycoon.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RollerCoasterTycoon.Model.ParkItems;
using RollerCoasterTycoon.Model.ParkItems.Items;
using System.Diagnostics;
using System.Resources;
using RollerCoasterTycoon.Properties;
using RollerCoasterTycoon.Model.EventArgument;
using System.Reflection;
using RollerCoasterTycoon.View;

namespace RollerCoasterTycoon
{
    /// <summary>
    /// The main form, it shows the whole game.
    /// Shown elements: parkitems, time, balance, currently building item, visitors, janitors, menuitems and entrance.
    /// </summary>
    public partial class GameView : Form
    {
        #region Fields
        ///<value>A grid with buttons, the buttons's poperties are shown in the form.///</value>
        private Button[,] buttonGrid;

        ///<value>A timer to measure time in the game.///</value>
        private Timer timer;

        private BuildElementDialog buildElementDialog;

        ///<value>The amusementpark's model.///</value>
        private AmusementPark model;

        ///<value>The current parkitem what the user wants to build.///</value>
        private string currentItem;

        private int elapsedTime;

        private SetElementDialog setElementDialog;

        private ElementSettingsDialog elementSettingsDialog;

        ///<value>
        ///A table with buttons.
        ///It gets its properties form buttongrid.
        ///It is shown in the form, and it is neccessary to be able to move the view in the park.
        /// ///</value>
        private TableLayoutPanel tableFix;

        /// <summary>
        /// The which needs to be add to tablefix x coordinates to get button properties form buttonGrid.
        /// </summary>
        private int scrollX;

        /// <summary>
        /// The which needs to be add to tablefix x coordinates to get button properties form buttonGrid.
        /// </summary>
        private int scrollY;

        private JanitorsDialog janitorsDialog;

        private GuestsDialog guestsDialog;
        #endregion

        #region Constructor
        /// <summary>
        /// GameView constructor.
        /// Sets the main properties for the form main game view (e.g backgroundcolor, labels and menuitems).
        /// Connects every evenethandler to the events.
        /// Starts the timer.
        /// Initializes every field and dialog form.
        /// Makes a gametable.
        /// </summary>
        public GameView()
        {
            InitializeComponent();

            //timer
            timer = new Timer();
            timer.Interval = 1000;  //1 mp-s tickelés
            timer.Start();

            //model
            model = new AmusementPark();

            //eventhandlers
            timer.Tick += new EventHandler(TimerTick);
            NewGameButton.Click += new EventHandler(NewGameClick);
            ExitButton.Click += new EventHandler(ExitGameClick);
            AttractionsMenu.Click += new EventHandler(GamesClick);
            RestaurantsMenu.Click += new EventHandler(RestaurantsClick);
            PlantsMenu.Click += new EventHandler(PlantsClick);
            SidewalkMenu.Click += new EventHandler(SidewalksClick);
            EntranceFeeButton.Click += new EventHandler(EntranceFeeClick);
            model.BalanceChanged += new EventHandler(BalanceChange);
            model.EndOfBuilding += new EventHandler<EndOfBuildingEventArgs>(EndOfBuilding);
            OpenParkButton.Click += new EventHandler(OpenParkClick);
            model.GameOverEvent += new EventHandler(GameOver);
            this.KeyDown += new KeyEventHandler(KeyPressed);
            JanitorsMenu.Click += new EventHandler(JanitorsClick);
            GuestsMenu.Click += new EventHandler(GuestsClick);
            model.SidewalkDirtinessChanged += new EventHandler<SidewalkDirtinessChangedEventArgs>(SidewalkDirtinessChanged);

            //table for moving
            tableFix = new TableLayoutPanel();
            tableFix.RowCount = 20;
            tableFix.ColumnCount = 20;
            tableFix.Size = GamePanel.Size;
            tableFix.Dock = DockStyle.Fill;

            //x,y for scrolling
            scrollX = 0;
            scrollY = 0;

            //make a gametable
            GenerateTable();

            //set entrance
            buttonGrid[model.Entrance.Y, model.Entrance.X].BackgroundImage = Image.FromFile("../../../Resources/gate_closed.png");
            buttonGrid[model.Entrance.Y, model.Entrance.X].BackColor = Color.DarkGray;
            tableFix.GetControlFromPosition(model.Entrance.Y, model.Entrance.X).BackgroundImage = Image.FromFile("../../../Resources/gate_closed.png");
            tableFix.GetControlFromPosition(model.Entrance.Y, model.Entrance.X).BackColor = Color.DarkGray;

            //initilaize dialogs
            buildElementDialog = new BuildElementDialog();
            setElementDialog = new SetElementDialog();
            elementSettingsDialog = new ElementSettingsDialog();
            janitorsDialog = new JanitorsDialog();
            guestsDialog = new GuestsDialog();

            //write out the balance
            BalanceLabel.Text = "Balance: " + model.Balance.ToString() + " | ";
            currentItem = "";

            elapsedTime = 480;

            //time label
            TimerLabel.Text = "Time: 08:00";

            //entrancefeelabel
            EntranceFeeLabel.Text = " | Entrance fee: " + model.EntranceFee.ToString();

            //gamepanel
            GamePanel.Dock = DockStyle.Fill;
        }

        #endregion

        #region Menu eventhandlers

        /// <summary>
        /// Calls the NewGame() method
        /// </summary>
        /// <param name="sender">The sender is the NewGameButton</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void NewGameClick(Object sender, EventArgs e)
        {
            NewGame();
        }

        /// <summary>
        /// Shows the BuildElementDialog with pictures of the attractions and labels with informations (name, size, price).
        /// Picture's are backgroundimages of buttons.
        /// </summary>
        /// <param name="sender">The sender is the AttractionsMenu.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void GamesClick(Object sender, EventArgs e)
        {

            //clear the dialog
            buildElementDialog.Controls.Clear();

            //sets dilog's size
            buildElementDialog.Size = new Size(800, 600);
            buildElementDialog.MaximumSize = new Size(800, 600);
            buildElementDialog.MinimumSize = new Size(800, 600);

            //attraction buttons
            string carouselPicture = "../../../Resources/carousel_button.png";
            string rollercoasterPicture = "../../../Resources/rollercoaster_button.png";
            string ferriswheelPicture = "../../../Resources/ferriswheel_button.png";
            string hauntedhousePicture = "../../../Resources/hauntedhouse_button.png";
            Button CarouselButton = MakeButton(95, 110, carouselPicture);
            CarouselButton.Name = "Carousel";
            Button RollerCoasterButton = MakeButton(500, 110, rollercoasterPicture);
            RollerCoasterButton.Name = "RollerCoaster";
            Button FerrisWheelButton = MakeButton(95, 395, ferriswheelPicture);
            FerrisWheelButton.Name = "FerrisWheel";
            Button HauntedHouseButton = MakeButton(500, 395, hauntedhousePicture);
            HauntedHouseButton.Name = "HauntedHouse";
            CarouselButton.Click += new EventHandler(BuildElementClick);
            RollerCoasterButton.Click += new EventHandler(BuildElementClick);
            FerrisWheelButton.Click += new EventHandler(BuildElementClick);
            HauntedHouseButton.Click += new EventHandler(BuildElementClick);
            buildElementDialog.Controls.Add(CarouselButton);
            buildElementDialog.Controls.Add(RollerCoasterButton);
            buildElementDialog.Controls.Add(FerrisWheelButton);
            buildElementDialog.Controls.Add(HauntedHouseButton);

            //attraction labels
            string carouselText = Carousel.Name + "\r\n" + "Price: " + Carousel.Price + "\r\n" +
                "Size: " + Carousel.Width + "x" + Carousel.Height;
            string rollercoasterText = RollerCoaster.Name + "\r\n" + "Price: " + RollerCoaster.Price + "\r\n" +
                "Size: " + RollerCoaster.Width + "x" + RollerCoaster.Height;
            string ferriswheelText = FerrisWheel.Name + "\r\n" + "Price: " + FerrisWheel.Price + "\r\n" +
                "Size: " + FerrisWheel.Width + "x" + FerrisWheel.Height;
            string hauntedhouseText = HauntedHouse.Name + "\r\n" + "Price: " + HauntedHouse.Price + "\r\n" +
                "Size: " + HauntedHouse.Width + "x" + HauntedHouse.Height;
            Label carouselLabel = MakeLabel(carouselText, 84, 18);
            Label rollercoasterLabel = MakeLabel(rollercoasterText, 501, 18);
            Label ferriswheelLabel = MakeLabel(ferriswheelText, 84, 304);
            Label hauntedhouseLabel = MakeLabel(hauntedhouseText, 501, 304);
            buildElementDialog.Controls.Add(carouselLabel);
            buildElementDialog.Controls.Add(rollercoasterLabel);
            buildElementDialog.Controls.Add(ferriswheelLabel);
            buildElementDialog.Controls.Add(hauntedhouseLabel);

            //show the dialog
            buildElementDialog.Text = "Attractions";
            buildElementDialog.ShowDialog(this);
        }

        /// <summary>
        /// Shows the BuildElementDialog with pictures of the restaurants and labels with informations (name, size, price).
        /// Picture's are backgroundimages of buttons.
        /// </summary>
        /// <param name="sender">The sender is the RestaurantsMenu.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void RestaurantsClick(Object sender, EventArgs e)
        {

            //clear the dialog
            buildElementDialog.Controls.Clear();

            //set dialog's size
            buildElementDialog.Size = new Size(800, 600);
            buildElementDialog.MaximumSize = new Size(800, 600);
            buildElementDialog.MinimumSize = new Size(800, 600);

            //restaurant buttons
            string buffetPicture = "../../../Resources/buffet_button.png";
            string chimneycakePicture = "../../../Resources/chimneycake_button.png";
            string langosPicture = "../../../Resources/langos_button.png";
            string sweetsPicture = "../../../Resources/sweets_button.png";
            Button BuffetButton = MakeButton(95, 110, buffetPicture);
            BuffetButton.Name = "Buffet";
            Button ChimneyCakeButton = MakeButton(500, 110, chimneycakePicture);
            ChimneyCakeButton.Name = "ChimneyCake";
            Button LangosButton = MakeButton(95, 395, langosPicture);
            LangosButton.Name = "Langos";
            Button SweetsButton = MakeButton(500, 395, sweetsPicture);
            SweetsButton.Name = "Sweets";
            BuffetButton.Click += new EventHandler(BuildElementClick);
            ChimneyCakeButton.Click += new EventHandler(BuildElementClick);
            LangosButton.Click += new EventHandler(BuildElementClick);
            SweetsButton.Click += new EventHandler(BuildElementClick);
            buildElementDialog.Controls.Add(BuffetButton);
            buildElementDialog.Controls.Add(ChimneyCakeButton);
            buildElementDialog.Controls.Add(LangosButton);
            buildElementDialog.Controls.Add(SweetsButton);

            //restaurant labels
            string buffetText = Buffet.Name + "\r\n" + "Price: " + Buffet.Price + "\r\n" +
                "Size: " + Buffet.Width + "x" + Buffet.Height;
            string chimneycakeText = ChimneyCake.Name + "\r\n" + "Price: " + ChimneyCake.Price + "\r\n" +
                "Size: " + ChimneyCake.Width + "x" + ChimneyCake.Height;
            string langosText = Langos.Name + "\r\n" + "Price: " + Langos.Price + "\r\n" +
                "Size: " + Langos.Width + "x" + Langos.Height;
            string sweetsText = Sweets.Name + "\r\n" + "Price: " + Sweets.Price + "\r\n" +
                "Size: " + Sweets.Width + "x" + Sweets.Height;
            Label buffetLabel = MakeLabel(buffetText, 84, 18);
            Label chimneycakeLabel = MakeLabel(chimneycakeText, 501, 18);
            Label langosLabel = MakeLabel(langosText, 84, 304);
            Label sweetsLabel = MakeLabel(sweetsText, 501, 304);
            buildElementDialog.Controls.Add(buffetLabel);
            buildElementDialog.Controls.Add(chimneycakeLabel);
            buildElementDialog.Controls.Add(langosLabel);
            buildElementDialog.Controls.Add(sweetsLabel);

            //show the dialog
            buildElementDialog.Text = "Restaurants";
            buildElementDialog.ShowDialog(this);
        }

        /// <summary>
        /// Shows the BuildElementDialog with pictures of plants and labels with informations (name, size, price).
        /// Picture's are backgroundimages of buttons.
        /// </summary>
        /// <param name="sender">The sender is the PlantsMenu.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void PlantsClick(Object sender, EventArgs e)
        {

            //clear dialog
            buildElementDialog.Controls.Clear();

            //set dialog's size
            buildElementDialog.Size = new Size(800, 600);
            buildElementDialog.MaximumSize = new Size(800, 600);
            buildElementDialog.MinimumSize = new Size(800, 600);

            //plant buttons
            string grassPicture = "../../../Resources/grass_button.png";
            string bushPicture = "../../../Resources/bush_button.png";
            string treePicture = "../../../Resources/tree_button.png";
            Button grassButton = MakeButton(95, 110, grassPicture);
            grassButton.Name = "Grass";
            Button bushButton = MakeButton(500, 110, bushPicture);
            bushButton.Name = "Bush";
            Button treeButton = MakeButton(300, 395, treePicture);
            treeButton.Name = "Tree";
            grassButton.Click += new EventHandler(BuildElementClick);
            bushButton.Click += new EventHandler(BuildElementClick);
            treeButton.Click += new EventHandler(BuildElementClick);
            buildElementDialog.Controls.Add(grassButton);
            buildElementDialog.Controls.Add(bushButton);
            buildElementDialog.Controls.Add(treeButton);

            //plant labels
            string grassText = Model.ParkItems.PlantType.Grass + "\r\n" + "Size: " + Model.ParkItems.Plant.Width + "x" + Model.ParkItems.Plant.Height + "\r\n" + "Price: " + Plant.Price;
            string bushText = Model.ParkItems.PlantType.Bush + "\r\n" + "Size: " + Model.ParkItems.Plant.Width + "x" + Model.ParkItems.Plant.Height + "\r\n" + "Price: " + Plant.Price;
            string treeText = Model.ParkItems.PlantType.Tree + "\r\n" + "Size: " + Model.ParkItems.Plant.Width + "x" + Model.ParkItems.Plant.Height + "\r\n" + "Price: " + Plant.Price;
            Label grassLabel = MakeLabel(grassText, 84, 18);
            Label bushLabel = MakeLabel(bushText, 501, 18);
            Label treeLabel = MakeLabel(treeText, 290, 304);
            buildElementDialog.Controls.Add(grassLabel);
            buildElementDialog.Controls.Add(bushLabel);
            buildElementDialog.Controls.Add(treeLabel);

            //show dialog
            buildElementDialog.Text = "Plants";
            buildElementDialog.ShowDialog(this);
        }

        /// <summary>
        /// Shows the BuildElementDialog with pictures of a sidewalk and of a rubbish bin and labels with informations (name, size, price).
        /// Picture's are backgroundimages of buttons.
        /// </summary>
        /// <param name="sender">The sender is the SidewalksMenu.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void SidewalksClick(Object sender, EventArgs e)
        {

            //clear dialog
            buildElementDialog.Controls.Clear();

            //set dialog's size
            buildElementDialog.Size = new Size(800, 600);
            buildElementDialog.MaximumSize = new Size(800, 600);
            buildElementDialog.MinimumSize = new Size(800, 600);

            //sidewalk buttons
            string binPicture = "../../../Resources/bin_button.png";
            Button sidewalkButton = MakeButton(95, 225, binPicture);
            sidewalkButton.BackgroundImage = null;
            sidewalkButton.Name = "Sidewalk";
            sidewalkButton.BackColor = Color.DarkGray;
            Button binButton = MakeButton(500, 225, binPicture);
            binButton.BackColor = Color.DarkGray;
            binButton.Name = "Bin";
            buildElementDialog.Controls.Add(sidewalkButton);
            sidewalkButton.Click += new EventHandler(BuildElementClick);
            binButton.Click += new EventHandler(BuildElementClick);
            buildElementDialog.Controls.Add(binButton);

            //sidewalk labels
            string sidewalkText = Model.ParkItems.Sidewalk.Name + "\r\n" + "Size: " + Model.ParkItems.Sidewalk.Width + "x" + Model.ParkItems.Sidewalk.Height + "\r\n" + "Price: " + Sidewalk.Price;
            string binText = "Rubbish bin" + "\r\n" + "Size: " + Model.ParkItems.Sidewalk.Width + "x" + Model.ParkItems.Sidewalk.Height + "\r\n" + "Price: " + Sidewalk.BinPrice;
            Label sidewalkLabel = MakeLabel(sidewalkText, 84, 130);
            Label binLabel = MakeLabel(binText, 501, 130);
            buildElementDialog.Controls.Add(sidewalkLabel);
            buildElementDialog.Controls.Add(binLabel);

            //show dialog
            buildElementDialog.Text = "Sidewalk";
            buildElementDialog.ShowDialog(this);
        }

        /// <summary>
        /// Shows the SetElementDialog with a label a textbox and a button.
        /// Sets the model's entrance fee.
        /// Entrace fee is visible on the main game form.
        /// </summary>
        /// <param name="sender">The sender is the EntranceFeeButton.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void EntranceFeeClick(Object sender, EventArgs e)
        {

            setElementDialog.ShowDialog(this);
            model.ChangeEntranceFee(setElementDialog.EntranceFee);
            EntranceFeeLabel.Text = " | Entrance fee: " + model.EntranceFee.ToString();
        }

        /// <summary>
        /// Shows the JanitorsDialog with labels, a button and a numericupdown.
        /// Sets the number of working janitors in model.
        /// </summary>
        /// <param name="sender">The sender is the JanitorsMenu.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void JanitorsClick(object sender, EventArgs e)
        {
            janitorsDialog.SetCostOfJanitorLabel(Janitor.Wage.ToString());
            janitorsDialog.ShowDialog(this);
            model.ChangeJanitorNum(janitorsDialog.NumberOfJanitors);
            janitorsDialog.SetJanitorsNumericUpDown(model.CountWorkingJanitors());
        }

        /// <summary>
        /// Shows the JanitorsDialog with a datadridview, and shows the visitors' state.
        /// </summary>
        /// <param name="sender">The sender is the GuestsMenu.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void GuestsClick(object sender, EventArgs e)
        {

            guestsDialog.SetVisitors(model.Visitors);
            guestsDialog.FillDataGrid();
            guestsDialog.ShowDialog(this);
        }

        /// <summary>
        /// Opens the amusementpark in model.
        /// Sets OpenParkButton to disabled. (the user can open only once the park).
        /// Sets entrance picture.
        /// </summary>
        /// <param name="sender">The sender is the OpenParkButton.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void OpenParkClick(object sender, EventArgs e)
        { 
            
            model.OpenPark();
            if (model.IsOpen)
            {
                buttonGrid[model.Entrance.Y, model.Entrance.X].BackgroundImage = Image.FromFile("../../../Resources/gate_opened.png");
                OpenParkButton.Enabled = false;

            }
            else
            {
                buttonGrid[model.Entrance.Y, model.Entrance.X].BackgroundImage = Image.FromFile("../../../Resources/gate_closed.png");
                OpenParkButton.Enabled = true;
            }

            RefreshTable();
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="sender">The sender is the ExitButton.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void ExitGameClick(Object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Model eventhandlers

        /// <summary>
        /// Changes the dirtiness of one sidewalk.
        /// If sidewalk is dirty, it shows rubbish, if sidewalk is clean, it doesn't show rubbish.
        /// </summary>
        /// <param name="sender">The amusementpark model.</param>
        /// <param name="e">Position of the changed sidewalk.</param>
        private void SidewalkDirtinessChanged(object sender, SidewalkDirtinessChangedEventArgs e)
        {
            if (e.IsDirty)
            {
                buttonGrid[e.Position.Y, e.Position.X].Text = "*";
                
                buttonGrid[e.Position.Y, e.Position.X].ForeColor = Color.Olive;
                buttonGrid[e.Position.Y, e.Position.X].Font = new Font("Gill Sans Ultra Bold", 16, FontStyle.Bold);
                buttonGrid[e.Position.Y, e.Position.X].TextAlign = ContentAlignment.MiddleCenter;
            }
            else {
                buttonGrid[e.Position.Y, e.Position.X].Text = null;
            }

            RefreshTable();
            
        }

        /// <summary>
        /// Changes the dirtiness of one sidewalk.
        /// If sidewalk is dirty, it shows rubbish, if sidewalk is clean, it doesn't show rubbish.
        /// </summary>
        /// <param name="sender">The amusementpark model.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void GameOver(object sender, EventArgs e)
        {
            //stop timer
            timer.Stop();

            //messagebox
            string message = "Játék vége! Elfogyott a pénzed / nincsenek emberek..";
            string caption = "Game Over";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        /// <summary>
        /// Changes the picture of built parkitems.
        /// If the building time is over the normal picture of the attraction can be seen.
        /// </summary>
        /// <param name="sender">The amusementpark model.</param>
        /// <param name="e">The built attraction.</param>
        private void EndOfBuilding(object sender, EndOfBuildingEventArgs e)
        {
            int x = e.Attraction.Pos.Y;
            int y = e.Attraction.Pos.X;

            int k = 0;
            for (int i = x; i < x + model.GetHeight(y, x); ++i)
            {
                int s = 0;
                for (int j = y; j < y + model.GetWidth(y, x); ++j)
                {
                    string img = (string)(e.Attraction.GetItemType()).GetProperty("Name", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);

                    Image imag = Image.FromFile("../../../Resources/" + img + ".png");
                    int widthPart = (int)((double)imag.Width / model.GetHeight(y, x));
                    int heightPart = (int)((double)imag.Height / model.GetWidth(y, x));
                    Bitmap[,] bmps = new Bitmap[model.GetHeight(y, x), model.GetWidth(y, x)];


                    bmps[k, s] = new Bitmap(widthPart, heightPart);
                    Graphics g = Graphics.FromImage(bmps[k, s]);
                    g.DrawImage(imag, new Rectangle(0, 0, widthPart, heightPart), new Rectangle(s * widthPart, k * heightPart, widthPart, heightPart), GraphicsUnit.Pixel);
                    g.Dispose();

                    buttonGrid[i, j].BackgroundImage = bmps[k, s];

                    s++;
                }
                k++;
            }
            RefreshTable();
        }

        /// <summary>
        /// Changes the balancelabel if balance changed in model.
        /// </summary>
        /// <param name="sender">The amusementpark model.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void BalanceChange(Object sender, EventArgs e)
        {
            BalanceLabel.Text = "Balance: " + model.Balance.ToString() + " | ";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates the buttonGrid and the visible tableFix with buttons.
        /// Sets the main properties, like backgroundcolor, and border.
        /// Connects eventhandlers to events:
        ///     ButtonGridClick to tableFix's buttons' MouseClick event.
        ///     OnMouseEnterButton to tableFix's buttons' MouseEnter event
        ///     OnMouseLeaveButton to tableFix's buttons' MouseLeave event
        /// </summary>
        private void GenerateTable()
        {
            buttonGrid = new Button[model.Size, model.Size];
            for (int i = 0; i < model.Size; ++i)
            {
                for (int j = 0; j < model.Size; ++j)
                {
                    buttonGrid[i, j] = new Button();
                    buttonGrid[i, j].FlatAppearance.BorderSize = 0;  //there isn't border
                    buttonGrid[i, j].BackColor = Color.Khaki;   //backgroundcolor
                    buttonGrid[i, j].TabIndex = 100 + i * model.Size + j; // button's number in the TabIndex
                }
            }

            for (int i = 0; i < tableFix.RowCount; i++)
            {
                for (int j = 0; j < tableFix.ColumnCount; j++)
                {
                    Button b = new Button();
                    b.Padding = new Padding(0);
                    b.Margin = new Padding(0);
                    b.Size = new Size(40, 40); //size
                    b.FlatStyle = FlatStyle.Flat; //borderstyle
                    b.FlatAppearance.BorderSize = 0;  //there isn't border
                    b.BackColor = Color.Khaki;   //backgroundcolor
                    b.TabIndex = 100 + i * model.Size + j; //button's number in the tabindex
                    b.MouseClick += new MouseEventHandler(ButtonGridClick); //common eventhandler for every button's click event
                    b.MouseEnter += OnMouseEnterButton;     //common eventhandler for every button's MouseEnter event
                    b.MouseLeave += OnMouseLeaveButton;     //common eventhandler for every button's MouseLeave event
                    tableFix.Controls.Add(b, j, i);
                }
            }

            GamePanel.Controls.Add(tableFix);
        }

        /// <summary>
        /// Makes a button with given parameters.
        /// </summary>
        /// <param name="x">Button's location's x coordinate.</param>
        /// <param name="y">Button's location's y coordinate.</param>
        /// <param name="picture">Path of BackgroundImage.</param>
        /// <returns>A button with specific settings of its properties.</returns>
        private Button MakeButton(int x, int y, string picture)
        {
            Button button = new Button();
            button.Size = new Size(200, 150);
            button.Location = new Point(x, y);
            button.BackgroundImage = Image.FromFile(picture);
            return button;
        }

        /// <summary>
        /// Makes a label with given parameters.
        /// </summary>
        /// <param name="text">The label's text.</param>
        /// <param name="x">Label's location's x coordinate.</param>
        /// <param name="y">Label's's location's x coordinate.</param>
        /// <returns>A label with specific settings of its properties.</returns>
        private Label MakeLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Size = new Size(230, 90);
            label.Text = text;
            label.ForeColor = Color.DarkBlue;
            label.Font = new Font("Gill Sans Ultra Bold", 10, FontStyle.Bold);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(x, y);
            return label;
        }

        /// <summary>
        /// Calls model's NewGame() method.
        /// Starts a new game.
        /// Closes the park, and set entrance's backgroundImage to closed gate.
        /// Restarts the timer.
        /// Settings for buttonGrid and TableFix. (buttons without any pictures).
        /// Sets entrance fee label.
        /// Sets currentitem to nothing.
        /// </summary>
        private void NewGame()
        {
            //new game starts in model
            model.NewGame();

            //OpenParkButton settings
            if (model.IsOpen)
            {
                buttonGrid[model.Entrance.Y, model.Entrance.X].BackgroundImage = Image.FromFile("../../../Resources/gate_opened.png");
                OpenParkButton.Enabled = false;
            }
            else
            {
                buttonGrid[model.Entrance.Y, model.Entrance.X].BackgroundImage = Image.FromFile("../../../Resources/gate_closed.png");
                OpenParkButton.Enabled = true;
            }

            //starting time
            elapsedTime = 480;

            //amusement park's left corner is shown
            scrollX = 0;
            scrollY = 0;

            //timer settings
            if (timer.Enabled)
            {
                timer.Stop();
                
                timer.Start();
                elapsedTime = 480;
            }
            else
            {
                
                timer.Start();
                elapsedTime = 480;
            }

            TimerLabel.Text = "Time: 08:00";

            //buttonGrid settings
            for (int i = 0; i < model.Size; ++i)
            {
                for (int j = 0; j < model.Size; ++j)
                {
                    buttonGrid[i, j].BackColor = Color.Khaki;
                    buttonGrid[i, j].BackgroundImage = null;
                    buttonGrid[i, j].Text = null;
                }
            }

            //tableFix settings
            for (int i = 0; i < tableFix.RowCount; i++)
            {
                for (int j = 0; j < tableFix.ColumnCount; j++)
                {
                    tableFix.GetControlFromPosition(j, i).BackColor = Color.Khaki;
                    tableFix.GetControlFromPosition(j, i).BackgroundImage = null;
                    tableFix.GetControlFromPosition(j, i).Text = null; //remove rubbish
                }
            }


            //entrancefee label
            EntranceFeeLabel.Text = " | Entrance fee: " + model.EntranceFee.ToString();

            //currentItem
            currentItem = "";
            CurrentItemLabel.Text = "| Current item: ";

            //entrance settings
            buttonGrid[model.Entrance.Y, model.Entrance.X].BackgroundImage = Image.FromFile("../../../Resources/gate_closed.png");
            buttonGrid[model.Entrance.Y, model.Entrance.X].BackColor = Color.DarkGray;
            tableFix.GetControlFromPosition(model.Entrance.Y, model.Entrance.X).BackgroundImage = Image.FromFile("../../../Resources/gate_closed.png");
            tableFix.GetControlFromPosition(model.Entrance.Y, model.Entrance.X).BackColor = Color.DarkGray;

            //janitors dialog into start style
            janitorsDialog.SetJanitorsNumericUpDown(model.CountWorkingJanitors());
            

            RefreshTable();
        }

        /// <summary>
        /// Sets tableFix's button's poperties according to buttonGrid button's properties.
        /// </summary>
        private void RefreshTable() {
            for (int i = 0; i < tableFix.RowCount; i++)
            {
                for (int j = 0; j < tableFix.ColumnCount; j++)
                {
                    tableFix.GetControlFromPosition(j, i).BackColor = buttonGrid[scrollX + i, scrollY + j].BackColor;
                    tableFix.GetControlFromPosition(j, i).BackgroundImage = buttonGrid[scrollX + i, scrollY + j].BackgroundImage;
                    tableFix.GetControlFromPosition(j, i).Text = buttonGrid[scrollX + i, scrollY + j].Text;
                }
                
            }
            
        }

        #endregion

        #region Other eventhandlers
        /// <summary>
        /// Draws the area, where currentitem is going to be built.
        /// </summary>
        /// <param name="sender">tableFix's buttons.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void OnMouseEnterButton(object sender, EventArgs e)
        {
            if (currentItem != "" && currentItem != "Janitor")
            {
                
                //get button's position
                var button = (Button)sender;
                int x = -1;
                int y = -1;
                for (int n = 0; n < tableFix.RowCount; ++n)
                {
                    for (int m = 0; m < tableFix.ColumnCount; ++m)
                    {
                        if (tableFix.GetControlFromPosition(m, n) == button)
                        {
                            x =  n;
                            y = m;
                        }
                    }
                }

                //get height and width
                Type type;

                int height;
                int width;

                if (currentItem == "Sidewalk" || currentItem == "Bin")
                {
                    type = typeof(Sidewalk);
                    height = (int)type.GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                    width = (int)type.GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);

                }
                else if (currentItem == "Grass" || currentItem == "Bush" || currentItem == "Tree")
                {
                    type = typeof(Plant);
                    height = (int)type.GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                    width = (int)type.GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                }
                else
                { 
                    type = Type.GetType("RollerCoasterTycoon.Model.ParkItems.Items." + currentItem);
                    height = (int)type.GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                    width = (int)type.GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                }

                if (tableFix.GetControlFromPosition(y, x).BackColor == Color.Khaki && tableFix.GetControlFromPosition(y, x).BackgroundImage == null)
                {
                    int i = x;
                    while (i < x + width && i != tableFix.RowCount)
                    {
                        int j = y;
                        while (j < y + height && j != tableFix.ColumnCount)
                        {
                            if (tableFix.GetControlFromPosition(j, i).BackColor == Color.Khaki && tableFix.GetControlFromPosition(j, i).BackgroundImage == null)
                            {
                                tableFix.GetControlFromPosition(j, i).Text = "X";
                            }
                            ++j;
                        }
                        ++i;
                    }
                }
            }
        }

        /// <summary>
        /// Draws back the original button settings, whne user leaves the current buttons.
        /// </summary>
        /// <param name="sender">tableFix's buttons.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void OnMouseLeaveButton(object sender, EventArgs e)
        {
            if (currentItem != "" && currentItem != "Janitor")
            {

                //get button's position
                var button = (Button)sender;
                int x = -1;
                int y = -1;
                for (int n = 0; n < tableFix.RowCount; ++n)
                {
                    for (int m = 0; m < tableFix.ColumnCount; ++m)
                    {
                        if (tableFix.GetControlFromPosition(m, n) == button)
                        {
                            x = n;
                            y = m;
                        }
                    }
                }


                //get height and width
                Type type;

                int height;
                int width;

                if (currentItem == "Sidewalk" || currentItem == "Bin")
                {
                    type = typeof(Sidewalk);
                    height = (int)type.GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                    width = (int)type.GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);

                }
                else if (currentItem == "Grass" || currentItem == "Bush" || currentItem == "Tree")
                {
                    type = typeof(Plant);
                    height = (int)type.GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                    width = (int)type.GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                }
                else
                {
                    type = Type.GetType("RollerCoasterTycoon.Model.ParkItems.Items." + currentItem);
                    height = (int)type.GetProperty("Height", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                    width = (int)type.GetProperty("Width", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
                }

                //button's settings
                int i = x;
                while (i < x + width && i != tableFix.RowCount)
                {
                    int j = y;
                    while (j < y + height && j != tableFix.ColumnCount)
                    {
                        if (tableFix.GetControlFromPosition(j, i).BackColor == Color.Khaki && tableFix.GetControlFromPosition(j, i).BackgroundImage == null)
                        {
                            tableFix.GetControlFromPosition(j, i).Text = null;
                        }
                        ++j;
                    }
                    ++i;
                }
            }
        }

        /// <summary>
        /// With keys AWSD, user can move in the park.
        /// With key backspace the user can delete the currentitem.
        /// </summary>
        /// <param name="sender">Keys.</param>
        /// <param name="e">KeyCode is used.</param>
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            tableFix.SuspendLayout();
            if (e.KeyCode == Keys.S)
            {
                if (scrollX < model.Size - tableFix.RowCount)
                {
                    scrollX++;
                    RefreshTable();
                    
                }
                
            }
            if (e.KeyCode == Keys.W)
            {
                if (scrollX > 0)
                {
                    scrollX--;
                    RefreshTable();
                    
                }
                
            }
            if (e.KeyCode == Keys.D)
            {
                if (scrollY < model.Size - tableFix.ColumnCount)
                {
                    scrollY++;
                    RefreshTable();
                    
                }
            }
            if (e.KeyCode == Keys.A)
            {
                if (scrollY > 0)
                {
                    scrollY--;
                    RefreshTable();
                    
                }
            }

            if (e.KeyCode == Keys.Back)
            {
                currentItem = "";
                CurrentItemLabel.Text = "| Current item: ";
                for (int i = 0; i < tableFix.RowCount; ++i)
                {
                    for (int j = 0; j < tableFix.ColumnCount; ++j)
                    {
                        if (tableFix.GetControlFromPosition(j, i).BackColor == Color.Khaki)
                        {
                            tableFix.GetControlFromPosition(j, i).Text = null;
                        }
                    }
                }
            }
            
            tableFix.ResumeLayout();
        }
        /// <summary>
        /// In every one second does the following things:
        ///     Calls model's TimerTickedInView method.
        ///     Increments the elapsed time.
        ///     Refreshes the TimeLabel.
        ///     Moves visitors and janitors on the gametable.
        /// </summary>
        /// <param name="sender">Timer</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void TimerTick(Object sender, EventArgs e)
        {
            this.Focus();

            model.TimerTickedInView();

            elapsedTime++;

            if (elapsedTime == 1444)
            {
                elapsedTime = 0;
            }

            string hour = "";
            if (elapsedTime / 60 < 10)
            {
                hour = "0";
            }
            hour += (elapsedTime / 60).ToString();

            string minute = "";
            if (elapsedTime % 60 < 10)
            {
                minute = "0";
            }
            minute += (elapsedTime % 60).ToString();

            if (elapsedTime % 2 == 0)
            {
                TimerLabel.Text = "Time: " + hour + ":" + minute;
            }
            else
            {
                TimerLabel.Text = "Time: " + hour + " " + minute;
            }

            //move people and janitors
            for (int i = 0; i < model.Size; i++)
            {
                for (int j = 0; j < model.Size; j++)
                {
                    if (model.IsSidewalk(j, i))
                    {
                        if (model.GetJanitorNum(j, i) > 0)
                        {
                            if (model.GetJanitorNum(j, i) <= 4)
                            {
                                buttonGrid[i, j].BackgroundImage = Image.FromFile("../../../Resources/Janitor" + model.GetJanitorNum(j, i).ToString() + ".png");
                            }
                            else
                            {
                                buttonGrid[i, j].BackgroundImage = Image.FromFile("../../../Resources/Janitor4.png");
                            }
                        }
                        else
                        {
                            if (model.GetVisitorNum(j, i) > 0 && model.GetVisitorNum(j, i) <= 5)
                            {
                                buttonGrid[i, j].BackgroundImage = Image.FromFile("../../../Resources/Person" + model.GetVisitorNum(j, i).ToString() + ".png");
                            }
                            else if (model.GetVisitorNum(j, i) > 5)
                            {
                                buttonGrid[i, j].BackgroundImage = Image.FromFile("../../../Resources/Person5.png");
                            }
                        }

                        if (model.GetVisitorNum(j, i) == 0 && model.GetJanitorNum(j, i) == 0)
                        {
                            if (model.IsBin(j, i))
                            {
                                buttonGrid[i, j].BackgroundImage = Image.FromFile("../../../Resources/Bin.png");
                            }
                            else
                            {
                                buttonGrid[i, j].BackgroundImage = null;
                            }
                            if (model.IsOpen)
                            {
                                buttonGrid[model.Entrance.Y, model.Entrance.X].BackgroundImage = Image.FromFile("../../../Resources/gate_opened.png");
                                OpenParkButton.Enabled = false;

                            }
                            else
                            {
                                buttonGrid[model.Entrance.Y, model.Entrance.X].BackgroundImage = Image.FromFile("../../../Resources/gate_closed.png");
                                OpenParkButton.Enabled = true;
                            }
                        }
                    }
                }
            }
            RefreshTable();
        }
        /// <summary>
        /// Cases:
        ///     1. When currentitem is null, ad the user clicks on a restaurant or attraction, then buildelementdialog is shown.
        ///     2. When currentitem is null, and user clicks on a janitor currentitem becomes "janitor"
        ///     and after that if user clicks on another sidewalk, the new position of one janitor will be that sidewalk.
        ///     3. when currentitem is a parkitem, then if users clicks on an empty area, that parkitem is going to be built.
        ///     4. if user ants to build a parkitem, but he/she does not have enough money or place, a messagebox appears.
        /// </summary>
        /// <param name="sender">TableFix's buttons.</param>
        /// <param name="e">MouseEventArgs</param>
        private void ButtonGridClick(Object sender, MouseEventArgs e)
        {
            GamePanel.Focus();
            var button = (Button)sender;
            int x = -1;
            int y = -1;

            for (int i = 0; i < tableFix.RowCount; ++i)
            {
                for (int j = 0; j < tableFix.ColumnCount; ++j)
                {
                    if (tableFix.GetControlFromPosition(j, i) == button)
                    {
                        x = scrollX  + i;
                        y = scrollY + j;
                    }
                }
            }
            

            if (model.ParkArea[y, x] != null && model.GetItemType(y, x) != typeof(Sidewalk) && (model.GetItemType(y, x) != typeof(Plant) || (model.ParkArea[y, x] as Plant).PlantType != PlantType.Grass))
            {
                Type type = model.GetItemType(y, x);
                List<int> nums = new List<int>();
                nums.Add(model.ParkArea[y, x].MaintenanceCost);
                nums.Add(model.ParkArea[y, x].OperationCost);
                nums.Add(model.ParkArea[y, x].MoodOrSatietyValue);
                nums.Add(model.ParkArea[y, x].Capacity);
                nums.Add(model.ParkArea[y, x].Users.Count);
                nums.Add(model.ParkArea[y, x].Line.Count);
                nums.Add(model.ParkArea[y, x].TimeOfUse);

                if (type.IsSubclassOf(typeof(Attraction)))
                {
                    elementSettingsDialog.ChangeToAttraction(model.ParkArea[y,x].StartMin, model.ParkArea[y, x].CostOfUse);
                    string name = type.Name;
                    string text = model.ParkArea[y, x].State.ToString();
                    nums.Add(model.ParkArea[y, x].BuildTime);
                    elementSettingsDialog.SetAttractionLabels(name, text, nums);
                    elementSettingsDialog.SetCostOfUseTextBox(model.ParkArea[y, x].CostOfUse.ToString());
                    elementSettingsDialog.SetMinTextBox(model.ParkArea[y, x].StartMin.ToString());
                    elementSettingsDialog.SetAdrenalinLabel(model.ParkArea[y, x].AdrenalinFact.ToString());
                    elementSettingsDialog.ShowDialog(this);
                }
                else if (type.IsSubclassOf(typeof(Restaurant)))
                {
                    elementSettingsDialog.ChangeToRestaurant(-1, model.ParkArea[y, x].CostOfUse);
                    string name = type.Name;
                    string text = model.ParkArea[y, x].State.ToString();
                    nums.Add(100);
                    elementSettingsDialog.SetRestaurantLabels(name, text, nums);
                    elementSettingsDialog.SetCostOfUseTextBox(model.ParkArea[y, x].CostOfUse.ToString());
                    elementSettingsDialog.ShowDialog(this);
                }


                if (type.IsSubclassOf(typeof(Attraction)))
                {
                    model.ChangeCostOfUse(y, x, elementSettingsDialog.CostOfUse);
                    model.ChangeStartMin(y, x, elementSettingsDialog.MinStart);
                }
                else if (type.IsSubclassOf(typeof(Restaurant)))
                {
                    model.ChangeCostOfUse(y, x, elementSettingsDialog.CostOfUse);
                }
            }
            else
            {
                if (currentItem == "")
                {
                    if (model.ParkArea[y,x] != null && model.GetItemType(y, x) == typeof(Sidewalk))
                    {
                        if (model.StopJanitor(y,x))
                        {
                            currentItem = "Janitor";
                            CurrentItemLabel.Text = "| Current item: " + currentItem;
                        }
                    }
                    
                return;
                }


                if (currentItem == "Janitor")
                {
                    if (model.MoveJanitor(y, x))
                    {
                        currentItem = "";
                        CurrentItemLabel.Text = "| Current item: " + currentItem;

                    }
                    else
                    {
                        string message = "Csak útra lehet áthelyezni takarítót!";
                        string caption = "Error relocate janitor";
                        MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        model.StartJanitor();
                        currentItem = "";
                        CurrentItemLabel.Text = "| Current item: " + currentItem;

                    }
                    return;
                }

                bool success = model.BuildItem(y, x, currentItem);
                if (success)
                {
                    int k = 0;
                    int s = 0;
                    for (int i = x; i < x + model.GetHeight(y, x); ++i)
                    {
                        s = 0;
                        for (int j = y; j < y + model.GetWidth(y, x); ++j)
                        {
                            if (currentItem == "Sidewalk") buttonGrid[i, j].BackColor = Color.DarkGray;
                            else
                            { 
                                string img = currentItem;
                                if (model.GetItemType(y, x).IsSubclassOf(typeof(Attraction)))
                                {
                                    img += "_building";
                                } 
                                
                                Image imag = Image.FromFile("../../../Resources/" + img + ".png");
                                int widthPart = (int)((double)imag.Width / model.GetHeight(y, x));
                                int heightPart = (int)((double)imag.Height / model.GetWidth(y, x));
                                Bitmap[,] bmps = new Bitmap[model.GetHeight(y, x), model.GetWidth(y, x)];
                                
                                   
                                bmps[k, s] = new Bitmap(widthPart, heightPart);
                                Graphics g = Graphics.FromImage(bmps[k, s]);
                                g.DrawImage(imag, new Rectangle(0, 0, widthPart, heightPart), new Rectangle(s * widthPart, k * heightPart, widthPart, heightPart), GraphicsUnit.Pixel);
                                g.Dispose();
                                
                                buttonGrid[i, j].BackgroundImage = bmps[k, s];
                                if (currentItem == "Bin") buttonGrid[i, j].BackColor = Color.DarkGray;
                                ++s;
                            }
                        }
                        ++k;
                    }
                }
                else if (success == false && currentItem != "")
                {
                    string message = "Nincs hely vagy nincs elég pénze az elem megépítéséhez!" + "\r\n" + "Keressen másik helyet és ellenőrizze a vagyonát!";
                    string caption = "Error building element";
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            RefreshTable();
        }
        /// <summary>
        /// Sets the currentitem, which can be a parkitem (from buildelementdialog), or janitor.
        /// </summary>
        /// <param name="sender">BuildElementDialogs' buttons</param>
        /// <param name="e"></param>
        private void BuildElementClick(Object sender, EventArgs e)
        {
            buildElementDialog.Close();
            currentItem = ((Button)sender).Name;
            CurrentItemLabel.Text = "| Current item: " + currentItem;
        }
        #endregion

    }
}
