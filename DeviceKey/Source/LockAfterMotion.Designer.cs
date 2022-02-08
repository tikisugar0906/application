namespace DeviceKey
{
    partial class LockAfterMotion
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LockAfterMotion));
            this.CountLabel = new System.Windows.Forms.Label();
            this.MotionTimer = new System.Windows.Forms.Timer(this.components);
            this.MotionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CountLabel
            // 
            this.CountLabel.AutoSize = true;
            this.CountLabel.Location = new System.Drawing.Point(53, 31);
            this.CountLabel.Name = "CountLabel";
            this.CountLabel.Size = new System.Drawing.Size(17, 12);
            this.CountLabel.TabIndex = 0;
            this.CountLabel.Text = "10";
            // 
            // MotionLabel
            // 
            this.MotionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MotionLabel.AutoSize = true;
            this.MotionLabel.Location = new System.Drawing.Point(12, 9);
            this.MotionLabel.Name = "MotionLabel";
            this.MotionLabel.Size = new System.Drawing.Size(100, 12);
            this.MotionLabel.TabIndex = 1;
            this.MotionLabel.Text = "ロック(Windows)まで";
            // 
            // LockAfterMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 65);
            this.Controls.Add(this.MotionLabel);
            this.Controls.Add(this.CountLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LockAfterMotion";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LockAfterMotion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CountLabel;
        private System.Windows.Forms.Timer MotionTimer;
        private System.Windows.Forms.Label MotionLabel;
    }
}