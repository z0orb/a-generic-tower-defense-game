using System;
using System.Drawing;

namespace FinalProject 
{
    class ProjectileClass : IngameObjectTracker 
    {
        // Ini isian buat tujuan peluru ditembakkan, jarak antara towernya sama musuh yang lagi ditarget, terus selisih antara koordinat X musuh sama koordinat X towernya. Buat koordinat Y juga sama caranya.
        private PointF startPos;
        private PointF destination;
        private float distance;
        private float xDif;
        private float yDif;
        

        public ProjectileClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, PointF destination)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed) 
        {
            position.X -= 8;
            position.Y -= 8;

            this.startPos = this.position;
            this.destination = destination;

            //Calculates the distance between targeted enemy and tower.
            //X and Y position of targeted enemy.
            float targetXPosition = destination.X;
            float targetYPosition = destination.Y;

            //X and Y position of the tower shooting.
            float towerXPosition = position.X;
            float towerYPosition = position.Y;

            //Difference between target x position and tower x position.
            xDif = targetXPosition - towerXPosition;
            yDif = targetYPosition - towerYPosition;  //Difference between target y position and tower y position.

            distance = (float)Math.Sqrt(xDif * xDif + yDif * yDif);
        }

        //method collision projecktile
        public override void OnCollision(IngameObjectTracker other) 
        {

        }

        //projectile travel update
        public override void Update(float deltaTime) 
        {
            position.X += deltaTime * (speed * xDif / 150);
            position.Y += deltaTime * (speed * yDif / 150);

            //Calculates the distance between targeted projectile and startposition.
            //X and Y position of targeted enemy.
            float projectilePositionX = position.X;
            float projectilePositionY = position.Y;

            //X and Y position of the tower shooting.
            float startingPosX = startPos.X;
            float startingPosY = startPos.Y;

            //Difference between target x position and tower x position.
            float newDifX = projectilePositionX - startingPosX;
            float newDifY = projectilePositionY - startingPosY;  //Difference between target y position and tower y position.

            float traveled = (float)Math.Sqrt(newDifX * newDifX + newDifY * newDifY);

            if (traveled >= distance) 
            {
                GameWorldClass.RemoveObjects.Add(this);
            }
        }
    }
}