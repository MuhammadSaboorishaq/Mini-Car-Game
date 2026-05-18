using System;
using UnityEngine;

public static partial class EventManager{
    
    public static Action OnGameStart;
    
    public static void DoFireOnGameStart()
    {
        OnGameStart?.Invoke();
    }
}
