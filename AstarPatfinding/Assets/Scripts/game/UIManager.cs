using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text pointsText;
    [SerializeField] private Text hpText;

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
        pointsText.text = (int.Parse(pointsText.text) + 1).ToString();
    }

    public void UpdateHealthUI(int hp)
    {
        hpText.text = hp.ToString();
    }

    public void SetDeathScreen()
    {

    }

}
