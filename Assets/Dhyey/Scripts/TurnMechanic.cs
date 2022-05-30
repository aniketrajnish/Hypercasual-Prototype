using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMechanic : MonoBehaviour
{
    public bool turnLeft = false;
    public bool turnRight = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        this.gameObject.SetActive(false);
    }
}
