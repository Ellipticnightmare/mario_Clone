using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    #region accessible
    public Text scoreDisplay, timeDisplay;
    [Header("How many of this goes into a single second, 1 = 1 second")]
    [Range(1, 10)]
    public int timeScale = 4;
    #endregion
    #region protected
    static int points;
    float timeScaleReal;
    float timer = 300;
    #endregion
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        timeScaleReal = (1 / timeScale) + 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime * timeScaleReal;
        timeDisplay.text = ((int)timer).ToString();
        scoreDisplay.text = points.ToString();
    }
    public static void GainPoints(int pointGain)
    {
        points += pointGain;
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