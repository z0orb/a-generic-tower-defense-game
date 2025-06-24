namespace FinalProject
{
    partial class EndScreen
    {
        //variable untuk designer form
        private System.ComponentModel.IContainer components = null;

        //fungsi garbage cleanup
        /// disposing = true if butuh dihapus, else false
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        //he rek ojo diedit amoh ngko
        private void InitializeComponent()
        {
            this.btnBackMenu = new System.Windows.Forms.Button();
            this.SuspendLayout();
            
            //bckmenu button
            this.btnBackMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnBackMenu.BackgroundImage = global::FinalProject.Properties.Resources.backtomenu;
            this.btnBackMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBackMenu.FlatAppearance.BorderSize = 0;
            this.btnBackMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBackMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBackMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackMenu.Location = new System.Drawing.Point(98, 143);
            this.btnBackMenu.Margin = new System.Windows.Forms.Padding(2);
            this.btnBackMenu.Name = "btnBackMenu";
            this.btnBackMenu.Size = new System.Drawing.Size(117, 45);
            this.btnBackMenu.TabIndex = 0;
            this.btnBackMenu.UseVisualStyleBackColor = false;
            this.btnBackMenu.Click += new System.EventHandler(this.btnBackMenu_Click);
            
            //section endscreen
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::FinalProject.Properties.Resources.endbackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(299, 199);
            this.Controls.Add(this.btnBackMenu);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "EndScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EndScreen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EndScreen_FormClosed);
            this.Load += new System.EventHandler(this.EndScreen_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnBackMenu;
    }
}