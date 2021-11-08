using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RollerCoasterTycoon.View
{
    /// <summary>
    /// A form in which the user can see the data of one ParkItem.
    /// Its controls appear dynamically when user clicks on a ParkItem in the GameView form.
    /// </summary>
    public partial class ElementSettingsDialog : Form
    {
        /// <summary>
        /// ElementSettingsDialog constructor.
        /// Sets the main properties for the form ParkItems' settings. (e.g backgroundcolor)
        /// Connects SettingClick eventhandler to SettingButton's Click event.
        /// </summary>
        public ElementSettingsDialog()
        {
            InitializeComponent();

            SettingsButton.Click += new EventHandler(SettingClick);
        }

        /// <summary>
        /// This method sets the property values from the form's textboxes: MinStart and CostOfUse
        /// In case of invalid values, a MessageBoxes appear.
        /// </summary>
        /// <param name="sender">The sender button.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void SettingClick(object sender, EventArgs e)
        {
            double parsedValue;
            int parsedValue2;
            if (MinStart != -1)
            {
                if (!double.TryParse(MinTextBox.Text, out parsedValue))
                {
                    MessageBox.Show("This is a number only field!");
                    return;
                }
                else if (parsedValue < 0 || parsedValue > 1)
                {
                    MessageBox.Show("Minimum utilization rate must be between 0 and 1!");
                    return;
                }

                if (!int.TryParse(TicketTextBox.Text, out parsedValue2))
                {
                    MessageBox.Show("This is a number only field!");
                    return;
                }
                else if (parsedValue2 < 0)
                {
                    MessageBox.Show("Ticket/Order price must be non-negative!");
                    return;
                }

                if (parsedValue >= 0 && parsedValue <= 1 && parsedValue2 >= 0)
                {
                    MinStart = parsedValue;
                    CostOfUse = parsedValue2;
                    this.Close();
                }
            }
            else
            {
                if (!int.TryParse(TicketTextBox.Text, out parsedValue2))
                {
                    MessageBox.Show("This is a number only field!");
                    return;
                }
                else if (parsedValue2 < 0)
                {
                    MessageBox.Show("Ticket/Order price must be non-negative!");
                    return;
                }
                else if (parsedValue2 >= 0)
                {
                    CostOfUse = parsedValue2;
                    this.Close();
                }
            }
        }

        /// <summary>
        /// This method change the dialog's style according to restaurant settings.
        /// The attraction-specific labels aren't seen.
        /// </summary>
        /// <param name="minstart">The mimnimum utilization rate. At restaurants it is not relevant.</param>
        /// <param name="costofuse">It is the order price for a restaurant.</param>
        public void ChangeToRestaurant(double minstart, int costofuse)
        {
            MinLabel.Visible = false;
            MinTextBox.Visible = false;
            AdrenalinLabel.Visible = false;
            MoodLabel.Text = "Satiety value: ";
            BuildTimeLabel.Visible = false;
            TicketLabel.Text = "Order price:";
            MinStart = minstart;
            CostOfUse = costofuse;
        }

        /// <summary>
        /// This method change the dialog's style according to attraction settings.
        /// The attraction-specific labels are not visible.
        /// </summary>
        /// <param name="minstart">The mimnimum utilization rate.</param>
        /// <param name="costofuse">It is the ticket price for an attraction.</param>
        public void ChangeToAttraction(double minstart, int costofuse)
        {
            MinLabel.Visible = true;
            MinTextBox.Visible = true;
            BuildTimeLabel.Visible = true;
            MoodLabel.Text = "Mood label: ";
            TicketLabel.Text = "Ticket price:";
            MinStart = minstart;
            CostOfUse = costofuse;
        }

        /// <summary>
        /// Write out into the minstart textbox the actual minimum utilization rate value.
        /// In GameView it is known from the model.
        /// </summary>
        /// <param name="value">The value of minium utilization rate for an attraction.</param>
        public void SetMinTextBox(string value)
        {
            MinTextBox.Text = value;
        }

        /// <summary>
        /// Write out into the costofuse textbox the actual ticket or order price value.
        /// In GameView it is known from the model.
        /// </summary>
        /// <param name="value">The value of ticket or order price for an attraction or a restaurant.</param>
        public void SetCostOfUseTextBox(string value)
        {
            TicketTextBox.Text = value;
        }

        /// <summary>
        /// Sets the text of adrenalin label.
        /// It writes out the adrenalin factor of one attraction.
        /// </summary>
        /// <param name="value">The andrenalin factor of one attraction in string format.</param>
        public void SetAdrenalinLabel(string value)
        {
            AdrenalinLabel.Text = "Adrenalin factor: " + value;
        }


        /// <summary>
        /// Sets the labels' texts for attractions with specific values.
        /// </summary>
        /// <param name="name">Attraction's name.</param>
        /// <param name="text">Current state of specific attraction.</param>
        /// <param name="nums">Property values of an attraction.</param>
        public void SetAttractionLabels(string name, string text, List<int> nums)
        {
            this.Text = name;
            StateLabel.Text = "State: " + text;

            MaintenanceLabel.Text = "Maintenance cost: " + nums[0].ToString();
            OperationLabel.Text = "Operation cost: " + nums[1].ToString();
            MoodLabel.Text = "Mood value: " + nums[2].ToString();
            CapacityLabel.Text = "Capacity: " + nums[3].ToString();
            UsersLabel.Text = "Users: " + nums[4].ToString();
            LineLabel.Text = "Guests in line: " + nums[5].ToString();
            UseTimeLabel.Text = "Time of use: " + nums[6].ToString();
            BuildTimeLabel.Text = "Time of build: " + nums[7].ToString();
        }

        /// <summary>
        /// Sets the labels' texts for restaurants with specific values.
        /// </summary>
        /// <param name="name">Restaurant's name.</param>
        /// <param name="text">Current state of specific restaurant.</param>
        /// <param name="nums">Property values of a restaurant</param>
        public void SetRestaurantLabels(string name, string text, List<int> nums)
        {
            this.Text = name;
            StateLabel.Text = "State: " + text;

            MaintenanceLabel.Text = "Maintenance cost: " + nums[0].ToString();
            OperationLabel.Text = "Cost of sold products: " + nums[1].ToString();
            MoodLabel.Text = "Satiety value: " + nums[2].ToString();
            CapacityLabel.Text = "Capacity: " + nums[3].ToString();
            UsersLabel.Text = "Users: " + nums[4].ToString();
            LineLabel.Text = "Guests in line: " + nums[5].ToString();
            UseTimeLabel.Text = "Time of use: " + nums[6].ToString();
            
        }

        /// <value> The value of the Minimum utilization rate for one attraction./// </value>
        public double MinStart { get; private set; }

        /// <value> The value of the ticket price or order price rate for an attraction or for a restaurant./// </value>
        public int CostOfUse { get; private set; }

    }
}
