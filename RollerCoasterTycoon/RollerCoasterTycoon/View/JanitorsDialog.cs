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
    /// A form in which the user can see the current number of janitors and cost of one janitor.
    /// Appears when user clicks on "Janitors" menuitem in GameView form.
    /// </summary>
    public partial class JanitorsDialog : Form
    {
        /// <value>The number of working janitors./// </value>
        public int NumberOfJanitors { get; private set; }

        /// <summary>
        /// JanitorsDialog constructor.
        /// Sets the main properties for the form of guests' data, like backgroundcolor and labels.
        /// Connects JanitorsButtonClick eventhandler to JanitorsButton's Click event.
        /// </summary>
        public JanitorsDialog()
        {
            InitializeComponent();
            JanitorsButton.Click += new EventHandler(JanitorsButtonClick);
        }

        /// <summary>
        /// Sets the NumberOfJanitors property value and close the dialog form.
        /// </summary>
        /// <param name="sender">The sender button (JanitorsButton).</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void JanitorsButtonClick(object sender, EventArgs e)
        {
            NumberOfJanitors = (int)JanitorsNumericUpDown.Value;
            Close();
        }

        /// <summary>
        /// Sets the JanitorsNumericUpDown's value to the number of currently working janitors.
        /// The number is known from the model in GameView.
        /// </summary>
        /// <param name="value">Number of working a janitors in string format.</param>
        public void SetJanitorsNumericUpDown(int value)
        {
            JanitorsNumericUpDown.Value = value;
            NumberOfJanitors = value;
        }

        /// <summary>
        /// Sets the CostOfJanitorLabel.
        /// </summary>
        /// <param name="str">The cost of janitor.</param>
        public void SetCostOfJanitorLabel(string str)
        {
            CostOfJanitorLabel.Text = "Cost of janitor: " + str;
        }
    }
}
