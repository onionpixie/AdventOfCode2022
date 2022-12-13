using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day12
    {
        public int Solve12a () {
            var map = File.ReadAllLines("./Day12a.txt").ToList();
            var start = new Node();
            start.Y = map.FindIndex(x => x.Contains("S"));
            start.X = map[start.Y].IndexOf("S");
            start.Height = 'a';
            Console.WriteLine($"{start.X}, {start.Y}, {start.Height}");
            var finish = new Node();
            finish.Y = map.FindIndex(x => x.Contains("E"));
            finish.X = map[finish.Y].IndexOf("E");
            finish.Height = 'z';
            Console.WriteLine($"{finish.X}, {finish.Y}, {finish.Height}");

            start.SetDistance(finish.X, finish.Y);

            var steps = FindShortestPath(start, finish, map);
            return steps.Value;
        }

        public int Solve12b () {
            var map = File.ReadAllLines("./Day12a.txt").ToList();
            var start = new Node();
            start.Y = map.FindIndex(x => x.Contains("S"));
            start.X = map[start.Y].IndexOf("S");
            start.Height = 'a';
            Console.WriteLine($"{start.X}, {start.Y}, {start.Height}");
            var finish = new Node();
            finish.Y = map.FindIndex(x => x.Contains("E"));
            finish.X = map[finish.Y].IndexOf("E");
            finish.Height = 'z';
            Console.WriteLine($"{finish.X}, {finish.Y}, {finish.Height}");

            start.SetDistance(finish.X, finish.Y);
            var shortestPath = FindShortestPath(start, finish, map);

            // find each a,
            // calculate path,
            // store shortest

            for (int i = 0; i < map.Count(); i++)
            {
                var chars = map[i].ToCharArray();
                for (int j = 0; j < chars.Count(); j++)
                {
                    if (chars[j] == 'a'){
                        var potentialStart = new Node();
                        potentialStart.Y = i;
                        potentialStart.X = j;
                        potentialStart.Height = 'a';
                        var path = FindShortestPath(potentialStart, finish, map);
                        if (path != null && path < shortestPath){
                            shortestPath = path;
                        }
                    }
                }

            }

            return shortestPath.Value;
        }

        private int? FindShortestPath(Node Start, Node End, List<string> Map){
            var start = Start;
            var finish = End;
            var map = Map;

            var activeNodes = new List<Node>();
            activeNodes.Add(start);
            var visitedNodes = new List<Node>();

            while (activeNodes.Any()){
                var checkedNode = activeNodes.OrderBy(x => x.CostDistance).First();
                if(checkedNode.X == finish.X && checkedNode.Y == finish.Y)
                {
                    //We found the destination and we can be sure (Because the the OrderBy above)
                    //That it's the most low cost option. 
                    var node = checkedNode;
                    Console.WriteLine("Retracing steps backwards...");
                    var count = 0;
                    while(true)
                    {
                        //Console.WriteLine($"{node.X} : {node.Y}");
                        count ++;
                        if(map[node.Y][node.X] == ' ')
                        {
                            var newMapRow = map[node.Y].ToCharArray();
                            newMapRow[node.X] = '*';
                            map[node.Y] = new string(newMapRow);
                        }
                        node = node.Parent;
                        if(node == null)
                        {
                            //Console.WriteLine("Map looks like :");
                            //map.ForEach(x => Console.WriteLine(x));
                            Console.WriteLine($"Step taken {count-1}");
                            //Console.WriteLine("Done!");
                            return count-1;
                        }
                    }
                }
                visitedNodes.Add(checkedNode);
		        activeNodes.Remove(checkedNode);

                var walkableNodes = GetWalkableNodes(map, checkedNode, finish);
                //Console.WriteLine($"Walkable Nodes = {walkableNodes.Count()}");

                foreach(var walkableNode in walkableNodes)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedNodes.Any(x => x.X == walkableNode.X && x.Y == walkableNode.Y)){
                        //Console.WriteLine($"Already visited {walkableNode.X}, {walkableNode.Y}");
                        continue;
                    }

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    if(activeNodes.Any(x => x.X == walkableNode.X && x.Y == walkableNode.Y))
                    {
                        //Console.WriteLine($"Already active! {walkableNode.X}, {walkableNode.Y}");
                         var existingNode = activeNodes.First(x => x.X == walkableNode.X && x.Y == walkableNode.Y);
                         if(existingNode.CostDistance > checkedNode.CostDistance)
                         {
                            //Console.WriteLine($"Its better! {walkableNode.X}, {walkableNode.Y}");
                             activeNodes.Remove(existingNode);
                             activeNodes.Add(existingNode);
                         }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        //Console.WriteLine($"New node! {walkableNode.X}, {walkableNode.Y}");
                        activeNodes.Add(walkableNode);
                    }
                }
            }
            Console.WriteLine("No path found!");
            return null;
        }

        private List<Node> GetWalkableNodes(List<string> map, Node currentNode, Node targetNode){
            var possibleNodes = new List<Node>(){
		            new Node { X = currentNode.X, Y = currentNode.Y - 1, Parent = currentNode, Cost=currentNode.Cost+1 },
		            new Node { X = currentNode.X, Y = currentNode.Y + 1, Parent = currentNode, Cost=currentNode.Cost+1 },
		            new Node { X = currentNode.X - 1, Y = currentNode.Y, Parent = currentNode, Cost=currentNode.Cost+1 },
		            new Node { X = currentNode.X + 1, Y = currentNode.Y, Parent = currentNode, Cost=currentNode.Cost+1 },
            }.ToList();

           
            possibleNodes.ForEach(node => node.SetDistance(targetNode.X, targetNode.Y));

            var maxX = map.First().Length - 1;
            var maxY = map.Count - 1;
            var inBoundNodes = possibleNodes
            .Where(node => node.X >= 0 && node.X <= maxX)
            .Where(node => node.Y >= 0 && node.Y <= maxY)
            .ToList();
            try{
                foreach (var item in inBoundNodes)
                {
                    var heightOfNode =  map[item.Y].ToCharArray()[item.X];
                if (heightOfNode == 'S'){
                    heightOfNode = 'a';
                }
                if (heightOfNode == 'E'){
                    heightOfNode = 'z';
                }
                    item.Height = heightOfNode;
                
                }
                //inBoundNodes.ForEach(node => node.Height = map[node.Y].ToCharArray()[node.X]);
            }
            catch{
                throw new Exception("eep");
            }
            
            var toReturn = inBoundNodes
            .Where(node => node.Height <= currentNode.Height + 1  )
            .ToList();

            //Console.WriteLine(toReturn.Count());
            return toReturn;

        }

        internal class Node{
            public int X { get; set; }
            public int Y { get; set; }
            public int Height { get; set; }
            public Node Parent { get; set; }
            public int Cost { get; set; }
            public int Distance { get; set; }
            public int CostDistance => Cost + Distance;

            public void SetDistance(int targetX, int targetY)
            {
                this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
            }
        }
    }
}