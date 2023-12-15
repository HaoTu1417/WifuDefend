using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    [SerializeField] private bool IsSelected;

    // Update is called once per frame
    void Update()
    {
        if (IsSelected)
        {
            transform.position = LevelGrid.Instance.GetWorldPosition(LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition()));
        }
    }

    public void SetIsSelected(bool status)
    {
        IsSelected = status;
    }
}