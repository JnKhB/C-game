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
    /// A form in which the user can set the entrance fee.
    /// Appears when user clicks on "Entrance fee" menuitem in GameView form.
    /// </summary>
    public partial class SetElementDialog : Form
    {
        /// <summary>
        /// SetElementDialog constructor.
        /// Sets the main properties for the form of entrancefee, like backgroundcolor andlabels.
        /// Connects the EntranceFeeClick eventhandler to SetEntranceFeeButton's Click event.
        /// </summary>
        public SetElementDialog()
        {
            InitializeComponent();

            SetEntranceFeeButton.Click += new EventHandler(EntranceFeeClick);
        }

        /// <summary>
        /// This method sets the EntranceFee property from the form's textbox EntranceFeeTextBox.
        /// In case of invalid values, a MessageBoxe appears.
        /// </summary>
        /// <param name="sender">The sender button(SetEntranceFeeButton).</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void EntranceFeeClick(object sender, EventArgs e)
        {
            int parsedValue;
            if (!int.TryParse(EntranceFeeTextBox.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field!");
                return;
            }
            else if(parsedValue < 0)
            {
                MessageBox.Show("Entrance fee must be non-negative!");
                return;
            }
            else if(parsedValue >=0){
                EntranceFee = parsedValue;
                this.Close();
            }
        }
        /// <value> The value of the entrance fee./// </value>
        public int EntranceFee { get; private set; }
    }
}
