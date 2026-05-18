using System;
using UnityEngine;
using UnityEngine.Events;


public class CarController : MonoBehaviour
{
    private ICarBrain _brain;
    private CarMotor _motor;
    private CarSteering _steering;
    private CarCollisionHandler _collisionHandler;
    private CurrencyCollector _currencyCollector;
    
    [Header("Car Settings")]
    [SerializeField] private CarTypes carType= CarTypes.Player1;
    
    [Header("Controlling")]
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float stunDuration = 1f;

    [Header("Currency Deduction")]
    [SerializeField] private int deductionAmount = 10000;
    
    [SerializeField] private UnityEvent onPlayerCollision;
    
    private float _stunTimer;
    public Action OnFinished;

    private void Awake()
    {
        _brain = GetComponent<ICarBrain>();
        _motor = GetComponent<CarMotor>();
        _steering = GetComponent<CarSteering>();
        _collisionHandler = GetComponent<CarCollisionHandler>();
        _currencyCollector = GetComponent<CurrencyCollector>();

        if (_collisionHandler != null)
        {
            _collisionHandler.OnCollectableCollision += HandleCollection;
            _collisionHandler.OnTrafficCollision += HandleCollision;
            _collisionHandler.OnPlayerCollision += HandleCollisionWithPlayer;
        }
    }

   
    private void FixedUpdate()
    {
        if (_brain == null) return;

        Vector3 target = _brain.GetTargetPosition();

        _steering.UpdateRotation(target);
        _motor.MoveTowards(target, stoppingDistance);

        if (_brain is TraficNPCBrain npc)
        {
            bool finished = npc.TryAdvance(transform.position);

            if (finished)
            {
                OnFinished?.Invoke();
                
            }
        }
    }

    
    private void UpdateUI()
    {
        int money = _currencyCollector.GetMoney();
        EventManager.DoFireScoreChanged(carType, money);
        if (money>=1000000) // if money is 1M
        {
            EventManager.OnWinConditionMet(carType.ToString());
        }
    }
    private void HandleCollection(Collision collision)
    {
       Invoke(nameof(UpdateUI), 0.1f);
    }

    private void HandleCollision(Collision collision)
    {
        _currencyCollector.DeductMoney(deductionAmount);
        _stunTimer = stunDuration;
        _motor.Decelerate();
        Invoke(nameof(UpdateUI), 0.1f);
        onPlayerCollision?.Invoke();
    }
    
    private void HandleCollisionWithPlayer(Collision obj)
    {
        onPlayerCollision?.Invoke();
    }
}

public enum CarTypes : byte
{
    Player1,
    Player2,
    Player3,
    Player4,
    NPC
}