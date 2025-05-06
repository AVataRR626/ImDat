using UnityEngine;

public class InvisibleOnStart : MonoBehaviour
{
    public MeshRenderer myRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(myRenderer == null)
            myRenderer = GetComponent<MeshRenderer>();

        if(myRenderer != null)
            myRenderer.enabled = false;
    }

}
