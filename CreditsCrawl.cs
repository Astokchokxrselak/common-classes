using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsCrawl : MonoBehaviour
{
    public float crawlSpeed;
    public Vector3 start, end;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, end, crawlSpeed);
    }
    private void OnEnable()
    {
        rectTransform.anchoredPosition = start;
    }
}
