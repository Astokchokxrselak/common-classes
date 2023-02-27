using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Common;
using Common.Pools;
using Common.Extensions;

public class TelegraphLine : MonoBehaviour, IPoolObject
{
    public Transform Transform => this.transform;
    public GameObject GameObject => this.gameObject;
    public BasePool Pool { get; set; }

    public void SetPoint(int index, Vector3 point) => lineRenderer.SetPosition(index, point);
    public Vector2 GetDirection(int index0, int index1) => (lineRenderer.GetPosition(index1) - lineRenderer.GetPosition(index0)).normalized;
    public Vector2 GetDirection(int index0) => GetDirection(index0, lineRenderer.positionCount - 1);
    public Vector2 GetDirection() => GetDirection(0);
    LineRenderer lineRenderer;
    public Vector3[] vertices;
    public void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = vertices?.Length ?? 2;
    }
    public void Spawn(Vector2 start, Vector2 end, float duration, float delayPerBlink)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        StartCoroutine(Spawn(duration, delayPerBlink));
    }
    private IEnumerator Spawn(float duration, float delayPerBlink)
    {
        WaitForSeconds pause = new(delayPerBlink);
        for (int i = 0; i < (int)(duration / delayPerBlink); i++)
        {
            lineRenderer.enabled = !lineRenderer.enabled;
            yield return pause;
        }
        Pool.Return(this);
    }
}
