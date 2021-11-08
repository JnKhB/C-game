
namespace RollerCoasterTycoon
{
    partial class GameView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.BalanceLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TimerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.EntranceFeeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentItemLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolMenu = new System.Windows.Forms.ToolStrip();
            this.NewGameButton = new System.Windows.Forms.ToolStripButton();
            this.BuildButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AttractionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RestaurantsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.PlantsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SidewalkMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.EntranceFeeButton = new System.Windows.Forms.ToolStripButton();
            this.PeopleButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.GuestsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.JanitorsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenParkButton = new System.Windows.Forms.ToolStripButton();
            this.ExitButton = new System.Windows.Forms.ToolStripButton();
            this.GamePanel = new System.Windows.Forms.Panel();
            this.statusStrip.SuspendLayout();
            this.ToolMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.BackColor = System.Drawing.Color.Thistle;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BalanceLabel,
            this.TimerLabel,
            this.EntranceFeeLabel,
            this.CurrentItemLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 803);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(802, 50);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // BalanceLabel
            // 
            this.BalanceLabel.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BalanceLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.BalanceLabel.Name = "BalanceLabel";
            this.BalanceLabel.Size = new System.Drawing.Size(103, 44);
            this.BalanceLabel.Text = "Balance: |";
            // 
            // TimerLabel
            // 
            this.TimerLabel.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TimerLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.TimerLabel.Name = "TimerLabel";
            this.TimerLabel.Size = new System.Drawing.Size(64, 44);
            this.TimerLabel.Text = "Time: ";
            // 
            // EntranceFeeLabel
            // 
            this.EntranceFeeLabel.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EntranceFeeLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.EntranceFeeLabel.Name = "EntranceFeeLabel";
            this.EntranceFeeLabel.Size = new System.Drawing.Size(147, 44);
            this.EntranceFeeLabel.Text = "| Entrance fee:";
            // 
            // CurrentItemLabel
            // 
            this.CurrentItemLabel.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CurrentItemLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.CurrentItemLabel.Name = "CurrentItemLabel";
            this.CurrentItemLabel.Size = new System.Drawing.Size(150, 44);
            this.CurrentItemLabel.Text = "| Current item:";
            // 
            // ToolMenu
            // 
            this.ToolMenu.AutoSize = false;
            this.ToolMenu.BackColor = System.Drawing.Color.Thistle;
            this.ToolMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewGameButton,
            this.BuildButton,
            this.EntranceFeeButton,
            this.PeopleButton,
            this.OpenParkButton,
            this.ExitButton});
            this.ToolMenu.Location = new System.Drawing.Point(0, 0);
            this.ToolMenu.Name = "ToolMenu";
            this.ToolMenu.Size = new System.Drawing.Size(802, 35);
            this.ToolMenu.TabIndex = 6;
            this.ToolMenu.Text = "toolStrip1";
            // 
            // NewGameButton
            // 
            this.NewGameButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.NewGameButton.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.NewGameButton.ForeColor = System.Drawing.Color.DarkBlue;
            this.NewGameButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewGameButton.Name = "NewGameButton";
            this.NewGameButton.Size = new System.Drawing.Size(132, 32);
            this.NewGameButton.Text = "New Game";
            // 
            // BuildButton
            // 
            this.BuildButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BuildButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AttractionsMenu,
            this.RestaurantsMenu,
            this.PlantsMenu,
            this.SidewalkMenu});
            this.BuildButton.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.BuildButton.ForeColor = System.Drawing.Color.DarkBlue;
            this.BuildButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(87, 32);
            this.BuildButton.Text = "Build";
            // 
            // AttractionsMenu
            // 
            this.AttractionsMenu.BackColor = System.Drawing.Color.Thistle;
            this.AttractionsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AttractionsMenu.ForeColor = System.Drawing.Color.DarkBlue;
            this.AttractionsMenu.Name = "AttractionsMenu";
            this.AttractionsMenu.Size = new System.Drawing.Size(237, 30);
            this.AttractionsMenu.Text = "Attractions";
            // 
            // RestaurantsMenu
            // 
            this.RestaurantsMenu.BackColor = System.Drawing.Color.Thistle;
            this.RestaurantsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RestaurantsMenu.ForeColor = System.Drawing.Color.DarkBlue;
            this.RestaurantsMenu.Name = "RestaurantsMenu";
            this.RestaurantsMenu.Size = new System.Drawing.Size(237, 30);
            this.RestaurantsMenu.Text = "Restaurants";
            // 
            // PlantsMenu
            // 
            this.PlantsMenu.BackColor = System.Drawing.Color.Thistle;
            this.PlantsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.PlantsMenu.ForeColor = System.Drawing.Color.DarkBlue;
            this.PlantsMenu.Name = "PlantsMenu";
            this.PlantsMenu.Size = new System.Drawing.Size(237, 30);
            this.PlantsMenu.Text = "Plants";
            // 
            // SidewalkMenu
            // 
            this.SidewalkMenu.BackColor = System.Drawing.Color.Thistle;
            this.SidewalkMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SidewalkMenu.ForeColor = System.Drawing.Color.DarkBlue;
            this.SidewalkMenu.Name = "SidewalkMenu";
            this.SidewalkMenu.Size = new System.Drawing.Size(237, 30);
            this.SidewalkMenu.Text = "Sidewalk";
            // 
            // EntranceFeeButton
            // 
            this.EntranceFeeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EntranceFeeButton.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EntranceFeeButton.ForeColor = System.Drawing.Color.DarkBlue;
            this.EntranceFeeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EntranceFeeButton.Name = "EntranceFeeButton";
            this.EntranceFeeButton.Size = new System.Drawing.Size(161, 32);
            this.EntranceFeeButton.Text = "Entrance fee";
            // 
            // PeopleButton
            // 
            this.PeopleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.PeopleButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GuestsMenu,
            this.JanitorsMenu});
            this.PeopleButton.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PeopleButton.ForeColor = System.Drawing.Color.DarkBlue;
            this.PeopleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PeopleButton.Name = "PeopleButton";
            this.PeopleButton.Size = new System.Drawing.Size(102, 32);
            this.PeopleButton.Text = "People";
            // 
            // GuestsMenu
            // 
            this.GuestsMenu.BackColor = System.Drawing.Color.Thistle;
            this.GuestsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GuestsMenu.ForeColor = System.Drawing.Color.DarkBlue;
            this.GuestsMenu.Name = "GuestsMenu";
            this.GuestsMenu.Size = new System.Drawing.Size(190, 30);
            this.GuestsMenu.Text = "Guests";
            // 
            // JanitorsMenu
            // 
            this.JanitorsMenu.BackColor = System.Drawing.Color.Thistle;
            this.JanitorsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.JanitorsMenu.ForeColor = System.Drawing.Color.DarkBlue;
            this.JanitorsMenu.Name = "JanitorsMenu";
            this.JanitorsMenu.Size = new System.Drawing.Size(190, 30);
            this.JanitorsMenu.Text = "Janitors";
            // 
            // OpenParkButton
            // 
            this.OpenParkButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OpenParkButton.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OpenParkButton.ForeColor = System.Drawing.Color.DarkBlue;
            this.OpenParkButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenParkButton.Name = "OpenParkButton";
            this.OpenParkButton.Size = new System.Drawing.Size(132, 32);
            this.OpenParkButton.Text = "Open park";
            // 
            // ExitButton
            // 
            this.ExitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ExitButton.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ExitButton.ForeColor = System.Drawing.Color.DarkBlue;
            this.ExitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(130, 32);
            this.ExitButton.Text = "Exit game";
            // 
            // GamePanel
            // 
            this.GamePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GamePanel.Location = new System.Drawing.Point(0, 35);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(802, 768);
            this.GamePanel.TabIndex = 7;
            // 
            // GameView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(802, 853);
            this.Controls.Add(this.GamePanel);
            this.Controls.Add(this.ToolMenu);
            this.Controls.Add(this.statusStrip);
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(820, 900);
            this.Name = "GameView";
            this.Text = "ROLLER COASTER TYCOON";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ToolMenu.ResumeLayout(false);
            this.ToolMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel BalanceLabel;
        private System.Windows.Forms.ToolStripStatusLabel TimerLabel;
        private System.Windows.Forms.ToolStripStatusLabel EntranceFeeLabel;
        private System.Windows.Forms.ToolStrip ToolMenu;
        private System.Windows.Forms.ToolStripButton NewGameButton;
        private System.Windows.Forms.ToolStripDropDownButton BuildButton;
        private System.Windows.Forms.ToolStripMenuItem AttractionsMenu;
        private System.Windows.Forms.ToolStripMenuItem RestaurantsMenu;
        private System.Windows.Forms.ToolStripMenuItem PlantsMenu;
        private System.Windows.Forms.ToolStripMenuItem SidewalkMenu;
        private System.Windows.Forms.ToolStripButton EntranceFeeButton;
        private System.Windows.Forms.ToolStripDropDownButton PeopleButton;
        private System.Windows.Forms.ToolStripMenuItem GuestsMenu;
        private System.Windows.Forms.ToolStripMenuItem JanitorsMenu;
        private System.Windows.Forms.ToolStripButton OpenParkButton;
        private System.Windows.Forms.ToolStripButton ExitButton;
        private System.Windows.Forms.Panel GamePanel;
        private System.Windows.Forms.ToolStripStatusLabel CurrentItemLabel;
    }
}

