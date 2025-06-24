using System.Drawing;

namespace FinalProject {
    class SlowingTower : TowerClass {
        /// <summary>
        /// A value that defines the movement reduction when getting hit from the SlowTower projectile
        /// </summary>
        private float slowSpeed;

        /// <summary>
        /// Used to get access to slowSpeed above. Therefor the property is public and not private
        /// </summary>
        public float SlowSpeed {
            get { return slowSpeed; }
            set { slowSpeed = value; }
        }

        /// <summary>
        /// Sets the damage, range, price, totalvalue and speed for the SlowTower. 
        /// </summary>
        /// <param name="imagePath">Gets the path to a single sprite or to multiple sprites</param>
        /// <param name="startPos">All SlowTower position</param>
        /// <param name="animationSpeed">The speed for animations</param>
        /// <param name="scaleFactor">A factor to scale sprites</param>
        /// <param name="speed">The speed for which the tower shoots</param>
        /// <param name="placed">A bool which checks if a tower is placed</param>
        /// <param name="beingPlaced">A bool that checks if a tower is beingplaced</param>
        /// <param name="selected">A bool that checks if a tower is selected</param>
        public SlowingTower(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, bool placed, bool beingPlaced, bool selected)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed, placed, beingPlaced, selected) {
            damage = 3;
            range = 75;
            price = 7;
            TotalValue = 7;
            slowSpeed = 0.80f;
        }

        /// <summary>
        /// Overrides the method that is created in GameObject. Updates the sprite for SlowTower depending on players amount of currency. 
        /// </summary>
        /// <param name="fps">Used to get a more stabile game speed</param>
        public override void UpdateAnimation(float fps) {
            if (beingPlaced || (!placed && GameWorldClass.Currency < price)) {
                Sprite = Image.FromFile(@"sprites\towers\slowtowerOpac.png");
            }
            else if (!placed && GameWorldClass.Currency >= price) {
                Sprite = Image.FromFile(@"sprites\towers\slowtower.png");
            }

            base.UpdateAnimation(fps);
        }

        /// <summary>
        /// Overrides the absract SetNextUpgrade method from the Tower class
        /// </summary>
        public override void SetNextUpgrade() {
            if (upgradeLevel < 5) {
                GameWorldClass.Currency -= price;
                upgradeLevel++;
                TotalValue += price;

                damage = (int)(damage * 1.6);
                range = 75;
                speed = 400;
                price = (int)(price * 5f);
                slowSpeed -= 0.05f;
            }
        }

        /// <summary>
        /// Has no function in this class
        /// </summary>
        /// <param name="other">Is what the tower collides with, for instance an enemy</param>
        public override void OnCollision(IngameObjectTracker other) {
        }
    }
}