using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timeTillDead;
    private void Awake() {
        Destroy(gameObject, timeTillDead);
    }
}
