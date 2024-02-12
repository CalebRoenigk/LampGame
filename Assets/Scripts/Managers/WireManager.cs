using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    public static WireManager Instance;
    
    public GameObject Wire;

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

    public void PlaceWire(Vector2Int position, WireDirection direction, WireSegment segment)
    {
        Cell cell = GridManager.Instance.Grid.Cells[position];

        // Only add a wire if a wire like the one to be placed does not already exist in this cell
        if (cell.Wires.FindIndex(x => x.Direction == direction && x.Segment == segment) == -1)
        {
            Wire wire = Instantiate(Wire, cell.GetWorldPosition(), Quaternion.identity, transform).GetComponent<Wire>();
            cell.AddWire(wire);

            wire.SetWire(direction, segment);
        }
    }
}
