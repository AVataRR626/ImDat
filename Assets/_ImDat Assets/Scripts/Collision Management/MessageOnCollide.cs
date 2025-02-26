using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MessageOnCollide : MonoBehaviour
{
    public string message;
    public float messageDelay = 0.5f;
    public List<string> excludeTags;
    public bool armed = true;    

    public void Arm(bool newState)
    {
        armed = newState;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + "MessageOnCollide(): disableMode:");
        if (armed)
        {
            if (!excludeTags.Contains(collision.collider.tag))
            {
                IEnumerator coroutine;
                coroutine = DelayedMessage(collision.gameObject, messageDelay);
                StartCoroutine(coroutine);
            }
        }
    }

    public IEnumerator DelayedMessage(GameObject o, float wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);

            Debug.Log(name + " DelayedMessage(): " + o.name + ": " + message);
            o.SendMessage(message, SendMessageOptions.DontRequireReceiver);

            break;
        }
    }
}
