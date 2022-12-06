using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day6
    {
        public int Solve6a () {
            var lines = File.ReadAllLines("./Day6a.txt");
            return CheckNextPartOfString(lines.Single().ToCharArray(), 0, 4);
        }

        private int CheckNextPartOfString(IEnumerable<char> testString, int count, int numToCheckForDistinct){
            var next4 = testString.Take(numToCheckForDistinct);
            if(next4.Distinct().Count() != numToCheckForDistinct){
                return CheckNextPartOfString(testString.TakeLast(testString.Count()-1), count+1, numToCheckForDistinct);
            }

            return (count + numToCheckForDistinct);
        }

        public int Solve6b () {
            var lines = File.ReadAllLines("./Day6a.txt");
            return CheckNextPartOfString(lines.Single().ToCharArray(), 0, 14);
        }
    }
}