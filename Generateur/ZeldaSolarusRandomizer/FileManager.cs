using System;
using System.Collections.Generic;
using System.IO;

namespace ZeldaSolarusRandomizer
{
    public class FileManager
    {
        public const int COFFRE_1 = 137002; 
        public const int NB_BYTES_FOR_NEXT_CHEST = 14; 
        public byte[] _fileBytes;

        public void ReadFile(string fileName)
        {
            if (File.Exists(fileName))
            {

                _fileBytes = File.ReadAllBytes(fileName);
                /*
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    while(reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        _fileBytes.in = reader.();
                    }
                }*/

                string data = Utils.ToHex(_fileBytes);
               // Console.WriteLine(data);
                /*
                foreach(byte fileByte in _fileBytes)
                {
                    
                    Console.WriteLine(fileByte);
                }*/
            }
            else
            {
                Console.WriteLine("File not exist !! ");
            }
        }

        public void SetChestList(List<Chest> ChestsList)
        {
            int index = COFFRE_1;
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
                //Console.WriteLine("byte avant: " +_fileBytes[index]);
                _fileBytes[index] = value;
                //Console.WriteLine("byte apres: " + _fileBytes[index]);
            }
            else
            {
                Console.WriteLine(string.Format("Index {0} invalide  !!", index));
            }
        }

        public void  Write(string outputFileName)
        {
            //string sData = Utils.ToHex(_fileBytes);
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
