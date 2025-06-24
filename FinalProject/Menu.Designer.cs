namespace FinalProject
{
    partial class Menu
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
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnHowToPlay = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.pnlHowTo = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.BackgroundImage = global::FinalProject.Properties.Resources.playgame;
            this.btnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Location = new System.Drawing.Point(41, 137);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(267, 114);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnHowToPlay
            // 
            this.btnHowToPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnHowToPlay.BackgroundImage = global::FinalProject.Properties.Resources.howto;
            this.btnHowToPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHowToPlay.FlatAppearance.BorderSize = 0;
            this.btnHowToPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHowToPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHowToPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHowToPlay.Location = new System.Drawing.Point(41, 265);
            this.btnHowToPlay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnHowToPlay.Name = "btnHowToPlay";
            this.btnHowToPlay.Size = new System.Drawing.Size(267, 120);
            this.btnHowToPlay.TabIndex = 1;
            this.btnHowToPlay.UseVisualStyleBackColor = false;
            this.btnHowToPlay.Click += new System.EventHandler(this.btnHowToPlay_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.Transparent;
            this.btnQuit.BackgroundImage = global::FinalProject.Properties.Resources.quit;
            this.btnQuit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnQuit.FlatAppearance.BorderSize = 0;
            this.btnQuit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnQuit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuit.Location = new System.Drawing.Point(41, 425);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(267, 119);
            this.btnQuit.TabIndex = 2;
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // pnlHowTo
            // 
            this.pnlHowTo.BackColor = System.Drawing.Color.Transparent;
            this.pnlHowTo.BackgroundImage = global::FinalProject.Properties.Resources.howtotext;
            this.pnlHowTo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlHowTo.Location = new System.Drawing.Point(326, 143);
            this.pnlHowTo.Name = "pnlHowTo";
            this.pnlHowTo.Size = new System.Drawing.Size(452, 400);
            this.pnlHowTo.TabIndex = 3;
            this.pnlHowTo.Visible = false;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::FinalProject.Properties.Resources.menubackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(788, 562);
            this.Controls.Add(this.pnlHowTo);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnHowToPlay);
            this.Controls.Add(this.btnPlay);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Start Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnHowToPlay;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Panel pnlHowTo;
    }
}

