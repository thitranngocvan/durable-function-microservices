﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var dataFile = Path.Combine(dir, "cars.json");
            var dataString = File.ReadAllTextAsync(dataFile).GetAwaiter().GetResult();
            var results = JsonConvert.DeserializeObject<PriceResultWrapper>(dataString);
            var outPath = Path.Combine(dir, "cars2.json");
            File.WriteAllText(outPath,JsonConvert.SerializeObject(results));
            Console.ReadLine();
        }
    }
    public class PriceResultWrapper
    {
        public List<PriceResult> PriceResults { get; set; }
    }
    public class PriceResult
    {
        public int SupplierId { get; set; }
        public decimal LocalPrice { get; set; }
        public string CarName { get; set; }
        public string CarImage { get; set; }
        public string CarClassId { get; set; }
        public string Doors { get; set; }
        public string Seats { get; set; }

    }
}
