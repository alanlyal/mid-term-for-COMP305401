using UnityEngine;

public class PowerUp : MonoBehaviour, IItem
{
    public void Collect()
    {
        Destroy(gameObject);
    }
}
