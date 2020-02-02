using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField, FMODUnity.EventRef] string scoreIncreaseSnd;

    int lastScore = 0;
    public void SetScore(int score)
    {
        scoreText.SetText(score.ToString());
        if (score > lastScore)
        {
            Debug.Log("Play Sound");
            FMODUnity.RuntimeManager.PlayOneShot(scoreIncreaseSnd, transform.position);
        }
        lastScore = score;

    }
}
