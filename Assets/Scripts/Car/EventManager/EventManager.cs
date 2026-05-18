using System;
using UnityEngine;

public static partial class EventManager{

    public static Action<CarTypes , int> OnScoreChanged;

    public static void DoFireScoreChanged(CarTypes type, int score)
    {
        OnScoreChanged?.Invoke(type, score);
    }
    
    public static Action<string> OnWinConditionMet;
    
    public static void DoFireWinConditionMet(string message)
    {
        OnWinConditionMet?.Invoke(message);
    }
}
