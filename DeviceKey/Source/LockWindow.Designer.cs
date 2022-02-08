namespace DeviceKey
{
    partial class LockWindow
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
            this.PassTextBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.PassLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PassTextBox
            // 
            this.PassTextBox.Location = new System.Drawing.Point(820, 532);
            this.PassTextBox.Name = "PassTextBox";
            this.PassTextBox.PasswordChar = '●';
            this.PassTextBox.ShortcutsEnabled = false;
            this.PassTextBox.Size = new System.Drawing.Size(224, 19);
            this.PassTextBox.TabIndex = 0;
            this.PassTextBox.UseSystemPasswordChar = true;
            this.PassTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PassTextBox_KeyDown);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(1050, 530);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // PassLabel
            // 
            this.PassLabel.AutoSize = true;
            this.PassLabel.Location = new System.Drawing.Point(818, 517);
            this.PassLabel.Name = "PassLabel";
            this.PassLabel.Size = new System.Drawing.Size(166, 12);
            this.PassLabel.TabIndex = 2;
            this.PassLabel.Text = "デバイスがない時の緊急パスワード";
            // 
            // LockWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1605, 683);
            this.ControlBox = false;
            this.Controls.Add(this.PassLabel);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.PassTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LockWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "LockWindow";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LockWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PassTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label PassLabel;
    }
}