public class VFXMuzzlePooling : ObjectPoolBase 
{
    public static VFXMuzzlePooling SharedInstance;
    public PoolingConfig Config;

    void Awake() 
    {
        SharedInstance = this;
        SetPoolingAmount(Config.VFXMuzzlePrefabs, Config.VFXMuzzleAmount);
    }
}
