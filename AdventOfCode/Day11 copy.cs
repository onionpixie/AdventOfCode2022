using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day11Alt
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
                if (itemToTest.Divisors[(int)TestValue]){
                    Console.Write("");
                }
                return itemToTest.Divisors[(int)TestValue];
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
                public Dictionary<int, bool> Divisors { get; set; }

                public Dictionary<int, int> Additions { get; set; }

                public ItemWorry(int value){
                    this.Value = value;
                    this.Divisors = new Dictionary<int, bool>();
                    this.Additions = new Dictionary<int, int>();
                }

                public void Add(int addValue){
                    foreach (var item in Additions)
                    {
                        Additions[item.Key] = Additions[item.Key] + addValue;
                    }
                    UpdateDivisors();
                }

                public void Double(){
                    foreach (var item in Divisors)
                    {
                        if ((Value + Additions[item.Key]) % item.Key == 0){
                            Divisors[item.Key] = true;
                        }
                        //Additions[item.Key] = Additions[item.Key] * Additions[item.Key];
                    }
                }

                public void Multiply(int multiple){
                    foreach (var item in Divisors)
                    {
                        if (multiple % item.Key == 0){
                            Divisors[item.Key] = true;
                        }
                        //Additions[item.Key] = Additions[item.Key] * multiple;
                    }
                }

                private void UpdateDivisors(){
                    foreach (var item in Divisors)
                    {
                        if (Additions[item.Key] == 0){
                            continue;
                        }
                        else if ((Value + Additions[item.Key]) % item.Key == 0){
                            Divisors[item.Key] = true;
                            Additions[item.Key] = 0;
                        }
                        else{
                            Divisors[item.Key] = false;
                        }
                    }
                }

                internal void SetDivisors(int[] divisors)
                {
                    foreach (var item in divisors){
                        Divisors.Add(item, Value % item == 0);
                        Additions.Add(item, 0);
                    }
                }
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

                Console.WriteLine($"After Round {k+1}");
                 Console.WriteLine($"Monkey 0: {string.Join(", ", monkeys.Single(m => m.Id == 0).ItemWorryLevel.Select(x => x.Value.ToString()).ToArray())}");
                 Console.WriteLine($"Monkey 1: {string.Join(", ", monkeys.Single(m => m.Id == 1).ItemWorryLevel.Select(x => x.Value.ToString()).ToArray())}");
                 Console.WriteLine($"Monkey 2: {string.Join(", ", monkeys.Single(m => m.Id == 2).ItemWorryLevel.Select(x => x.Value.ToString()).ToArray())}");
                 Console.WriteLine($"Monkey 3: {string.Join(", ", monkeys.Single(m => m.Id == 3).ItemWorryLevel.Select(x => x.Value.ToString()).ToArray())}");
               // Console.WriteLine($"Monkey 2: {monkeys.Single(m => m.Id == 2).InspectionCount}");
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

        public int Solve11b () {
            var monkeys = ProcessInputIntoMonkeys();
            for (int k = 0; k < 20; k++)
            {
                for (int i = 0; i < monkeys.Count(); i++)
                {
                    var currentMonkey = monkeys.Single(m => m.Id == i);
                    while(currentMonkey.ItemWorryLevel.Any())
                    {
                        currentMonkey.Inspect();
                        var monkeyToCatch = currentMonkey.ThrowItem(out var itemWorryLevel);
                        monkeys.Single(m => m.Id == monkeyToCatch).RecieveItem(itemWorryLevel);
                        
                        Console.WriteLine($"Monkey {i} throw to Monkey {monkeyToCatch}");
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
            var answer = 1;
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