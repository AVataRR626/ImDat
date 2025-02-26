using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnCollide : MonoBehaviour
{
    public float disableDelay = 0.5f;
    public List<string> excludeTags;

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + " DisableOnCollide: ");

        if (!excludeTags.Contains(collision.collider.tag))
        {
            IEnumerator coroutine;
            coroutine = DisableDelayed(collision.gameObject, disableDelay);
            StartCoroutine(coroutine);
        }
    }

    public IEnumerator DisableDelayed(GameObject o, float wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);
            o.SetActive(false);

            break;
        }
    }
}
