public class VFXExplosionPooling : ObjectPoolBase 
{
    public static VFXExplosionPooling SharedInstance;
    public PoolingConfig Config;

    void Awake() 
    {
        SharedInstance = this;
        SetPoolingAmount(Config.VFXExplosionPrefabs, Config.VFXExplosionAmount);
    }
}
