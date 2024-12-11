using System.Collections;

public interface ICharacter 
{
    void InitCharacter();

    void DeadCharacter(bool bScored = false);

    int GetHealth();

    IEnumerator SpawnProjectiles();
}
