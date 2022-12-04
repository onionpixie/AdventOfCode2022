using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day3
    {
        public int Solve3a () {
            var lines = File.ReadAllLines("./Day3a.txt");
            var score = 0;
            foreach(var line in lines){
                var entries = line.ToCharArray();
                var count = entries.Length;
                var part1 = entries.Take(count/2);
                var part2 = entries.TakeLast(count/2);
                var commonChar = part1.Intersect(part2).First();
                if (commonChar < 91) {// it's uppercase
                    score += ((int)commonChar - 38);
                }
                else{
                    score += ((int)commonChar - 96);
                }
            }
            return score;
        }

        public int Solve3b () {
            var lines = File.ReadAllLines("./Day3a.txt");
            var score = 0;
            for (int i = 0; i < lines.Length; i+=3)
            {
                var elf1Pack = lines[i].ToCharArray();
                var elf2Pack = lines[i+1].ToCharArray();
                var elf3Pack = lines[i+2].ToCharArray();
                var commonChar = elf1Pack.Intersect(elf2Pack.Intersect(elf3Pack)).First();

                if (commonChar < 91) {// it's uppercase
                    score += ((int)commonChar - 38);
                }
                else{
                    score += ((int)commonChar - 96);
                }
            }
            return score;
        }
    }
}