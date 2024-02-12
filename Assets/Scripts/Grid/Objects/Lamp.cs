using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : PowerableObject
{
    public int LightRadius = 5;
    [SerializeField] private GameObject _particleFizz;
    [SerializeField] private AudioClip _lightOn;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        ObjectMaterial.SetVector("_Emit_Offset", new Vector2(0, -19));
    }

    public override void SetState()
    {
        if (IsPowered)
        {
            // Request new tiles from the grid manager
            GridManager.Instance.IlluminateTiles(CellParent.Position, LightRadius);
            _audioSource.PlayOneShot(_lightOn);
        }
        
        ObjectMaterial.SetInt("_Powered", IsPowered ? 1 : 0);
        _particleFizz.SetActive(IsPowered);
    }
}
