using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day11b
    {
        internal class Monkey{
            public int Id {get; set;} // Monkey Number

            public List<ItemWorry> ItemWorryLevel { get; set; } //e.g. 79. 98

            public Operation Operator {get; set;}

            public int? OperatorValue {get; set;}

            public Operation Test {get; set;} = Operation.Divide;

            public int TestValue {get; set;}

            public int TestPostive {get; set;} // Monkey to throw to is test is true

            public int TestNegative {get; set;} // Monkey to throw to is test is false

            public int InspectionCount {get; set;}

            public int SquaredCount{get; set;}

            public Monkey(int monkeyId){
                this.Id = monkeyId;
                this.ItemWorryLevel = new List<ItemWorry>();
            }

            public bool Inspect(){
                switch (Operator){
                    case Operation.Addition:
                        var item = ItemWorryLevel.First();
                        item.Add((int)(OperatorValue ?? item.Value));
                    break;
                    case Operation.Mulitply:
                        if (OperatorValue == null){
                            ItemWorryLevel.First().Double();
                        }
                        else{
                            ItemWorryLevel.First().Multiply((int)OperatorValue);
                        }
                    break;
                    default:
                        throw new NotImplementedException();
                }

                InspectionCount++;
                return true;
            }

            public void Bored(){
                ItemWorryLevel[0].Value = ItemWorryLevel.First().Value / 3;
            }

             public int ThrowItem(out ItemWorry itemWorryLevel){
                var monkeyToThrowTo = TestResult();
                itemWorryLevel = ItemWorryLevel.First();
                ItemWorryLevel = ItemWorryLevel.TakeLast(ItemWorryLevel.Count() - 1).ToList();
                return monkeyToThrowTo ? TestPostive : TestNegative;
            }

            public bool TestResult(){
                var itemToTest = ItemWorryLevel.First();
                return itemToTest.Divisors[(int)TestValue] == 0;
            }

            public void RecieveItem(ItemWorry itemWorryLevel){
                ItemWorryLevel.Add(itemWorryLevel);
            }

            public enum Operation{
                Mulitply,
                Addition,
                Divide,
            }

            public class ItemWorry{
                public int Value { get; set; }
                public Dictionary<int, int> Divisors { get; set; }

                public ItemWorry(int value){
                    this.Value = value;
                    this.Divisors = new Dictionary<int, int>();
                }

                public void Add(int addValue){
                    foreach (var item in Divisors)
                    {
                        Divisors[item.Key] = Divisors[item.Key] + addValue;
                    }
                    UpdateDivisors();
                }

                public void Double(){
                    foreach (var item in Divisors)
                    {
                        Divisors[item.Key] = Divisors[item.Key] * Divisors[item.Key];
                    }
                    UpdateDivisors();
                }

                public void Multiply(int multiple){
                    foreach (var item in Divisors)
                    {
                        Divisors[item.Key] = Divisors[item.Key] * multiple;
                    }
                    UpdateDivisors();
                }

                private void UpdateDivisors(){
                    foreach (var item in Divisors)
                    {
                        Divisors[item.Key] = Divisors[item.Key] % item.Key;
                    }
                }

                internal void SetDivisors(int[] divisors)
                {
                    foreach (var item in divisors){
                        Divisors.Add(item, Value % item);
                    }
                }
            }
        }

        public long Solve11b () {
            var monkeys = ProcessInputIntoMonkeys();
            Console.WriteLine(monkeys.Count());
            for (int k = 0; k < 10000; k++)
            {
                for (int i = 0; i < monkeys.Count(); i++)
                {
                    var currentMonkey = monkeys.Single(m => m.Id == i);
                    while(currentMonkey.ItemWorryLevel.Any())
                    {
                        currentMonkey.Inspect();
                        var monkeyToCatch = currentMonkey.ThrowItem(out var itemWorryLevel);
                        if (monkeys.SingleOrDefault(m => m.Id == monkeyToCatch) == null){
                            Console.WriteLine(monkeyToCatch);
                        }
                        monkeys.Single(m => m.Id == monkeyToCatch).RecieveItem(itemWorryLevel);
                    }
                }   

                if ((k+1) % 1000 == 0 || k+1==20 || k+1==1){
                Console.WriteLine($"Round {k + 1}");
                Console.WriteLine($"Monkey 0: {monkeys.Single(m => m.Id == 0).InspectionCount}");
                Console.WriteLine($"Monkey 1: {monkeys.Single(m => m.Id == 1).InspectionCount}");
                Console.WriteLine($"Monkey 2: {monkeys.Single(m => m.Id == 2).InspectionCount}");
                Console.WriteLine($"Monkey 3: {monkeys.Single(m => m.Id == 3).InspectionCount}");
                } 
            }
            var mostBusyMonkeys = monkeys.OrderByDescending(m => m.InspectionCount).Take(2);
            long answer = 1;
            foreach (var monkey in mostBusyMonkeys)
            {
                Console.WriteLine(monkey.InspectionCount);
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
                            currentMonkey.ItemWorryLevel.Add(new Monkey.ItemWorry(startingItem));
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
                             if (int.TryParse(operatorValue, out var value)){
                                currentMonkey.OperatorValue = value;
                             }
                             else throw new Exception($"what is this value {operatorValue}");
                             break;
                    }
                }

                if (lines[i].Contains("Test: ")){
                    if (int.TryParse(lines[i].Replace("  Test: divisible by ", ""), out var testValue)){
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

            var divisors = monkeyList.Select(m => m.TestValue).ToArray();
            monkeyList.ForEach(m => m.ItemWorryLevel.ForEach(i => i.SetDivisors(divisors)));

            return monkeyList;
        }
    }
}