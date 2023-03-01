using System.Collections;
using System.Collections.Generic;
using ThirdPersonTemplate;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Movement movement))
            movement.OnStartSwimming();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Movement movement))
            movement.OnStopSwimming();

    }
}
