using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuildingNode : Node
{
    public static Action<Unit> OnUnitSpawned;

    [SerializeField] private Transform unitPrefab;
    [SerializeField] private int unitCost = 3;
    [SerializeField] private float unitSpawnTime = 10f;

    private bool canSpawnUnit = true;
    private Button unitSpawnButton;
    private Timer timer;


    private void OnEnable()
    {
        Timer.OnTimerEnd += Timer_OnTimerEnd;
    }
    private void OnDisable()
    {
        Timer.OnTimerEnd -= Timer_OnTimerEnd;
    }
    protected override void Start()
    {
        base.Start();

        unitSpawnButton = UIManager.Instance.GetSpawnUnitButtonGO().GetComponent<Button>();
        unitSpawnButton.interactable = false;

        timer = GetComponent<Timer>();
    }

    private void Update()
    {
        if (Inventory.Instance.GetResourceStackSize(GameAssets.Instance.foodResourceData) >= unitCost && canSpawnUnit)
        {
            unitSpawnButton.interactable = true;
            //UIManager.Instance.GetUnitSpawnTimerImage().gameObject.SetActive(false);
        }
        else
        {
            unitSpawnButton.interactable = false;
            //UIManager.Instance.GetUnitSpawnTimerImage().gameObject.SetActive(true);
            UIManager.Instance.GetUnitSpawnTimerImage().fillAmount = timer.GetRemainingTimeNormalized();
        }
    }

    private void Timer_OnTimerEnd()
    {
        unitSpawnButton.interactable = true;
        canSpawnUnit = true;
        UIManager.Instance.GetUnitSpawnTimerImage().gameObject.SetActive(false);
        Debug.Log("timer ended");
    }
    public void SpawnUnit()
    {
        if(Inventory.Instance.GetResourceStackSize(GameAssets.Instance.foodResourceData) >= unitCost && canSpawnUnit)
        {
            timer.StartTimer(unitSpawnTime);
            canSpawnUnit = false;
            unitSpawnButton.interactable = false;
            UIManager.Instance.GetUnitSpawnTimerImage().gameObject.SetActive(true);

            Inventory.Instance.RemoveAmount(GameAssets.Instance.foodResourceData, unitCost);

            Vector3 pos = GetMovePointList()[0];
            Transform unitTransform = Instantiate(unitPrefab, pos, Quaternion.identity);
            Unit unit = unitTransform.GetComponent<Unit>();
            OnUnitSpawned?.Invoke(unit);
        }
    }
}
