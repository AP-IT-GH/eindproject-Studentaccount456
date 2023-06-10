using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreEvaluation : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI scoreText;
   [SerializeField] private int score;

    private string[] scoreMessages = {
        "Your score of {0} indicates you're a pro!",
        "Your score of {0} means you escaped with a few scratches",
        "You escaped and earned a score of {0}, but you lost a limb...",
        "Your score is {0}, that means that you cheated somehow"
    };

    private int[] scoreRanges = { 80, 50, 30 };

    private void Update()
    {
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        for (int i = 0; i < scoreRanges.Length; i++)
        {
            if (score >= scoreRanges[i])
            {
                scoreText.text = string.Format(scoreMessages[i], score);
                return;
            }
        }
        scoreText.text = string.Format(scoreMessages[scoreMessages.Length - 1], score);
    }
}
