using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Day1
    {
        public int Solve1a () {
            var lines = File.ReadAllLines("./Day1a.txt");
            var maxCalories = 0;
            var currentCalories = 0;
            foreach(var line in lines){
                if (int.TryParse(line, out var calories)){
                    currentCalories += calories;
                }
                else{
                    if (currentCalories > maxCalories){
                    maxCalories = currentCalories;
                }
                currentCalories = 0;
                }
            }
            return maxCalories;
        }

        public int Solve1b () {
            var lines = File.ReadAllLines("./Day1a.txt");
            var allCalories = new List<int>();
            var currentCalories = 0;
            foreach(var line in lines) {
                if (int.TryParse(line, out var calories)) {
                    currentCalories += calories;
                }
                else {
                    allCalories.Add(currentCalories);
                    currentCalories = 0;
                }
            }

            allCalories.Sort((x,y)=> y.CompareTo(x));  
            return allCalories[0] + allCalories[1] + allCalories[2];
        }
    }
}