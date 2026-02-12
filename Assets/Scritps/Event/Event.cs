using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event<T> where T : Event<T>
{
    public static Action OnEvent;

    public static void Register(Action onEvent)
    {
        OnEvent += onEvent;
    }

    public static void UnRegister(Action onEvent)
    {
        OnEvent -= onEvent;
    }

    public static void Trigger()
    {
        OnEvent?.Invoke();
    }
}
