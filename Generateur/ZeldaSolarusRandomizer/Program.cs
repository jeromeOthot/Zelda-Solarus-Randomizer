﻿using System;

namespace ZeldaSolarusRandomizer
{
    public static class main
    {
        public const int MAX_SEED_NUMBER = 16777215; //FFFFFF

        static void Main(string[] args)
        {
            int seed = 0;
            string output = @"..\..\..\..\jeu zelda_mystery_of_solarus_randomizer\RPG_RT.ldb";
            Console.Write("Enter the Seed number: ");
            string input = Console.ReadLine();
            Console.WriteLine(" ");
            int inputSeed = 0;
            int.TryParse(input, out inputSeed);

            seed = inputSeed;
            if(inputSeed <= 0 || inputSeed > MAX_SEED_NUMBER)
            {
                Random randomizerSeed = new Random();
                seed = randomizerSeed.Next(1, MAX_SEED_NUMBER);
            }

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
            }

            randomizer.PrintChestList();
            
            FileManager fileManager = new FileManager();
            fileManager.BackupFileRPG_RT();
            
            fileManager.ReadFile("RPG_RT.ldb");
            fileManager.GetStartChestIndex();
            fileManager.SetChestList(randomizer.ChestsList);
            fileManager.SetSeedByte(seed);
            fileManager.WriteOutputGame(output);

            Console.WriteLine("Generation complete !!");
            Console.Read();  
        }
    }
}
