using RollerCoasterTycoon.Model;
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
    /// A form in which the user can see the current state of each guest in a table.
    /// Appears when user clicks on "Guests" menuitem in GameView form.
    /// The visible properties: satiety, mood, money, adrenalin preference, if he/she has trash, destination
    /// </summary>
    public partial class GuestsDialog : Form
    {
        /// <summary>
        /// GuestsDialog constructor.
        /// Sets the main properties for the form of guests' data, like backgroundcolor and datagridview.
        /// Connects GuestsDataGridDataBindingComplete eventhandler to GuestsDataGrid's DataBindingComplete event.
        /// Sets the GuestsDataGrid's AutoSizeColumnMode to AllCells: columns' size will be the same as widest cell.
        /// </summary>
        public GuestsDialog()
        {
            InitializeComponent();
            GuestsDataGrid.DataBindingComplete += GuestsDataGridDataBindingComplete;
            GuestsDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Set  GuestsDataGrid's columns' visibility and header text.
        /// </summary>
        /// <param name="sender">The GuestsDataGrid.</param>
        /// <param name="e">There aren't any given parameters.</param>
        private void GuestsDataGridDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            GuestsDataGrid.Columns["MaxMood"].Visible = false;
            GuestsDataGrid.Columns["MaxSatiety"].Visible = false;
            GuestsDataGrid.Columns["ThrowTrashTime"].Visible = false;
            GuestsDataGrid.Columns["Pos"].Visible = false;
            GuestsDataGrid.Columns["Destination"].Visible = false;
            GuestsDataGrid.Columns["DestinationPosition"].Visible = false;
            GuestsDataGrid.Columns["DestinationString"].HeaderText = "Destination";

        }


        /// <value> The Visitors' data will appear on GuestsDialog/// </value>
        public List<Visitor> Visitors { get; private set; }

        /// <summary>
        /// Sets value of Visitors property.
        /// </summary>
        /// <param name="list">The list of the visitors.</param>
        public void SetVisitors(List<Visitor> list)
        {
            Visitors = new List<Visitor>();
            for (int i = 0; i < list.Count; ++i)
            {
                Visitors.Add(list[i]);
            }
        }

        /// <summary>
        /// Fills the GuetsDataGrid with visitors' data.
        /// </summary>
        public void FillDataGrid()
        {
            GuestsDataGrid.DataSource = Visitors;
        }
    }
}
