using System.Drawing;

namespace FinalProject 
{
    class AATower : TowerClass 
    {
        /// set dmg, range, total value, etc 
        /// imagePath = path ke sprite tower
        /// startPos = posisi starting
        /// animationSpeed = speed animasi
        /// scaleFactor = scaling 
        /// speed = fire rate tower e
        /// placed = is placed ato ga (bool)
        /// beingPlaced = bool cek tower selected sebelum di place
        /// selected = bool cek tower di select ato ga
        public AATower(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, bool placed, bool beingPlaced, bool selected)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed, placed, beingPlaced, selected) 
        {
            damage = 17;
            range = 100;
            price = 10;
            totalValue = price;
            base.speed = speed;
        }

        // ganti sprite ke opaque klo duit ga cukup
        /// fps = agar stable game nya
        public override void UpdateAnimation(float fps) 
        {
            if (beingPlaced || (!placed && GameWorldClass.Currency < price)) 
            {
                Sprite = Image.FromFile(@"sprites\towers\airtowerOpac.png");
            }
            else if (!placed && GameWorldClass.Currency >= price) 
            {
                Sprite = Image.FromFile(@"sprites\towers\airtower.png");
            }

            base.UpdateAnimation(fps);
        }

        /// Override absract SetNextUpgrade method dari Tower class
        public override void SetNextUpgrade() 
        {
            if (upgradeLevel < 20) 
            {
                GameWorldClass.Currency -= price;
                upgradeLevel++;
                TotalValue += price;

                damage = (int)(damage * 1.6);
                range = 100;
                speed = 400;
                price = (int)(price * 1.6);
            }
        }

       // ga kepakai
        public override void OnCollision(IngameObjectTracker other) 
        {

        }
    }
}