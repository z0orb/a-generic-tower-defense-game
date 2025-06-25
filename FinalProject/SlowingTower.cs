using System.Drawing;

namespace FinalProject 
{
    class SlowingTower : TowerClass 
    {

        private float slowSpeed;

        //utk dapt value slowspeed
        public float SlowSpeed 
        {
            get { return slowSpeed; }
            set { slowSpeed = value; }
        }

        public SlowingTower(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, bool placed, bool beingPlaced, bool selected)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed, placed, beingPlaced, selected) 
        {
            damage = 3;
            range = 75;
            price = 7;
            TotalValue = 7;
            slowSpeed = 0.80f;
        }

        public override void UpdateAnimation(float fps) 
        {
            if (beingPlaced || (!placed && GameWorldClass.Currency < price)) 
            {
                Sprite = Image.FromFile(@"sprites\towers\slowtowerOpac.png");
            }
            else if (!placed && GameWorldClass.Currency >= price) 
            {
                Sprite = Image.FromFile(@"sprites\towers\slowtower.png");
            }

            base.UpdateAnimation(fps);
        }

        public override void SetNextUpgrade() 
        {
            if (upgradeLevel < 5) 
            {
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

        public override void OnCollision(IngameObjectTracker other) {
        }
    }
}