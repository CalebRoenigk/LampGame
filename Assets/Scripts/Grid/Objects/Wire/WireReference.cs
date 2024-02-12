using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WireReference", menuName = "ScriptableObjects/Wire Reference", order = 1)]
public class WireReference : ScriptableObject
{
    public List<WireData> WireData;

    public WireData GetWireData(WireDirection direction, WireSegment segment)
    {
        List<WireData> directionWires = WireData.FindAll((x) => x.Direction == direction);
        return directionWires.Find(x => x.Segment == segment);
    }
}
