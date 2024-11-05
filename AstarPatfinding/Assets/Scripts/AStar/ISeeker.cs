using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISeeker
{
    void OnPathFound(List<Vector3> path, bool pathFound);
    void Poll();
}
