using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyCharacter : MonoBehaviour, ICharacter, IDamageable 
{
    public UnityAction<bool, bool> actionEnemyDead;

    [SerializeField] GameConfig gameConfig;
    [SerializeField] EnemyConfig enemyConfig;

    private Rigidbody _rigidbody = null;

    private int _health = 2;
    private int _damage = 1;
    private bool _canFire = false;
    private bool _canEscap = false;

    private float oscillationAmplitude;
    private float oscillationFrequency;
    private float oscillationInitialX;


    void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start() 
    {
        StartCoroutine(SpawnProjectiles());
    }

    void Update() 
    {
        
    }

    void FixedUpdate()
    {
        MoveEnemy();
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PlayerProjectile":
                int projectileDamage = other.GetComponent<IDamageable>().GetDamage();
                TakeDamage(projectileDamage);
                other.GetComponent<IDamageable>().TakeDamage(_damage);
                break;
        }
    }


    public void SpawnEnemy(int health, bool canFire, bool canEscape)
    {
        _health = health;
        _canFire = canFire;
        _canEscap = canEscape;

        InitCharacter();
    }

    void MoveEnemy() 
    {
        Vector3 targetPosition = _rigidbody.position;

        if (_canEscap) 
        {
            targetPosition.x = oscillationInitialX + Mathf.Sin(Time.time * oscillationFrequency) * oscillationAmplitude;
        }

        targetPosition += Vector3.down * (enemyConfig.MoveSpeed * Time.deltaTime);
        _rigidbody.MovePosition(targetPosition);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 360f, 0) * Time.fixedDeltaTime * 0.2f);
        _rigidbody.MoveRotation(_rigidbody.rotation * targetRotation);


        if (targetPosition.y < gameConfig.AliveLimitDownHeight) 
        {
            DeadCharacter();
        }
    }

    #region ICharacter
    public void InitCharacter()
    {
        if (_canEscap && Camera.main != null)
        {
            oscillationInitialX = _rigidbody.position.x;
            oscillationAmplitude = Random.Range(0, Camera.main.orthographicSize * Camera.main.aspect * 4 / 3);
            oscillationFrequency = Random.Range(0.7f, 1.7f);
        }

        gameObject.SetActive(true);
    }

    public void DeadCharacter(bool bScored = false)
    {
        _canFire = false;
        gameObject.SetActive(false);

        if (bScored)
            actionEnemyDead?.Invoke(_canFire, _canEscap);
    }

    public int GetHealth()
    {
        return _health;
    }

    public IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            if (_canFire)
            {
                GameObject objectEnemyProjectile = EnemyProjectilePooling.SharedInstance.GetPooledObject();
                if (objectEnemyProjectile != null)
                {
                    objectEnemyProjectile.transform.position = transform.position;
                    objectEnemyProjectile.GetComponent<Projectile>().FireProjectile(3.0f);
                }
            }

            yield return new WaitForSeconds(enemyConfig.FireSpeed);
        }
    }
    #endregion


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
        _health -= damage;

        GameObject objectVFXOnHit = VFXOnHitPooling.SharedInstance.GetPooledObject();
        if (objectVFXOnHit != null)
        {
            objectVFXOnHit.transform.position = transform.position;
            objectVFXOnHit.SetActive(true);
        }

        if (_health < 1)
        {
            GameObject objectVFXExplosion = VFXExplosionPooling.SharedInstance.GetPooledObject();
            if (objectVFXExplosion != null)
            {
                objectVFXExplosion.transform.position = transform.position;
                objectVFXExplosion.SetActive(true);
            }

            DeadCharacter(true);
        }

        AudioController.SharedInstance.PlayEffect(AudioController.SharedInstance.DamageSound);
    }
    #endregion
}
