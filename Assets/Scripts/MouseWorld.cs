using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;

    public event EventHandler<Unit> OnSelectedUnitChanged;
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private Unit selectedUnit;

    [SerializeField] private Vector3 debugMousePosition;
    [SerializeField] private Vector3 debugMouseInputPosition;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        debugMousePosition = GetPosition();
        debugMouseInputPosition = Input.mousePosition;
        if (TryHandleUnitSelection())
        {
            return;
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    // if (unit == selectedUnit)
                    // {
                    //     // Unit is already selected
                    //     return false;
                    // }
                    //
                    // if (unit.IsEnemy())
                    // {
                    //     // Clicked on an Enemy
                    //     return false;
                    // }
                    
                    SetSelectedUnit(unit);
                    //unit.StartMoving();
                    return true;
                }
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        //SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, unit);
    }


    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}