using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day4
    {
        public int Solve4a () {
            var lines = File.ReadAllLines("./Day4a.txt");
            var numOverlappingPairs = 0;
            foreach(var line in lines){
                var entries = line.Split(',');
                var elf1 = entries[0];
                var elf2 = entries[1];  
                var elf1Sections = elf1.Split('-');
                var elf2Sections = elf2.Split('-');
                int.TryParse(elf1Sections[0], out var elf1Min);
                int.TryParse(elf1Sections[1], out var elf1Max);
                int.TryParse(elf2Sections[0], out var elf2Min);
                int.TryParse(elf2Sections[1], out var elf2Max);

                if (elf1Min <= elf2Min && elf1Max >= elf2Max){
                    numOverlappingPairs ++;
                }
                else if (elf2Min <= elf1Min && elf2Max >= elf1Max){
                    numOverlappingPairs ++;
                }
            }
            return numOverlappingPairs;
        }

        public int Solve4b () {
            var lines = File.ReadAllLines("./Day4a.txt");
            var numOverlappingPairs = 0;
            foreach(var line in lines){
                var entries = line.Split(',');
                var elf1 = entries[0];
                var elf2 = entries[1];  
                var elf1Sections = elf1.Split('-');
                var elf2Sections = elf2.Split('-');
                int.TryParse(elf1Sections[0], out var elf1Min);
                int.TryParse(elf1Sections[1], out var elf1Max);
                int.TryParse(elf2Sections[0], out var elf2Min);
                int.TryParse(elf2Sections[1], out var elf2Max);

                if (elf1Min <= elf2Min && elf1Max >= elf2Min ){
                    numOverlappingPairs ++;
                }
                else if (elf1Min <= elf2Max && elf1Max >= elf2Max ){
                    numOverlappingPairs ++;
                }
                else if (elf2Min <= elf1Min && elf2Max >= elf1Min){
                    numOverlappingPairs ++;
                }
                else if (elf2Min <= elf1Max && elf2Max >= elf1Max){
                    numOverlappingPairs ++;
                }
            }
            return numOverlappingPairs;
        }
    }
}