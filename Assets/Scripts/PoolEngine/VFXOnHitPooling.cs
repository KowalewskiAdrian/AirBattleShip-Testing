public class VFXOnHitPooling : ObjectPoolBase 
{
    public static VFXOnHitPooling SharedInstance;
    public PoolingConfig Config;

    void Awake() 
    {
        SharedInstance = this;
        SetPoolingAmount(Config.VFXOnHitPrefabs, Config.VFXOnHitAmount);
    }
}
