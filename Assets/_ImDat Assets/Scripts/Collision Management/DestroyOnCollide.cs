using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
    public float disableDelay = 0.5f;
    public List<string> excludeTags;
    public bool armed = true;
    public bool disableMode = true;

    public void Arm(bool newState)
    {
        armed = newState;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + " DeleteOnCollide(): disableMode:" + disableMode);
        if (armed)
        {
            if (!excludeTags.Contains(collision.collider.tag))
            {
                IEnumerator coroutine;
                coroutine = DestroyDelayed(collision.gameObject, disableDelay);
                StartCoroutine(coroutine);
            }
        }
    }

    public IEnumerator DestroyDelayed(GameObject o, float wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);

            if(disableMode)
                o.SetActive(false);
            else
                Destroy(o);

            break;
        }
    }
}
