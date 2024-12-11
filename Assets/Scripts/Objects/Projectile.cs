using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    [SerializeField] GameConfig gameConfig;

    [SerializeField] Vector3 _direction = Vector3.up;

    private int _damage = 1;
    private float _speed = 1.0f;


    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        MoveProjectile();
    }


    void MoveProjectile()
    {
        Vector3 targetPosision = transform.position;

        targetPosision += _direction * (_speed * Time.deltaTime);
        transform.position = targetPosision;

        if (targetPosision.y < gameConfig.AliveLimitDownHeight || targetPosision.y > gameConfig.AliveLimitTopHeight)
        {
            HideProjectile();
        }
    }

    public void FireProjectile(float speed = 2.0f, int damage = 1)
    {
        _speed = speed;
        _damage = damage;

        gameObject.SetActive(true);
    }

    public void HideProjectile()
    {
        if (GetComponent<TrailRenderer>() != null)
            GetComponent<TrailRenderer>().Clear();

        gameObject.SetActive(false);
    }


    #region IDamageable
    public int GetDamage()
    {
        return _damage;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void TakeDamage(int damage)
    {
        HideProjectile();
    }
    #endregion
}
