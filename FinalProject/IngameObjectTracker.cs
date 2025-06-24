using System.Collections.Generic;
using System.Drawing;

namespace FinalProject {
    public abstract class IngameObjectTracker {
        /// <summary>
        /// Speed & Positions of an object.
        /// </summary> 
        protected float speed;
        protected PointF position;

        /// <summary>
        /// Graphic List/Variables to handle the sprite, scale & animation.
        /// </summary> 
        private Image sprite;
        protected List<Image> animationFrames = new List<Image>();
        protected float currentFrameIndex;
        private float animationSpeed;
        public float scaleFactor;

        /// <summary>
        /// Properties.
        /// </summary> 
        public PointF Position {
            get { return position; }
            set { position = value; }
        }
        public Image Sprite {
            get { return sprite; }
            set { sprite = value; }
        }
        public float Speed {
            get { return speed; }
            set { speed = value; }
        }
        public virtual RectangleF CollisionBox {
            get {
                return new RectangleF(position.X, position.Y, sprite.Width * scaleFactor, sprite.Height * scaleFactor);
            }
        }

        /// <summary>
        /// Game objects constructor - Set sup each GameObject
        /// </summary>
        /// <param name="imagePath">Gets the path of the game objects</param>
        /// <param name="startPos">Start position</param>
        /// <param name="animationSpeed">The speed for animations</param>
        /// <param name="scaleFactor">A factor to scale sprites</param>
        /// <param name="speed"></param> 
        public IngameObjectTracker(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed) {
            // Sets up the list of sprite, scale & animation variables
            string[] imagePaths = imagePath.Split(';');
            this.animationFrames = new List<Image>();
            foreach (string path in imagePaths) {
                animationFrames.Add(Image.FromFile(path));
            }
            this.sprite = this.animationFrames[0];
            this.animationSpeed = animationSpeed;
            this.scaleFactor = scaleFactor;

            // Sets the current speed & position of GameObject
            this.position = startPos;
            this.speed = speed;
        }

        /// <summary>
        /// Base update method - Used to check collision.
        /// </summary>
        /// <param name="deltaTime"></param> 
        public virtual void Update(float deltaTime) {
            CheckCollision();
        }

        /// <summary>
        /// Base UpdateAnimations - Used to update images/animation in GameObject
        /// </summary>
        /// <param name="fps"></param> 
        public virtual void UpdateAnimation(float fps) {
            if (animationSpeed > 0) {
                // Finds currentFrameindex
                float deltatime = 1 / fps;
                currentFrameIndex += deltatime * animationSpeed;

                // Checks if currentFrameIndex is higher than total Images
                if (currentFrameIndex >= animationFrames.Count) {
                    // Sets index to first image
                    currentFrameIndex = 0;
                }

                // Sets the sprites image to the newfound image
                sprite = animationFrames[(int)currentFrameIndex];
            }
        }

        /// <summary>
        /// Base Draw Method - Draws out the GameObject.
        /// </summary>
        /// <param name="dc"></param> 
        public virtual void Draw(Graphics dc) {
            dc.DrawImage(sprite, position.X, position.Y, sprite.Width * scaleFactor, sprite.Height * scaleFactor);
        }

        /// <summary>
        /// Method to check for Object collision.
        /// </summary>
        public void CheckCollision() {
            foreach (IngameObjectTracker obj in GameWorldClass.Objects) {
                if (obj != this) {
                    if (this.IsCollidingWith(obj)) {
                        // Runs OnCollision() method if a collision is detected
                        OnCollision(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Method that checks if two objects CollisionBoxes are Intersecting.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns> 
        public bool IsCollidingWith(IngameObjectTracker other) {
            return CollisionBox.IntersectsWith(other.CollisionBox);
        }

        /// <summary>
        /// Base OnCollision Method
        /// </summary>
        /// <param name="other"></param> 
        public virtual void OnCollision(IngameObjectTracker other) { }
    }
}