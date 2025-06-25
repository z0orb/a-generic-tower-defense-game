using System;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject {
    public partial class GameForm : Form 
    {
        private static GameForm gameForm;

        // instansiasi gameworld and graphics
        GameWorldClass gW;
        Graphics dc;

        public static GameForm SelfGameForm 
        {
            get { return GameForm.gameForm; }
        }

        public GameForm() 
        {
            gameForm = this;
            InitializeComponent();
        }

        private void GameForm_Load(object sender, EventArgs e) 
        {
            // bikin element graphics
            dc = CreateGraphics();

            // set display rectangle
            GameWorldClass.SetRectangle(this.DisplayRectangle);

            // create game nya
            gW = new GameWorldClass(dc);
        }

        private void GameLoop_Tick(object sender, EventArgs e) 
        {
            // mulai method GameLoop
            gW.GameLoop(this.PointToClient(Cursor.Position));
        }

        private void picNext_Click(object sender, EventArgs e) 
        {
            GameWorldClass.WaveKeeper.NextWave();
        }

        private void GameForm_MouseDown(object sender, MouseEventArgs e) 
        {
            gW.ClickChecker(true);
        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e) 
        {
            gW.ClickChecker(false);
        }
    }
}