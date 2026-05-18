using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button reloadButton;
    [SerializeField] private TextMeshProUGUI playerNameText;

    private void OnEnable()
    {
        reloadButton.onClick.AddListener(ReloadNewGame);
        EventManager.OnWinConditionMet += OnWinConditionMet;
    }


    private void OnDisable()
    {
        reloadButton.onClick.RemoveListener(ReloadNewGame);
        EventManager.OnWinConditionMet -= OnWinConditionMet;
    }
    
    
    private void OnWinConditionMet(string playerName)
    {
        playerNameText.text = $"Congratulations {playerName}!";
        Show();
        PauseGame();
    }

    private void Show()
    {
        gameOverPanel.SetActive(true);
    }


    private void PauseGame()
    {
        Time.timeScale = 0f;
    }
    
    private void ReloadNewGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
