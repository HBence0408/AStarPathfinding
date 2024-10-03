using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathRequest
{
    private Vector3 pathStart;
    public Vector3 pathEnd;
    private Action<List<Vector3>, bool> callback;

    public Vector3 PathStart { get => pathStart; }
    public Vector3 PathEnd { get => pathEnd; }
    public Action<List<Vector3>, bool> Callback { get => callback; }

    public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<List<Vector3>, bool> callback)
    {
        this.pathStart = pathStart;
        this.pathEnd = pathEnd;
        this.callback = callback;
    }
}
