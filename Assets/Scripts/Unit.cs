using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    [SerializeField] private Tower towerData;
    [SerializeField] private bool isEnemy;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private Moveable moveableComponent;

    //   private BaseAction[] baseActionArray;
    private int actionPoints = 2;


    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        // baseActionArray = GetComponents<BaseAction>();
        moveableComponent = GetComponent<Moveable>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        // TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }


    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public List<GridPosition> GetValidAttackGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = GetGridPosition();

        for (int x = -towerData.Radius; x <= towerData.Radius; x++)
        {
            for (int z = -towerData.Radius; z <= -towerData.Radius; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position already occupied with another Unit
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public void StopMoving()
    {
        moveableComponent.SetIsSelected(false);
    }

    public void StartMoving()
    {
        moveableComponent.SetIsSelected(true);
    }


    // public T GetAction<T>() where T: BaseAction
    // {
    //     foreach(BaseAction baseAction in baseActionArray)
    //     {
    //         if(baseAction is T)
    //         {
    //             return (T)baseAction;
    //         }
    //     }
    //     return null;
    // }


    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }


    // public BaseAction[] GetBaseActionArray()
    // {
    //     return baseActionArray;
    // }

    // public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    // {
    //     if (CanSpendActionPointsToTakeAction(baseAction))
    //     {
    //         SpendActionPoints(baseAction.GetActionPointsCost());
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    //
    // }


    // public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    // {
    //     if (actionPoints >= baseAction.GetActionPointsCost())
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // beacuse this callback is triggered by everyTurn Changed, so there is a situation that when player end turn end the player's action got reseted
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPositioin(gridPosition, this);
        Destroy(gameObject);

        OnAnyUnitDead(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
}