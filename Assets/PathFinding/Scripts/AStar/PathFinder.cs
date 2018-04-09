using System;
using UnityEngine;

namespace PathFinding
{
    public static class PathFinder
    {
        public static BreadCrumb FindPath(Grid world, Point start, Point end)
        {
            var bc = FindPathReversed(world, start, end);
            var temp = new BreadCrumb[256];

            if (bc != null)
            {
                var index = 0;
                while (bc != null)
                {
                    temp[index] = bc;
                    bc = bc.Next;
                    index++;
                }

                index -= 2;

                var current = new BreadCrumb(start);
                var head = current;

                while (index >= 0)
                {
                    current.Next = new BreadCrumb(temp[index].Position);
                    current = current.Next;
                    index--;
                }

                return head;
            }
            else
            {
                return null;
            }
        }

        private static BreadCrumb FindPathReversed(Grid world, Point start, Point end)
        {
            var openList = new MinHeap<BreadCrumb>(256);
            var brWorld = new BreadCrumb[world.Right, world.Top];

            var current = new BreadCrumb(start) { Cost = 0 };

            var finish = new BreadCrumb(end);
            brWorld[current.Position.X, current.Position.Y] = current;
            openList.Add(current);

            while (openList.Count > 0)
            {
                //Find best item and switch it to the 'closedList'
                current = openList.ExtractFirst();
                current.OnClosedList = true;

                //Find neighbours
                for (var i = 0; i < Surrounding.Length; i++)
                {
                    var tmp = new Point(current.Position.X + Surrounding[i].X, current.Position.Y + Surrounding[i].Y);

                    if (!world.ConnectionIsValid(current.Position, tmp))
                        continue;

                    try
                    {
                        //Check if we've already examined a neighbour, if not create a new node for it.
                        BreadCrumb node;
                        if (brWorld[tmp.X, tmp.Y] == null)
                        {
                            node = new BreadCrumb(tmp);
                            brWorld[tmp.X, tmp.Y] = node;
                        }
                        else
                        {
                            node = brWorld[tmp.X, tmp.Y];
                        }

                        //If the node is not on the 'closedList' check it's new score, keep the best
                        if (!node.OnClosedList)
                        {
                            var diff = 0;
                            if (current.Position.X != node.Position.X)
                            {
                                diff += 1;
                            }

                            if (current.Position.Y != node.Position.Y)
                            {
                                diff += 1;
                            }

                            var distance =
                                (int)Mathf.Pow(
                                    Mathf.Max(Mathf.Abs(end.X - node.Position.X), Mathf.Abs(end.Y - node.Position.Y)), 2);
                            var cost = current.Cost + diff + distance;

                            if (cost < node.Cost)
                            {
                                node.Cost = cost;
                                node.Next = current;
                            }

                            //If the node wasn't on the openList yet, add it 
                            if (!node.OnOpenList)
                            {
                                //Check to see if we're done
                                if (node.Equals(finish))
                                {
                                    node.Next = current;
                                    return node;
                                }

                                node.OnOpenList = true;
                                openList.Add(node);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(tmp);
                    }
                }
            }
            return null; //no path found
        }

        //Neighbour options
        //Our diamond pattern offsets top/bottom/left/right by 2 instead of 1
        private static readonly Point[] Surrounding =
        {
            new Point(0, 2), new Point(-2, 0), new Point(2, 0), new Point(0, -2),
            new Point(-1, 1), new Point(-1, -1), new Point(1, 1), new Point(1, -1)
        };
    }
}