using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class DelHammer : MonoBehaviour
{
    public enum DelHammerMode { master, client};
    public DelHammerMode mode = DelHammerMode.master;
    public float eventDelay = 0.25f;
    public UnityEvent onHit;
    public UnityEvent onClientHit;
    public UnityEvent onMasterHit;

    public bool armed = true;

    public void Arm(bool newState)
    {
        armed = newState;
    }


    public void OnCollisionEnter(Collision collision)
    {
        DelHammer otherHammer = collision.gameObject.GetComponent<DelHammer>();

        if (otherHammer != null)
        {
            IEnumerator coroutine;
            coroutine = DelayedEvent(otherHammer, eventDelay);

            if(isActiveAndEnabled)
                StartCoroutine(coroutine);
        }
    }

    public IEnumerator DelayedEvent(DelHammer o, float wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);

            Debug.Log(name + " DelHammer: " + o.name + " | " + mode);

            onHit.Invoke();

            if (mode == DelHammerMode.master)
            {
                onMasterHit.Invoke();

                //only master hammers can trigger client hammers;

                if (o.mode == DelHammerMode.client)
                    o.onClientHit.Invoke();
            }

            break;
        }
    }
}
