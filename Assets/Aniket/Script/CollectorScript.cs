using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectorScript : MonoBehaviour
{
    Transform container;
    int count;
    [SerializeField] float heightFactor;
    void Start()
    {
        container = transform.GetChild(0);
    }
    public void Stack(Transform collectible)
    {
        Vector3 desiredPos = new Vector3(0, heightFactor * count);
        collectible.DOJump(container.position + desiredPos, 1.5f, 1, .1f).OnComplete(
            () =>
            {
                collectible.SetParent(container, true);
                collectible.localPosition = desiredPos;
                collectible.localRotation = Quaternion.identity;
                count++;
            }

            );
    }
}
