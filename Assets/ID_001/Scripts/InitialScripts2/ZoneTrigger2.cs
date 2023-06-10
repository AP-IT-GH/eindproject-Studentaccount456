using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneTrigger2 : MonoBehaviour
{
    public bool TargetEntered { get; set; }
    public void OnTriggerEnter(Collider other)
    {
        TargetEntered = true;
    }
}
