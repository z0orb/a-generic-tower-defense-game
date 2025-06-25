using System.Drawing;

namespace FinalProject
{
    class UpgradeClass : IngameObjectTracker
    {
        // Variable yg kepake klo nanti mau upgrade tower
        private TowerClass upgradeTarget;

        // imagePath: Path untuk sprite tunggal or multiple
        // startPos: Posisi tombol upgrade
        // animationSpeed: Kecepatan animasi
        // scaleFactor: Faktor untuk scaling sprite
        // speed: Kecepatan tembakan tower  
        // upgradeTarget: Tower target yang akan diupgrade
        public UpgradeClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, TowerClass upgradeTarget)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed)
        {
            this.upgradeTarget = upgradeTarget;
        }

        // Method gae upgrade tower
        public void UpgradeTower()
        {
            if (GameWorldClass.Currency >= upgradeTarget.Price)
            {
                if ((upgradeTarget as BasicTower) != null)
                {
                    ((BasicTower)upgradeTarget).SetNextUpgrade();
                }
                if ((upgradeTarget as AATower) != null)
                {
                    ((AATower)upgradeTarget).SetNextUpgrade();
                }
                if ((upgradeTarget as AreasplashTower) != null)
                {
                    ((AreasplashTower)upgradeTarget).SetNextUpgrade();
                }
                if ((upgradeTarget as SlowingTower) != null)
                {
                    ((SlowingTower)upgradeTarget).SetNextUpgrade();

                    if (upgradeTarget.UpgradeLevel >= 5)
                    {
                        GameWorldClass.RemoveObjects.Add(this);
                    }
                }
            }
            if (upgradeTarget.UpgradeLevel >= 20)
            {
                GameWorldClass.RemoveObjects.Add(this);
            }
        }

       
        // update upgrade image e tergantung amu duwe duwek po ra/cukup po ra
        public override void UpdateAnimation(float fps)
        {
            // nek cukup run image kui
            if (GameWorldClass.Currency < upgradeTarget.Price)
            {
                Sprite = Image.FromFile(@"sprites\buttons\upgradeOpac.png");
            }
            // nek ga cukup iki
            else if (GameWorldClass.Currency >= upgradeTarget.Price)
            {
                Sprite = Image.FromFile(@"sprites\buttons\upgrade.png");
            }

            base.UpdateAnimation(fps);
        }
    }
}