using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailMaker : MonoBehaviour {
    private LineRenderer lineWriter;

    // Use this for initialization
    void Awake()
    {
        lineWriter = gameObject.GetComponent<LineRenderer>();
    }

    public void drawLine(Transform obj1, Transform obj2)
    {
        lineWriter.SetPosition(0, obj1.transform.position);
        lineWriter.startWidth = .8f;
        lineWriter.endWidth = .8f;
        lineWriter.SetPosition(1, obj2.position);
        float distance = Vector3.Distance(obj1.position, obj2.position);
        lineWriter.material.mainTextureScale = new Vector2(distance * 1.1f, 1f);
    }
}