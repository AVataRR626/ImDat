using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
    public float disableDelay = 0.5f;
    public List<string> excludeTags;

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + " DeleteOnCollide: ");

        if (!excludeTags.Contains(collision.collider.tag))
        {
            IEnumerator coroutine;
            coroutine = DestroyDelayed(collision.gameObject, disableDelay);
            StartCoroutine(coroutine);
        }
    }

    public IEnumerator DestroyDelayed(GameObject o, float wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);
            Destroy(o);
        }
    }
}
