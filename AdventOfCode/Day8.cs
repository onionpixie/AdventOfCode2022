using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day78
    {
        private const string dirPattern = @"^dir ";
        private const string changeDirectoryPattern = @"\$ cd ";
        private const string backDirectoryPattern = "$ cd ..";
        private const string listFiles = "$ ls";

        public int Solve8a () {
            var directorySizes = CalculateVisibleTrees();
            return 0;
        }

        private int CalculateVisibleTrees(){
            var lines = File.ReadAllLines("./Day8a.txt");
            var totalRows = lines.Count();
            var totalColumns = lines.First().ToCharArray().Count();
            var totalTrees = totalRows * totalColumns;

            var gridOfTrees = new int[totalRows, totalColumns];
            for (int i = 0; i < lines.Count(); i++)
            {
                var treesInRow = lines[i].ToCharArray();
                foreach(var tree in treesInRow){
                    int.TryParse(tree.ToString(), out var treeHeight);

                }
            }

            return 0;
        }

        public int Solve7b () {
            
            return 0;
        }
    }
}