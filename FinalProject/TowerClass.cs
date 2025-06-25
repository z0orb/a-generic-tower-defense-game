using System;
using System.Diagnostics;
using System.Drawing;

namespace FinalProject
{
    public abstract class TowerClass : IngameObjectTracker
    {
        // variabel untuk cek apakah tower sudah di-place, sedang di-place, atau selected
        protected bool placed;
        protected bool beingPlaced;
        protected bool selected;

        // Variabel Tower
        protected int upgradeLevel;         // Level upgrade saat ini
        protected int damage;               // Damage saat ini
        protected int range;                // Jarak tembak saat ini
        protected int price;                // Harga tower (juga dipakai untuk upgrade)
        protected float totalValue;         // Total nilai tower

        // Target enemy dari tower ini
        protected EnemyClass target;

        // Stopwatch untuk menghitung waktu antara tembakan
        Stopwatch sw;

        // Elemen untuk drawing
        SolidBrush b;
        Pen p;

        // Properties untuk variabel-variabel
        public bool Placed
        {
            get { return placed; }
            set { placed = value; }
        }
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        public float TotalValue
        {
            get { return totalValue; }
            set { totalValue = value; }
        }
        public int UpgradeLevel
        {
            get { return upgradeLevel; }
            set { upgradeLevel = value; }
        }

        // imagePath: Path untuk single sprite atau multiple sprites  
        // startPos: Posisi awal tower  
        // animationSpeed: Kecepatan animasi  
        // scaleFactor: Faktor untuk scaling sprite  
        // speed: Kecepatan tembakan tower  
        // placed: Boolean untuk cek apakah tower sudah di-place  
        // beingPlaced: Boolean untuk cek apakah tower sedang di-place  
        // selected: Boolean untuk cek apakah tower sedang selected  
        public TowerClass(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed, bool placed, bool beingPlaced, bool selected)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed)
        {
            // Set kondisi awal tower
            this.placed = placed;
            this.beingPlaced = beingPlaced;
            this.selected = selected;
            upgradeLevel = 1;

            // Buat stopwatch baru
            sw = new Stopwatch();

            // Set brush dan Pen
            b = new SolidBrush(Color.White);
            p = new Pen(Color.Red, 2);
        }

        // Update Method untuk Tower
        public override void Update(float deltaTime)
        {
            // Update posisi tower berdasarkan cursor (saat tower sedang di-place)
            if (beingPlaced)
            {
                position.X = GameWorldClass.MousePos.X - Sprite.Width / 2;
                position.Y = GameWorldClass.MousePos.Y - Sprite.Height / 2;
            }

            if (sw.Elapsed.Seconds > 60)
            {
                sw.Stop();
            }

            // Cek apakah ada monster dalam jangkauan
            if (placed && target == null)
            {
                // Cek semua object untuk mencari monster jika tower belum punya target
                foreach (IngameObjectTracker m in GameWorldClass.Objects)
                {
                    if ((m as EnemyClass) != null)
                    {
                        if (((EnemyClass)m).CurrentHealth <= 0)
                        {
                            ((EnemyClass)m).OnDeath();
                        }

                        // Hitung jarak ke monster
                        float targetX = m.Position.X + m.Sprite.Width / 2;
                        float targetY = m.Position.Y + m.Sprite.Height / 2;

                        float fromX = this.Position.X + this.Sprite.Width / 2;
                        float fromY = this.Position.Y + this.Sprite.Height / 2;

                        float x = targetX - fromX;
                        float y = targetY - fromY;
                        float distance = (float)Math.Sqrt(x * x + y * y);

                        // Jika monster dalam jangkauan, set sebagai target
                        if (distance < range + 10)
                        {
                            if ((this as SlowingTower) != null)
                            {
                                if (!((EnemyClass)m).IsSlowed)
                                {
                                    target = (EnemyClass)m;
                                    break;
                                }
                                else
                                {
                                    target = (EnemyClass)m;
                                }
                            }
                            else
                            {
                                target = (EnemyClass)m;
                                break;
                            }
                        }
                    }
                }
            }
            else if (target != null)
            {
                // Cek jarak ke target saat ini
                float targetX = target.Position.X + target.Sprite.Width / 2;
                float targetY = target.Position.Y + target.Sprite.Height / 2;

                float fromX = this.Position.X + this.Sprite.Width / 2;
                float fromY = this.Position.Y + this.Sprite.Height / 2;

                float x = targetX - fromX;
                float y = targetY - fromY;
                float distance = (float)Math.Sqrt(x * x + y * y);

                // Handle target
                if (target.CurrentHealth <= 0)
                {
                    // Jika target mati, remove dan reset target
                    target.OnDeath();
                    foreach (IngameObjectTracker m in GameWorldClass.Objects)
                    {
                        if ((m as EnemyClass) != null)
                        {
                            if (((EnemyClass)m).CurrentHealth <= 0)
                            {
                                ((EnemyClass)m).OnDeath();
                            }
                        }
                    }

                    target = null;
                }
                else if (distance < range + 10)
                {
                    // Tembak target jika dalam jangkauan dan tower tidak dalam cooldown
                    if (sw.ElapsedMilliseconds > speed || !sw.IsRunning)
                    {
                        if ((this as BasicTower) != null)
                        {
                            GameWorldClass.NewObjects.Add(new ProjectileClass(@"sprites\towers\normalprojectile.png", new PointF(position.X + (Sprite.Width / 2), position.Y + (Sprite.Width / 2)), 1, 1, (2000 - speed), target.Position));
                            target.CurrentHealth -= damage;
                        }

                        if ((this as AATower) != null)
                        {
                            if (target.Flying)
                            {
                                GameWorldClass.NewObjects.Add(new ProjectileClass(@"sprites\towers\airprojectile.png", new PointF(position.X + (Sprite.Width / 2), position.Y + (Sprite.Width / 2)), 1, 1, (2000 - speed), target.Position));
                                target.CurrentHealth -= damage;
                            }
                        }

                        if ((this as AreasplashTower) != null)
                        {
                            if (!target.Flying)
                            {
                                GameWorldClass.NewObjects.Add(new ProjectileClass(@"sprites\towers\splashprojectile.png", new PointF(position.X + (Sprite.Width / 2), position.Y + (Sprite.Width / 2)), 1, 1, (2000 - speed), target.Position));
                                foreach (IngameObjectTracker m in GameWorldClass.Objects)
                                {
                                    if ((m as EnemyClass) != null)
                                    {
                                        // Hitung jarak ke enemy lain
                                        float mTargetX = m.Position.X + m.Sprite.Width / 2;
                                        float mTargetY = m.Position.Y + m.Sprite.Height / 2;

                                        float mFromX = target.Position.X + target.Sprite.Width / 2;
                                        float mFromY = target.Position.Y + target.Sprite.Height / 2;

                                        float mX = mTargetX - mFromX;
                                        float mY = mTargetY - mFromY;
                                        float mDistance = (float)Math.Sqrt(mX * mX + mY * mY);

                                        // Jika monster dalam splash range, beri damage
                                        if (mDistance < ((AreasplashTower)this).SplashRange + 16)
                                        {
                                            ((EnemyClass)m).CurrentHealth -= damage;
                                        }
                                    }
                                }
                            }
                        }

                        // Jika tower adalah SlowTower, tambahkan projectile baru
                        if ((this as SlowingTower) != null)
                        {
                            GameWorldClass.NewObjects.Add(new ProjectileClass(@"sprites\towers\slowprojectile.png", new PointF(position.X + (Sprite.Width / 2), position.Y + (Sprite.Width / 2)), 1, 1, (2000 - speed), target.Position));

                            // Jika enemy belum slowed dan tidak immune, slow enemy
                            if (!target.IsSlowed)
                            {
                                if (!target.Immune)
                                {
                                    target.Speed = target.Speed * ((SlowingTower)this).SlowSpeed;
                                    target.SlowMonster();
                                }
                            }
                            // Jika target sudah slowed, cek apakah perlu di-slow lagi
                            else
                            {
                                if (target.TemporarySpeed * ((SlowingTower)this).SlowSpeed < target.Speed)
                                {
                                    target.Speed = target.TemporarySpeed;
                                    target.Speed = target.Speed * ((SlowingTower)this).SlowSpeed;
                                    target.SlowMonster();
                                }
                            }

                            target.CurrentHealth -= damage;
                            target = null;
                        }

                        // Reset cooldown Stopwatch
                        sw.Reset();
                        sw.Start();
                    }
                }
                else
                {
                    // Jika target keluar jangkauan, reset target
                    target = null;
                }
            }

            // Jalankan base.Update
            base.Update(deltaTime);
        }

        // Override method Draw untuk menggambar circle range dan upgrade options
        public override void Draw(Graphics dc)
        {
            // Set font
            Font f = new Font("Arial", 18);

            // Jalankan base.Draw
            base.Draw(dc);

            // Gambar range tower ketika sedang di-place atau selected
            if (selected || beingPlaced)
            {
                p.Color = Color.DarkGray;
                dc.DrawEllipse(p, Position.X - range + Sprite.Size.Width / 2, Position.Y - range + Sprite.Size.Height / 2, range * 2, range * 2);
                p.Color = Color.Yellow;

                // Gambar elemen khusus untuk tower yang selected (upgrade, sell area)
                if (selected)
                {
                    dc.DrawRectangle(p, CollisionBox.X, CollisionBox.Y, CollisionBox.Width, CollisionBox.Height);

                    // Set font & gambar menu upgrade
                    f = new Font("Arial", 13, FontStyle.Bold);
                    dc.DrawString("Upgrade Tower: level " + upgradeLevel, f, Brushes.Black, 561, 290);

                    // Headline untuk damage dan range saat ini
                    f = new Font("Arial", 11, FontStyle.Bold | FontStyle.Underline);
                    dc.DrawString("Now: ", f, Brushes.Black, 561, 325);

                    // Tampilkan damage saat ini
                    f = new Font("Arial", 8, FontStyle.Bold);
                    dc.DrawString("Damage: ", f, Brushes.Black, 561, 350);
                    f = new Font("Arial", 8, FontStyle.Regular);
                    dc.DrawString(damage.ToString(), f, Brushes.Black, 612, 350);

                    // Tampilkan splash range untuk AreasplashTower
                    if ((this as AreasplashTower) != null)
                    {
                        f = new Font("Arial", 8, FontStyle.Bold);
                        dc.DrawString("Splash Range: ", f, Brushes.Black, 561, 370);
                        f = new Font("Arial", 8, FontStyle.Regular);
                        dc.DrawString(((AreasplashTower)this).SplashRange.ToString(), f, Brushes.Black, 640, 370);
                    }
                    // Tampilkan slow percentage untuk SlowingTower
                    else if ((this as SlowingTower) != null)
                    {
                        f = new Font("Arial", 8, FontStyle.Bold);
                        dc.DrawString("Slow: ", f, Brushes.Black, 561, 370);
                        f = new Font("Arial", 8, FontStyle.Regular);
                        dc.DrawString((Math.Round((1 - (((SlowingTower)this).SlowSpeed)) * 100, 0)).ToString() + "%", f, Brushes.Black, 595, 370);
                    }
                    // Tampilkan range untuk tower lainnya
                    else
                    {
                        f = new Font("Arial", 8, FontStyle.Bold);
                        dc.DrawString("Range: ", f, Brushes.Black, 561, 370);
                        f = new Font("Arial", 8, FontStyle.Regular);
                        dc.DrawString(range.ToString(), f, Brushes.Black, 603, 370);
                    }

                    // Cek level upgrade dan tampilkan stat setelah upgrade
                    if ((this as SlowingTower) == null && upgradeLevel < 20 || (this as SlowingTower) != null && upgradeLevel < 5)
                    {
                        // Stat setelah upgrade
                        f = new Font("Arial", 11, FontStyle.Bold | FontStyle.Underline);
                        dc.DrawString("After: ", f, Brushes.Black, 680, 325);

                        // Tampilkan damage setelah upgrade untuk BasicTower
                        if ((this as BasicTower) != null)
                        {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Damage: ", f, Brushes.Black, 680, 350);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(((int)(damage * 1.6)).ToString(), f, Brushes.Black, 730, 351);
                        }
                        // Tampilkan damage setelah upgrade untuk AATower
                        else if ((this as AATower) != null)
                        {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Damage: ", f, Brushes.Black, 680, 350);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(((int)(damage * 1.6)).ToString(), f, Brushes.Black, 730, 351);
                        }
                        // Tampilkan damage setelah upgrade untuk SlowingTower
                        else if ((this as SlowingTower) != null)
                        {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Damage: ", f, Brushes.Black, 680, 350);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(((int)(damage * 1.6)).ToString(), f, Brushes.Black, 730, 351);
                        }
                        // Tampilkan damage setelah upgrade untuk tower lainnya
                        else
                        {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Damage: ", f, Brushes.Black, 680, 350);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(((int)(damage * 1.7)).ToString(), f, Brushes.Black, 730, 351);
                        }

                        // Tampilkan splash range setelah upgrade untuk AreasplashTower
                        if ((this as AreasplashTower) != null)
                        {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Splash Range: ", f, Brushes.Black, 680, 370);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString((((AreasplashTower)this).SplashRange + 5).ToString(), f, Brushes.Black, 760, 371);
                        }
                        // Tampilkan slow percentage setelah upgrade untuk SlowingTower
                        else if ((this as SlowingTower) != null)
                        {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Slow: ", f, Brushes.Black, 680, 370);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString((Math.Round((1 - (((SlowingTower)this).SlowSpeed)) * 100, 0) + 5).ToString() + "%", f, Brushes.Black, 715, 371);
                        }
                        // Tampilkan range setelah upgrade untuk tower lainnya
                        else
                        {
                            f = new Font("Arial", 8, FontStyle.Bold);
                            dc.DrawString("Range: ", f, Brushes.Black, 680, 370);
                            f = new Font("Arial", 8, FontStyle.Regular);
                            dc.DrawString(range.ToString(), f, Brushes.Black, 721, 371);
                        }

                        // Tampilkan harga upgrade
                        f = new Font("Arial", 11, FontStyle.Bold);
                        dc.DrawString("Upgrade Cost: ", f, Brushes.Black, 576, 400);
                        f = new Font("Arial", 11, FontStyle.Bold);
                        dc.DrawString("$ " + price, f, Brushes.Gold, 680, 400);
                    }
                }
            }

#if DEBUG
            // DEBUG STUFF!!!
#endif
        }

        // Tampilkan radius tower dan tombol upgrade/sell saat tower di-select
        public void OnSelection()
        {
            selected = true;

            if ((this as SlowingTower) == null && upgradeLevel < 20 || (this as SlowingTower) != null && upgradeLevel < 5)
            {
                GameWorldClass.NewObjects.Add(new UpgradeClass(@"sprites\buttons\upgrade.png", new PointF(650, 440), 0, 1, 1, this));
            }

            GameWorldClass.NewObjects.Add(new SellClass(@"sprites\buttons\sell.png", new PointF(570, 440), 0, 1, 1, this));
        }

        // Hapus tombol upgrade/sell saat tower di-deselect
        public void OnDeselection()
        {
            selected = false;

            foreach (IngameObjectTracker obj in GameWorldClass.Objects)
            {
                if ((obj as UpgradeClass) != null || (obj as SellClass) != null)
                {
                    GameWorldClass.RemoveObjects.Add(obj);
                }
            }
        }

        // Method abstract untuk set upgrade berikutnya (diimplementasikan di subclass)
        public abstract void SetNextUpgrade();
    }
}