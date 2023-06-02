using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public bool TargetEntered { get; set; }
    public void OnTriggerEnter(Collider other)
    {
        TargetEntered = true;
    }
}
