using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegretTextRandomizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] regretTexts;

    private string[] dyingRegrets = {
        "\"If only I didn't enter the forest...\"",
        "\"I shouldn't have gone out so late...\"",
        "\"I should've procrastinated becoming a target. Timing wasn't on my side...\"",
        "\"I wish I had taken a different path...\"",
        "\"Why did I ignore the warning signs...\"",
        "\"I underestimated the danger...\"",
        "\"I should've listened to my instincts...\"",
        "\"I thought I could handle it, but I was wrong...\""
    };

    private void Start()
    {
        RandomizeRegretTexts();
    }

    private void RandomizeRegretTexts()
    {
        int[] randomIndices = GenerateRandomIndices(dyingRegrets.Length);

        for (int i = 0; i < regretTexts.Length; i++)
        {
            int index = randomIndices[i];
            if (index < dyingRegrets.Length)
                regretTexts[i].text = dyingRegrets[index];
        }
    }

    private int[] GenerateRandomIndices(int length)
    {
        // Fisher-Yates algoritme om je array te shufflen.
        int[] indices = new int[length];
        for (int i = 0; i < length; i++)
        {
            indices[i] = i;
        }
        for (int i = length - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        return indices;
    }
}
