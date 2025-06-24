using System;
using System.Diagnostics;
using System.Drawing;

namespace FinalProject {
    public abstract class TowerClass : IngameObjectTracker {
        /// <summary>
        ///  Variable to check if tower is placed, being placed or selected.
        /// </summary>
        protected bool placed;
        protected bool beingPlaced;
        protected bool selected;

        /// <summary>
        ///  Tower Variables.
        /// </summary>
        protected int upgradeLevel;         // Towers current upgrade level
        protected int damage;               // Towers current damage
        protected int range;                // Towers current Range
        protected int price;                // Towers current price, also used for upgrades
        protected float totalValue;         // Towers Total Value

        /// <summary>
        /// This towers current target.
        /// </summary>
        protected EnemyClass target;

        /// <summary>
        /// Stopwatch to keep track of shots.
        /// </summary> 
        Stopwatch sw;

        /// <summary>
        /// Draw Elements
        /// </summary>
        SolidBrush b;
        Pen p;

        /// <summary>
        /// Properties for our variables.
        /// </summary> 
        public bool Placed {
            get { return placed; }
            set { placed = value; }
        }
        public bool Selected {
            get { return selected; }
            set { selected = value; }
        }
        public int Price {
            get { return price; }
            set { price = value; }
        }
        public float TotalValue {
            get { return totalValue; }
            set { totalValue = value; }
        }
        public int UpgradeLevel {
            get { return upgradeLevel; }
            set { upgradeLevel = value; }
        }

        /// <summary>
        /// Sets the values for the different variables.
        /// </summary>
        /// <param name="imagePath">Gets the path to a single sprite or to multiple sprites</param>
        /// <param name="startPos">All Towers position</param>
        /// <param name="animationSpeed">The speed for animations</param>
        /// <param name="scaleFactor">A factor to scale sprites</param>
        /// <param name="speed">The speed for which the tower shoots</param>
        /// <param name="placed">A bool which checks if a tower is placed</param>
        /// <param name="beingPlaced">A bool that checks if a tower is beingplaced</param>
        /// <param name="selected">A bool that checks if a tower is selected</param>
        public TowerClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, bool placed, bool beingPlaced, bool selected)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed) {
            /// Creates tower with incoming values
            this.placed = placed;
            this.beingPlaced = beingPlaced;
            this.selected = selected;
            upgradeLevel = 1;

            // Creates Stopwatch
            sw = new Stopwatch();

            // Sets brush and Pen
            b = new SolidBrush(Color.White);
            p = new Pen(Color.Red, 2);
        }

        /// <summary>
        /// Update Method for Tower.
        /// </summary>
        /// <param name="deltaTime"></param> 
        public override void Update(float deltaTime) {
            // Updates position to be cursor position if the current tower is being placed
            if (beingPlaced) {
                position.X = GameWorldClass.MousePos.X - Sprite.Width / 2;
                position.Y = GameWorldClass.MousePos.Y - Sprite.Height / 2;
            }

            if (sw.Elapsed.Seconds > 60) {
                sw.Stop();
            }

            // Checks if a monster is within range
            if (placed && target == null) {
                // Checks all objects for monsters if tower has no target
                foreach (IngameObjectTracker m in GameWorldClass.Objects) {
                    if ((m as EnemyClass) != null) {
                        if (((EnemyClass)m).CurrentHealth <= 0) {
                            ((EnemyClass)m).OnDeath();
                        }

                        // Calculates distance to monster
                        float targetX = m.Position.X + m.Sprite.Width / 2;
                        float targetY = m.Position.Y + m.Sprite.Height / 2;

                        float fromX = this.Position.X + this.Sprite.Width / 2;
                        float fromY = this.Position.Y + this.Sprite.Height / 2;

                        float x = targetX - fromX;
                        float y = targetY - fromY;
                        float distance = (float)Math.Sqrt(x * x + y * y);

                        // if a monster is close, it will be set as target
                        if (distance < range + 10) {
                            if ((this as SlowingTower) != null) {
                                if (!((EnemyClass)m).IsSlowed) {
                                    target = (EnemyClass)m;
                                    break;
                                }
                                else {
                                    target = (EnemyClass)m;
                                }
                            }
                            else {
                                target = (EnemyClass)m;
                                break;
                            }
                        }
                    }
                }
            }
            else if (target != null) {
                // Checks the distance to current target
                float targetX = target.Position.X + target.Sprite.Width / 2;
                float targetY = target.Position.Y + target.Sprite.Height / 2;

                float fromX = this.Position.X + this.Sprite.Width / 2;
                float fromY = this.Position.Y + this.Sprite.Height / 2;

                float x = targetX - fromX;
                float y = targetY - fromY;
                float distance = (float)Math.Sqrt(x * x + y * y);

                // Handles target
                if (target.CurrentHealth <= 0) {
                    // if target dies, it will be removed & target reset
                    target.OnDeath();
                    foreach (IngameObjectTracker m in GameWorldClass.Objects) {
                        if ((m as EnemyClass) != null) {
                            if (((EnemyClass)m).CurrentHealth <= 0) {
                                ((EnemyClass)m).OnDeath();
                            }
                        }
                    }

                    target = null;
                }
                else if (distance < range + 10) {
                    // target is shot at if it's close & tower has no Cooldown
                    if (sw.ElapsedMilliseconds > speed || !sw.IsRunning) {
                        if ((this as BasicTower) != null) {
                            GameWorldClass.NewObjects.Add(new ProjectileClass(@"sprites\towers\normalprojectile.png", new PointF(position.X + (Sprite.Width / 2), position.Y + (Sprite.Width / 2)), 1, 1, (2000 - speed), target.Position));
                            target.CurrentHealth -= damage;
                        }

                        if ((this as AATower) != null) {
                            if (target.Flying) {
                                GameWorldClass.NewObjects.Add(new ProjectileClass(@"sprites\towers\airprojectile.png", new PointF(position.X + (Sprite.Width / 2), position.Y + (Sprite.Width / 2)), 1, 1, (2000 - speed), target.Position));
                                target.CurrentHealth -= damage;
                            }
                        }

                        if ((this as AreasplashTower) != null) {
                            if (!target.Flying) {
                                GameWorldClass.NewObjects.Add(new ProjectileClass(@"sprites\towers\splashprojectile.png", new PointF(position.X + (Sprite.Width / 2), position.Y + (Sprite.Width / 2)), 1, 1, (2000 - speed), target.Position));
                                foreach (IngameObjectTracker m in GameWorldClass.Objects) {
                                    if ((m as EnemyClass) != null) {
                                        /// Calculates distance to enemy & other enemy
                                        float mTargetX = m.Position.X + m.Sprite.Width / 2;
                                        float mTargetY = m.Position.Y + m.Sprite.Height / 2;


                                        float mFromX = target.Position.X + target.Sprite.Width / 2;

                                        float mFromY = target.Position.Y + target.Sprite.Height / 2;

                                        float mX = mTargetX - mFromX;
                                        float mY = mTargetY - mFromY;
                                        float mDistance = (float)Math.Sqrt(mX * mX + mY * mY);

                                        // if a monster is close, it will be set as target
                                        if (mDistance < ((AreasplashTower)this).SplashRange + 16) {
                                            ((EnemyClass)m).CurrentHealth -= damage;
                                        }
                                    }
                                }
                            }
                        }

                        //If the tower is a SlowTower a new projectile is added to the NewObjects list.
                        if ((this as SlowingTower) != null) {
                            GameWorldClass.NewObjects.Add(new ProjectileClass(@"sprites\towers\slowprojectile.png", new PointF(position.X + (Sprite.Width / 2), position.Y + (Sprite.Width / 2)), 1, 1, (2000 - speed), target.Position));

                            //If the targeted enemy is not slowed and is not immune the enemies is slowed
                            if (!target.IsSlowed) {
                                if (!target.Immune) {
                                    target.Speed = target.Speed * ((SlowingTower)this).SlowSpeed;
                                    target.SlowMonster();
                                }
                            }
                            //If the target is slowed then it checks on how much this target is slowed. 
                            //If that is less than the acutal target speed a new slowSpeed is given. 
                            else {
                                if (target.TemporarySpeed * ((SlowingTower)this).SlowSpeed < target.Speed) {
                                    target.Speed = target.TemporarySpeed;
                                    target.Speed = target.Speed * ((SlowingTower)this).SlowSpeed;
                                    target.SlowMonster();
                                }
                            }

                            target.CurrentHealth -= damage;

                            target = null;

                        }

                        // Resets cooldown Stopwatch
                        sw.Reset();
                        sw.Start();
                    }
                }
                else {
                    // If target gets out of range, it will be resat to be able to get a new target
                    target = null;
                }
            }

            // Runs base.Update
            base.Update(deltaTime);
        }

        /// <summary>
        /// Overrides GameObjects Draw method to draw a circle around a selected tower to show range, and draw the strings for upgrade options as seen below.
        /// </summary>
        /// <param name="dc"></param>
        public override void Draw(Graphics dc) {
            // Sets a font
            Font f = new Font("Arial", 18);

            // Runs base.Draw
            base.Draw(dc);

            // Draws the range of the tower when being placed or selected
            if (selected || beingPlaced) {
                p.Color = Color.DarkGray;
                dc.DrawEllipse(p, Position.X - range + Sprite.Size.Width / 2, Position.Y - range + Sprite.Size.Height / 2, range * 2, range * 2);
                p.Color = Color.Yellow;

                // Draws things only for selected tower (Upgrade, sell area)
                if (selected) {
                    dc.DrawRectangle(p, CollisionBox.X, CollisionBox.Y, CollisionBox.Width, CollisionBox.Height);

                    // Sets font & draws tower upgrade menu
                    f = new Font("Arial", 13, FontStyle.Bold);
                    dc.DrawString("Upgrade Tower: level " + upgradeLevel, f, Brushes.Black, 561, 290);

                    // Headline for the current damage and range
                    f = new Font("Arial", 11, FontStyle.Bold | FontStyle.Underline);
                    dc.DrawString("Now: ", f, Brushes.Black, 561, 325);

                    //Shows the string "Danage: " under the string "Now: "
                    f = new Font("Arial", 8, FontStyle.Bold);
                    dc.DrawString("Damage: ", f, Brushes.Black, 561, 350);
                    f = new Font("Arial", 8, FontStyle.Regular);
                    dc.DrawString(damage.ToString(), f, Brushes.Black, 612, 350);

                    // Shows the current splashrange for a selected splashtower
                    if ((this as AreasplashTower) != null) {
                        f = new Font("Arial", 8, FontStyle.Bold);
                        dc.DrawString("Splash Range: ", f, Brushes.Black, 561, 370);
                        f = new Font("Arial", 8, FontStyle.Regular);
                        dc.DrawString(((AreasplashTower)this).SplashRange.ToString(), f, Brushes.Black, 640, 370);
                    }

                    // Shows the current slowrange for a selected slowtower
                    else if ((this as SlowingTower) != null) {
                        f = new Font("Arial", 8, FontStyle.Bold);
                        dc.DrawString("Slow: ", f, Brushes.Black, 561, 370);
                        f = new Font("Arial", 8, FontStyle.Regular);
                        dc.DrawString((Math.Round((1 - (((SlowingTower)this).SlowSpeed)) * 100, 0)).ToString() + "%", f, Brushes.Black, 595, 370);
                    }
                    // Shows the current range for any other selected tower
                    else {
                        f = new Font("Arial", 8, FontStyle.Bold);
                        dc.DrawString("Range: ", f, Brushes.Black, 561, 370);
                        f = new Font("Arial", 8, FontStyle.Regular);
                        dc.DrawString(range.ToString(), f, Brushes.Black, 603, 370);
                    }

                    //Checks possible upgradelevel and tower type, and writes damage and range for a selected tower after upgrade 
                    if ((this as SlowingTower) == null && upgradeLevel < 20 || (this as SlowingTower) != null && upgradeLevel < 5) {
                        // Which damange and range does the selected tower have after upgrade
                        f = new Font("Arial", 11, FontStyle.Bold | FontStyle.Underline);
                        dc.DrawString("After: ", f, Brushes.Black, 680, 325);

                        // Shows damage for NormalTower after upgrade
                        if ((this as BasicTower) != null) {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Damage: ", f, Brushes.Black, 680, 350);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(((int)(damage * 1.6)).ToString(), f, Brushes.Black, 730, 351);

                        }
                        // Shows damage for AirTower after upgrade
                        else if ((this as AATower) != null) {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Damage: ", f, Brushes.Black, 680, 350);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(((int)(damage * 1.6)).ToString(), f, Brushes.Black, 730, 351);

                        }

                        // Shows damage for SlowTower after upgrade
                        else if ((this as SlowingTower) != null) {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Damage: ", f, Brushes.Black, 680, 350);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(((int)(damage * 1.6)).ToString(), f, Brushes.Black, 730, 351);

                        }
                        // Shows damage for SplashTower after upgrade
                        else {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Damage: ", f, Brushes.Black, 680, 350);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(((int)(damage * 1.7)).ToString(), f, Brushes.Black, 730, 351);

                        }

                        // Shows the new splashrange if a selected splashtower is upgraded. 
                        if ((this as AreasplashTower) != null) {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Splash Range: ", f, Brushes.Black, 680, 370);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString((((AreasplashTower)this).SplashRange + 5).ToString(), f, Brushes.Black, 760, 371);
                        }
                        // Shows the new slowspeed  for enemies in % if a selected splashtower is upgraded.
                        else if ((this as SlowingTower) != null) {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Slow: ", f, Brushes.Black, 680, 370);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString((Math.Round((1 - (((SlowingTower)this).SlowSpeed)) * 100, 0) + 5).ToString() + "%", f, Brushes.Black, 715, 371);
                        }

                        // Shows the new range if any other a selected tower is upgraded.
                        else {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Range: ", f, Brushes.Black, 680, 370);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(range.ToString(), f, Brushes.Black, 721, 371);
                        }


                        // Shows how much it costs to upgrade a selected tower.
                        f = new Font("Arial", 11, FontStyle.Bold);
                        dc.DrawString("Upgrade Cost: ", f, Brushes.Black, 576, 400);
                        f = new Font("Arial", 11, FontStyle.Bold);
                        dc.DrawString("$ " + price, f, Brushes.Gold, 680, 400);
                    }
                }
            }

#if DEBUG
            // DEBUG STUFF!!!

#endif

        }

        /// <summary>
        /// Shows towers radius, the posibilly to upgrade or sell. Also a yellow square is shows around the tower selected.
        /// </summary>
        public void OnSelection() {
            selected = true;

            if ((this as SlowingTower) == null && upgradeLevel < 20 || (this as SlowingTower) != null && upgradeLevel < 5) {
                GameWorldClass.NewObjects.Add(new UpgradeClass(@"sprites\buttons\upgrade.png", new PointF(650, 440), 0, 1, 1, this));
            }

            GameWorldClass.NewObjects.Add(new SellClass(@"sprites\buttons\sell.png", new PointF(570, 440), 0, 1, 1, this));
        }

        /// <summary>
        /// Removes upgrade and sell buttons if a tower is deselected. This way you only see the buttons if a tower is selected
        /// </summary>
        public void OnDeselection() {
            selected = false;

            foreach (IngameObjectTracker obj in GameWorldClass.Objects) {
                if ((obj as UpgradeClass) != null || (obj as SellClass) != null) {
                    GameWorldClass.RemoveObjects.Add(obj);
                }
            }
        }

        /// <summary>
        /// An method overrided the Tower subclasses. Sets the next upgrade for a selected tower. 
        /// For splash-,normal and airtower there is 20 upgrade levels, where price and damage increases with each upgrade.
        /// For slow there is only five upgrades, but again damage and price is increased. But also the speed of which enemies is slowed to.
        /// </summary>
        public abstract void SetNextUpgrade();
    }
}
