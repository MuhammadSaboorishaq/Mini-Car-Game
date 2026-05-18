using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    // using coroutine to handle the countdown timer
    [SerializeField] private float countdownTime = 3f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownPanel;
    
    private void Start()
    {
        StartCoroutine(Countdown());
    }
    
    
    
    
    private System.Collections.IEnumerator Countdown()
    {
        float currentTime = countdownTime;
        while (currentTime > 0)
        {
            countdownText.text = Mathf.Ceil(currentTime).ToString();
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
        }
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f);
        countdownPanel.SetActive(false);
        EventManager.DoFireOnGameStart();
    }
}
