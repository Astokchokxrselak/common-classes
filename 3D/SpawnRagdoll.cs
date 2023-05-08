using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRagdoll : MonoBehaviour
{
    public Transform ragdoll;
    private void Start()
    {
        CreateRagdoll();
    }
    void CreateRagdoll()
    {
        var rag = Instantiate(ragdoll, transform.position, transform.rotation);
        rag.localScale = transform.localScale;

        List<string> children = new();
        foreach (Transform child in rag)
        {
            child.transform.localPosition = transform.Find(child.name).localPosition;
            child.transform.localRotation = transform.Find(child.name).localRotation;
            children.Add(child.name);
        }
        foreach (Transform child in transform)
        {
            if (!children.Contains(child.name))
            {
                child.SetParent(rag, false);
            }
        }
        Destroy(gameObject);
    }
}
