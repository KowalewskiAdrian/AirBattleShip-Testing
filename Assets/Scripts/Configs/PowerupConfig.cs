using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupConfig", menuName = "Configurations/PowerupConfig")]
public class PowerupConfig : ScriptableObject
{
    [SerializeField] List<GameObject> _lstPowerUpPrefabs;

    public List<GameObject> PowerUpPrefabList { get { return _lstPowerUpPrefabs; } }
}