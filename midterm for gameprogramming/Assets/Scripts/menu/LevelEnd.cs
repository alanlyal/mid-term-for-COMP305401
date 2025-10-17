using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public static event Action LoadLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LoadLevel.Invoke();
    }
}
