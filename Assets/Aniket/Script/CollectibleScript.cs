using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    bool collected;
    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if(other.CompareTag("Player"))
        {
            if (other.CompareTag("Player"))
            {
                CollectorScript cs;

                if (other.TryGetComponent(out cs))
                {
                    cs.Stack(transform);
                    collected = true;
                }
            }
        }
    }
}
