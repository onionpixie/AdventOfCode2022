using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day7
    {
        public int Solve7a () {
            Dictionary<string, int> directorySizes = new Dictionary<string, int>();
            var lines = File.ReadAllLines("./Day7a.txt");
            string dirPattern = @"dir ";
            var dirRegex = new Regex(dirPattern);
            string goToRoot = @"\$ cd /";
            var cdRootRegex = new Regex(goToRoot);
            string changeDirectoryPattern = @"\$ cd ";
            var cdRegex = new Regex(changeDirectoryPattern);
            string backDirectoryPattern = "$ cd ..";
            string listFiles = @"$ ls";
            var listFilesRegex = new Regex(listFiles);
            var currentDirectoryTree = new Stack<string>();
            var directorues = new List<string>();
            foreach (var line in lines)
            {
                if (dirRegex.Matches(line).Any()){
                    var directory = line.Split(' ')[1];
                    if (!directorues.Any(c => c == directory)){
                        directorues.Add(directory);
                    }
                }

                if (listFilesRegex.Matches(line).Any()){
                    Console.WriteLine($"listFilesRegex match {line}");
                    continue;
                }

                if(cdRootRegex.Matches(line).Any()) {
                    currentDirectoryTree.Clear();
                    currentDirectoryTree.Push("/");
                    Console.WriteLine($"cdRootRegex match {line}");
                    continue;
                }

                if (string.Equals(line, backDirectoryPattern)) {
                    Console.WriteLine($"cdBackRegex match {line}");
                    if(currentDirectoryTree.TryPeek(out var _)){
                        currentDirectoryTree.Pop();
                    }
                    continue;
                }

                if (cdRegex.Matches(line).Any()){
                    Console.WriteLine($"cdRegex match {line}");
                    Console.WriteLine(line.Split(' ')[2]);
                    currentDirectoryTree.Push(line.Split(' ')[2]);
                    continue;
                }

                Console.WriteLine($"else {line}");
                if (int.TryParse(line.Split(' ')[0], out var value)){
                    Console.WriteLine($"value {value} and tree count {currentDirectoryTree.Count()}");

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
            foreach (var item in directorySizes.Where(c => c.Value <= 100000))
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            }

            Console.WriteLine(directorySizes.Where(c => c.Value <= 100000).Count());
            Console.WriteLine($"Total directories {directorues.Count()}, totla dictionary entries = {directorySizes.Count()}");

            var smallEnoughFolders = directorySizes.Where(c => c.Value <= 100000).Sum(c => c.Value);
            return smallEnoughFolders;
        }

        public int Solve6b () {
            var lines = File.ReadAllLines("./Day6a.txt");
            return 0;
        }
    }
}