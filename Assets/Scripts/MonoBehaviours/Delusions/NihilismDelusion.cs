﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NihilismDelusion : Delusion
{
    public GameObject playerCharacter;
    public Material dissolveMaterial;

    //private Renderer _renderer;
    private Material _baseMaterial;

    private GameLogicController _glc;

    private void Start()
    {
        // _renderer = playerCharacter.GetComponent<Renderer>();
        //if (_renderer != null)
        //    _baseMaterial = _renderer.material;

        _glc = FindObjectOfType<GameLogicController>();

        _as = _sc.nihilismDelusionSource;
    }

    public override void DelusionForecast()
    {
        Debug.Log("Vibration Forecast");
        Handheld.Vibrate();
    }

    public override void DelusionContent()
    {
        //_renderer.material = dissolveMaterial;
        _glc.NotifyDelayedInputs(true);
    }

    public override void DelusionCloseDown()
    {
        //if (_renderer != null)
        //    _renderer.material = _baseMaterial;
        _glc.NotifyDelayedInputs(false);
    }
}
