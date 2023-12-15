using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Tower", order = 1)]
public class Tower : ScriptableObject
{
    public string prefabName;

    public float Damage;

    public float FireRate;

    public int Radius;
}