using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitManagement : MonoBehaviour
{
    public static UnitManagement Instance { get; set; }

    public List<GameObject> allNormalUnitsOnRoute1 = new();
    public List<GameObject> allNormalUnitsOnRoute2 = new();
    public List<GameObject> units = new(); // List of all units we can spawn
    public List<int> unitPrice = new(); // List of corresponding cost of every unit in list 'units'
    public List<GameObject> unitOnclickedBtn = new();
    public List<GameObject> unitIcons = new();
    public List<GameObject> deployFlags = new();
    public GameObject deployFlag;
    public int selectedUnitIndex = -1;

    public Vector3[] waypoints_1, waypoints_2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        } else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        waypoints_1 = new Vector3[] {
            new(-18.8f, 4.1f, 0), new(-16.1f, 2, 0), new(-13.5f, 2, 0),
            new(-13.5f, 9.2f, 0), new(4.4f, 9.2f, 0), new(4.4f, 6, 0),
            new(11.4f, 6, 0), new(11.4f, 10.2f, 0), new(19.5f, 10.2f, 0),
            new(19.5f, 6.6f, 0), new(22.2f, 3.9f, 0)
        };

        waypoints_2 = new Vector3[] {
            new(-18.8f, -1, 0), new(-20.55f, -6.9f, 0), new(-13.5f, -6.9f, 0),
            new(-13.5f, -8.9f, 0), new(-10.55f, -8.9f, 0), new(-10.55f, 2.1f, 0),
            new(-3.5f, 2.1f, 0), new(-3.5f, -2.9f, 0), new(1.4f, -2.9f, 0),
            new(1.4f, -6.6f, 0), new(4, -8, 0), new(13.9f, -8, 0),
            new(16.4f, -5.6f, 0), new(16.4f, -3.2f, 0), new(13.2f, -3.2f, 0),
            new(11.5f, -1, 0), new(11.5f, 0.9f, 0), new(18.4f, 0.9f, 0),
            new(20.8f, -0.9f, 0), new(22.2f, -0.9f, 0)
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("UnitSpawner"))
            {
                DeselectUnitSpawner();
            }
        }
    }

    public void SpawnUnit(int unitIndex)
    {
        if (selectedUnitIndex != unitIndex)
        {
            DeselectUnitSpawner();
        }
        SelectUnitSpawner(unitIndex);
    }

    private void SelectUnitSpawner(int unitIndex)
    {
        selectedUnitIndex = unitIndex;

        deployFlag.SetActive(true);

        unitOnclickedBtn[selectedUnitIndex].SetActive(true); // display other button's background
        unitIcons[selectedUnitIndex].transform.localScale = new(1.2f, 1.2f, 1); //scale icon
    }

    private void DeselectUnitSpawner()
    {
        if (selectedUnitIndex != -1)
        {
            deployFlag.SetActive(false);

            unitOnclickedBtn[selectedUnitIndex].SetActive(false); // display original button's background
            unitIcons[selectedUnitIndex].transform.localScale = new(1, 1, 1); //scale icon

            selectedUnitIndex = -1;
        }
    }

    public void DeployUnitLine1()
    {
        if (BaseManagement.Instance.souls < unitPrice[selectedUnitIndex]) return;
        Vector3 spawnPosition = new(-24.25f, 4.1f, 0);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject deployedUnit = Instantiate(units[selectedUnitIndex], spawnPosition, spawnRotation);
        BaseManagement.Instance.souls -= unitPrice[selectedUnitIndex];

        UnitMovement unitMovement = deployedUnit.GetComponent<UnitMovement>();
        unitMovement.allNormalUnitsOnRoute = allNormalUnitsOnRoute1;
        unitMovement.waypoints = waypoints_1;

        allNormalUnitsOnRoute1.Add(deployedUnit);
    }
    public void DeployUnitLine2()
    {
        if (BaseManagement.Instance.souls < unitPrice[selectedUnitIndex]) return;
        Vector3 spawnPosition = new(-24.25f, -1, 0);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject deployedUnit = Instantiate(units[selectedUnitIndex], spawnPosition, spawnRotation);
        BaseManagement.Instance.souls -= unitPrice[selectedUnitIndex];

        UnitMovement unitMovement = deployedUnit.GetComponent<UnitMovement>();
        unitMovement.waypoints = waypoints_2;
        unitMovement.allNormalUnitsOnRoute = allNormalUnitsOnRoute2;

        allNormalUnitsOnRoute2.Add(deployedUnit);
    }
}

