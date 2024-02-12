using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : EmitterObject
{
    public WireDirection Direction;
    public WireSegment Segment;
    [SerializeField] private MeshFilter _meshFilter;
    public Cell Cell;

    [SerializeField] private WireReference _wireReference;

    public void SetWire(WireDirection direction, WireSegment segment)
    {
        WireData wireData = _wireReference.GetWireData(direction, segment);
        Direction = direction;
        Segment = segment;
        _meshFilter.mesh = wireData.WireMesh;
        transform.rotation = Quaternion.Euler(wireData.Rotation.x, wireData.Rotation.y, wireData.Rotation.z);
        ObjectMaterial.SetFloat("_Visibility", 1f);
        Setup(); // Turn the emit state to on
    }
}
