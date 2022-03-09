using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    #region accessible
    public Text scoreDisplay, timeDisplay, lifeDisplay;
    public marioController Player;
    public GameObject loadingScreen;
    public AudioSource gameMusic;
    [Header("How many of this goes into a single second, 1 = 1 second")]
    [Range(1, 10)]
    public int timeScale = 4;
    [Header("How long the lives display is up before the game starts")]
    [Range(1, 10)]
    public int startDelay = 3;
    #endregion
    #region protected
    static int points;
    int lives, score;
    float timeScaleReal;
    float timer = 300;
    bool inGame;
    #endregion
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        lives = PlayerPrefs.GetInt("Lives");
        points = PlayerPrefs.GetInt("curScore");
        Player.enabled = false;
        timeScaleReal = (1 / timeScale) + 1;
        StartCoroutine(RunOpening());
    }

    // Update is called once per frame
    void Update()
    {
        timeDisplay.text = "TIMER \n" + ((int)timer);
        scoreDisplay.text = "SCORE \n" + points;
        if (inGame)
        {
            score = points;
            timer -= Time.deltaTime * timeScaleReal;
            timeDisplay.text = "TIMER \n" + ((int)timer).ToString();
            scoreDisplay.text = "SCORE \n" + points.ToString();
        }
        else
            lifeDisplay.text = "LIVES \n" + + lives;
    }
    public static void GainPoints(int pointGain)
    {
        points += pointGain;
    }
    public void GainCoin()
    {
        GainPoints(100);
    }
    public void PlayerDied()
    {
        lives--;
        if (lives >= 0)
        {
            PlayerPrefs.SetInt("Lives", lives);
            PlayerPrefs.SetInt("curScore", score);
        }
        else
        {
            if (score > PlayerPrefs.GetInt("HighScore"))
                PlayerPrefs.SetInt("HighScore", score);
            SceneManager.LoadScene("MainMenu");
        }
    }
    public static void RunFinish()
    {
        if (points > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", points);
        SceneManager.LoadScene("MainMenu");
    }
    public void LifeUp()
    {
        lives++;
    }
    IEnumerator RunOpening()
    {
        yield return new WaitForSeconds(startDelay);
        Player.enabled = true;
        loadingScreen.SetActive(false);
        inGame = true;
        gameMusic.Play();
    }
}
#region Externals
#region Structs
[System.Serializable]
public struct keyBindingData //basic struct just to make things easier in the inspector
{
    public string commandName, keyName;
}
#endregion
#region Classes
#endregion
#endregion
