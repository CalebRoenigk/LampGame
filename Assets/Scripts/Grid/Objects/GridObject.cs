using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class GridObject : MonoBehaviour
{
    public Cell CellParent;
    public Material ObjectMaterial;
    public bool IsTraversable = true;
    public bool IsCheckpoint = false;
    
    private void Awake()
    {
        ObjectMaterial = GetComponent<MeshRenderer>().material;
    }

    public void UpdateVisibility(float delay = 0f)
    {
        // Debug.Log(this.gameObject.name + " updating visibility");
        ObjectMaterial.DOFloat(1f, "_Visibility", 0.75f).SetEase(Ease.InOutSine).SetDelay(delay);
    }
}
