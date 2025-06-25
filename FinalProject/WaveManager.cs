using System.Drawing;

namespace FinalProject
{
    public class WaveManager : IngameObjectTracker
    {
        // Variable gae managing waves
        private int waveCounter;
        private int currentWave;
        private float difficulty;
        private float difficultyIncrease;
        private int monsterHealth;

        // currency management
        private bool earnInterest;
        private int interest;

        // variable gae atur spawn
        private bool spawning;
        private int spawnCounter;
        private float spawnAccumilator = 0.0f;
        private float spawnPerSec = 2f;

        // Properties
        public int CurrentWave
        {
            get { return currentWave; }
            set { currentWave = value; }
        }

        // imagePath: Path untuk gambar creep
        // startPos: Posisi wave
        // animationSpeed: Kecepatan animasi
        // scaleFactor: Faktor scaling sprite
        // speed: Kecepatan pergerakan wave
        public WaveManager(string imagePath, PointF startPos, float animationSpeed, float scaleFactor, float speed)
            : base(imagePath, startPos, animationSpeed, scaleFactor, speed)
        {
            // Reset waves
            waveCounter = 0;
            currentWave = 0;
            interest = 10;
            difficulty = 1.175f;
            difficultyIncrease = 10;
        }

        // Method untuk atur next wave
        public void NextWave()
        {
            if (!spawning)
            {
                // spawning on
                spawning = true;

           
                // cek klo masi ada monster yg masi hidup dari last wave
                foreach (IngameObjectTracker obj in GameWorldClass.Objects)
                {
                    if ((obj as EnemyClass) != null)
                    {
                        spawning = false;
                    }
                }

                if (spawning)
                {

                    // jika gaada counter, next wave bakal lgsg mulai di update selanjutny
                    // reset spawn counter untuk next wave
                    spawnCounter = 0;

                    earnInterest = true;

             
                    // cek klo wave counter lebih tinggi dri total wave, jika iya reset counter
                    if (waveCounter >= 5)
                    {
                        // Resets waves
                        waveCounter = 0;
                    }

                 
                    // increment current wave and wave cointer
                    waveCounter++;
                    currentWave++;

                    monsterHealth += (int)(difficultyIncrease);

                    difficultyIncrease *= difficulty;
                }
            }
        }


        // method update
        public override void Update(float deltaTime)
        {
            
            // hotung dengan framerate berapa monster yg bakal di spawn 
            spawnAccumilator += spawnPerSec * deltaTime;
            int toSpawn = (int)spawnAccumilator;
            spawnAccumilator -= (float)toSpawn;

            // loop spawn enemy smpe 25
            for (int i = 0; i < toSpawn && spawnCounter < 25 && spawning; i++)
            {
          
                spawnCounter++;

            
                // wtoch case buat manage wave mana yg spawn
                switch (waveCounter)
                {
                    case 1:
                        GameWorldClass.NewObjects.Add(new EnemyClass(@"sprites\enemies\normalmob.png", new PointF(130, -40), 0, 1, monsterHealth, 100, 1 + (int)(difficultyIncrease * 0.04), false, false));
                        break;
                    case 2:
                        GameWorldClass.NewObjects.Add(new EnemyClass(@"sprites\enemies\fastmob.png", new PointF(130, -40), 0, 1, monsterHealth * 0.8f, 150, 1 + (int)(difficultyIncrease * 0.04), false, false));
                        break;
                    case 3:
                        GameWorldClass.NewObjects.Add(new EnemyClass(@"sprites\enemies\airmob.png", new PointF(130, -40), 0, 1, monsterHealth, 100, 1 + (int)(difficultyIncrease * 0.04), true, false));
                        break;
                    case 4:
                        GameWorldClass.NewObjects.Add(new EnemyClass(@"sprites\enemies\immunemob.png", new PointF(130, -40), 0, 1, monsterHealth * 0.8f, 100, 1 + (int)(difficultyIncrease * 0.04), false, true));
                        break;
                    case 5:
                        GameWorldClass.NewObjects.Add(new EnemyClass(@"sprites\enemies\bossmob.png", new PointF(130, -40), 0, 1, (monsterHealth * 8), 75, 10 + (int)(difficultyIncrease * 0.4), false, false));
                        spawnCounter = 25;
                        break;
                    default:
                        break;
                }
            }

        
            // stop spawn pas udh ada 25 monster yg ke spawn tiap wave
            if (spawnCounter >= 25)
            {
                spawning = false;
            }

            if (earnInterest && !spawning)
            {
                bool completionChecker = true;


                // cek jika masi ada monster yg hidup dri last wave
                foreach (IngameObjectTracker obj in GameWorldClass.Objects)
                {
                    if ((obj as EnemyClass) != null)
                    {
                        completionChecker = false;
                        break;
                    }
                }
                foreach (IngameObjectTracker obj in GameWorldClass.NewObjects)
                {
                    if ((obj as EnemyClass) != null)
                    {
                        completionChecker = false;
                        break;
                    }
                }

                if (earnInterest && completionChecker)
                {
                    GameWorldClass.Currency += interest + (int)(difficultyIncrease * 0.2);
                    earnInterest = false;
                }
            }

            base.Update(deltaTime);
        }

       
        // method gae gambar text yg show current and next creep wave
        public override void Draw(Graphics dc)
        {
            Font font = new Font("Arial", 12);

            if (waveCounter == 0)
            {
                dc.DrawString("First enemy type: Slime", font, Brushes.Black, 580, 540);
            }
            if (waveCounter == 1)
            {
                dc.DrawString("Enemy type: Slime", font, Brushes.Black, 561, 53);
                dc.DrawString("Next enemy type: Wisp", font, Brushes.Black, 580, 540);
            }
            if (waveCounter == 2)
            {
                dc.DrawString("Enemy type: Wisp", font, Brushes.Black, 561, 53);
                dc.DrawString("Next enemy type: Bat", font, Brushes.Black, 580, 540);
            }
            if (waveCounter == 3)
            {
                dc.DrawString("Enemy type: Bat", font, Brushes.Black, 561, 53);
                dc.DrawString("Next enemy type: Golem", font, Brushes.Black, 580, 540);
            }
            if (waveCounter == 4)
            {
                dc.DrawString("Enemy type: Golem", font, Brushes.Black, 561, 53);
                dc.DrawString("Next enemy type: Demon", font, Brushes.Black, 580, 540);
            }
            if (waveCounter == 5)
            {
                dc.DrawString("Enemy type: Demon", font, Brushes.Black, 561, 53);
                dc.DrawString("Next enemy type: Slime", font, Brushes.Black, 580, 540);
            }

            base.Draw(dc);

#if DEBUG
            // DEBUG STUFF!
            // FPS
            //dc.DrawString("BaseHP: " + monsterHealth.ToString(), f, Brushes.Red, 0, 300);
#endif
        }
    }
}