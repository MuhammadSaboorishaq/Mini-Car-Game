using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBotController : MonoBehaviour
{

    [SerializeField] private AiTargetDispacher targetDispacher;
    [SerializeField] private BotPlayerBrain brain;
    private Coroutine currentRoutine;
    #region State & Control

    
    public BotState currentState = BotState.RandomMovement;
    private BotState previousState;
    [SerializeField] private BotState defaultState = BotState.CollectCurrency;
    [SerializeField] private float stateTimeLimit = 2f;
    [SerializeField] private List<BotStateChance> stateChancesRatio = new()
    {
        new BotStateChance { State = BotState.RandomMovement, Weight = 65 },
        new BotStateChance { State = BotState.CollectCurrency, Weight = 35 },
    };
    
    [SerializeField] private List<BotStateChance> stateChancesAfterHit = new()
    {
        new BotStateChance { State = BotState.RandomMovement, Weight = 45   },
        new BotStateChance { State = BotState.Idle, Weight = 65 },
    };
    
    #endregion


    private void OnEnable()
    {
        EventManager.OnGameStart+= StartBot;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart-= StartBot;
    }

    private void StartBot()
    {
        ChangeState(defaultState);
        
    }
    #region State Logic

    public void ChangeState(BotState newState)
    {
        StopCurrentRoutine();
        previousState = currentState;
        currentState = newState;

        // OnHitEvent?.Invoke();

        switch (newState)
        {
            case BotState.RandomMovement: currentRoutine = StartCoroutine(RandomMovementRoutine()); break;
            case BotState.CollectCurrency: currentRoutine = StartCoroutine(CollectCurrencyRoutine()); break;
            case BotState.AttackPlayer: currentRoutine = StartCoroutine(AttackPlayerRoutine()); break;
            case BotState.EvadePlayer: currentRoutine = StartCoroutine(EvadePlayerRoutine()); break;
            case BotState.Idle: currentRoutine = StartCoroutine(IdlePlayerRoutine()); break;
            default:
                currentRoutine = StartCoroutine(RandomMovementRoutine()); break;
        }
    }

    private void StopCurrentRoutine()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
    }
    #endregion

    
    #region Random Movement Routine
    
    private IEnumerator RandomMovementRoutine()
    {
        Vector3 newPostion = targetDispacher.GetRandomPointInRadius();
        brain.TargetPosition = newPostion;
        
        Debug.Log("in RandomMovementRoutine");
        yield return WaitUntilOrTimeout(()=> Vector3.Distance(transform.position, newPostion) < 0.1f, stateTimeLimit);

        
        Debug.Log("Reached point out of RandomMovementRoutine");
        
        SelectNextAction(stateChancesRatio);

    }
    
    #endregion

    #region Collect Currency Routine

    private IEnumerator CollectCurrencyRoutine()
    {
        Vector3 newPostion = targetDispacher.GetRandomCurrencyPoint();
        brain.TargetPosition = newPostion;
        
        Debug.Log("in CollectCurrencyRoutine");
        yield return WaitUntilOrTimeout(()=> Vector3.Distance(transform.position, newPostion) < 0.1f, stateTimeLimit);
        
        Debug.Log("Reached point out of CollectCurrencyRoutine");
        
        SelectNextAction(stateChancesRatio);
    }

    #endregion
    
    #region Attack Player Routine

    private IEnumerator AttackPlayerRoutine()
    {
        yield return  null;
    }

    #endregion
    
    #region Evade Player Routine

    private IEnumerator EvadePlayerRoutine()
    {
        yield return   null;
    }
    
    #endregion

    #region Idle Routine

    private IEnumerator IdlePlayerRoutine()
    {
        brain.TargetPosition = null;
        yield return WaitUntilOrTimeout(()=> true, 1.5f);
        SelectNextAction(stateChancesRatio);
    }

    #endregion
    
    
    private void SelectNextAction(List<BotStateChance> stateChances)
    {
        int roll = UnityEngine.Random.Range(0, 100), cumulative = 0;
        foreach (var sc in stateChances)
        {
            cumulative += sc.Weight;
            if (roll < cumulative)
            {
                ChangeState(sc.State);
                return;
            }
        }
        ChangeState(BotState.RandomMovement);
    }


    #region Helper

    public void OnHit()//get called by car controller when bot get hit by NPC or Bot
    {
        brain.TargetPosition = null;
        SelectNextAction(stateChancesAfterHit);
    }
    
    public static IEnumerator WaitUntilOrTimeout(System.Func<bool> condition, float timeout)
    {
        float timer = 0f;

        while (!condition() && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
    

    #endregion
}

public enum BotState
{
    RandomMovement,
    CollectCurrency,
    AttackPlayer,
    EvadePlayer,
    Idle
}

[Serializable]
public class BotStateChance
{
    public BotState State;
    [Range(0, 100)] public int Weight;
}