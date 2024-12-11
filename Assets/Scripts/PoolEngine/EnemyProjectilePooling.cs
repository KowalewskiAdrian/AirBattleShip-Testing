public class EnemyProjectilePooling : ObjectPoolBase 
{
    public static EnemyProjectilePooling SharedInstance;
    public PoolingConfig Config;

    void Awake() 
    {
        SharedInstance = this;
        SetPoolingAmount(Config.EnemyProjectilePrefabs, Config.EnemyCharacterAmount);
    }
}
