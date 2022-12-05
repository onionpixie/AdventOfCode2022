using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day5
    {
        public string Solve5a () {
            var lines = File.ReadAllLines("./Day5a.txt");
            var gap = 0;
            var numberOfColumns = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if(string.IsNullOrWhiteSpace(lines[i])){
                    var numberAsCharacter = lines[i-1].Trim().ToCharArray().Last().ToString();
                    int.TryParse(numberAsCharacter, out numberOfColumns);
                    gap = i;
                    break;
                }
            }
            var stacks = new List<Stack<char>>();
            for (int i = 0; i < numberOfColumns; i++)
            {
                var newStack = new Stack<char>();
                
                for (int j = gap-2; j >= 0; j--)
                {
                    var crates = lines[j].ToCharArray();
                    var charNumberOfNextCreate = (i * 4) + 1;
                    var crate = crates.Count() > charNumberOfNextCreate ? crates[charNumberOfNextCreate] : ' ';
                    if (char.IsWhiteSpace(crate)){
                        break;
                    }
                    newStack.Push(crate);
                }
                stacks.Add(newStack);
            }

            var answer = "";
            foreach (var stack in stacks)
            {
                var stackArray = stack.ToArray();
                answer += stackArray.Last().ToString();
            }
            Console.WriteLine(answer);


            for (int i = gap + 1; i < lines.Count(); i++)
            {
                var instructions = lines[i].Split(" ");
                int.TryParse(instructions[1], out var moveNumber);
                int.TryParse(instructions[3], out var fromColumn);
                int.TryParse(instructions[5], out var toColumn);

                for (int j = 0; j < moveNumber; j++)
                {
                    var movingCrate = stacks[fromColumn - 1].Pop();
                    stacks[toColumn - 1].Push(movingCrate);
                }
            }

            answer = "";
            foreach (var stack in stacks)
            {
                answer += stack.Peek().ToString();
            }
            return answer;
        }

        public string Solve5b () {
            var lines = File.ReadAllLines("./Day5a.txt");
            var gap = 0;
            var numberOfColumns = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if(string.IsNullOrWhiteSpace(lines[i])){
                    var numberAsCharacter = lines[i-1].Trim().ToCharArray().Last().ToString();
                    int.TryParse(numberAsCharacter, out numberOfColumns);
                    gap = i;
                    break;
                }
            }
            var stacks = new List<Stack<char>>();
            for (int i = 0; i < numberOfColumns; i++)
            {
                var newStack = new Stack<char>();
                
                for (int j = gap-2; j >= 0; j--)
                {
                    var crates = lines[j].ToCharArray();
                    var charNumberOfNextCreate = (i * 4) + 1;
                    var crate = crates.Count() > charNumberOfNextCreate ? crates[charNumberOfNextCreate] : ' ';
                    if (char.IsWhiteSpace(crate)){
                        break;
                    }
                    newStack.Push(crate);
                }
                stacks.Add(newStack);
            }

            var answer = "";
            foreach (var stack in stacks)
            {
                var stackArray = stack.ToArray();
                answer += stackArray.Last().ToString();
            }
            Console.WriteLine(answer);


            for (int i = gap + 1; i < lines.Count(); i++)
            {
                var instructions = lines[i].Split(" ");
                int.TryParse(instructions[1], out var moveNumber);
                int.TryParse(instructions[3], out var fromColumn);
                int.TryParse(instructions[5], out var toColumn);

                var tempStack = new Stack<char>();
                for (int j = 0; j < moveNumber; j++)
                {
                    tempStack.Push(stacks[fromColumn - 1].Pop());
                    
                }
                for (int j = 0; j < moveNumber; j++)
                {
                    stacks[toColumn - 1].Push(tempStack.Pop());
                }
            }

            answer = "";
            foreach (var stack in stacks)
            {
                answer += stack.Peek().ToString();
            }
            return answer;
        }
    }
}