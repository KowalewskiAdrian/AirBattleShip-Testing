public class EnemyCharacterPooling : ObjectPoolBase 
{
    public static EnemyCharacterPooling SharedInstance;
    public PoolingConfig Config;

    void Awake() 
    {
        SharedInstance = this;
        SetPoolingAmount(Config.EnemyCharacterPrefabs, Config.EnemyCharacterAmount);
    }
}
