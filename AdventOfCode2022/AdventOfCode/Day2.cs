using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Day2
    {
        public int Solve2a () {
            var scoreDictionary = new Dictionary<char, int>(){
                {'A', 1}, {'B', 2}, {'C', 3}, {'X', 1}, {'Y', 2}, {'Z', 3}
            };

            var lines = File.ReadAllLines("./Day2a.txt");
            var score = 0;
            foreach(var line in lines){
                var entries = line.Replace(" ", "").ToCharArray();
                var opponent = entries[0];
                var you = entries[1];
                switch (opponent) {
                    case 'A':
                        if (you == 'Y') {
                            score += 6;
                        }
                        else if (you == 'X'){
                            score += 3;
                        }
                    break;
                    case 'B':
                        if (you == 'Z') {
                            score += 6;
                        }
                        else if (you == 'Y'){
                            score += 3;
                        }
                    break;
                    case 'C':
                        if (you == 'X') {
                            score += 6;
                        }
                        else if (you == 'Z'){
                            score += 3;
                        }
                    break;
                }

                score += scoreDictionary[you];
            }
            return score;
        }

        public int Solve2b () {
            var scoreDictionary = new Dictionary<char, int>(){
                {'A', 1}, {'B', 2}, {'C', 3}
            };

            var howToLose = new Dictionary<char, char>(){
                {'A', 'C'}, {'B', 'A'}, {'C', 'B'}
            };

            var howToWin = new Dictionary<char, char>(){
                {'A', 'B'}, {'B', 'C'}, {'C', 'A'}
            };

            var lines = File.ReadAllLines("./Day2a.txt");
            var score = 0;
            foreach(var line in lines) {
                var entries = line.Replace(" ", "").ToCharArray();
                var opponent = entries[0];
                var you = entries[1];
                switch (you){
                    case 'Y':
                        score += scoreDictionary[opponent];
                        score += 3;
                        break;
                    case 'Z':
                        score += scoreDictionary[howToWin[opponent]];
                        score += 6;
                        break;
                    case 'X':
                        score += scoreDictionary[howToLose[opponent]];
                        break;
                }
            }
            return score;

        }
    }
}