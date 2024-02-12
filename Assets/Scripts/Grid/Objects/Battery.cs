using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : EmitterObject
{
    public bool EffectedPlayer = false;
    public int PowerAddition = 8;
    [SerializeField] private GameObject _particleFizz;
    [SerializeField] private AudioClip _powerUp;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        ObjectMaterial.SetVector("_Emit_Offset", new Vector2(0, -8));
    }

    public void AddPowerToPlayer()
    {
        Setup();
        if (!EffectedPlayer)
        {
            _audioSource.PlayOneShot(_powerUp);
            _particleFizz.SetActive(true);
            PlayerController.Instance.Power += this.PowerAddition;
            EffectedPlayer = true;
        }
    }
}
