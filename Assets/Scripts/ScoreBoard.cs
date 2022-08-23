using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;


public class ScoreBoard : MonoBehaviour
{

 
     private void Awake()
    {
        MenuManager.Instance.LoadScoreBoard();
    }
    
   
}
