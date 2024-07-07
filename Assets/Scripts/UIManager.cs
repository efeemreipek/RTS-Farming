using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    [Header("Resources")]
    [SerializeField] private TextMeshProUGUI goldAmountText;


    public TextMeshProUGUI GetGoldAmountText() => goldAmountText;
}
