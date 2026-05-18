using System;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
   [SerializeField]private List<CarScoreBinding> scoreBindings = new ();

   private void Awake()
   {
      ResetAllScore();
   }

   private void OnEnable()
   {
      EventManager.OnScoreChanged += UpdatedScore;
   }
   
   private void OnDisable()
   {
      EventManager.OnScoreChanged -= UpdatedScore;
   }


   private void ResetAllScore()
   {
      foreach (var item in scoreBindings)
      {
         item.scoreUI.SetScore();
      }
   }
   
   public void UpdatedScore(CarTypes type, int score)
   {
      var binding = scoreBindings.Find(x => x.type == type);
      if (binding != null)
      {
         binding.scoreUI.SetScore(score);
      }
   }
   
}

[System.Serializable]
public class CarScoreBinding
{
   public CarTypes type;
   public ScoreItem scoreUI;
}