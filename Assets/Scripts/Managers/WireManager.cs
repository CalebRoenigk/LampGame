using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    public static WireManager Instance;
    
    public GameObject WireUpStart;
    public GameObject WireUp;
    public GameObject WireUpEnd;
    public GameObject WireDownStart;
    public GameObject WireDown;
    public GameObject WireDownEnd;
    public GameObject WireLeftStart;
    public GameObject WireLeft;
    public GameObject WireLeftEnd;
    public GameObject WireRightStart;
    public GameObject WireRight;
    public GameObject WireRightEnd;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaceWires(Vector2Int start, Vector2Int end)
    {
        Vector2Int direction = end - start;
        if (direction.x == 0)
        {
            direction = direction.y > 0 ? Vector2Int.up : Vector2Int.down;
        }
        else
        {
            direction = direction.x > 0 ? Vector2Int.right : Vector2Int.left;
        }

        if (direction == Vector2Int.up)
        {
            Instantiate(WireUpStart, new Vector3(start.x, 0f, start.y), Quaternion.identity, transform);
            GridManager.Instance.Grid.SetPower(start, true);
            Instantiate(WireUpEnd, new Vector3(end.x, 0f, end.y), Quaternion.identity, transform);
            GridManager.Instance.Grid.SetPower(end, true);

            for (int y = start.y + 1; y <= end.y - 1; y++)
            {
                Vector2Int pos = new Vector2Int(start.x, y);
                GridManager.Instance.Grid.SetPower(pos, true);
                Instantiate(WireUp, new Vector3(start.x, 0f, y), Quaternion.identity, transform);
            }
        } 
        else if (direction == Vector2Int.down)
        {
            Instantiate(WireDownStart, new Vector3(start.x, 0f, start.y), Quaternion.identity, transform);
            GridManager.Instance.Grid.SetPower(start, true);
            Instantiate(WireDownEnd, new Vector3(end.x, 0f, end.y), Quaternion.identity, transform);
            GridManager.Instance.Grid.SetPower(end, true);

            for (int y = start.y - 1; y >= end.y + 1; y--)
            {
                Vector2Int pos = new Vector2Int(start.x, y);
                GridManager.Instance.Grid.SetPower(pos, true);
                Instantiate(WireDown, new Vector3(start.x, 0f, y), Quaternion.identity, transform);
            }
        }
        else if (direction == Vector2Int.left)
        {
            Instantiate(WireLeftStart, new Vector3(start.x, 0f, start.y), Quaternion.identity, transform);
            GridManager.Instance.Grid.SetPower(start, true);
            Instantiate(WireLeftEnd, new Vector3(end.x, 0f, end.y), Quaternion.identity, transform);
            GridManager.Instance.Grid.SetPower(end, true);

            for (int x = start.x - 1; x >= end.x + 1; x--)
            {
                Vector2Int pos = new Vector2Int(x, start.y);
                GridManager.Instance.Grid.SetPower(pos, true);
                Instantiate(WireLeft, new Vector3(x, 0f, start.y), Quaternion.identity, transform);
            }
        }
        else if (direction == Vector2Int.right)
        {
            Instantiate(WireRightStart, new Vector3(start.x, 0f, start.y), Quaternion.identity, transform);
            GridManager.Instance.Grid.SetPower(start, true);
            Instantiate(WireRightEnd, new Vector3(end.x, 0f, end.y), Quaternion.identity, transform);
            GridManager.Instance.Grid.SetPower(end, true);

            for (int x = start.x + 1; x <= end.x - 1; x++)
            {
                Vector2Int pos = new Vector2Int(x, start.y);
                GridManager.Instance.Grid.SetPower(pos, true);
                Instantiate(WireRight, new Vector3(x, 0f, start.y), Quaternion.identity, transform);
            }
        }
    }
}
