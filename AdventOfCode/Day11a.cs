using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day11a
    {
        internal class Monkey{
            public int Id {get; set;} // Monkey Number

            public List<long> ItemWorryLevel { get; set; } //e.g. 79. 98

            public Operation Operator {get; set;}

            public long? OperatorValue {get; set;}

            public Operation Test {get; set;} = Operation.Divide;

            public long TestValue {get; set;}

            public int TestPostive {get; set;} // Monkey to throw to is test is true

            public int TestNegative {get; set;} // Monkey to throw to is test is false

            public int InspectionCount {get; set;}

            public int SquaredCount{get; set;}

            public Monkey(int monkeyId){
                this.Id = monkeyId;
                this.ItemWorryLevel = new List<long>();
            }

            public bool Inspect(){
                switch (Operator){
                    case Operation.Addition:
                        var newValue = ItemWorryLevel.First() + (OperatorValue ?? ItemWorryLevel.First());
                        if (double.IsInfinity(newValue)){
                            return false;
                        }
                        ItemWorryLevel[0] = newValue;
                    break;
                    case Operation.Mulitply:
                        if (OperatorValue == null){
                            ItemWorryLevel[0] = ItemWorryLevel[0] * ItemWorryLevel[0];
                        }
                        else{
                            ItemWorryLevel[0] = ItemWorryLevel[0] * OperatorValue.Value;
                        }
                    break;
                    default:
                        throw new NotImplementedException();
                }

                InspectionCount++;
                return true;
            }

            public void Bored(){
                ItemWorryLevel[0] = ItemWorryLevel.First() / 3;
            }

             public int ThrowItem(out long itemWorryLevel){
                var monkeyToThrowTo = TestResult();
                itemWorryLevel = ItemWorryLevel.First();
                ItemWorryLevel = ItemWorryLevel.TakeLast(ItemWorryLevel.Count() - 1).ToList();
                return monkeyToThrowTo ? TestPostive : TestNegative;
            }

            public bool TestResult(){
                var itemToTest = ItemWorryLevel.First();
                return itemToTest % TestValue == 0;
            }

            public void RecieveItem(long itemWorryLevel){
                ItemWorryLevel.Add(itemWorryLevel);
            }

            public enum Operation{
                Mulitply,
                Addition,
                Divide,
                Subtract
            }
        }

        public int Solve11a () {
            var monkeys = ProcessInputIntoMonkeys();
            for (int k = 0; k < 20; k++)
            {
                for (int i = 0; i < monkeys.Count(); i++)
                {
                    var currentMonkey = monkeys.Single(m => m.Id == i);
                    while(currentMonkey.ItemWorryLevel.Any())
                    {
                        currentMonkey.Inspect();
                        currentMonkey.Bored();
                        var monkeyToCatch = currentMonkey.ThrowItem(out var itemWorryLevel);
                        monkeys.Single(m => m.Id == monkeyToCatch).RecieveItem(itemWorryLevel);
                    }
                }   
            }
            Console.WriteLine($"Monkey 0: {monkeys.Single(m => m.Id == 0).InspectionCount}");
            Console.WriteLine($"Monkey 1: {monkeys.Single(m => m.Id == 1).InspectionCount}");
            Console.WriteLine($"Monkey 2: {monkeys.Single(m => m.Id == 2).InspectionCount}");
            Console.WriteLine($"Monkey 3: {monkeys.Single(m => m.Id == 3).InspectionCount}");
            var mostBusyMonkeys = monkeys.OrderByDescending(m => m.InspectionCount).Take(2);
            var answer = 1;
            foreach (var monkey in mostBusyMonkeys)
            {
                Console.WriteLine($"{monkey.Id}: {monkey.InspectionCount}");
                answer = answer * monkey.InspectionCount;
            }
            return answer;
        }

        private const string MonkeyPattern = "^Monkey ";
        
        private const string StartingItemsPattern = "  Starting items: ";
        
        private const string OperationPattern = "  Operation: ";

        private List<Monkey> ProcessInputIntoMonkeys(){
            var lines = File.ReadAllLines("./Day11a.txt");
            var monkeyRegex = new Regex(MonkeyPattern);
            Monkey currentMonkey = null;
            var monkeyList = new List<Monkey>();
            for (int i = 0; i < lines.Count(); i++)
            {
                if(string.IsNullOrWhiteSpace(lines[i])){
                    monkeyList.Add(currentMonkey);
                    currentMonkey = null;
                    continue;
                }

                if (monkeyRegex.Matches(lines[i]).Any()){
                    int.TryParse(lines[i].Replace("Monkey ", "").Replace(":", ""), out var id);
                    currentMonkey = new Monkey(id);
                    continue;
                }

                if (lines[i].Contains(StartingItemsPattern)){
                    var startingItems = lines[i].Replace(StartingItemsPattern, "").Split(", ");
                    foreach (var item in startingItems){
                        if(int.TryParse(item, out var startingItem)){
                            currentMonkey.ItemWorryLevel.Add(startingItem);
                        }
                         else throw new Exception($"what is this {item}");
                    }
                }

                if (lines[i].Contains(OperationPattern)){
                    var stringParts = lines[i].Replace("  Operation: new = old ", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    switch (stringParts[0]){
                        case "*":
                            currentMonkey.Operator = Monkey.Operation.Mulitply;
                        break;
                        case "+":
                            currentMonkey.Operator = Monkey.Operation.Addition;
                        break;
                        default:
                            Console.WriteLine(stringParts[0]);
                            throw new ArgumentOutOfRangeException();
                    }

                    var operatorValue = stringParts[1];
                    switch (operatorValue){
                        case "old":
                            currentMonkey.OperatorValue = null;
                            break;
                        default:
                             if (long.TryParse(operatorValue, out var value)){
                                currentMonkey.OperatorValue = value;
                             }
                             else throw new Exception($"what is this value {operatorValue}");
                             break;
                    }
                }

                if (lines[i].Contains("Test: ")){
                    if (long.TryParse(lines[i].Replace("  Test: divisible by ", ""), out var testValue)){
                        currentMonkey.TestValue = testValue;
                        i += 1;
                        int.TryParse(lines[i].Replace("    If true: throw to monkey ", ""), out var testPostiveValue);
                        currentMonkey.TestPostive = testPostiveValue;
                        i += 1;
                        int.TryParse(lines[i].Replace("    If false: throw to monkey ", ""), out var testNegativeValue);
                        currentMonkey.TestNegative = testNegativeValue;
                    }
                    else throw new InvalidDataException();
                }
            }

            return monkeyList;
        }
    }
}