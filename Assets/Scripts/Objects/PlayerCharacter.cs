
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Powerup;
using Random = UnityEngine.Random;

public class PlayerCharacter : MonoBehaviour, ICharacter, IDamageable
{
    public UnityAction actionPlayerDead;
    public UnityAction<int> actionTakeHit;

    [SerializeField] GameConfig gameConfig;
    [SerializeField] PlayerConfig playerConfig;

    [SerializeField] RectTransform _stickParent;
    [SerializeField] Transform _projectileSpawnLocationC;
    [SerializeField] Transform _projectileSpawnLocationL;
    [SerializeField] Transform _projectileSpawnLocationR;
    [SerializeField] Transform _objAirShip;
    [SerializeField] GameObject _objBackTurret;
    [SerializeField] MeshRenderer _mshRenderBody;

    private Rigidbody _rigidbody = null;

    private int _health = 3;
    private int _damage = 1;
    private bool _hasInput = false;
    private bool _isTriplet = false;
    private bool _isInvincible = false;
    private float _fireSpeed = 0.4f;

    private Quaternion _baseRotation;
    private Vector3 _targetPosition;

    private Dictionary<PowerupType, float> _powerupBuffers;


    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start() 
    {
        StartCoroutine(SpawnProjectiles());
    }

    void Update()
    {
        CheckPowerupBuffers();
    }

    void FixedUpdate()
    {
        RotateAirShip();
    }

    void OnTriggerEnter(Collider other)
    {
        int damage = 0;
        switch (other.tag)
        {
            case "Enemy":
                damage = other.GetComponent<IDamageable>().GetDamage();
                TakeDamage(damage);
                other.GetComponent<ICharacter>().DeadCharacter();
                break;

            case "EnemyProjectile":
                damage = other.GetComponent<IDamageable>().GetDamage();
                TakeDamage(damage);
                other.GetComponent<IDamageable>().TakeDamage(_damage);
                break;

            case "PlayerPowerUp":
                AddPowerUp(other.GetComponent<Powerup>());
                other.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }


    #region InputEvent Part
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.ReadValue<Vector2>();
        Vector3 stickPosition = _stickParent.InverseTransformPoint(touchPosition);
        _targetPosition = new Vector3(
            stickPosition.x / Screen.width * _stickParent.rect.width / 100.0f,
            stickPosition.y / Screen.height * _stickParent.rect.height / 100.0f + 6.5f,
            0);
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _hasInput = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _hasInput = false;
        }
    }
    #endregion



    void RotateAirShip()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);

        float tiltSideways = -localVelocity.x * playerConfig.TiltThreshold;

        if (Mathf.Abs(localVelocity.x) > 0 && Mathf.Abs(localVelocity.x) < playerConfig.StopThreshold)
        {
            tiltSideways = Mathf.Lerp(_objAirShip.eulerAngles.z, 180, Time.deltaTime * playerConfig.TiltSpeed);
        }

        Quaternion targetRotation = _baseRotation * Quaternion.Euler(0, 0, tiltSideways);

        _objAirShip.rotation = Quaternion.Lerp(_objAirShip.rotation, targetRotation, Time.deltaTime * playerConfig.TiltSpeed);

        Tween.RigidbodyMovePosition(_rigidbody, _targetPosition, playerConfig.MoveSpeed, Ease.OutQuad, 1, CycleMode.Yoyo);
    }

    void LaunchProjectiles(Transform spawnLocation)
    {
        GameObject objectPlayerProjectile = PlayerProjectilePooling.SharedInstance.GetPooledObject();
        if (objectPlayerProjectile != null)
        {
            objectPlayerProjectile.transform.position = spawnLocation.position;
            objectPlayerProjectile.GetComponent<Projectile>().FireProjectile(5.0f);
        }

        GameObject objectVFXMuzzle = VFXMuzzlePooling.SharedInstance.GetPooledObject();
        if (objectVFXMuzzle != null)
        {
            objectVFXMuzzle.transform.position = spawnLocation.position;
            objectVFXMuzzle.SetActive(true);
        }
    }


    #region Pickup Part
    void AddPowerUp(Powerup powerUp)
    {
        if (powerUp == null)
            return;

        _powerupBuffers[powerUp.Type] = Time.time + powerUp.CoolTime;

        switch (powerUp.Type)
        {
            case PowerupType.NORMAL:
                _health = 3;
                actionTakeHit?.Invoke(_health);
                break;

            case PowerupType.DAMAGE:
                _mshRenderBody.material.color = Color.red;
                _damage *= 2;
                break;

            case PowerupType.HEALTH:
                if (_health < 3)
                    _health++;

                actionTakeHit?.Invoke(_health);
                break;

            case PowerupType.TRIPLE:
                _isTriplet = true;
                _objBackTurret.SetActive(true);
                break;

            case PowerupType.SHIELD:
                _mshRenderBody.material.color = Color.clear;
                _isInvincible = true;
                break;

            case PowerupType.SPEED:
                _mshRenderBody.material.color = Color.green;
                _fireSpeed = playerConfig.FireSpeed * 0.8f;
                break;
        }

        GameObject objVFXOnPickUp = VFXOnPickUpPooling.SharedInstance.GetPooledObject();
        if (objVFXOnPickUp != null)
        {
            objVFXOnPickUp.transform.position = transform.position;
            objVFXOnPickUp.SetActive(true);
        }

        AudioController.SharedInstance.PlayEffect(AudioController.SharedInstance.HealingPickupSound);
    }

    Color GetModifiableColor(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.DAMAGE:
                return Color.red;

            case PowerupType.SHIELD:
                return Color.clear;

            case PowerupType.SPEED:
                return Color.green;

            default:
                return Color.white;
        }
    }

    void ClearBuffer(PowerupType type)
    {
        _powerupBuffers[type] = 0;

        switch (type)
        {
            case PowerupType.DAMAGE:
                _damage = playerConfig.FireDamage;
                break;

            case PowerupType.SHIELD:
                _isInvincible = false;
                break;

            case PowerupType.SPEED:
                _fireSpeed = playerConfig.FireSpeed;
                break;

            case PowerupType.TRIPLE:
                _isTriplet = false;
                _objBackTurret.SetActive(false);
                break;
        }
    }

    void CheckPowerupBuffers()
    {
        List<PowerupType> types = Enum.GetValues(typeof(PowerupType)).Cast<PowerupType>().ToList();
        Color remainColor = Color.white;
        bool isChanged = false;

        foreach (var type in types)
        {
            float coolTime;
            if (_powerupBuffers.TryGetValue(type, out coolTime) == false)
                continue;

            if (coolTime <= 0)
                continue;

            if (coolTime > Time.time)
            {
                Color buffColor = GetModifiableColor(type);
                if (buffColor != Color.white)
                    remainColor = buffColor;

                continue;
            }

            ClearBuffer(type);
            isChanged = true;
        }

        if (isChanged)
            _mshRenderBody.material.color = remainColor;
    }
    #endregion


    #region ICharacter
    public void InitCharacter() 
    {
        _health = gameConfig.PlayerBaseHealth;
        _damage = playerConfig.FireDamage;
        _fireSpeed = playerConfig.FireSpeed;

        _powerupBuffers = new Dictionary<PowerupType, float>();
        List<PowerupType> types = Enum.GetValues(typeof(PowerupType)).Cast<PowerupType>().ToList();
        foreach (var type in types)
        {
            _powerupBuffers.TryAdd(type, 0);
        }

        transform.position = Vector3.zero;
        _targetPosition = transform.position;
        if (_objAirShip)
            _baseRotation = _objAirShip.localRotation;

        gameObject.SetActive(true);
    }

    public void DeadCharacter(bool bScored = false)
    {
        Destroy(gameObject);
        actionPlayerDead?.Invoke();
    }

    public int GetHealth()
    {
        return _health;
    }

    public IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            if (_hasInput == true)
            {
                LaunchProjectiles(_projectileSpawnLocationC);

                if (_isTriplet)
                {
                    LaunchProjectiles(_projectileSpawnLocationL);
                    LaunchProjectiles(_projectileSpawnLocationR);
                }
            }

            yield return new WaitForSeconds(_fireSpeed);
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
        GameObject objVFXOnHit = VFXOnHitPooling.SharedInstance.GetPooledObject();
        if (objVFXOnHit != null)
        {
            objVFXOnHit.transform.position = transform.position;
            objVFXOnHit.SetActive(true);
        }

        if (_isInvincible)
            return;

        _health -= damage;

        if (_health <= 0)
        {
            GameObject objVFXExplosion = VFXExplosionPooling.SharedInstance.GetPooledObject();
            if (objVFXExplosion != null)
            {
                objVFXExplosion.transform.position = transform.position;
                objVFXExplosion.SetActive(true);
            }

            DeadCharacter();
            return;
        }

        actionTakeHit?.Invoke(_health);
    }
    #endregion
}