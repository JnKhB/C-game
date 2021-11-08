using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RollerCoasterTycoon
{
    /// <summary>
    /// A form in which the user can choose which partkitem he/she wants to build.
    /// Its controls appear dynamically when user clicks on spesific menuitems in the GameView form.
    /// </summary>
    public partial class BuildElementDialog : Form
    {
        /// <summary>
        /// BuildElementDialog constructor.
        /// Sets the main properties for the form of build elements, like backgroundcolor.
        /// </summary>
        public BuildElementDialog()
        {
            InitializeComponent();
        }
    }
}
