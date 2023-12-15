using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitSystem : MonoBehaviour
{
    public static UnitSystem Instance { get; private set; }


    private Unit selectedUnit;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedUnit)
            {
                selectedUnit.StopMoving();
                selectedUnit = null;
            }
        }
    }



    public void AHandleSpawnUnitAtMousePosition(Unit unit)
    {
        SpawnUnitAtWorldPosition(unit, MouseWorld.GetPosition());
    }

    private void SpawnUnitAtWorldPosition(Unit unit, Vector3 position)
    {
        Unit go = Instantiate(unit, position, Quaternion.identity, transform);
        selectedUnit = go;
    }
}