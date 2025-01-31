using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class XRLineLinker : MonoBehaviour
{
    public Transform source;
    public Transform destination;

    public LineRendererLink myLine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SyncLineRenderer();

    }

    private void Update()
    {
        SyncLineRenderer();
    }


    private void OnCollisionEnter(Collision collision)
    {
        XRLineNode node = collision.gameObject.GetComponent<XRLineNode>();

        if (node != null)
        {

            if(source != null)
            {

            }

            destination = node.transform;

            SyncLineRenderer();
        }
    }

    public void SyncLineRenderer()
    {
        if (destination != null)
            myLine.linkPoint = destination;
        else
            myLine.linkPoint = transform;

        if(source != null)
            myLine.transform.position = source.position;
        else
            myLine.transform.position = transform.position;
    }
}
