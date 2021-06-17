using System;

namespace ZeldaSolarusRandomizer
{
    public static class main
    {
        static void Main(string[] args)
        {
            Form1 form = new Form1();
            form.Show();            
        }

        public static void Generate()
        {
            int seed = Environment.TickCount;
            Console.WriteLine("Seed: " + seed);
            Randomizer randomizer = new Randomizer(seed);
            bool success = false;
            int iteration = 0;
            while (!success)
            {
                randomizer.InitChestsLists();
                randomizer.InitChestPool();

                randomizer.RandomizedChestList();
                success = randomizer.VerifyChests();
                iteration++;
                Console.WriteLine(iteration);
            }

            randomizer.PrintChestList();
            //randomizer.PrintChestPool();


            FileManager fileManager = new FileManager();
            fileManager.ReadFile("RPG_RT.ldb");
            fileManager.SetChestList(randomizer.ChestsList);
            fileManager.Write("RPG_RT_output.ldb");

            Console.WriteLine("Generation complete !!");
            Console.Read();  
        }
    }
}
