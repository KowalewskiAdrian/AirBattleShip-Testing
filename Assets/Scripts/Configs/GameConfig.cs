using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configurations/GameConfig")]
public class GameConfig : ScriptableObject 
{
    [SerializeField] int _playerBaseHealth = 3;

    [SerializeField] int _enemyBaseHealth = 2;
    [SerializeField] float _enemySpawnSpeed = 1.25f;

    [SerializeField] Vector3 _enemySpawnPosition = new Vector3(0, 16, 0);
    [SerializeField] Vector3 _enemySpawnOffsets = new Vector3(2.8f, 0, 0);

    [SerializeField] float _aliveLimitDownHeight = -3.0f;
    [SerializeField] float _aliveLimitTopHeight = 17.0f;


    public int PlayerBaseHealth { get { return _playerBaseHealth; } }

    public int EnemyBaseHealth { get { return _enemyBaseHealth; } }
    public float EnemySpawnSpeed { get { return _enemySpawnSpeed; } }

    public Vector3 EnemySpawnPosition { get { return _enemySpawnPosition; } }
    public Vector3 EnemySpawnOffsets { get { return _enemySpawnOffsets; } }

    public float AliveLimitDownHeight { get { return _aliveLimitDownHeight; } }
    public float AliveLimitTopHeight { get { return _aliveLimitTopHeight; } }
    
}
