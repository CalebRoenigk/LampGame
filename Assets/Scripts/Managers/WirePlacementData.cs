using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WirePlacementData
{
    public float Time;
    public WireDirection Direction;
    public WireSegment Segment;
    public Vector2Int Position;

    public WirePlacementData(float time, WireDirection direction, WireSegment segment, Vector2Int position)
    {
        Time = time;
        Direction = direction;
        Segment = segment;
        Position = position;
    }
}
