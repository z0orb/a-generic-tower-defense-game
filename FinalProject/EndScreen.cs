using System;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject {
    public partial class EndScreen : Form {
        public EndScreen() {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            Graphics dc = this.CreateGraphics();
            Font font = new Font("Arial", 55);
            if (GameWorldClass.WaveKeeper.CurrentWave < 10) {
                dc.DrawString(GameWorldClass.WaveKeeper.CurrentWave.ToString(), font, Brushes.Red, 125, 60);
            }
            else if (GameWorldClass.WaveKeeper.CurrentWave < 100 && GameWorldClass.WaveKeeper.CurrentWave >= 10) {
                dc.DrawString(GameWorldClass.WaveKeeper.CurrentWave.ToString(), font, Brushes.Red, 105, 60);
            }
            else {
                dc.DrawString(GameWorldClass.WaveKeeper.CurrentWave.ToString(), font, Brushes.Red, 80, 60);
            }
        }

        private void EndScreen_Load(object sender, EventArgs e) {
        }

        private void btnBackMenu_Click(object sender, EventArgs e) {
            GameForm.SelfGameForm.Close();
            FinalProject.Menu.SelfMenu.Show();
            this.Close();
        }

        private void EndScreen_FormClosed(object sender, FormClosedEventArgs e) {
            GameForm.SelfGameForm.Close();
            FinalProject.Menu.SelfMenu.Show();
        }
    }
}