using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Image[] hearts;

    private PlayerHealth playerHealth;



    void Update()
    {
        // Find playerHealth if we haven't already
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth == null)
                    playerHealth = player.GetComponentInChildren<PlayerHealth>();
            }
            if (playerHealth == null)
                return; // still not found
        }

        // Update UI hearts every frame
        health = playerHealth.health;
        maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;
            if (fullHeart == null || emptyHeart == null) continue;

            hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
            hearts[i].enabled = (i < maxHealth);
        }
    }
}
    
