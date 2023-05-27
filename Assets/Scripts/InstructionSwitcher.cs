using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSwitcher : MonoBehaviour
{
    public GameObject canvas;

    public void HideInstructions()
    {
        canvas.SetActive(false);
    }

    public void ShowInstructions()
    {
        canvas.SetActive(true);
    }
}
