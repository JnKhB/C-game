
namespace RollerCoasterTycoon.View
{
    partial class JanitorsDialog
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
            this.JanitorsNumberLabel = new System.Windows.Forms.Label();
            this.CostOfJanitorLabel = new System.Windows.Forms.Label();
            this.JanitorsButton = new System.Windows.Forms.Button();
            this.JanitorsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.JanitorsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // JanitorsNumberLabel
            // 
            this.JanitorsNumberLabel.AutoSize = true;
            this.JanitorsNumberLabel.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.JanitorsNumberLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.JanitorsNumberLabel.Location = new System.Drawing.Point(36, 111);
            this.JanitorsNumberLabel.Name = "JanitorsNumberLabel";
            this.JanitorsNumberLabel.Size = new System.Drawing.Size(237, 26);
            this.JanitorsNumberLabel.TabIndex = 0;
            this.JanitorsNumberLabel.Text = "Number of janitors:";
            // 
            // CostOfJanitorLabel
            // 
            this.CostOfJanitorLabel.AutoSize = true;
            this.CostOfJanitorLabel.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CostOfJanitorLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.CostOfJanitorLabel.Location = new System.Drawing.Point(36, 45);
            this.CostOfJanitorLabel.Name = "CostOfJanitorLabel";
            this.CostOfJanitorLabel.Size = new System.Drawing.Size(182, 26);
            this.CostOfJanitorLabel.TabIndex = 1;
            this.CostOfJanitorLabel.Text = "Cost of janitor:";
            // 
            // JanitorsButton
            // 
            this.JanitorsButton.Location = new System.Drawing.Point(143, 185);
            this.JanitorsButton.Name = "JanitorsButton";
            this.JanitorsButton.Size = new System.Drawing.Size(147, 42);
            this.JanitorsButton.TabIndex = 2;
            this.JanitorsButton.Text = "Save";
            this.JanitorsButton.UseVisualStyleBackColor = true;
            // 
            // JanitorsNumericUpDown
            // 
            this.JanitorsNumericUpDown.Location = new System.Drawing.Point(309, 110);
            this.JanitorsNumericUpDown.Name = "JanitorsNumericUpDown";
            this.JanitorsNumericUpDown.Size = new System.Drawing.Size(72, 27);
            this.JanitorsNumericUpDown.TabIndex = 3;
            // 
            // JanitorsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Thistle;
            this.ClientSize = new System.Drawing.Size(482, 253);
            this.Controls.Add(this.JanitorsNumericUpDown);
            this.Controls.Add(this.JanitorsButton);
            this.Controls.Add(this.CostOfJanitorLabel);
            this.Controls.Add(this.JanitorsNumberLabel);
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "JanitorsDialog";
            this.Text = "Janitors";
            ((System.ComponentModel.ISupportInitialize)(this.JanitorsNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label JanitorsNumberLabel;
        private System.Windows.Forms.Label CostOfJanitorLabel;
        private System.Windows.Forms.Button JanitorsButton;
        private System.Windows.Forms.NumericUpDown JanitorsNumericUpDown;
    }
}