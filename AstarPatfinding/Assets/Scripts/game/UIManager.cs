using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text text;

    private static UIManager instance = null;
    public static UIManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("multiple UImanageg, deletinself");
            Destroy(this);
        }
    }

    public void AddPoint()
    {
        text.text = (int.Parse(text.text) + 1).ToString();
    }

}
