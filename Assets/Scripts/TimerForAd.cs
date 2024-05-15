using System;
using UnityEngine;

public class TimerForAd : MonoBehaviour
{
    public static TimerForAd Instance;

    public float time;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }
}