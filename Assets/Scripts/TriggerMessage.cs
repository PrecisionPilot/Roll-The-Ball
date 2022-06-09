using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMessage : MonoBehaviour
{
    [SerializeField]
    Arduino arduino;
    [SerializeField]
    string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            arduino.SendLine(message);
        }
    }
}
