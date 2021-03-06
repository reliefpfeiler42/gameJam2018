﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameObject[] portals;
    public GameObject player;

    private GameObject _exitPortal;

    public void Porting(string name)
    {
        int idx = -1;
        for (int i = 0; i < portals.Length - 1; i++)
        {
            if (portals[i].name == name)
            {
                idx = i;
                break;
            }
        }

        int selectedPortal;
        do
        {
            selectedPortal = Random.Range(0, portals.Length);
        } while (selectedPortal == idx);

        _exitPortal = portals[selectedPortal];
        player.transform.position = _exitPortal.GetComponent<PortalCollider>().offset.position;
    }
}
