using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configurations/EnemyConfig")]
public class EnemyConfig : ScriptableObject 
{
    [SerializeField] float _moveSpeed = 2.0f;
    [SerializeField] float _fireSpeed = 2.5f;


    public float MoveSpeed { get { return _moveSpeed; } }
    public float FireSpeed { get { return _fireSpeed; } }
}
