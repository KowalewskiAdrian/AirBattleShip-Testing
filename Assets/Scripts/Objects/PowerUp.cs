using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class Powerup : MonoBehaviour 
{
    public enum PowerupType
    {
        NORMAL = 0,
        DAMAGE = 1,
        HEALTH = 2,
        SHIELD = 3,
        SPEED = 4,
        TRIPLE = 5,
    }

    [SerializeField] private PowerupType _type;
    [SerializeField] private float _coolTime;


    public PowerupType Type { get { return _type; } }
    public float CoolTime { get { return _coolTime; } }
}
