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
        public int StartSeedIndex => StartChestIndex + (166 * 14);

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

        public void GetStartChestIndex()
        {
            bool isFound = false;
            int nbByteBeforeStartIndex = 22;
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

        public void  Write(string outputFileName)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFileName, FileMode.Create)))
            {
                foreach(byte data in _fileBytes)
                {
                    writer.Write(data);
                }

            }
        }
    }
}
