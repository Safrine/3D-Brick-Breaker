using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    public bool isCollide;

    private Material material;

    [SerializeField]
    private float startDuration;
    [SerializeField]
    private float endDuration;

    [SerializeField]
    private float max;
    [SerializeField]
    private float min;

    private float elapsed = 0;

    private bool doOffset;

    private void Start()
    {
        isCollide = false;
        material = GetComponent<Renderer>().material;
        elapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollide)
            EmissionColor();

        if (elapsed < endDuration && doOffset)
            OffColor();
        else
        {
            doOffset = false;
            elapsed = 0;
        }
    }

    private void EmissionColor()
    {
        isCollide = false;
        doOffset = true;
        material.SetFloat("_Power", -2f);
    }

    private void OffColor()
    {
        float a = (0 - (-2)) / (endDuration - 0);
        float b = (-2) - a * 0;
        float y = a * elapsed + b;
        material.SetFloat("_Power", y);
        elapsed += Time.deltaTime;
    }
}