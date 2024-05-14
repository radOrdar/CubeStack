using System.Collections;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float delay;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}