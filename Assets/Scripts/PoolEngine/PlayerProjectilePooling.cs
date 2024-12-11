public class PlayerProjectilePooling : ObjectPoolBase 
{
    public static PlayerProjectilePooling SharedInstance;
    public PoolingConfig Config;

    void Awake() 
    {
        SharedInstance = this;
        SetPoolingAmount(Config.PlayerProjectilePrefabs, Config.PlayerProjectileAmount);
    }
}
