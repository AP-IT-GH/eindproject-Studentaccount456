using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    
        [SerializeField] private Transform[] floatingAnimals;
        [SerializeField] private float floatAmplitude = 0.5f;
        [SerializeField] private float floatSpeed = 1f;

        private Vector3[] originalPositions;

        private void Start()
        {
            StoreOriginalPositions();
        }

        private void StoreOriginalPositions()
        {
            originalPositions = new Vector3[floatingAnimals.Length];

            for (int i = 0; i < floatingAnimals.Length; i++)
            {
                originalPositions[i] = floatingAnimals[i].position;
            }
        }

        private void Update()
        {
            ApplyFloatingEffect();
        }

        private void ApplyFloatingEffect()
        {
            for (int i = 0; i < floatingAnimals.Length; i++)
            {
                Transform floatingObject = floatingAnimals[i];

                Vector3 newPosition = originalPositions[i] + new Vector3(0f, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0f);
                floatingObject.position = newPosition;
            }
        }
    }