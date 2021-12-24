using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZeldaSolarusRandomizer
{
    public class Randomizer
    {
        public const string SPOILER_FILE =  "Spoiler.txt";
        public const int NB_CHEST = 166;
        public const int NB_HEART = 10;
        public const int NB_QUARTER_HEART = 28;
        public const int MAX_SEED_NUMBER = 16777215; //FFFFFF
        public const int MAX_OPTIONS = 255; //FF

        private readonly ItemType[] UsefulItems = new ItemType[29] { ItemType.Arc, ItemType.Armure,  ItemType.Boomerang, ItemType.Bottes, ItemType.Bouclier_Miroir, ItemType.Bouteille_Vide,
            ItemType.Cle_Bois, ItemType.Cle_Fer, ItemType.Cle_Glace, ItemType.Cle_Pierre, ItemType.Cle_Roc, ItemType.Cle_Rouge, ItemType.Cle_Terre, ItemType.Cle_Os,
            ItemType.code_secret, ItemType.Edelwiss, ItemType.Gant_Force, ItemType.Gant_Force, ItemType.Grappin, ItemType.Lanterne, ItemType.Lingot_Or, ItemType.Palmes, ItemType.Miroir_vert,
            ItemType.Pierre_inferno, ItemType.Pierre_inferno, ItemType.Pierre_inferno, ItemType.Plume, ItemType.Somaria_Canne, ItemType.Tarte };

        private readonly ItemType[] rupeeItems = new ItemType[25] { ItemType.Rupee1, ItemType.Rupee1, ItemType.Rupee1, ItemType.Rupee1, ItemType.Rupee1, ItemType.Rupee1, ItemType.Rupee1,
            ItemType.Rupees20, ItemType.Rupees20, ItemType.Rupees20,  ItemType.Rupees20, ItemType.Rupees20, ItemType.Rupees20,  ItemType.Rupees20, ItemType.Rupees20, ItemType.Rupees20,  ItemType.Rupees20,
            ItemType.Rupees50, ItemType.Rupees50, ItemType.Rupees50, ItemType.Rupees50, ItemType.Rupees50, ItemType.Rupees50,
            ItemType.Rupees100,ItemType.Rupees100
           };
        private readonly ItemType[] otherItems = new ItemType[13] {
            ItemType.Bombes5, ItemType.Bombes5, ItemType.Bombes5, ItemType.Bombes5,
           ItemType.Fleches10, ItemType.Fleches10, ItemType.Fleches10,
           ItemType.Armure, ItemType.BouclierBois, ItemType.Bouteille_Energie, ItemType.Carte_Monde, ItemType.Epee, ItemType.Magic_cape
            };

        private readonly ItemType[] itemsForGanon = new ItemType[] { ItemType.Arc, ItemType.Armure, ItemType.Bouclier_Miroir, ItemType.Bottes, ItemType.Bouteille_Vide, ItemType.Cle_Glace,
            ItemType.Cle_Roc, ItemType.code_secret, ItemType.Edelwiss, ItemType.Gant_Force, ItemType.Gant_Force, ItemType.Grappin, ItemType.Lanterne,
            ItemType.Palmes, ItemType.Pierre_inferno, ItemType.Pierre_inferno, ItemType.Pierre_inferno, ItemType.Plume, ItemType.Somaria_Canne };


        private List<Chest> dependenciesList;

        public StreamWriter SpoilerFile { get; set; }
        public List<int> ChestPool = new List<int>();
        public List<Chest> ChestsList { get; set; }
        public int Seed { get; set; }
        public OptionsEnum Option { get; set; }

        Random random;

        public Randomizer()
        {
            EnterSeed();
            EnterOption();
            random = new Random(Seed);
            SpoilerFile = new StreamWriter(SPOILER_FILE);
            WriteSpoilerHeader(Seed);
        }

        public void EnterSeed()
        {
            int seed = 0;
            Console.Write("Enter the Seed number: ");
            string input = Console.ReadLine();
            Console.WriteLine(" ");

            int.TryParse(input, out seed);
            if (seed <= 0 || seed > MAX_SEED_NUMBER)
            {
                Random randomizerSeed = new Random();
                seed = randomizerSeed.Next(1, MAX_SEED_NUMBER);
            }
            this.Seed = seed;
        }

        public void EnterOption()
        {
           // int option = 0;
            Console.Write("Enter the option number: ");
            string input = Console.ReadLine();
            Console.WriteLine(" ");

            int.TryParse(input, out int option);
            this.Option = (OptionsEnum)option;
        }

        public void InitChestPool()
        {
            //int chestToPlace = ChestsList.Where(x => x.IsVanilla).Count();
            for (int i=0; i < NB_CHEST; i++)
            {
                if( ChestsList[i].IsVanilla == false )
                    ChestPool.Add(i);
            }
            Console.WriteLine("Vanilla count: " + ChestsList.Where(x => x.IsVanilla).Count());
        }

        public void WriteSpoilerHeader(int seedNumber)
        {
            SpoilerFile.WriteLine("-----------------------------------------------------------------------------------------");
            SpoilerFile.WriteLine("Spoiler: seed no. " + seedNumber);
            SpoilerFile.WriteLine("-----------------------------------------------------------------------------------------");
        }

        public void InitChestsLists()
        {
            ChestsList = JsonConvert.DeserializeObject<List<Chest>>(File.ReadAllText("basicChestsList.json"));

            //Hack temporaire car il ne faut pas modifier directement la liste de chests, car j'ai besoin des dependencies pour valider les chests.
            dependenciesList = JsonConvert.DeserializeObject<List<Chest>>(File.ReadAllText("basicChestsList.json"));
        }
        
        public void ShuffleList(List<ItemType> list)
        {
            list.Sort((x, y) => random.Next(-1, 2));
        }

        public void RandomizedChestList()
        {
            int count = NB_CHEST - ChestPool.Count;
            
            PlaceUsefulItems();
            PlaceRupeeItems();
            PlaceHeartItems();
            PlaceQuarterHeartItems();
            PlaceOtherItems(); 
        }

        public void PlaceUsefulItems()
        {
            List<ItemType> itemList = new List<ItemType>(UsefulItems);
            ShuffleList(itemList);
            foreach(ItemType item in itemList)
            {
                PlaceUsefuItemInChest(item);
            }
        }

        public void PlaceRupeeItems()
        {
            int randomChestNumber = 0;
            int randomChestId = 0;
            List<ItemType> itemList = new List<ItemType>(rupeeItems);
            ShuffleList(itemList);
            foreach (ItemType item in itemList)
            {
                random.Next(0, ChestPool.Count - 1);
                randomChestId = ChestPool[randomChestNumber];
                ChestsList[randomChestId].Type = item;
                ChestPool.RemoveAt(randomChestNumber);
            }
        }

        public void PlaceHeartItems()
        {
            int randomChestNumber = 0;
            int randomChestId = 0;
            for (int i=0; i < NB_HEART; i++)
            {
                randomChestNumber = random.Next(0, ChestPool.Count);
                randomChestId = ChestPool[randomChestNumber];
                ChestsList[randomChestId].Type = ItemType.Coeur;
                ChestPool.RemoveAt(randomChestNumber);
            }
        }

        public void PlaceQuarterHeartItems()
        {
            int randomChestNumber = 0;
            int randomChestId = 0;
            for (int i = 0; i < NB_QUARTER_HEART; i++)
            {
                randomChestNumber = random.Next(0, ChestPool.Count - 1);
                randomChestId = ChestPool[randomChestNumber];
                ChestsList[randomChestId].Type = ItemType.QuartCoeur;
                ChestPool.RemoveAt(randomChestNumber);
            }
        }

        public void PlaceOtherItems()
        {
            int randomChestNumber = 0;
            int randomChestId = 0;
            List<ItemType> itemList = new List<ItemType>(otherItems);
            ShuffleList(itemList);
            foreach (ItemType item in itemList)
            {
                randomChestNumber = random.Next(0, ChestPool.Count - 1);
                randomChestId = ChestPool[randomChestNumber];
                ChestsList[randomChestId].Type = item;
                ChestPool.RemoveAt(randomChestNumber);
            }
        }

        public void PlaceUsefuItemInChest(ItemType type)
        {
            int randomChestNumber = 0;
            int randomChestId = 0;
            do
            {
                randomChestNumber = random.Next(0, ChestPool.Count);
                randomChestId = ChestPool[randomChestNumber];
                if (IsChestAvailable(randomChestId, type))
                    break;
            }
            while (ChestPool.Count > 0); 
            ChestsList[randomChestId].Type = type;
            ChestPool.RemoveAt(randomChestNumber);
            //RemoveDepencies(type);
        }

        

        public bool IsChestAvailable(int idChest, ItemType type)
        {
            if (ChestsList[idChest].IsVanilla)
                return false;
            if(ChestsList[idChest].Type != (int)ItemType.None)
                return false;
            if (!CanBeInChest(idChest, type))
                return false;

            return true;
        }

        public bool CanBeInChest(int chestId, ItemType type)
        {
            if (ChestsList[chestId].Dependencies.Count == 0)
                return true;

            foreach (var dependencies in ChestsList[chestId].Dependencies)
            {
                if (dependencies.Count == 0)
                    return true;

                for (int i = 0; i < dependencies.Count; i++)
                {
                    if(dependencies[i] == type)
                    {
                        break;
                    }
                    if(i == dependencies.Count-1)
                        return true;
                }
               
            }
            return false; 
        }
     

        public void CreateList()
        {
            ChestsList = new List<Chest>();
            JsonSerializer serializer = new JsonSerializer();
            
            for (int i = 0; i <= NB_CHEST; i++)
            {
                 ChestsList.Add(new Chest(i));
            }
            using (StreamWriter sw = new StreamWriter("basicChestsList.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, ChestsList);
            }
        }

        public List<Chest>  GetChestWithoutDepencies(List<Chest> ChestsList)
        {
            return ChestsList.Where(x => x.Dependencies.Count == 0).ToList();
        }

        public void RemoveDepencies(ItemType type)
        {
            foreach (var chest in ChestsList)
            {
                ItemType elem = ItemType.None;
                if (chest.Dependencies.Count == 0)
                    continue;
                
                foreach (var depencies in chest.Dependencies)
                {
                    elem = depencies.Find(x => x == type);
                    if (elem != (int)ItemType.None)
                    {
                        depencies.Remove(elem);
                    }
                }

            }
            
        }

        public void PrintChestList()
        {
            int cpt = 0;
            //String [] Itemtypes  = Enum.GetNames(typeof(ItemType));
            string chestString = "";

            SpoilerFile.WriteLine("-----------------------------------------------------------------------------------------");
            foreach (var chest in ChestsList)
            {
                chestString = "Chest " + chest.Id + ": " + chest.ParentLocation + " - " + chest.Location + " -> " + chest.Type;
                Console.WriteLine(chestString);
                SpoilerFile.WriteLine(chestString);
            }
            Console.WriteLine("Total Chest empty:: " + cpt);
            SpoilerFile.Close();
        }

        public void PrintChestPool()
        {
            foreach (int idChest in ChestPool)
            {
                Console.WriteLine("Remaining Chest: " + idChest);
            }
        }

        public bool VerifyChests()
        {
            Console.WriteLine("Start verification...");
            List<ItemType> inventory = new List<ItemType>();
            List<ItemType> temporaryInventory = new List<ItemType>();

            //HACK: Considérer Ganon comme un chest. 
            Chest ganon = new Chest();
            ganon.Dependencies = new List<List<ItemType>>();
            ganon.Dependencies.Add(new List<ItemType>());
            for (int i = 0; i < itemsForGanon.Length; i++)
            {
                ganon.Dependencies[0].Add(itemsForGanon[i]);
            }
            //HACK

            bool canBeatGanon = false;
            int sphere = 0;

            System.Text.StringBuilder walkthrough = new System.Text.StringBuilder();
           
            //Bloquer à 100 itérations pour éviter la boucle infinie.
            while (!canBeatGanon || sphere > 100)
            {
                for (int i = dependenciesList.Count - 1; i >= 0; i--)
                {
                    Chest chest = dependenciesList[i];
                    if (HasAllItemsForChest(chest))
                    {
                        Chest chestContent = ChestsList.Find(x => x.Id == chest.Id);
                        if (chest != null && UsefulItems.Contains(chestContent.Type))
                        {
                            string s = string.Format("Sphere {0}: Location: {3} - {1} Item: {2}", sphere, chest.Location, (ItemType)chestContent.Type, chest.ParentLocation);
                            walkthrough.AppendLine(s);
                            temporaryInventory.Add((ItemType)chestContent.Type);
                        }
                        dependenciesList.RemoveAt(i);
                    }
                }

                if (temporaryInventory.Count == 0)
                {
                    Console.WriteLine("Verification failed!");
                    return false;
                }

                foreach (ItemType item in temporaryInventory)
                   inventory.Add(item);

                temporaryInventory.Clear();
                canBeatGanon = HasAllItemsForChest(ganon);

                sphere++;
            }

            Console.WriteLine("Verification successful!");
            Console.WriteLine(walkthrough.ToString());
          
            SpoilerFile.WriteLine(walkthrough.ToString());

            return true;

            bool HasAllItemsForChest(Chest chest)
            {
                bool hasAllItems = true;
                for (int i = 0; i < chest.Dependencies.Count; i++)
                {
                    //Assumer qu'on a tous les items pour ce set de dépendences.
                    hasAllItems = true;

                    List<ItemType> dependency = chest.Dependencies[i];

                    //Hack pour Gants de force
                    int gantsDependencies = NumberOf(dependency, ItemType.Gant_Force);
                    if (gantsDependencies > 0 && gantsDependencies > NumberOf(inventory, ItemType.Gant_Force))
                    {
                        hasAllItems = false;
                        continue;
                    }

                    //Hack pour Pierres inferno
                    int pierreDependencies = NumberOf(dependency, ItemType.Pierre_inferno);
                    if (pierreDependencies > 0 && pierreDependencies > NumberOf(inventory, ItemType.Pierre_inferno))
                    {
                        hasAllItems = false;
                        continue;
                    }

                    for (int j = 0; j < dependency.Count; j++)
                    {
                        //Si on a pas un item, fail
                        if (!inventory.Contains((ItemType)dependency[j]))
                        {
                            hasAllItems = false;
                            break;
                        }
                    }

                    if (hasAllItems)
                        return true;
                }
                return hasAllItems;
            }
        }

        private static int NumberOf<T>(List<T> list, T item)
        {
            int count = 0;
            foreach (T i in list)
            {
                if (i.Equals(item))
                    count++;
            }

            return count;
        }
    }

}
