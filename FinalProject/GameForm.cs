using System;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject {
    public partial class GameForm : Form {
        private static GameForm gameForm;

        // Instantiates Game & Graphics
        GameWorldClass gW;
        Graphics dc;

        public static GameForm SelfGameForm {
            get { return GameForm.gameForm; }
        }

        public GameForm() {
            gameForm = this;
            InitializeComponent();
        }

        private void GameForm_Load(object sender, EventArgs e) {
            // Creates the graphics element
            dc = CreateGraphics();

            // Sets the display rectangle
            GameWorldClass.SetRectangle(this.DisplayRectangle);

            // Creates the game
            gW = new GameWorldClass(dc);
        }

        private void GameLoop_Tick(object sender, EventArgs e) {
            // Runs the games GameLoop method
            gW.GameLoop(this.PointToClient(Cursor.Position));
        }

        private void picNext_Click(object sender, EventArgs e) {
            GameWorldClass.WaveKeeper.NextWave();
        }

        private void GameForm_MouseDown(object sender, MouseEventArgs e) {
            gW.ClickChecker(true);
        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e) {
            gW.ClickChecker(false);
        }
    }
}