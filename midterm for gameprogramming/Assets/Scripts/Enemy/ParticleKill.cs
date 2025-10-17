using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Kill());
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
