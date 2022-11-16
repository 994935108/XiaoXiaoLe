using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonGameObject<GameManager>
{
    internal BlockManage newBehaviourScript;
    internal int HighestScoreInHistory { 
        get {
            return PlayerPrefs.GetInt("HighestScoreInHistory");
        }
        set {
            PlayerPrefs.SetInt("HighestScoreInHistory",value);
        }
    }
    public void RestartGame() {
        newBehaviourScript.Restart();
    }

    public void StartGame()
    {
        newBehaviourScript.StartGame();
       
    }

    private void ClearCount(int count) { 
       

    }

    private void GameOver() {
        newBehaviourScript.GameOver();
        
    }
}
