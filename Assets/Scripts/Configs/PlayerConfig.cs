using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configurations/PlayerConfig")]
public class PlayerConfig : ScriptableObject 
{
    [SerializeField] int _hitDamage = 5;
    [SerializeField] int _fireDamage = 1;

    [SerializeField] float _moveSpeed = 0.3f;
    [SerializeField] float _fireSpeed = 0.4f;
    [SerializeField] float _tiltSpeed = 5f;

    [SerializeField] float _tiltThreshold = 10f;
    [SerializeField] float _stopThreshold = 0.1f;


    public int HitDamage { get { return _hitDamage; } }
    public int FireDamage { get { return _fireDamage; } }

    public float MoveSpeed { get { return _moveSpeed; } }
    public float FireSpeed { get { return _fireSpeed; } }
    public float TiltSpeed { get { return _tiltSpeed; } }

    public float TiltThreshold { get { return _tiltThreshold; } }
    public float StopThreshold { get { return _stopThreshold; } }
}
