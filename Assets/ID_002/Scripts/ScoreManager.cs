using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI showText;
    [SerializeField] private static int amount;

    private void Start()
    {
        UpdateScore();
    }
    public void Reward()
    {
        amount += 5;
        UpdateScore();
    }
    public void Punishment()
    {
        amount -= 5;
        UpdateScore();
    }
    private void UpdateScore()
    {
        showText.text = "Score: " + amount.ToString();
    }
}