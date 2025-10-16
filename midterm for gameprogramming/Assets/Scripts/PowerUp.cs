using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GunStats newGunStats;    // Assign a GunStats asset in the Inspector
    public float destroyDelay = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerGun playerGun = collision.GetComponent<PlayerGun>();
            if (playerGun != null && newGunStats != null)
            {
                Debug.Log("Power-up picked up!");
                playerGun.stats = newGunStats;
            }

            Destroy(gameObject, destroyDelay);
        }
    }
}
