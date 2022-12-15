using System;

namespace ZeldaSolarusRandomizer
{
    public static class main
    {


        static void Main(string[] args)
        {
           
            string rpgrtFile = @"jeu zelda_mystery_of_solarus_randomizer\RPG_RT.ldb";
            Randomizer randomizer = new Randomizer();
            
            bool success = false;
            int iteration = 0;
            while (!success)
            {
               randomizer.InitChestsLists();
               randomizer.InitChestPool();

               randomizer.RandomizedChestList();
               success = randomizer.VerifyChests();
               iteration++;
            }

            randomizer.PrintChestList();
            
            FileManager fileManager = new FileManager();
            fileManager.BackupFileRPG_RT();
            
            fileManager.ReadFile(rpgrtFile);
            fileManager.GetStartChestIndex();
            fileManager.SetChestList(randomizer.ChestsList);
            fileManager.SetSeedByte(randomizer.Seed);
            fileManager.SetOptionByte((int)randomizer.Option);
            fileManager.WriteOutputGame(rpgrtFile);

            Console.WriteLine("Generation complete !!");
            Console.Read();  
        }
    }
}
