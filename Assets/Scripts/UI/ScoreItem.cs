using TMPro;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI scoreText;
   
   
      public void SetScore(int score=0)
      {
         scoreText.text = $"${score.ToShortNumberString()}";
      }
}
