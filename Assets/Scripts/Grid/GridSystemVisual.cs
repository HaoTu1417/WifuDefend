using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [System.Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow
    }

    [SerializeField] Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    [SerializeField] private Unit selectedUnitForDebug;
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;


    // // Start is called before the first frame update
    void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingelPrefab = Instantiate(gridSystemVisualSinglePrefab,
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingelPrefab.GetComponent<GridSystemVisualSingle>();
            }
        }

        // UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        // LevelGrid.Instance.OnAnyUnitMoveGridPosition += LevelGrid_OnAnyUnitMoveGridPosition;
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    //
    public void HideAllGridPosition()
    {
        for (int x = 0; x < gridSystemVisualSingleArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridSystemVisualSingleArray.GetLength(1); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }


    // this function can be used to show the attach range of tower 
    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositoinList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z < range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }


                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > range)
                {
                    continue;
                }

                gridPositoinList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositoinList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (var gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual()
    {
        if (selectedUnitForDebug == null)
        {
            return;
        }

        ShowGridPositionList(selectedUnitForDebug.GetValidAttackGridPositionList(), GridVisualType.Red);
        // HideAllGridPosition();
        //
        // Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        // BaseAction selectedActrion = UnitActionSystem.Instance.GetSelectedAction();
        //
        // GridVisualType gridVisualType;

        // switch (selectedActrion)
        // {
        //     default:
        //     case MoveAction moveAction:
        //         gridVisualType = GridVisualType.White;
        //         break;
        //     case SpinAction spinAction:
        //         gridVisualType = GridVisualType.Blue;
        //         break;
        //     case ShootAction shootAction:
        //         gridVisualType = GridVisualType.Red;
        //         ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
        //         break;
        // }


        //  ShowGridPositionList(selectedActrion.GetValidActionGridPositionList(), gridVisualType);
    }

    //
    // private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    // {
    //     UpdateGridVisual();
    // }
    // private void LevelGrid_OnAnyUnitMoveGridPosition(object sender, EventArgs e)
    // {
    //     UpdateGridVisual();
    // }
    //
    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridViusalTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridViusalTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridViusalTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;
    }
}