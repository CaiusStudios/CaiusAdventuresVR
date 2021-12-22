// source: https://unitycodemonkey.com/

// Methods:
// MyPathNode(MyGrid<MyPathNode>, int, int)
// CalculateFCost()
// SetIsWalkable(bool)
// ovveride ToString()
public class MyPathNode
{
    private MyGrid<MyPathNode> grid;
    public int x;
    public int y;

    public bool isWalkable;
    
    public int gCost;
    public int hCost;
    public int fCost;

    public MyPathNode cameFromNode;
    
    public MyPathNode(MyGrid<MyPathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    
    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }
    
    public override string ToString()
    {
        return x + "," + y;
    }
}
