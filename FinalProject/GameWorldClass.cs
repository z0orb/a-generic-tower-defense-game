using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FinalProject {
    class GameWorldClass {
        /// <summary>
        /// Creates Lists to Keep, add or remove Gameobjects
        /// </summary> 
        private static List<IngameObjectTracker> objects;                            // Current game objects
        private static List<IngameObjectTracker> newObjects;                         // Used to Add new objects
        private static List<IngameObjectTracker> removeObjects;                      // Used to Remove objects

        /// <summary>
        /// Used to check if the endScreen form is presented.
        /// </summary>
        bool displayedForm = true;

        /// <summary>
        /// Gameobject to keep track of current tower being placed.
        /// </summary> 
        private static IngameObjectTracker towerToPlace;                             // Used to keep a single tower at cursor while placing it

        /// <summary>
        /// Class to keep track of Waves
        /// </summary> 
        private static WaveManager waveKeeper;                                     // Keeps track of spawning waves

        /// <summary>
        /// Checkpoints checker for each Monster.
        /// </summary> 
        private static PointF[] checkpoints;

        /// <summary>
        /// Instantiates graphics, backbuffer and makes a rectangle for screensize.
        /// </summary> 
        private Graphics dc;
        private BufferedGraphics backBuffer;
        private static Rectangle displayRectangle;

        /// <summary>
        /// Image to keep background.
        /// </summary> 
        private Image backgroundImage;

        /// <summary>
        /// Position of mouse cursor and the cursor object.
        /// </summary> 
        private static Point mousePos;
        private MouseHandler cursor;

        /// <summary>
        /// Variables for calculating FPS.
        /// </summary> 
        float currentFPS;
        private DateTime endTime;

        /// <summary>
        /// Game Variables.
        /// </summary> 
        private static int lifeCounter;         // The players current amount of Life left
        private static int currency;            // The players in-game currency

        /// <summary>
        /// Properties.
        /// </summary> 
        public static List<IngameObjectTracker> Objects {
            get { return GameWorldClass.objects; }
            set { GameWorldClass.objects = value; }
        }
        public static List<IngameObjectTracker> NewObjects {
            get { return GameWorldClass.newObjects; }
            set { GameWorldClass.newObjects = value; }
        }
        public static List<IngameObjectTracker> RemoveObjects {
            get { return GameWorldClass.removeObjects; }
            set { GameWorldClass.removeObjects = value; }
        }
        public static IngameObjectTracker TowerToPlace {
            get { return GameWorldClass.towerToPlace; }
            set { GameWorldClass.towerToPlace = value; }
        }
        public static WaveManager WaveKeeper {
            get { return GameWorldClass.waveKeeper; }
            set { GameWorldClass.waveKeeper = value; }
        }
        public static Point MousePos {
            get { return mousePos; }
            set { mousePos = value; }
        }
        public static PointF[] Checkpoints {
            get { return checkpoints; }
            set { checkpoints = value; }
        }
        public static Rectangle DisplayRectangle {
            get { return displayRectangle; }
        }
        public static int LifeCounter {
            get { return GameWorldClass.lifeCounter; }
            set { GameWorldClass.lifeCounter = value; }
        }
        public static int Currency {
            get { return GameWorldClass.currency; }
            set { GameWorldClass.currency = value; }
        }

        /// <summary>
        /// Game Constructor - Used to Initialize Setup of the game.
        /// </summary>
        /// <param name="dc"></param> 
        public GameWorldClass(Graphics dc) {
            // Creates graphics objects
            this.backBuffer = BufferedGraphicsManager.Current.Allocate(dc, displayRectangle);
            this.dc = backBuffer.Graphics;

            // Runs the method SetupWorld();
            SetupWorld();
        }

        /// <summary>
        /// Method to set up the whole game from scratch.
        /// </summary> 
        public void SetupWorld() {
            // Creates all lists
            objects = new List<IngameObjectTracker>();
            newObjects = new List<IngameObjectTracker>();
            removeObjects = new List<IngameObjectTracker>();

            // Sets the background image
            backgroundImage = Image.FromFile(@"sprites\background\background.png");

            // Creates the wave object out of users reach & adds it to objects list
            waveKeeper = new WaveManager(@"sprites\cursor\pointer.png", new PointF(-1000, -1000), 0, 1, 1);
            objects.Add(waveKeeper);

            // Creates the player cursor & adds it to objects list
            cursor = new MouseHandler(@"sprites\cursor\pointer.png", MousePos, 1, 1, 1);
            objects.Add(cursor);

            // Creates the checkpoints for the monster to move to
            checkpoints = new PointF[] {
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


            // Sets player variables to default
            lifeCounter = 20;
            currency = 25;

            // Creates buttons for Towers
            // |        ObjectName     |Sprites                            |Position           |ASpd|SclFac|Spd|Placed|beingPlaced|Selected|
            objects.Add(new BasicTower(@"sprites\towers\normaltower.png", new PointF(571f, 104f), 0, 1, 1, false, false, false));
            objects.Add(new AATower(@"sprites\towers\airtower.png", new PointF(672f, 104f), 0, 1, 1, false, false, false));
            objects.Add(new SlowingTower(@"sprites\towers\slowtower.png", new PointF(571f, 190f), 0, 1, 1, false, false, false));
            objects.Add(new AreasplashTower(@"sprites\towers\splashtower.png", new PointF(672f, 190f), 0, 1, 1, false, false, false));

            // Sets endtime to prepare for gameloops FPS calculation
            endTime = DateTime.Now;

        }

        /// <summary>
        /// Adds the size of the displaywindow to a Rectangle in Game.cs.
        /// </summary>
        /// <param name="rect"></param> 
        public static void SetRectangle(Rectangle rect) {
            displayRectangle = rect;
        }

        /// <summary>
        /// Method to check for users click.
        /// </summary>
        /// <param name="click"></param> 
        public void ClickChecker(bool click) {
            // Handles the mouseclick in PlayerCursor.cs
            cursor.HandleMouseClick(click, (1 / currentFPS));
        }

        /// <summary>
        /// Main GameLoop for the Game.
        /// </summary>
        /// <param name="gMousePos"></param> 
        public void GameLoop(Point gMousePos) {
            // Finds the time since last GameUpdate and calculates the FPS
            DateTime startTime = DateTime.Now;
            TimeSpan deltaTime = startTime - endTime;
            int milliSeconds = deltaTime.Milliseconds > 0 ? deltaTime.Milliseconds : 1;
            currentFPS = 1000 / milliSeconds;
            endTime = DateTime.Now;

            // Updates the current cursorposition
            MousePos = new Point(gMousePos.X, gMousePos.Y);

            // Waits for User input, and resets the game if 'R' is pressed
            if (KeyboardHandler.IsKeyDown(Keys.R)) {
                ResetAll();
            }

            // Calls the Update() method
            Update();

            // Adds all objects from newObjects list to objects list, & clears newObjects
            objects.AddRange(newObjects);
            newObjects.Clear();

            // Removes all objects in removeObjects list from objects list, & clears removeObjects
            for (int i = 0; i < removeObjects.Count; i++) {
                objects.Remove(removeObjects[i]);
            }
            removeObjects.Clear();

            // Calls the UpdateAnimations() method
            UpdateAnimations();

            // Calls the Draw() method
            Draw();

            if (lifeCounter <= 0) {
                PlayerDied();
            }
        }

        /// <summary>
        /// Method to Update objects in Game.
        /// </summary> 
        private void Update() {
            // Updates cursor position in PlayerCursor.cs
            cursor.HandleMousePos();

            // Calculates deltatime for FPS & updates all objects
            float deltaTime = 1 / currentFPS;
            foreach (IngameObjectTracker obj in objects) {
                obj.Update(deltaTime);
            }
        }

        /// <summary>
        /// Method to update animations in gameobjects.
        /// </summary> 
        private void UpdateAnimations() {
            // Updates all objects animations
            foreach (IngameObjectTracker obj in objects) {
                obj.UpdateAnimation(currentFPS);
            }
        }

        /// <summary>
        /// Method to draw out all current objects and updated graphic.
        /// </summary> 
        private void Draw() {
            // Sets a font
            Font font = new Font("Arial", 18);

            // Draws out the background
            dc.DrawImage(backgroundImage, 0, 0, backgroundImage.Width, backgroundImage.Height);

            //Displaying text by the normal tower
            #region Normal Tower
            //Setting font type and size for tower type and tower cost for normal tower
            font = new Font("Arial", 10);
            dc.DrawString("Normal", font, Brushes.Black, 609, 97);
            dc.DrawString("Cost: $7", font, Brushes.Gold, 565, 139);

            //Setting font type and size for damage and which monsters it can attack
            font = new Font("Arial", 8);
            dc.DrawString("Dmg: 6", font, Brushes.Black, 609, 110);
            dc.DrawString("Air & Land", font, Brushes.Black, 609, 122);
            #endregion

            //Displaying text by the air tower
            #region Air Tower
            //Setting font type and size for tower type and tower cost for normal tower
            font = new Font("Arial", 10);
            dc.DrawString("Air", font, Brushes.Black, 709, 97);
            dc.DrawString("Cost: $10", font, Brushes.Gold, 665, 139);

            //Setting font type and size for damage and which monsters it can attack
            font = new Font("Arial", 8);
            dc.DrawString("Dmg: 17", font, Brushes.Black, 709, 110);
            dc.DrawString("Air Only", font, Brushes.Black, 709, 122);
            #endregion

            //Displaying text by the slow tower
            #region Slow Tower
            //Setting font type and size for tower type and tower cost for normal tower
            font = new Font("Arial", 10);
            dc.DrawString("Slow", font, Brushes.Black, 609, 185);
            dc.DrawString("Cost: $7", font, Brushes.Gold, 565, 226);

            //Setting font type and size for damage and which monsters it can attack
            font = new Font("Arial", 8);
            dc.DrawString("Dmg: 3", font, Brushes.Black, 609, 200);
            dc.DrawString("Air & Land", font, Brushes.Black, 609, 212);
            #endregion

            //Displaying text by the splash tower
            #region Splash Tower
            //Setting font type and size for tower type and tower cost for normal tower
            font = new Font("Arial", 10);
            dc.DrawString("Splash", font, Brushes.Black, 709, 185);
            dc.DrawString("Cost: $12", font, Brushes.Gold, 665, 226);

            //Setting font type and size for damage and which monsters it can attack
            font = new Font("Arial", 8);
            dc.DrawString("Dmg: 4", font, Brushes.Black, 709, 200);
            dc.DrawString("Land Only", font, Brushes.Black, 709, 212);
            #endregion

            // User Interface
            // Draws players current currency
            font = new Font("Arial", 18);
            dc.DrawString("$ " + currency.ToString(), font, Brushes.Gold, 561, 5);

            // Sets font & Draws players current lifes
            font = new Font("Arial", 14);
            dc.DrawString("Lives: " + lifeCounter.ToString(), font, Brushes.Cyan, 660, 30);

            // Draws current Wave
            dc.DrawString("Wave: " + WaveKeeper.CurrentWave.ToString(), font, Brushes.LightCyan, 561, 30);

            // Draws all objects in game
            foreach (IngameObjectTracker obj in objects) {
                obj.Draw(dc);
            }


#if DEBUG
            // DEBUG STUFF!
            // FPS
            //dc.DrawString("Fps: " + currentFPS.ToString(), font, Brushes.Red, 0, 25);

            // Total Objects
            //dc.DrawString("TotalObjects: " + (objects.Count).ToString(), font, Brushes.Gold, 0, 50);

            Pen p = new Pen(Color.Red, 2);

            // Draws checkpoint line
            //dc.DrawLines(p, GameWorldClass.Checkpoints);

#endif
            // Calls backbuffer to render & clears the displaywindow
            backBuffer.Render();
            dc.Clear(Color.White);


        }

        /// <summary>
        /// Method to Reset the game.
        /// </summary> 
        private void ResetAll() {
            // Clears all objects from game & runs the SetupWorld() method
            objects.Clear();
            SetupWorld();
            displayedForm = true;
        }

        /// <summary>
        /// Method to make sure the endScreen form is shown in the end.
        /// </summary>
        private void PlayerDied() {
            if (displayedForm) {
                displayedForm = false;
                EndScreen endScreen = new EndScreen();
                endScreen.Show();
                GameForm.SelfGameForm.Hide();
            }
        }
    }
}
