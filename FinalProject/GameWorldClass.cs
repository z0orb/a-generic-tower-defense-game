using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject
{
    class GameWorldClass
    {
        // buat list untuk simpan, buat dan remove objek
        private static List<IngameObjectTracker> objects; // current object
        private static List<IngameObjectTracker> newObjects; // object baru
        private static List<IngameObjectTracker> removeObjects; // hapus object

        // Cek apakah form end screen sudah ditampilkan
        bool displayedForm = true;

        // track current tower yang sudah di place
        private static IngameObjectTracker towerToPlace;

        // Pengatur wave musuh
        private static WaveManager waveKeeper;

        //  checkpoint checker tiap musuh 
        private static PointF[] checkpoints;

        // Grafik dan buffer
        private Graphics dc;
        private BufferedGraphics backBuffer;
        private static Rectangle displayRectangle;

        // Background game
        private Image backgroundImage;

        // Posisi mouse dan handler kursor
        private static Point mousePos;
        private MouseHandler cursor;

        // variable buat count FPS
        float currentFPS;
        private DateTime endTime;

        // Variabel nyawa dan uang
        private static int lifeCounter;
        private static int currency;

        // list property
        public static List<IngameObjectTracker> Objects
        {
            get { return GameWorldClass.objects; }
            set { GameWorldClass.objects = value; }
        }

        public static List<IngameObjectTracker> NewObjects
        {
            get { return GameWorldClass.newObjects; }
            set { GameWorldClass.newObjects = value; }
        }

        public static List<IngameObjectTracker> RemoveObjects
        {
            get { return GameWorldClass.removeObjects; }
            set { GameWorldClass.removeObjects = value; }
        }

        public static IngameObjectTracker TowerToPlace
        {
            get { return GameWorldClass.towerToPlace; }
            set { GameWorldClass.towerToPlace = value; }
        }

        public static WaveManager WaveKeeper
        {
            get { return GameWorldClass.waveKeeper; }
            set { GameWorldClass.waveKeeper = value; }
        }

        public static Point MousePos
        {
            get { return mousePos; }
            set { mousePos = value; }
        }

        public static PointF[] Checkpoints
        {
            get { return checkpoints; }
            set { checkpoints = value; }
        }

        public static Rectangle DisplayRectangle
        {
            get { return displayRectangle; }
        }

        public static int LifeCounter
        {
            get { return GameWorldClass.lifeCounter; }
            set { GameWorldClass.lifeCounter = value; }
        }

        public static int Currency
        {
            get { return GameWorldClass.currency; }
            set { GameWorldClass.currency = value; }
        }

        // inisialisasi game setup
        public GameWorldClass(Graphics dc)
        {
            this.backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;

            // runs metde setup world dibawah 
            SetupWorld();
        }

        // method untuk setup world from strach
        public void SetupWorld()
        {
            // setup list
            objects = new List<IngameObjectTracker>();
            newObjects = new List<IngameObjectTracker>();
            removeObjects = new List<IngameObjectTracker>();

            // setup background, mouse, wave keeper, dan checkpoints
            backgroundImage = Image.FromFile(@"sprites\background\background.png");

            waveKeeper = new WaveManager(@"sprites\cursor\pointer.png", new PointF(-1000, -1000), 0, 1, 1);
            objects.Add(waveKeeper);

            cursor = new MouseHandler(@"sprites\cursor\pointer.png", MousePos, 1, 1, 1);
            objects.Add(cursor);

            checkpoints = new PointF[]
            {
                new PointF(130, -40),
                new PointF(130, -40),
                new PointF(130, 98),
                new PointF(422, 98),
                new PointF(422, 297),
                new PointF(292, 297),
                new PointF(292, 198),
                new PointF(130, 198),
                new PointF(130, 396),
                new PointF(422, 396),
                new PointF(422, 496),
                new PointF(-40, 496)
            };

            // set default values
            lifeCounter = 20;
            currency = 25;

            // set buttons untuk tower
            objects.Add(new BasicTower(@"sprites\towers\normaltower.png", new PointF(571f, 104f), 0, 1, 1, false, false, false));
            objects.Add(new AATower(@"sprites\towers\airtower.png", new PointF(672f, 104f), 0, 1, 1, false, false, false));
            objects.Add(new SlowingTower(@"sprites\towers\slowtower.png", new PointF(571f, 190f), 0, 1, 1, false, false, false));
            objects.Add(new AreasplashTower(@"sprites\towers\splashtower.png", new PointF(672f, 190f), 0, 1, 1, false, false, false));

            endTime = DateTime.Now;
        }


        public static void SetRectangle(Rectangle rect)
        {
            displayRectangle = rect;
        }

       // method untuk cek player mouse click
        public void ClickChecker(bool click)
        {
            cursor.HandleMouseClick(click, (1 / currentFPS));
        }

        // method game loop 
        public void GameLoop(Point gMousePos)
        {
            // cari waktu pertama kali game update, lalu calculate fps nya
            DateTime startTime = DateTime.Now;
            TimeSpan deltaTime = startTime - endTime;
            int milliSeconds = deltaTime.Milliseconds > 0 ? deltaTime.Milliseconds : 1;
            currentFPS = 1000 / milliSeconds;
            endTime = DateTime.Now;

            // update mouse position 
            MousePos = new Point(gMousePos.X, gMousePos.Y);

            // cek user input, jika user tekan R maka reset game 
            if (KeyboardHandler.IsKeyDown(Keys.R))
            {
                ResetAll();
            }

            Update();

            objects.AddRange(newObjects);
            newObjects.Clear();

            for (int i = 0; i < removeObjects.Count; i++)
            {
                objects.Remove(removeObjects[i]);
            }
            removeObjects.Clear();

            UpdateAnimations();
            Draw();

            if (lifeCounter <= 0)
            {
                PlayerDied();
            }
        }

        // method untuk update game world per fps
        private void Update()
        {
            cursor.HandleMousePos();

            float deltaTime = 1 / currentFPS;
            foreach (IngameObjectTracker obj in objects)
            {
                obj.Update(deltaTime);
            }
        }

        // method untuk update animasi tiap objek
        private void UpdateAnimations()
        {
            foreach (IngameObjectTracker obj in objects)
            {
                obj.UpdateAnimation(currentFPS);
            }
        }

        // method untuk draw semua current object dan updated graphic 
        private void Draw()
        {
            Font font = new Font("Arial", 18);

            dc.DrawImage(backgroundImage, 0, 0, backgroundImage.Width, backgroundImage.Height);
         
            #region Normal Tower

            font = new Font("Arial", 10);
            dc.DrawString("Normal", font, Brushes.Black, 609, 97);
            dc.DrawString("Cost: $7", font, Brushes.Gold, 565, 139);

            font = new Font("Arial", 8);
            dc.DrawString("Dmg: 6", font, Brushes.Black, 609, 110);
            dc.DrawString("Air & Land", font, Brushes.Black, 609, 122);
            #endregion

            
            #region Air Tower
            
            font = new Font("Arial", 10);
            dc.DrawString("Air", font, Brushes.Black, 709, 97);
            dc.DrawString("Cost: $10", font, Brushes.Gold, 665, 139);

        
            font = new Font("Arial", 8);
            dc.DrawString("Dmg: 17", font, Brushes.Black, 709, 110);
            dc.DrawString("Air Only", font, Brushes.Black, 709, 122);
            #endregion

           
            #region Slow Tower
          
            font = new Font("Arial", 10);
            dc.DrawString("Slow", font, Brushes.Black, 609, 185);
            dc.DrawString("Cost: $7", font, Brushes.Gold, 565, 226);


            font = new Font("Arial", 8);
            dc.DrawString("Dmg: 3", font, Brushes.Black, 609, 200);
            dc.DrawString("Air & Land", font, Brushes.Black, 609, 212);
            #endregion


            #region Splash Tower
  
            font = new Font("Arial", 10);
            dc.DrawString("Splash", font, Brushes.Black, 709, 185);
            dc.DrawString("Cost: $12", font, Brushes.Gold, 665, 226);

            font = new Font("Arial", 8);
            dc.DrawString("Dmg: 4", font, Brushes.Black, 709, 200);
            dc.DrawString("Land Only", font, Brushes.Black, 709, 212);
            #endregion

            // User Interface
        
            font = new Font("Arial", 18);
            dc.DrawString("$ " + currency.ToString(), font, Brushes.Gold, 561, 5);

        
            font = new Font("Arial", 14);
            dc.DrawString("Lives: " + lifeCounter.ToString(), font, Brushes.Cyan, 660, 30);

            dc.DrawString("Wave: " + WaveKeeper.CurrentWave.ToString(), font, Brushes.LightCyan, 561, 30);

          
            foreach (IngameObjectTracker obj in objects)
            {
                obj.Draw(dc);
            }

#if DEBUG
            // Debug opsional
            Pen p = new Pen(Color.Red, 2);
            //dc.DrawLines(p, GameWorldClass.Checkpoints);
#endif

            backBuffer.Render();
            dc.Clear(Color.White);
        }

        // method untuk reset semua objek dan run setup world
        private void ResetAll()
        {
            objects.Clear();
            SetupWorld();
            displayedForm = true;
        }

        // method untuk show endscreen jika player mati 
        private void PlayerDied()
        {
            if (displayedForm)
            {
                displayedForm = false;
                EndScreen endScreen = new EndScreen();
                endScreen.Show();
                GameForm.SelfGameForm.Hide();
            }
        }
    }
}
