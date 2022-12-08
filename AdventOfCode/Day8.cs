using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day8
    {
        public int Solve8a () {
            return CalculateVisibleTrees();
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
                for (int j = 0; j < treesInRow.Count(); j++)
                {
                    int.TryParse(treesInRow[j].ToString(), out var treeHeight);
                    gridOfTrees[i, j] = treeHeight;
                }
            }

            var isVisibleCount = 0;
            for (int i = 0; i < gridOfTrees.GetLength(0); i++) {
                for (int j = 0; j < gridOfTrees.GetLength(1); j++){
                    var row = i;
                    var column = j;
                    // if we are on an edge, add one and move on
                    if (i == 0 || i == totalRows || j == 0 || j == totalColumns){
                        isVisibleCount += 1;
                        continue;
                    }

                    var treeHeight = gridOfTrees[i, j];
                    // check if visible upwards
                    var hiddenUp = false;
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (CheckHidden(gridOfTrees, k, j, treeHeight)){
                            hiddenUp = true;
                            break;
                        }
                    }

                    if (!hiddenUp){
                        isVisibleCount += 1;
                        continue;
                    }

                    // check if visible downwards
                    var hiddenDown = false;
                    for (int k = (i + 1); k < totalRows; k++)
                    {
                         if (CheckHidden(gridOfTrees, k, j, treeHeight)){
                            hiddenDown = true;
                            break;
                        }
                    }

                    if (!hiddenDown){
                        isVisibleCount += 1;
                        continue;
                    }


                    // check if visible upwards
                    var hiddenLeft = false;
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (CheckHidden(gridOfTrees, i, k, treeHeight)){
                            hiddenLeft = true;
                            break;
                        }
                    }

                    if (!hiddenLeft){
                        isVisibleCount += 1;
                        continue;
                    }

                    // check if visible downwards
                    var hiddenRight = false;
                    for (int k = j + 1; k < totalColumns; k++)
                    {
                         if (CheckHidden(gridOfTrees, i, k, treeHeight)){
                            hiddenRight = true;
                            break;
                        }
                    }

                    if (!hiddenRight){
                        isVisibleCount += 1;
                        continue;
                    }
                }
            }

            return isVisibleCount;
        }

        private bool CheckHidden(int[,] trees, int row, int column, int treeHeight){
            var isHidden = trees[row, column] >= treeHeight;
            return isHidden;
        }

        public int Solve8b () {
            return CalculateBestView();
        }

        private int CalculateBestView(){
            var lines = File.ReadAllLines("./Day8a.txt");
            var totalRows = lines.Count();
            var totalColumns = lines.First().ToCharArray().Count();
            var totalTrees = totalRows * totalColumns;

            var gridOfTrees = new int[totalRows, totalColumns];
            for (int i = 0; i < lines.Count(); i++)
            {
                var treesInRow = lines[i].ToCharArray();
                for (int j = 0; j < treesInRow.Count(); j++)
                {
                    int.TryParse(treesInRow[j].ToString(), out var treeHeight);
                    gridOfTrees[i, j] = treeHeight;
                }
            }

            var bestView = 0;
            
            for (int i = 0; i < gridOfTrees.GetLength(0); i++) {
                for (int j = 0; j < gridOfTrees.GetLength(1); j++){
                    var treesVisible = new List<int>();
                    
                    var row = i;
                    var column = j;
                    // if we are on an edge, our view always sucks!
                    if (i == 0 || i == totalRows-1 || j == 0 || j == totalColumns-1){
                        continue;
                    }

                    var treeHeight = gridOfTrees[i, j];
                    // check if visible upwards
                    var treesVisibleUp = 0;
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (!CheckHidden(gridOfTrees, k, j, treeHeight)){
                            treesVisibleUp++;
                        }
                        else{
                            treesVisibleUp++;
                            break;
                        }
                    }

                    // check if visible downwards
                    var treesVisibleDown = 0;
                    for (int k = (i + 1); k < totalRows; k++)
                    {
                         if (!CheckHidden(gridOfTrees, k, j, treeHeight)){
                            treesVisibleDown++;
                        }
                        else{
                            treesVisibleDown++;
                            break;
                        }
                    }

                    // check if visible upwards
                    var treesVisibleLeft = 0;
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (!CheckHidden(gridOfTrees, i, k, treeHeight)){
                            treesVisibleLeft++;
                        }
                        else{
                            treesVisibleLeft++;
                            break;
                        }
                    }

                    // check if visible downwards
                    var treesVisibleRight = 0;
                    for (int k = j + 1; k < totalColumns; k++)
                    {
                         if (!CheckHidden(gridOfTrees, i, k, treeHeight)){
                            treesVisibleRight++;
                        }
                        else{
                            treesVisibleRight++;
                            break;
                        }
                    }
                    
                    var view = treesVisibleUp * treesVisibleDown * treesVisibleLeft * treesVisibleRight;
                    if (view > bestView){
                        bestView = view;
                    }
                }
            }

            return bestView;
        }
    }
}