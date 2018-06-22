using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is a node used by the Astar algorithm
public class Node
{
    //The nodes grid position
	public Point GridPosition { get; private set; }

    //A reference to the tile that this node belongs to
    public TileScript TileRef { get; private set; }

    public Vector2 WorldPosition { get; set; }

    //A reference to the nodes parent
    public Node Parent { get; private set; }

    public int G { get; set; }

    public int H { get; set; }

    public int F { get; set; }

    public Node(TileScript tileRef)
    {
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
        this.WorldPosition = tileRef.WorldPosition;

    }

    public void CalcValues(Node parent, Node goal, int gCost)
    {
        this.Parent = parent;
        this.G = parent.G + gCost;
        this.H = ((Math.Abs(GridPosition.X - goal.GridPosition.X)) + (Math.Abs(goal.GridPosition.Y - GridPosition.Y))) * 10;
        this.F = G + H;
    }
}
