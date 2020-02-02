using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField, FMODUnity.EventRef] string scoreIncreaseSnd;

    int lastScore = 0;
    public void SetScore(int score, int combo)
    {
        scoreText.SetText(score.ToString());
        if (score > lastScore)
        {
            Debug.Log("Play Sound");
            FMODUnity.RuntimeManager.PlayOneShot(scoreIncreaseSnd, transform.position);
        }
        if (combo == 1)
            comboText.SetText("");
        else
            comboText.SetText(combo.ToString() + "x");
        lastScore = score;

    }
}
