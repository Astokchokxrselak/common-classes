using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolOnContact : MonoBehaviour
{
    public string targetTag;
    private void OnTriggerEnter(Collider collision)
    {
        if (string.IsNullOrWhiteSpace(targetTag) || collision.gameObject.CompareTag(targetTag))
        {
            gameObject.SetActive(false);
        }
    }
}
