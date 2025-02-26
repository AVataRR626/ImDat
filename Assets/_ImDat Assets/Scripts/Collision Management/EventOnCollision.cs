using UnityEngine;
using UnityEngine.Events;

public class EventOnCollision : MonoBehaviour
{
    public UnityEvent onCollide;

    public void OnCollisionEnter(Collision collision)
    {

        Debug.Log(name + " EventOnCollision:OnCollisionEnter() " + collision.gameObject.name);
        onCollide.Invoke();
    }

}
