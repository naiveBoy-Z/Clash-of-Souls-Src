using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BuildTower : MonoBehaviour
{
    public GameObject towerPrefab;

    public void BuildAt(GameObject spot)
    {
        Vector3 spawnPosition = transform.parent.position;
        Quaternion spawnRotation = Quaternion.identity;

        if (BaseManagement.Instance.coins >= towerPrefab.GetComponent<TowerController>().price)
        {
            Destroy(spot);
            GameObject tower = Instantiate(towerPrefab, spawnPosition, spawnRotation);
            BaseManagement.Instance.coins -= tower.GetComponent<TowerController>().price;
        }
    }
}
