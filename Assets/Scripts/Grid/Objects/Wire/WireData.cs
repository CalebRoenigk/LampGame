using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WireData
{
    public WireDirection Direction;
    public WireSegment Segment;
    public Mesh WireMesh;
    public Vector3 Rotation;
}
