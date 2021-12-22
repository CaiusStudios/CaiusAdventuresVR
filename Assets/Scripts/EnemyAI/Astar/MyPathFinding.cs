using System.Collections.Generic;
using UnityEngine;

// source: https://unitycodemonkey.com/

public class MyPathFinding
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private MyGrid<MyPathNode> grid;
    private List<MyPathNode> openList;  // for searching
    private List<MyPathNode> closedList;  // already searched
    
    public static MyPathFinding Instance { get; private set; }
    public MyPathFinding(int width, int height)
    {
        Instance = this;
        grid = new MyGrid<MyPathNode>(width, height, 10f, Vector3.zero,
             (MyGrid<MyPathNode> g, int x, int y) => new MyPathNode(g, x, y));
    }
    
    public MyGrid<MyPathNode> GetGrid() {
        return grid;
    }

    public List<MyPathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        MyPathNode startNode = grid.GetGridObject(startX, startY);
        MyPathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null)
        {
            // invalid path
            return null;
        }
        
        openList = new List<MyPathNode> {startNode};
        closedList = new List<MyPathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                MyPathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            MyPathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);  // because the currentNode as already been searched
            closedList.Add(currentNode);

            foreach (MyPathNode neighbourNode in GetNeighboursList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;  // already searched
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    // we have a better path
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // out of nodes on the openList (=> we searched on the whole map without a path
        return null;
    }

    private List<MyPathNode> GetNeighboursList(MyPathNode currentNode)
    {
        List<MyPathNode> neighbourList = new List<MyPathNode>();

        if (currentNode.x - 1 >= 0)  // check if the node on the left (x-1) is valid (>= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down (y-1)
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up (y+1)
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }

        if (currentNode.x + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add((GetNode(currentNode.x + 1, currentNode.y + 1)));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
        
        return neighbourList;
    }
    
    private List<MyPathNode> CalculatePath(MyPathNode endNode)
    {
        List<MyPathNode> path = new List<MyPathNode>();
        path.Add(endNode);
        MyPathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)  // while the current node has a parent
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    
    private int CalculateDistanceCost(MyPathNode a, MyPathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    
    private MyPathNode GetLowestFCostNode(List<MyPathNode> pathNodeList)
    {
        MyPathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
    
    public MyPathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y);
    }
}
