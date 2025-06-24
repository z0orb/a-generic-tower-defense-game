using System.Drawing;
using System.Linq;

namespace FinalProject {
    class MouseHandler : IngameObjectTracker {
        /// <summary>
        /// Cursor Variables
        /// </summary> 
        private bool click;             // Clicked or not
        private bool placing;           // Placing a tower or not
        private bool canPlace;          // Tower can be placed or not
        private PointF clickPos;        // Clicked position

        /// <summary>
        /// Constructor for the cursor.
        /// </summary>
        /// <param name="imagePath">Gets the path of the cursor</param>
        /// <param name="startPos">Cursor position</param>
        /// <param name="animationSpeed">The speed for animations</param>
        /// <param name="scaleFactor">A factor to scale sprites</param>
        /// <param name="speed">The speed for which the cursor travel</param> 
        public MouseHandler(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed) {
            // On start, the cursor is not placing a tower
            placing = false;
        }

        /// <summary>
        /// Method to handle MousePosition
        /// </summary> 
        public void HandleMousePos() {
            // Sets the mouse position
            this.Position = GameWorldClass.MousePos;
        }

        /// <summary>
        /// Method to handle the mouse click
        /// </summary>
        /// <param name="click"></param>
        /// <param name="deltaTime"></param> 
        public void HandleMouseClick(bool click, float deltaTime) {
            if (click) {
                // If mouse is clicked, the position is saved & collision will be checked

                this.click = true;
                clickPos = GameWorldClass.MousePos;

                base.Update(deltaTime);

                this.click = false;
            }
        }

        /// <summary>
        /// OnCollision Method
        /// </summary>
        /// <param name="other"></param> 
        public override void OnCollision(IngameObjectTracker other) {
            // if the collided object is a Tower & And the other tower is placed & a click is detected & cursor is not placing a tower & the position is on gamefield
            if ((other as Tower) != null && (other as Tower).Placed && click && !placing && GameWorldClass.MousePos.X < 520) {
                // if the colliede tower is not selected
                if (!(other as Tower).Selected) {
                    // Removes selection from every other tower
                    foreach (IngameObjectTracker obj in GameWorldClass.Objects) {
                        if ((obj as Tower) != null) {
                            if ((obj as Tower).Placed && (obj as Tower).Selected) {
                                (obj as Tower).OnDeselection();
                            }
                        }
                    }

                    // Add selected to collided tower
                    (other as Tower).OnSelection();

                }
                else {
                    // Remove selection from tower
                    (other as Tower).OnDeselection();
                }
            }


            // if collided object is a NormalTower
            if ((other as Tower) != null) {
                // if a click is detected & cursor is not placing & position is outside gamefield
                if (click && !placing && GameWorldClass.MousePos.X > 540) {
                    if ((other as BasicTower) != null) {
                        if (GameWorldClass.Currency >= ((BasicTower)other).Price) {
                            // Adds a tower to the cursor for the player to place
                            placing = true;
                            GameWorldClass.TowerToPlace = new BasicTower(@"Sprites\Towers\normaltower.png", new PointF(clickPos.X - 16, clickPos.Y - 16), 0, 1, 1, false, true, false);
                            GameWorldClass.NewObjects.Add(GameWorldClass.TowerToPlace);
                        }
                    }
                    if ((other as AATower) != null) {
                        if (GameWorldClass.Currency >= ((AATower)other).Price) {
                            // Adds a tower to the cursor for the player to place
                            placing = true;
                            GameWorldClass.TowerToPlace = new AATower(@"sprites\towers\airtower.png", new PointF(clickPos.X - 16, clickPos.Y - 16), 0, 1, 1, false, true, false);
                            GameWorldClass.NewObjects.Add(GameWorldClass.TowerToPlace);
                        }
                    }
                    if ((other as SplashTower) != null) {
                        if (GameWorldClass.Currency >= ((SplashTower)other).Price) {
                            // Adds a tower to the cursor for the player to place
                            placing = true;
                            GameWorldClass.TowerToPlace = new SplashTower(@"sprites\towers\splashtower.png", new PointF(clickPos.X - 16, clickPos.Y - 16), 0, 1, 1, false, true, false);
                            GameWorldClass.NewObjects.Add(GameWorldClass.TowerToPlace);
                        }
                    }
                    if ((other as SlowTower) != null) {
                        if (GameWorldClass.Currency >= ((SlowTower)other).Price) {
                            // Adds a tower to the cursor for the player to place
                            placing = true;
                            GameWorldClass.TowerToPlace = new SlowTower(@"sprites\towers\slowtower.png", new PointF(clickPos.X - 16, clickPos.Y - 16), 0, 1, 1, false, true, false);
                            GameWorldClass.NewObjects.Add(GameWorldClass.TowerToPlace);
                        }
                    }
                }
                else if (click && placing && GameWorldClass.MousePos.X > 540) {
                    GameWorldClass.RemoveObjects.Add(GameWorldClass.TowerToPlace);
                    GameWorldClass.TowerToPlace = null;
                    placing = false;
                }

                // if the player is currently placing a tower and clicks on the gamefield
                if (click && placing && GameWorldClass.MousePos.X < 520) {
                    // Tower can be placed unless another tower is colliding
                    canPlace = true;

                    // Creates a CollisionBox for the cursor
                    RectangleF newTowerCollision = new RectangleF(clickPos.X - 16, clickPos.Y - 16, 32, 32);

                    foreach (IngameObjectTracker obj in GameWorldClass.Objects) {
                        // Checks Tower in gameobjects to see if it's colliding with the tower being placed
                        if ((obj as Tower) != null &&
                            ((Tower)obj).Placed) {
                            // The rectangle of an already placed tower
                            RectangleF oldTowerCollision = obj.CollisionBox;

                            if (newTowerCollision.IntersectsWith(oldTowerCollision)) {
                                // If the new tower collides with an already placed tower, the tower cannot be placed!
                                canPlace = false;
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < (GameWorldClass.Checkpoints.Count() - 1); i++) {
                        // The rectangle of a checkpoint 'tile'
                        RectangleF roadCollision;

                        if (GameWorldClass.Checkpoints[(i + 1)].X - GameWorldClass.Checkpoints[i].X < 0 || GameWorldClass.Checkpoints[(i + 1)].Y - GameWorldClass.Checkpoints[i].Y < 0) {
                            roadCollision = new RectangleF(GameWorldClass.Checkpoints[(i + 1)].X, GameWorldClass.Checkpoints[(i + 1)].Y, ((GameWorldClass.Checkpoints[i].X - GameWorldClass.Checkpoints[(i + 1)].X) + 32), ((GameWorldClass.Checkpoints[i].Y - GameWorldClass.Checkpoints[(i + 1)].Y) + 32));
                        }
                        else {
                            roadCollision = new RectangleF(GameWorldClass.Checkpoints[i].X, GameWorldClass.Checkpoints[i].Y, ((GameWorldClass.Checkpoints[(i + 1)].X - GameWorldClass.Checkpoints[i].X) + 32), ((GameWorldClass.Checkpoints[(i + 1)].Y - GameWorldClass.Checkpoints[i].Y) + 32));
                        }


                        if (newTowerCollision.IntersectsWith(roadCollision)) {
                            // If the new tower collides with an already placed tower, the tower cannot be placed!
                            canPlace = false;
                            break;
                        }
                    }

                    // if the tower is not colliding with any already placed towers
                    if (canPlace) {
                        // Places the new tower and removes $XX from the player
                        if ((other as BasicTower) != null) {
                            GameWorldClass.NewObjects.Add(new BasicTower(@"Sprites\Towers\normaltower.png", new PointF(clickPos.X - 16, clickPos.Y - 16), 0, 1, 400, true, false, false));

                            GameWorldClass.Currency -= ((BasicTower)other).Price;
                        }
                        if ((other as AATower) != null) {
                            GameWorldClass.NewObjects.Add(new AATower(@"sprites\towers\airtower.png", new PointF(clickPos.X - 16, clickPos.Y - 16), 0, 1, 400, true, false, false));

                            GameWorldClass.Currency -= ((AATower)other).Price;
                        }
                        if ((other as SplashTower) != null) {
                            GameWorldClass.NewObjects.Add(new SplashTower(@"sprites\towers\splashtower.png", new PointF(clickPos.X - 16, clickPos.Y - 16), 0, 1, 1000, true, false, false));

                            GameWorldClass.Currency -= ((SplashTower)other).Price;

                        }
                        if ((other as SlowTower) != null) {
                            GameWorldClass.NewObjects.Add(new SlowTower(@"sprites\towers\slowtower.png", new PointF(clickPos.X - 16, clickPos.Y - 16), 0, 1, 500, true, false, false));

                            GameWorldClass.Currency -= ((SlowTower)other).Price;

                            foreach (SlowTower pTower in GameWorldClass.NewObjects) {
                                if ((pTower as SlowTower) != null) {
                                    ((SlowTower)pTower).Price = 16;
                                }
                            }
                        }
                        GameWorldClass.RemoveObjects.Add(GameWorldClass.TowerToPlace);
                        GameWorldClass.TowerToPlace = null;
                        placing = false;
                    }
                }
            }

            // if collided object is Upgradebutton
            if ((other as Upgrade) != null) {
                // if a click is detected & cursor is not placing & position is outside gamefield
                if (click && !placing && GameWorldClass.MousePos.X > 540) {
                    // Upgrades tower if currency is high enough
                    ((Upgrade)other).UpgradeTower();

                }
                if (click && placing && GameWorldClass.MousePos.X > 540) {
                    GameWorldClass.RemoveObjects.Add(GameWorldClass.TowerToPlace);
                    GameWorldClass.TowerToPlace = null;
                    placing = false;

                }
            }

            // if collided object is Sellbutton
            if ((other as Sell) != null) {
                // if a click is detected & cursor is not placing & position is outside gamefield
                if (click && !placing && GameWorldClass.MousePos.X > 540) {
                    // Adds a tower to the cursor for the player to place
                    ((Sell)other).SellTower();

                    foreach (IngameObjectTracker obj in GameWorldClass.Objects) {
                        if ((obj as Upgrade) != null || (obj as Sell) != null) {
                            GameWorldClass.RemoveObjects.Add(obj);
                        }
                    }

                }
                if (click && placing && GameWorldClass.MousePos.X > 540) {
                    GameWorldClass.RemoveObjects.Add(GameWorldClass.TowerToPlace);
                    GameWorldClass.TowerToPlace = null;
                    placing = false;

                }
            }
        }

        /// <summary>
        /// Draw Method
        /// </summary>
        /// <param name="dc"></param> 
        public override void Draw(Graphics dc) {
#if DEBUG
            // DEBUG STUFF!!!!

            // Draws out the cursor position
            Font f = new Font("Arial", 25);
            dc.DrawString("PointerPos: " + (Position.X + " : " + Position.Y), f, Brushes.Gold, 160, 0);
#endif

            // Runs base.Draw
            base.Draw(dc);
        }
    }
}
