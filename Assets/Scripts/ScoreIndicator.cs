using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIndicator : MonoBehaviour
{

    public Text scoreText;

    public void ChangeScore(int score)
    {
        scoreText.text = score.ToString();
    }

}
