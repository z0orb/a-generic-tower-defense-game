using System.Drawing;

namespace FinalProject 
{
    class SellClass : IngameObjectTracker 
    {
        //tentukan sell target tower
        private TowerClass sellTarget;

        public SellClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, TowerClass sellTarget)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed) 
        {
            this.sellTarget = sellTarget;
        }

       //dAPET gold seberapa
        public void SellTower() 
        {
            GameWorldClass.Currency += (int)(sellTarget.TotalValue * 0.75f);
            GameWorldClass.RemoveObjects.Add(sellTarget);
        }
    }
}
