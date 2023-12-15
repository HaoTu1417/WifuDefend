using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButtonUI : MonoBehaviour
{

    [SerializeField] private Unit unitPrefab;

    public void OnSpawnButtonClicked()
    {
        UnitSystem.Instance.AHandleSpawnUnitAtMousePosition(unitPrefab);
    }
}
