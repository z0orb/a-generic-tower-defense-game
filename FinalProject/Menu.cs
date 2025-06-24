using System;
using System.Windows.Forms;

namespace FinalProject {
    public partial class Menu : Form {
        private static Menu menu;

        public static Menu SelfMenu {
            get { return menu; }
        }

        public Menu() {
            menu = this;
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, EventArgs e) {
            GameForm gameForm = new GameForm();
            gameForm.Show();
            gameForm.Closed += new EventHandler(GameForm_Closed);
            this.Hide();
        }

        void GameForm_Closed(object sender, EventArgs e) {
            this.Show();
        }

        private void btnQuit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void btnHowToPlay_Click(object sender, EventArgs e) {
            if (pnlHowTo.Visible == true) {
                pnlHowTo.Visible = false;
            }
            else {
                pnlHowTo.Visible = true;
            }
        }
    }
}