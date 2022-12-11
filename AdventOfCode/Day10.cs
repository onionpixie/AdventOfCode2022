using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day10
    {
        public int Solve10a () {
            var cycleSums = CalculateCycleSums();
            return cycleSums;
        }

        private int CalculateCycleSums(){
            var lines = File.ReadAllLines("./Day10a.txt");
            var currentSum = 1;
            var currentCycle = 0;
            var outputSum = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                OutputToDisplay(currentCycle, currentSum);
                currentCycle++;
                outputSum = CalculateOutputSum(currentSum, currentCycle, outputSum);

                var lineParts = lines[i].Split(' ');
                if (lineParts.Count() == 1){ // noop
                    continue;
                }
                
                OutputToDisplay(currentCycle, currentSum);
                currentCycle++;
                outputSum = CalculateOutputSum(currentSum, currentCycle, outputSum);
                int.TryParse(lineParts[1], out var numToAdd);
                currentSum += numToAdd;
            }

            return outputSum;
        }

        private int CalculateOutputSum(int currentSum, int currentCycle, int outputSum){
            if (currentCycle == 20 || currentCycle == 60 || currentCycle == 100 || currentCycle == 140 || currentCycle == 180 || currentCycle == 220){
                    outputSum += (currentSum * currentCycle);
            }

            return outputSum;
        }

        private void OutputToDisplay(int currentCycle, int registerPosition){
            if (currentCycle != 0 && currentCycle % 40 == 0){
                Console.WriteLine("");
            }

            var positionToCheck = currentCycle;
            while (positionToCheck > 40){
                positionToCheck = positionToCheck - 40;
            }
            if (positionToCheck == registerPosition || positionToCheck == registerPosition + 1 || positionToCheck == registerPosition - 1){
                Console.Write("#");
            }
            else{
                Console.Write(".");
            }
        }

        public string Solve10b () {
            var cycleSums = CalculateCycleSums();
            return "";
        }
    }
}