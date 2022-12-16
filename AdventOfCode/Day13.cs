using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day13
    {
        public int SolveA () {
            var signal = File.ReadAllLines("./Day13.txt").ToList();
            
            return 0;
        }

        public int SolveB () {
            var map = File.ReadAllLines("./Day13.txt").ToList();

            return 0;
        }

        private int ProcessSignal(){
            var signal = File.ReadAllLines("./Day13.txt").ToList();
            var correctCount = 0;
            for (int i = 0; i < (signal.Count() + 1)/3; i++)
            {
                var nextLine = i*3;
                var leftValue = signal[nextLine].ToCharArray();
                var rightValue = signal[nextLine+1].ToCharArray();

                //var currentLeftNumbers = new List<int>();
                //var currentRightNumbers = new List<int>();
                for (int j = 0; j < leftValue.Count(); j++)
                {
                    var currentLeftNumber = 0;
                    var currentRightNumber = 0;
                    if (rightValue.Count() -1 == j){
                        break;
                    }

                    if (leftValue[j] == ']'){
                        if (leftValue.Count() - 1 == j){
                            if (rightValue.Count() - 1 >= j){
                                correctCount++;
                            }
                            break;
                        } 
                    }

                    switch(leftValue[j]){
                        case ',':
                        case ']':
                        case '[':
                        break;
                        default:
                            if (leftValue[j-1] != ','){
                                // do nothing we were in a 10 or similar
                            }
                            else if (leftValue[j+1] != ','){
                                currentLeftNumber = int.Parse($"{leftValue[j]} + {leftValue[j+1]}");
                                //currentLeftNumbers.Add(int.Parse($"{leftValue[j]} + {leftValue[j+1]}"));
                            }
                            else{
                                currentLeftNumber = int.Parse($"{leftValue[j]}";
                                //currentLeftNumbers.Add(int.Parse($"{leftValue[j]}"));
                            }
                        break;
                    }
                   
                    switch(rightValue[j]){
                        case ',':
                        case ']':
                        case '[':
                        break;
                        default:
                            if (rightValue[j-1] != ','){
                                // do nothing we were in a 10 or similar
                            }
                            else if (rightValue[j+1] != ','){
                                currentRightNumber = int.Parse($"{rightValue[j]} + {rightValue[j+1]}");
                                //currentRightNumbers.Add(int.Parse($"{rightValue[j]} + {rightValue[j+1]}"));
                            }
                            else{
                                currentRightNumber = int.Parse($"{rightValue[j]}");
                                //currentRightNumbers.Add(int.Parse($"{rightValue[j]}"));
                            }
                        break;

                        if (currentLeftNumber)
                    }
                }
            }
        }
    }
}