using System.Collections.Generic;
using System.Drawing;

namespace FinalProject 
{
    public abstract class IngameObjectTracker 
    {
        
        //speed sama position object
        protected float speed;
        protected PointF position;

        //atribut graphics utk sprite
        private Image sprite;
        protected List<Image> animationFrames = new List<Image>();
        protected float currentFrameIndex;
        private float animationSpeed;
        public float scaleFactor;

        public PointF Position 
        {
            get { return position; }
            set { position = value; }
        }
        public Image Sprite 
        {
            get { return sprite; }
            set { sprite = value; }
        }
        public float Speed 
        {
            get { return speed; }
            set { speed = value; }
        }
        public virtual RectangleF CollisionBox 
        {
            get {
                return new RectangleF(position.X, position.Y, sprite.Width * scaleFactor, sprite.Height * scaleFactor);
            }
        }


        //imagePath = path asset image 
        //startPos = starting position
        //animationSpeed = kecepatan animasi
        //scaleFactor = faktor scaling sprite
        //speed
        public IngameObjectTracker(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed) 
        {
            string[] imagePaths = imagePath.Split(';');
            this.animationFrames = new List<Image>();
            foreach (string path in imagePaths) 
            {
                animationFrames.Add(Image.FromFile(path));
            }
            this.sprite = this.animationFrames[0];
            this.animationSpeed = animationSpeed;
            this.scaleFactor = scaleFactor;

            // set speed dan posisi gameobject
            this.position = startPos;
            this.speed = speed;
        }

        //check collision
        public virtual void Update(float deltaTime) 
        {
            CheckCollision();
        }

          //update image sama animasi di gameobject
        public virtual void UpdateAnimation(float fps) 
        {
            if (animationSpeed > 0) 
            {
                // cari frameIndex current
                float deltatime = 1 / fps;
                currentFrameIndex += deltatime * animationSpeed;

                if (currentFrameIndex >= animationFrames.Count) 
                {
                    //sets index to first image
                    currentFrameIndex = 0;
                }

                //set sprite image ke image baru
                sprite = animationFrames[(int)currentFrameIndex];
            }
        }

        //base draw method utk drawing gameobject
        public virtual void Draw(Graphics dc) {
            dc.DrawImage(sprite, position.X, position.Y, sprite.Width * scaleFactor, sprite.Height * scaleFactor);
        }

        public void CheckCollision() 
        {
            foreach (IngameObjectTracker obj in GameWorldClass.Objects) 
            {
                if (obj != this) {
                    if (this.IsCollidingWith(obj)) 
                    {
                        // Runs OnCollision() method if a collision is detected
                        OnCollision(obj);
                    }
                }
            }
        }

          //check if dua collission box intersecting
        public bool IsCollidingWith(IngameObjectTracker other) 
        {
            return CollisionBox.IntersectsWith(other.CollisionBox);
        }

        //oncollision method base
        public virtual void OnCollision(IngameObjectTracker other) 
        { 

        }
    }
}