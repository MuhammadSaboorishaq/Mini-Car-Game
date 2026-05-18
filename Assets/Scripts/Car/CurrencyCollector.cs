using UnityEngine;

public class CurrencyCollector : MonoBehaviour
{
    private int _currentMoney = 0;

    public void AddMoney(int amount)
    {
        _currentMoney += amount;
    }

    public void DeductMoney(int amount)
    {
        if (_currentMoney - amount < 0)
            _currentMoney = 0;
        else
            _currentMoney -= amount;
    }
    
    public int GetMoney()
    {
        return _currentMoney;
    }
}