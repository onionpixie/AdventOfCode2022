using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day7
    {
        private const string dirPattern = @"^dir ";
        private const string changeDirectoryPattern = @"\$ cd ";
        private const string backDirectoryPattern = "$ cd ..";
        private const string listFiles = "$ ls";

        public int Solve7a () {
            var directorySizes = CalculateDirectorySizes();
            var smallEnoughFolders = directorySizes.Where(c => c.Value <= 100000).Sum(c => c.Value);
            return smallEnoughFolders;
        }

        private Dictionary<string, int> CalculateDirectorySizes(){
            var lines = File.ReadAllLines("./Day7a.txt");
            var dirRegex = new Regex(dirPattern);
            var cdRegex = new Regex(changeDirectoryPattern);
            var currentDirectoryTree = new List<string>();

            Dictionary<string, int> directorySizes = new Dictionary<string, int>();
            for (int i = 0; i < lines.Count(); i++)
            {
                if (dirRegex.Matches(lines[i]).Any() || string.Equals(lines[i], listFiles)){
                    continue;
                }

                if (string.Equals(lines[i], backDirectoryPattern)) {
                    currentDirectoryTree = currentDirectoryTree.SkipLast(1).ToList();
                    continue;
                }

                if (cdRegex.Matches(lines[i]).Any()){
                    currentDirectoryTree.Add(lines[i].Split(' ')[2] + string.Join('/', currentDirectoryTree));
                    continue;
                }
                
                if (int.TryParse(lines[i].Split(' ')[0], out var value)){
                    var currentTree = currentDirectoryTree.ToArray();
                    foreach (var folder in currentTree){
                        if (!directorySizes.ContainsKey(folder)){
                            directorySizes.Add(folder, value);
                        }
                        else{
                            directorySizes[folder] += value;
                        }
                    }
                }
            }

            return directorySizes;
        }

        public int Solve7b () {
            var directorySizes = CalculateDirectorySizes();
            var totalUsedSpace = directorySizes["/"];
            var unusedSpace = 70000000 - totalUsedSpace;
            var spaceNeeded = 30000000 - unusedSpace;
            var smallestDirectory = directorySizes.Where(c => c.Value > spaceNeeded).OrderBy(c => c.Value).First();
            return smallestDirectory.Value;
        }
    }
}