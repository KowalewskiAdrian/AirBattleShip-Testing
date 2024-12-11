using PrimeTween;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour 
{
    [SerializeField] PlayerCharacter _playerCharacter;
    [SerializeField] GamePlayPage _pageGamePlay;
    [SerializeField] GameOverPage _pageGameOver;

    [SerializeField] GameConfig gameConfig;
    [SerializeField] PowerupConfig powerupConfig;

    private float _startTime = 0;
    private int _highScore = 0;
    private int _playScore = 0;


    void Awake() 
    {
        Application.targetFrameRate = 60;
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
    }

    void Start() 
    {
        _playerCharacter.actionPlayerDead += OnPlayerDead;

        _highScore = PlayerPrefs.GetInt("HighScore", 0);

        StartCoroutine(SpawnEnemies());

        InitPlayer();

        AudioController.SharedInstance.PlayBackgroundMusic();
    }

    void Update()
    {
    }



    void InitPlayer() 
    {
        _startTime = Time.time;
        Time.timeScale = 1.0f;

        _playScore = 0;
        _playerCharacter.InitCharacter();
    }

    IEnumerator SpawnEnemies() 
    {
        while (true) 
        {
            GameObject objectEnemyCharacter = EnemyCharacterPooling.SharedInstance.GetPooledObject();
            if (objectEnemyCharacter != null) 
            {
                objectEnemyCharacter.transform.position = gameConfig.EnemySpawnPosition + new Vector3(
                    Random.Range(-gameConfig.EnemySpawnOffsets.x, gameConfig.EnemySpawnOffsets.x),
                    Random.Range(-gameConfig.EnemySpawnOffsets.y, gameConfig.EnemySpawnOffsets.y),
                    0.0f);

                EnemyCharacter enemyCharacter = objectEnemyCharacter.GetComponent<EnemyCharacter>();
                if (enemyCharacter != null) 
                {
                    enemyCharacter.actionEnemyDead = null;
                    enemyCharacter.actionEnemyDead += OnEnemyDead;
                    
                    bool canFire = Random.value < 0.4f;
                    bool canEscap = Random.value < 0.4f;
                    int health = gameConfig.EnemyBaseHealth + Mathf.Min(Mathf.FloorToInt((Time.time - _startTime) / 15f), 5);

                    enemyCharacter.SpawnEnemy(health, canFire, canEscap);
                }
            }

            yield return new WaitForSeconds(gameConfig.EnemySpawnSpeed);
        }
    }

    void OnPlayerDead() 
    {
        Time.timeScale = 0.0f;
        bool congratulations = false;

        if (_playScore > _highScore) 
        {
            PlayerPrefs.SetInt("HighScore", _playScore);
            PlayerPrefs.Save();

            AudioController.SharedInstance.PlayEffect(AudioController.SharedInstance.GameOverSoundWithHighScore);
            congratulations = true;
        }
        else
        {
            AudioController.SharedInstance.PlayEffect(AudioController.SharedInstance.GameOverSoundWithLowScore);
            congratulations = false;
        }

        _pageGamePlay.gameObject.SetActive(false);
        _pageGameOver.gameObject.SetActive(true);
        _pageGameOver.ShowGameOver(_playScore, _highScore, congratulations);
    }

    Powerup.PowerupType GetPowerupTypeOfEnemyKind(bool canFire, bool canEscap)
    {
        if(canFire && canEscap)
            return Powerup.PowerupType.TRIPLE;

        if(canFire)
            return Powerup.PowerupType.DAMAGE;

        if(canEscap)
            return Powerup.PowerupType.SHIELD;

        if (_playerCharacter.GetHealth() < 3)
            return Powerup.PowerupType.HEALTH;

        if (Random.value < 0.3f)
            return Powerup.PowerupType.NORMAL;

        return Powerup.PowerupType.SPEED;
    }

    void OnEnemyDead(bool canFire, bool CanEscape) 
    {
        _playScore++;
        _pageGamePlay.ShowPlayScore(_playScore);

        AudioController.SharedInstance.PlayEffect(AudioController.SharedInstance.DestroyEnemySound);

        if (Random.value < 0.7f)
            return;

        Powerup.PowerupType type = GetPowerupTypeOfEnemyKind(canFire, CanEscape);
        SpawnPowerupProjectile(type);
    }

    void SpawnPowerupProjectile(Powerup.PowerupType type)
    {
        GameObject objPowerUpProjectile = PlayerPowerupPooling.SharedInstance.GetPooledObject(type);
        if (objPowerUpProjectile != null)
        {
            objPowerUpProjectile.transform.position = new Vector3(Random.Range(0, 3), 17.0f, 0.0f);
            objPowerUpProjectile.SetActive(true);
        }
    }

}
