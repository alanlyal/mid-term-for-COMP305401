using UnityEngine;

public class PowerUp : MonoBehaviour, IItem
{
    [Header("Gun List")]
    public GunStats[] possibleGuns;

    public void Collect() { }

    public void CollectPowerUp(PlayerGun playerGun)
    {
        if (possibleGuns.Length > 0)
        {
            int rand = Random.Range(0, possibleGuns.Length);
            GunStats selectedGun = possibleGuns[rand];

            playerGun.SwapGun(selectedGun);
            Destroy(gameObject);
        }
    }
}
