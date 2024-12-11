public class VFXOnPickUpPooling : ObjectPoolBase 
{
    public static VFXOnPickUpPooling SharedInstance;
    public PoolingConfig Config;

    void Awake() 
    {
        SharedInstance = this;
        SetPoolingAmount(Config.VFXOnPickupPrefabs, Config.VFXOnPickupAmount);
    }
}