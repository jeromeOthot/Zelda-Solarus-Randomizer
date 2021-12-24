using System;
using System.Collections.Generic;
using System.IO;

namespace ZeldaSolarusRandomizer
{
    public class FileManager
    {
        public const int NB_BYTES_FOR_NEXT_CHEST = 14;
        public byte[] _fileBytes;

        public int StartChestIndex { get; set; }
        public int StartSeedIndex => StartChestIndex + (166 * NB_BYTES_FOR_NEXT_CHEST);
        public int StartOptionIndex => StartSeedIndex + (6 * NB_BYTES_FOR_NEXT_CHEST);

        public void ReadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                _fileBytes = File.ReadAllBytes(fileName);
            }
            else
            {
                Console.WriteLine("File not exist !! ");
            }
        }

        public void BackupFileRPG_RT()
        {
            string source = @"..\..\..\..\jeu zelda_mystery_of_solarus_randomizer\RPG_RT.ldb";
            string dest = @"..\..\..\..\jeu zelda_mystery_of_solarus_randomizer\RPG_RT_Backup.ldb";
            File.Copy(source, dest, true);
        }

        public void GetStartChestIndex()
        {
            bool isFound = false;
            int nbByteBeforeStartIndex = 34;
            byte[] startData = {
                0x44, 0x42, 0x5F, 0x56, 0x41, 0x52, 0x49, 0x41, 0x42, 0x4C, 0x45, 0x53
            };

            for(int i=0; i < _fileBytes.Length; i++ )
            {
                for (int s=0; s < startData.Length; s++)
                {
                    if (_fileBytes[i+s] != startData[s])
                        break;
                    if (s == startData.Length - 1)
                        isFound = true;
                }
                if(isFound == true)
                {
                    StartChestIndex = i + nbByteBeforeStartIndex;
                    break;
                }
            }
        }

        public void SetChestList(List<Chest> ChestsList)
        {
            int index = StartChestIndex;
            foreach (var chest in ChestsList)
            {
                SetChestItemType((byte)chest.Type, index);
                index += NB_BYTES_FOR_NEXT_CHEST;
            }
        }

        public void SetChestItemType(byte value, int index)
        {
            if (index > 0 && index < _fileBytes.Length)
            {
                _fileBytes[index] = value;
            }
            else
            {
                Console.WriteLine(string.Format("Index {0} invalide  !!", index));
            }
        }

        public void SetSeedByte(int seed)
        {
            byte hexSeed;
            int value;
            for (int i = 0; i < 6; i++)
            {
                value = seed % 16;
                seed = seed/16;
                hexSeed = (byte)value;
                _fileBytes[StartSeedIndex + 14 *i] = hexSeed;
            }
        }

        public void SetOptionByte(int option)
        {
            byte hexOption;
            int value;
            for (int i = 0; i < 2; i++)
            {
                value = option % 16;
                option = option / 16;
                hexOption = (byte)value;
                _fileBytes[StartOptionIndex + 14 * i] = hexOption;
            }
        }

        public void  WriteOutputGame(string outputFileName)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFileName, FileMode.Create)))
            {
                foreach(byte data in _fileBytes)
                {
                    writer.Write(data);
                }

            }
        }

        /*
        public void WriteSpoiler(string outputFileName)
        {
            string spoilerFile = "";
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFileName, FileMode.Create)))
            {
                foreach (byte data in _fileBytes)
                {
                    writer.Write(data);
                }

            }
        }*/
    }
}
