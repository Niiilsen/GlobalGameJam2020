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

        comboText.SetText((combo + 1).ToString() + "x");
        lastScore = score;

    }

    public void ResetCombo()
    {

        comboText.SetText("");
    }
}
