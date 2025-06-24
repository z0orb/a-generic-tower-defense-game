using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace FinalProject {
    public class EnemyClass : IngameObjectTracker {
        /// <summary>
        /// Checkpoints checker for each Enemy.
        /// </summary> 
        private int cPointChecker = 0;

        /// <summary>
        /// Enemies healthvariables.
        /// </summary> 
        private float currentHealth;
        private float maxHealth;
        private int healthPercentage;
        private bool dead;
        private float temporarySpeed;

        /// <summary>
        /// Special enemies fields.
        /// </summary> 
        private bool flying;
        private bool immune;
        private bool isSlowed;

        /// <summary>
        /// Loot.
        /// </summary> 
        private int dropCurrency;

        /// <summary>
        /// Draw elements for Enemy.
        /// </summary> 
        Rectangle healthBar;
        SolidBrush b;
        Pen p;

        /// <summary>
        /// Stopwatch for the amount of time a enemy is slowed.
        /// </summary> 
        Stopwatch sw;

        /// <summary>
        /// Properties which are used to get or set values for the corrosponding field outside this class
        /// </summary> 
        /// 

        public float CurrentHealth {
            get { return currentHealth; }
            set {
                currentHealth = value;
                if (currentHealth < 0) {
                    currentHealth = 0;
                }
            }
        }
        public bool Flying {
            get { return flying; }
            set { flying = value; }
        }
        public bool Immune {
            get { return immune; }
            set { immune = value; }
        }
        public bool IsSlowed {
            get { return isSlowed; }
            set { isSlowed = value; }
        }
        public float TemporarySpeed {
            get { return temporarySpeed; }
            set { temporarySpeed = value; }
        }

        /// <summary>
        /// Monsters constructer.
        /// </summary>
        /// <param name="imagePath">Gets the path of the creep images</param>
        /// <param name="startPos">Wave position</param>
        /// <param name="animationSpeed">The speed for animations</param>
        /// <param name="scaleFactor">A factor to scale sprites</param>
        /// <param name="health">The amount of health a monster has</param>
        /// <param name="speed">The speed for which the enemy moves</param> 
        /// <param name="dropCurrency">The currency the monsters drop</param>
        /// <param name="flying">If the monster is flying or not</param>
        /// <param name="immune">If the monster is immune or not</param>

        public EnemyClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float health, float speed, float dropCurrency, bool flying, bool immune)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed) {
            temporarySpeed = Speed;

            // Sets the enemies health on creation and creates healthbar
            this.maxHealth = health;
            this.currentHealth = health;
            healthBar = new Rectangle((int)position.X, (int)position.Y + 16, 100, 4);
            dead = false;

            // Special enemies fields
            this.flying = flying;
            this.immune = immune;

            // Sets loot
            this.dropCurrency = (int)dropCurrency;

            // Sets the brush colors
            b = new SolidBrush(Color.White);
            p = new Pen(Color.Red, 2);

            // Creates Stopwatch
            sw = new Stopwatch();
        }

        /// <summary>
        /// Update Method.
        /// </summary>
        /// <param name="deltaTime"></param> 
        public override void Update(float deltaTime) {

            // Checks if the enemy is close to a checkpoint
            if (position.X + (speed / 100 * 3) >= GameWorldClass.Checkpoints[cPointChecker].X && position.X - (speed / 100 * 3) <= GameWorldClass.Checkpoints[cPointChecker].X && position.Y + (speed / 100 * 3) >= GameWorldClass.Checkpoints[cPointChecker].Y && position.Y - (speed / 100 * 3) <= GameWorldClass.Checkpoints[cPointChecker].Y) {
                // Enemy is close enough
                // Sets the position to the checkpoint
                position.X = GameWorldClass.Checkpoints[cPointChecker].X;
                position.Y = GameWorldClass.Checkpoints[cPointChecker].Y;

                // CHecks if it is the last checkpoint
                if (cPointChecker + 1 < GameWorldClass.Checkpoints.Count()) {
                    // Continues to next checkpoint
                    cPointChecker++;
                }
                else {
                    // Resets checkpoints for enemy to start from beginning
                    cPointChecker = 0;
                    position.X = GameWorldClass.Checkpoints[cPointChecker].X;
                    position.Y = GameWorldClass.Checkpoints[cPointChecker].Y;

                    // Player loses 1 life
                    GameWorldClass.LifeCounter -= 1;
                }
            }
            else {
                // Enemy is not close enough
                // Enemy will move towards next checkpoint
                if (position.X != GameWorldClass.Checkpoints[cPointChecker].X) {
                    if (position.X + 1 > GameWorldClass.Checkpoints[cPointChecker].X) {
                        position.X -= deltaTime * speed;
                    }
                    else if (position.X - 1 < GameWorldClass.Checkpoints[cPointChecker].X) {
                        position.X += deltaTime * speed;
                    }
                }
                else if (Position.X + 1 >= GameWorldClass.Checkpoints[cPointChecker].X && Position.X - 1 <= GameWorldClass.Checkpoints[cPointChecker].X) {
                    position.X = GameWorldClass.Checkpoints[cPointChecker].X;
                }
                if (position.Y != GameWorldClass.Checkpoints[cPointChecker].Y) {
                    if (position.Y + 1 > GameWorldClass.Checkpoints[cPointChecker].Y) {
                        position.Y -= deltaTime * speed;
                    }
                    else if (Position.Y - 1 < GameWorldClass.Checkpoints[cPointChecker].Y) {
                        position.Y += deltaTime * speed;
                    }
                }
                else if (Position.Y + 1 >= GameWorldClass.Checkpoints[cPointChecker].Y && Position.Y - 1 <= GameWorldClass.Checkpoints[cPointChecker].Y) {
                    position.Y = GameWorldClass.Checkpoints[cPointChecker].Y;
                }
            }

            if (sw.ElapsedMilliseconds > 5000) {
                isSlowed = false;
                Speed = temporarySpeed;

            }

            // Runs base.Update
            base.Update(deltaTime);

        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        /// <param name="dc"></param> 
        public override void Draw(Graphics dc) {
            // Runs base.Draw
            base.Draw(dc);

#if DEBUG
            // DEBUG STUFF!

            // Draws monsters collisionbox
            dc.DrawRectangle(p, CollisionBox.X, CollisionBox.Y, CollisionBox.Width, CollisionBox.Height);

#endif

            // Calculates the enemies current health & position
            healthPercentage = (int)(((double)currentHealth / (double)maxHealth * 100));
            healthBar.X = (int)position.X - 2;
            healthBar.Y = (int)position.Y + 30;
            healthBar.Width = (int)((double)healthPercentage * 0.4);

            // Draws a black Rectangle to show TotalHealth
            b.Color = Color.Black;
            dc.FillRectangle(b, new Rectangle((int)healthBar.X, (int)healthBar.Y, (int)(100 * 0.4f), 4));

            // Changes color depending on enemies health
            if (healthPercentage >= 50) {
                b.Color = Color.ForestGreen;
            }
            if (healthPercentage < 50) {
                b.Color = Color.Orange;
            }
            if (healthPercentage < 20) {
                b.Color = Color.Red;
            }

            // Draws the enemies currentHealth in a Rectangle atop the black Rectangle 
            dc.FillRectangle(b, healthBar);

        }

        /// <summary>
        /// Method for Collision - This happens when a enemy collies with something.
        /// </summary>
        /// <param name="other"></param> 
        public override void OnCollision(IngameObjectTracker other) {
        }

        /// <summary>
        /// Method for what happens when the enemy die.
        /// </summary>
        public void OnDeath() {
            if (!dead) {
                GameWorldClass.RemoveObjects.Add(this);
                GameWorldClass.Currency += dropCurrency;

                dead = true;
            }
        }

        /// <summary>
        /// Method that determines if the enemy is slowed or not. 
        /// </summary>
        public void SlowMonster() {
            isSlowed = true;

            sw.Reset();
            sw.Start();
        }
    }
}
