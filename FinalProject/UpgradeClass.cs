using System.Drawing;

namespace FinalProject {
    class UpgradeClass : IngameObjectTracker {
        /// <summary>
        /// Variable used when wanting to upgrade a tower.
        /// </summary>
        private TowerClass upgradeTarget;

        /// <summary>
        /// Sets the values for the different variables.
        /// </summary>
        /// <param name="imagePath">Gets the path to a single sprite or to multiple sprites</param>
        /// <param name="startPos">Position for the upgrade button</param>
        /// <param name="animationSpeed">The speed for animations</param>
        /// <param name="scaleFactor">A factor to scale sprites</param>
        /// <param name="speed">The speed for which the tower shoots</param>
        /// <param name="upgradeTarget"></param>
        public UpgradeClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, TowerClass upgradeTarget)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed) {
            this.upgradeTarget = upgradeTarget;
        }

        /// <summary>
        /// Our method when we want to upgrade a tower.
        /// </summary>
        public void UpgradeTower() {
            if (GameWorldClass.Currency >= upgradeTarget.Price) {
                if ((upgradeTarget as BasicTower) != null) {
                    ((BasicTower)upgradeTarget).SetNextUpgrade();
                }
                if ((upgradeTarget as AATower) != null) {
                    ((AATower)upgradeTarget).SetNextUpgrade();
                }
                if ((upgradeTarget as AreasplashTower) != null) {
                    ((AreasplashTower)upgradeTarget).SetNextUpgrade();
                }
                if ((upgradeTarget as SlowingTower) != null) {
                    ((SlowingTower)upgradeTarget).SetNextUpgrade();

                    if (upgradeTarget.UpgradeLevel >= 5) {
                        GameWorldClass.RemoveObjects.Add(this);
                    }
                }
            }
            if (upgradeTarget.UpgradeLevel >= 20) {
                GameWorldClass.RemoveObjects.Add(this);
            }
        }

        /// <summary>
        /// A method that updates the Upgrade image used depending if you can afford it or not.
        /// </summary>
        /// <param name="fps"></param>
        public override void UpdateAnimation(float fps) {
            if (GameWorldClass.Currency < upgradeTarget.Price) {
                Sprite = Image.FromFile(@"sprites\buttons\upgradeOpac.png");
            }
            else if (GameWorldClass.Currency >= upgradeTarget.Price) {
                Sprite = Image.FromFile(@"sprites\buttons\upgrade.png");
            }

            base.UpdateAnimation(fps);
        }
    }
}