
namespace RollerCoasterTycoon.View
{
    partial class SetElementDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SetEntranceFeeButton = new System.Windows.Forms.Button();
            this.EntranceFeeTextBox = new System.Windows.Forms.TextBox();
            this.entrancefeeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SetEntranceFeeButton
            // 
            this.SetEntranceFeeButton.Location = new System.Drawing.Point(130, 140);
            this.SetEntranceFeeButton.Name = "SetEntranceFeeButton";
            this.SetEntranceFeeButton.Size = new System.Drawing.Size(130, 40);
            this.SetEntranceFeeButton.TabIndex = 0;
            this.SetEntranceFeeButton.Text = "Set entrance fee";
            this.SetEntranceFeeButton.UseVisualStyleBackColor = true;
            // 
            // EntranceFeeTextBox
            // 
            this.EntranceFeeTextBox.Location = new System.Drawing.Point(130, 90);
            this.EntranceFeeTextBox.Name = "EntranceFeeTextBox";
            this.EntranceFeeTextBox.Size = new System.Drawing.Size(130, 27);
            this.EntranceFeeTextBox.TabIndex = 1;
            // 
            // entrancefeeLabel
            // 
            this.entrancefeeLabel.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.entrancefeeLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.entrancefeeLabel.Location = new System.Drawing.Point(114, 47);
            this.entrancefeeLabel.Name = "entrancefeeLabel";
            this.entrancefeeLabel.Size = new System.Drawing.Size(230, 40);
            this.entrancefeeLabel.TabIndex = 2;
            this.entrancefeeLabel.Text = "Entrance fee:";
            // 
            // SetElementDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Thistle;
            this.ClientSize = new System.Drawing.Size(382, 203);
            this.Controls.Add(this.entrancefeeLabel);
            this.Controls.Add(this.EntranceFeeTextBox);
            this.Controls.Add(this.SetEntranceFeeButton);
            this.MaximumSize = new System.Drawing.Size(400, 250);
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "SetElementDialog";
            this.Text = "Set entrance fee";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SetEntranceFeeButton;
        private System.Windows.Forms.TextBox EntranceFeeTextBox;
        private System.Windows.Forms.Label entrancefeeLabel;
    }
}