using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day9
    {
        private const char Up = 'U';
        private const char Down = 'D';
        private const char Left = 'L';
        private const char Right = 'R';

        internal class Coordinate{
            public int X { get; set; }
            public int Y { get; set; }

            public Coordinate(int X, int Y){
                this.X = X;
                this.Y = Y;
            }

            public void MoveUp() {
                this.Y += 1;
            }

            public void MoveDown() {
                this.Y -= 1;
            }

            public void MoveLeft() {
                this.X -= 1;
            }

            public void MoveRight() {
                this.X += 1;
            }
        }

        public int Solve9a () {
            var tailCoordinates = CalculatePositionsOfTail();
            return tailCoordinates.Distinct().Count();
        }

        private List<Coordinate> CalculatePositionsOfTail(int numberOfSegments = 0){
            var lines = File.ReadAllLines("./Day9a.txt");
            var instructions = ExtractInstructions(lines);
            var segmentPosition = new Coordinate[numberOfSegments + 2];
            segmentPosition[0] = new Coordinate(0, 0);
            for (int i = 0; i <= numberOfSegments; i++)
            {
                segmentPosition[i+1] = new Coordinate(0, 0);
            }
            var placesTailHasBeen = new List<Coordinate>() {new Coordinate(0, 0)};
            for (int i = 0; i < instructions.Count(); i++)
            {
                for (int j = 0; j < instructions[i].Value; j++)
                {
                    var direction = instructions[i].Key;
                    switch (direction){
                        case Up:
                            segmentPosition[0].MoveUp();
                            MoveSegments(segmentPosition);
                        break;
                        case Down:
                            segmentPosition[0].MoveDown();
                            MoveSegments(segmentPosition);
                        break;
                        case Left:
                            segmentPosition[0].MoveLeft();
                            MoveSegments(segmentPosition);
                            
                        break;
                        case Right:
                            segmentPosition[0].MoveRight();
                            MoveSegments(segmentPosition);
                        break;
                    }

                    var exists = placesTailHasBeen.Where(c => c.X == segmentPosition.Last().X && c.Y == segmentPosition.Last().Y).Any();
                    if (!exists){
                        placesTailHasBeen.Add(new Coordinate(segmentPosition.Last().X, segmentPosition.Last().Y));
                    }
                }
            }

            return placesTailHasBeen;
        }

        private void MoveSegments(Coordinate[] segmentPosition){
            for (int k = 1; k < segmentPosition.Length; k++)
            {
                if (IsTouching(segmentPosition[k-1], segmentPosition[k])) break;
                if (segmentPosition[k-1].Y > segmentPosition[k].Y){
                    segmentPosition[k].MoveUp();
                } 
                else if (segmentPosition[k-1].Y < segmentPosition[k].Y){
                    segmentPosition[k].MoveDown();
                }
                if (segmentPosition[k-1].X > segmentPosition[k].X){
                    segmentPosition[k].MoveRight();
                } 
                else if (segmentPosition[k-1].X < segmentPosition[k].X){
                    segmentPosition[k].MoveLeft();
                }
            }
        }

        private bool IsTouching(Coordinate Head, Coordinate Tail){
            if (Head.X == Tail.X || Head.X == (Tail.X + 1) || Head.X == (Tail.X -1)){
                if (Head.Y == Tail.Y || Head.Y == (Tail.Y + 1) || Head.Y == (Tail.Y -1)){
                    return true;
                }
            }
            return false;
        }

        private List<KeyValuePair<char, int>> ExtractInstructions(string[] lines){
            var instructions = new List<KeyValuePair<char, int>>();
            for (int i = 0; i < lines.Count(); i++){
                var nextInstruction = lines[i].Split(' ');
                int.TryParse(nextInstruction[1], out var numberOfSteps);
                instructions.Add(new KeyValuePair<char, int>(char.Parse(nextInstruction[0]), numberOfSteps));
            }
            return instructions;
        }

        public int Solve9b () {
            var tailCoordinates = CalculatePositionsOfTail(8);
            return tailCoordinates.Count();
        }
    }
}