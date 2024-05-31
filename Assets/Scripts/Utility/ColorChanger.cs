using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ColorChanger : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    public static readonly Color32 DEFAULT_COLOR = new Color32(154, 23, 250, 255);

    // magenta 
    public Color32 _color = DEFAULT_COLOR;

    private Color32 _currentColor = DEFAULT_COLOR;

    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = _currentColor;
    }


    private void Update()
    {
        bool has_color_changed =
            (_color.r != _currentColor.r) || (_color.g != _currentColor.g) || (_color.b != _currentColor.b) ||
            (_color.a != _currentColor.a);
        if (has_color_changed)
        {
            _meshRenderer.material.color = _color;
            _currentColor = _color;
        }
    }
}