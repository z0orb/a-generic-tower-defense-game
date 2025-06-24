using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace FinalProject
{
    public class EnemyClass : IngameObjectTracker
    {
        // checkpoint checker gae tiap musuh
        private int cPointChecker = 0;

        // variabel e health musuh
        private float currentHealth;
        private float maxHealth;
        private int healthPercentage;
        private bool dead;
        private float temporarySpeed;

        // variabel action/effect musuh e
        private bool flying;
        private bool immune;
        private bool isSlowed;

        // loot musuh pas mati
        private int dropCurrency;

        // ui kae health bar
        Rectangle healthBar;
        SolidBrush b;
        Pen p;

        // stopwatch gae efek slow
        Stopwatch sw;

        // getter setter property
        public float CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                currentHealth = value;
                if (currentHealth < 0)
                    currentHealth = 0;
            }
        }

        public bool Flying
        {
            get { return flying; }
            set { flying = value; }
        }

        public bool Immune
        {
            get { return immune; }
            set { immune = value; }
        }

        public bool IsSlowed
        {
            get { return isSlowed; }
            set { isSlowed = value; }
        }

        public float TemporarySpeed
        {
            get { return temporarySpeed; }
            set { temporarySpeed = value; }
        }

        /// constructor musuh
        /// imagePath = path ke sprite musuh
        /// startPos = posisi awal musuh
        /// animationSpeed = kecepatan animasi
        /// scaleFactor = scaling sprite
        /// health = jumlah HP musuh
        /// speed = kecepatan musuh
        /// dropCurrency = uang yang dijatuhkan saat mati
        /// flying = apakah musuh terbang
        /// immune = apakah musuh kebal
        public EnemyClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float health, float speed, float dropCurrency, bool flying, bool immune)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed)
        {
            temporarySpeed = Speed;

            this.maxHealth = health;
            this.currentHealth = health;
            healthBar = new Rectangle((int)position.X, (int)position.Y + 16, 100, 4);
            dead = false;

            this.flying = flying;
            this.immune = immune;
            this.dropCurrency = (int)dropCurrency;

            b = new SolidBrush(Color.White);
            p = new Pen(Color.Red, 2);

            sw = new Stopwatch();
        }

        // update per frame
        public override void Update(float deltaTime)
        {
            // cek musuh uwis dekat ke checkpoint po ga
            if (position.X + (speed / 100 * 3) >= GameWorldClass.Checkpoints[cPointChecker].X &&
                position.X - (speed / 100 * 3) <= GameWorldClass.Checkpoints[cPointChecker].X &&
                position.Y + (speed / 100 * 3) >= GameWorldClass.Checkpoints[cPointChecker].Y &&
                position.Y - (speed / 100 * 3) <= GameWorldClass.Checkpoints[cPointChecker].Y)
            {
                // nek wis cukup set posisi musuh e ke cek point
                position.X = GameWorldClass.Checkpoints[cPointChecker].X;
                position.Y = GameWorldClass.Checkpoints[cPointChecker].Y;

                // cek po ik checkpoint terakhir
                if (cPointChecker + 1 < GameWorldClass.Checkpoints.Count())
                {
                    cPointChecker++;
                }
                else
                {
                    // reset checkpoint ke awal
                    cPointChecker = 0;
                    position.X = GameWorldClass.Checkpoints[0].X;
                    position.Y = GameWorldClass.Checkpoints[0].Y;

                    // player ilang 1 nyawa
                    GameWorldClass.LifeCounter -= 1;
                }
            }

            else // enemy kurang dekat ke checkpoint
            {
                // musuh gerak nek checkpoint
                if (position.X != GameWorldClass.Checkpoints[cPointChecker].X)
                {
                    if (position.X + 1 > GameWorldClass.Checkpoints[cPointChecker].X)
                        position.X -= deltaTime * speed;
                    else if (position.X - 1 < GameWorldClass.Checkpoints[cPointChecker].X)
                        position.X += deltaTime * speed;
                }
                else if (Position.X + 1 >= GameWorldClass.Checkpoints[cPointChecker].X &&
                         Position.X - 1 <= GameWorldClass.Checkpoints[cPointChecker].X)
                {
                    position.X = GameWorldClass.Checkpoints[cPointChecker].X;
                }

                if (position.Y != GameWorldClass.Checkpoints[cPointChecker].Y)
                {
                    if (position.Y + 1 > GameWorldClass.Checkpoints[cPointChecker].Y)
                        position.Y -= deltaTime * speed;
                    else if (position.Y - 1 < GameWorldClass.Checkpoints[cPointChecker].Y)
                        position.Y += deltaTime * speed;
                }
                else if (Position.Y + 1 >= GameWorldClass.Checkpoints[cPointChecker].Y &&
                         Position.Y - 1 <= GameWorldClass.Checkpoints[cPointChecker].Y)
                {
                    position.Y = GameWorldClass.Checkpoints[cPointChecker].Y;
                }
            }

            // efek slow durasinya abis
            if (sw.ElapsedMilliseconds > 5000)
            {
                isSlowed = false;
                Speed = temporarySpeed;
            }

            base.Update(deltaTime);
        }

        // render musuh + health bar
        public override void Draw(Graphics dc)
        {
            base.Draw(dc);

            // hitbox debug (optional)
            // dc.DrawRectangle(p, CollisionBox.X, CollisionBox.Y, CollisionBox.Width, CollisionBox.Height);

            // update posisi & ukuran health bar
            healthPercentage = (int)(((double)currentHealth / (double)maxHealth) * 100);
            healthBar.X = (int)position.X - 2;
            healthBar.Y = (int)position.Y + 30;
            healthBar.Width = (int)((double)healthPercentage * 0.4);

            // draw background bar
            b.Color = Color.Black;
            dc.FillRectangle(b, new Rectangle((int)healthBar.X, (int)healthBar.Y, (int)(100 * 0.4f), 4));

            // warna bar berdasarkan sisa HP
            if (healthPercentage >= 50) b.Color = Color.ForestGreen;
            if (healthPercentage < 50) b.Color = Color.Orange;
            if (healthPercentage < 20) b.Color = Color.Red;

            // draw HP bar
            dc.FillRectangle(b, healthBar);
        }

       // terjai apan tabrakan karo objek laen
        public override void OnCollision(IngameObjectTracker other)
        {
        }

        // efek ketika musuh mati
        public void OnDeath()
        {
            if (!dead)
            {
                GameWorldClass.RemoveObjects.Add(this);
                GameWorldClass.Currency += dropCurrency;
                dead = true;
            }
        }

        // dipanggil kalau musuh terkena efek slow
        public void SlowMonster()
        {
            isSlowed = true;
            sw.Reset();
            sw.Start();
        }
    }
}
