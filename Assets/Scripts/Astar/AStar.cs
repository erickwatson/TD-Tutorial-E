using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static Stack<Node> GetPath(Point start, Point goal)
    {
        if (nodes == null)
        {
            CreateNodes();
        }
        
        //Creates an open list to be used with the A* Algorithm
        HashSet<Node> openList = new HashSet<Node>();

        //Creates an closed list to be used with the A* Algorithm
        HashSet<Node> closedList = new HashSet<Node>();

        Stack<Node> finalPath = new Stack<Node>();

        //Finds the start node and creates a reference to it called current node
        Node currentNode = nodes[start];

        //1. Adds the start node to the OpenList
        openList.Add(currentNode);

        while (openList.Count > 0) //10.
        {

            //2. Runs through all the neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
                    //Debug.Log(neighbourPos.X + " " +neighbourPos.Y);

                    if (LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].WalkAble && neighbourPos != currentNode.GridPosition)
                    {

                        //Sets the initial value of 'g' to 0
                        int gCost = 0;

                        //[14][10][14]
                        //[10][ST][10]
                        //[14][10][14]
                        
                        if (Math.Abs(x - y) == 1) //Check if we need to score 10 to a tile (X and Y horizontal tiles)
                        {
                            gCost = 10;
                        }
                        else //Scores 14 if tile is diagonal (Four diagonal tiles)
                        {
                            if (!ConnectedDiagonally(currentNode, nodes[neighbourPos]))
                            {
                                continue;
                            }
                            gCost = 14;
                        }


                        //3. Adds the neighbour to the open list
                        Node neighbour = nodes[neighbourPos];

                        if (openList.Contains(neighbour))
                        {
                            if (currentNode.G + gCost < neighbour.G)//9.4
                            {
                                neighbour.CalcValues(currentNode, nodes[goal], gCost);
                            }
                        }

                        else if (!closedList.Contains(neighbour)) //9.1.
                        {
                            openList.Add(neighbour); //9.2
                            neighbour.CalcValues(currentNode, nodes[goal], gCost); //9.3
                        }


                        

                    }



                }

            }

            //5. & 8. Moves the current node from the open list to the closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0)//7.
            {
                //Sorts the List by F value, and selects the first on the list
                currentNode = openList.OrderBy(n => n.F).First();
            }

            if (currentNode == nodes[goal])
            {
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                break;
            }

        }

        return finalPath;

        //DEBUGGING ONLY, REMOVE LATER
        //GameObject.Find("AStarDebug").GetComponent<AStarDebug>().DebugPath(openList, closedList, finalPath);



    }

    private static bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Point direction = neighbor.GridPosition - currentNode.GridPosition;

        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y + direction.Y);

        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

        if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].WalkAble)
        {
            return false;
        }
        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].WalkAble)
        {
            return false;
        }

        return true;
    }
}
