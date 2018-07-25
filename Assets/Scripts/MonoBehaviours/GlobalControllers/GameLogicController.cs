﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate bool InputsInvertedDelegate(bool inverted);

public class GameLogicController : MonoBehaviour
{
    public event System.Action<bool> InputsInvertedEvent;

    public event System.Action<bool> InputsDelayedEvent;

    public event System.Action PlayerDiedEvent;

    public void NotifyInvertedInputs(bool inverted)
    {
        if (InputsInvertedEvent != null)
            InputsInvertedEvent(inverted);
    }

    public void NotifyDelayedInputs(bool delayed)
    {
        if (InputsInvertedEvent != null)
            InputsDelayedEvent(delayed);
    }

    public void NotifyPlayerDeath()
    {
        if (PlayerDiedEvent != null)
            PlayerDiedEvent();
    }
}
