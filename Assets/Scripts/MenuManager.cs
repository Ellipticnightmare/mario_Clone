using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Text highScoreText;
    public void HitPlay()
    {
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.SetInt("curScore", 0);
        SceneManager.LoadScene("GameScene");
    }
    public void HitQuit() => Application.Quit();
    private void Update()
    {
        int scoreVal = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = (scoreVal.ToString("000000"));
    }
}
