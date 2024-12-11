using UnityEngine;

[CreateAssetMenu(fileName = "PoolingConfig", menuName = "Configurations/PoolingConfig")]
public class PoolingConfig : ScriptableObject 
{
    [SerializeField] GameObject _playerProjectilePrefabs;
    [SerializeField] int _playerProjectileAmount = 5;


    [SerializeField] GameObject _enemyCharacterPrefabs;
    [SerializeField] int _enemyCharacterAmount = 10;

    [SerializeField] GameObject _enemyProjectilePrefabs;
    [SerializeField] int _enemyProjectileAmount = 5;


    [SerializeField] GameObject _vfxMuzzlePrefabs;
    [SerializeField] int _vfxMuzzleAmount = 2;

    [SerializeField] GameObject _vfxOnHitPrefabs;
    [SerializeField] int _vfxOnHitAmount = 2;

    [SerializeField] GameObject _vfxExplosionPrefabs;
    [SerializeField] int _vfxExplosionAmount = 5;

    [SerializeField] GameObject _vfxOnPickupPrefabs;
    [SerializeField] int _vfxOnPickupAmount = 2;



    public GameObject PlayerProjectilePrefabs { get { return _playerProjectilePrefabs; } }
    public int PlayerProjectileAmount { get { return _playerProjectileAmount; } }


    public GameObject EnemyCharacterPrefabs { get { return _enemyCharacterPrefabs; } }
    public int EnemyCharacterAmount { get { return _enemyCharacterAmount; } }

    public GameObject EnemyProjectilePrefabs { get { return _enemyProjectilePrefabs; } }
    public int EnemyProjectileAmount { get { return _enemyProjectileAmount; } }


    public GameObject VFXMuzzlePrefabs { get { return _vfxMuzzlePrefabs; } }
    public int VFXMuzzleAmount { get { return _vfxMuzzleAmount; } }

    public GameObject VFXOnHitPrefabs { get { return _vfxOnHitPrefabs; } }
    public int VFXOnHitAmount { get { return _vfxOnHitAmount; } }

    public GameObject VFXExplosionPrefabs { get { return _vfxExplosionPrefabs; } }
    public int VFXExplosionAmount { get { return _vfxExplosionAmount; } }

    public GameObject VFXOnPickupPrefabs { get { return _vfxOnPickupPrefabs; } }
    public int VFXOnPickupAmount { get { return _vfxOnPickupAmount; } }
}
