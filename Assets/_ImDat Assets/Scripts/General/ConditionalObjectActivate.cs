using UnityEngine;

public class ConditionalObjectActivate : MonoBehaviour
{
    public bool allowActivate;
    public GameObject subject;

    public void ActivateSubject()
    {
        if (allowActivate)
        {
            subject.SetActive(true);
        }

    }

    public void ActivateSubject(bool newAllow)
    {
        ActivateSubject();
        allowActivate = newAllow;
    }
}
