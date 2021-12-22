using System.Collections.Generic;
using UnityEngine;

// source: https://unitycodemonkey.com/
// Simple Comment for hotfix: need to consider VR controller, not the mouse

public class MyTesting : MonoBehaviour
{

    private MyPathFinding pathfinding;
    
    // Start is called before the first frame update
    private void Start()
    {
        pathfinding = new MyPathFinding(20, 10);
        Debug.Log(Input.mousePosition);
    }

    private void Update()
    {
        Debug.DrawLine(new Vector3(0,0,0), new Vector3(20, 20, 0), Color.red, 25f, true);
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = MyUtils.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<MyPathNode> path = pathfinding.FindPath(0, 0, x, y);
            if (path != null)
            {
                for (int i=0; i<path.Count - 1; i++) {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, 
                        new Vector3(path[i+1].x, path[i+1].y) * 10f + Vector3.one * 5f, Color.red, 100f);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = MyUtils.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }
}
