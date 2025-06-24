using System;
using System.Drawing;

namespace FinalProject {
    class ProjectileClass : IngameObjectTracker {
        /// <summary>
        /// Fields for the destination point of the projectiles, the distance between tower and targeted enemy and the difference between target x coordinate and tower x cooordinate. Same goes for y coordinates.
        /// </summary>
        private PointF startPos;
        private PointF destination;
        private float distance;
        private float xDif;
        private float yDif;
        
        /// <summary>
        /// Projectile constructor that takes the same arguments as the gameobject
        /// </summary>
        /// <param name="imagePath">Gets the path to a single sprite or to multiple sprites</param>
        /// <param name="startPos">Projectile position</param>
        /// <param name="animationSpeed">The speed for animations</param>
        /// <param name="scaleFactor">A factor to scale sprites</param>
        /// <param name="speed">The speed for which the projectiles travel</param>
        /// <param name="destination">Where the projectile is supposed to land</param>
        public ProjectileClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, PointF destination)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed) {
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

        /// <summary>
        /// Method for the collision between projectile and what it collides with
        /// </summary>
        /// <param name="other"></param>
        public override void OnCollision(IngameObjectTracker other) {

        }

        /// <summary>
        /// Updates for projectile.
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(float deltaTime) {
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

            if (traveled >= distance) {
                GameWorldClass.RemoveObjects.Add(this);
            }
        }
    }
}