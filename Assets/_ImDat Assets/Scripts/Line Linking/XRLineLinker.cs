using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections.Generic;
using UnityEngine.UIElements;

[RequireComponent(typeof(XRGrabInteractable))]
public class XRLineLinker : MonoBehaviour
{
    public enum State { NoLinks, OneLink, TwoLinks }

    [Header("Main Settings")]    
    public State state;
    public Transform source;
    public Transform destination;

    [Header("Graphics Settings")]
    public Material connectedMaterial;
    public Material disconnectedMaterial;
    public LineRendererLink myLine;
    public List<Renderer> myRenderers;

    [Header("System Stuff (usually don't touch)")]
    public Quaternion startRot;

    
    void Start()
    {
        Reset();
        SyncLineRenderer();
        myLine.transform.parent = null;

        if (myRenderers == null)
        {
            myRenderers = new List<Renderer>();
            myRenderers.Add(GetComponent<Renderer>());
        }

        startRot = transform.rotation;
    }

    private void OnDestroy()
    {
        if (myLine != null)
        {
            Destroy(myLine.gameObject);
        }
    }

    private void OnDisable()
    {
        if (myLine != null)
        {
            myLine.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        SyncLineRenderer();
        SyncPosition();
        SyncMaterials();
    }


    private void OnCollisionEnter(Collision collision)
    {
        XRLineLinker xrll = collision.gameObject.GetComponent<XRLineLinker>();

        if (xrll == null)
        {
            if (source == null)
            {
                source = collision.transform;
                state = State.OneLink;
            }
            else if (source != null)
            {
                if (destination == null)
                {
                    if (source != collision.transform)
                    {
                        destination = collision.transform;
                        state = State.TwoLinks;
                    }
                }
            }
        }
    }

    public void SyncLineRenderer()
    {
        if (myLine != null)
        {
            //the same source can't be the destination!
            if (source == destination)
                destination = null;

            if (source != null)
                myLine.transform.position = source.position;
            else
                myLine.transform.position = transform.position;

            if (destination != null)
                myLine.linkPoint = destination;
            else
                myLine.linkPoint = transform;
        }

        if (source == null)
        {
            state = State.NoLinks;
        }
        else if (source != null)
        {
            state = State.OneLink;

            if (destination != null)
            {
                state = State.TwoLinks;
            }
        }
    }

    public void SyncPosition()
    {
        if (state == State.TwoLinks)
        {
            if (source != null && destination != null)
            {
                transform.position = (source.position + destination.position) / 2;
                transform.LookAt(source.position);
            }
        }

        if (state == State.NoLinks)
        {
            transform.rotation = startRot;
        }
    }

    public void SyncMaterials()
    {
        if(state == State.NoLinks || state == State.OneLink)
        {
            SyncMaterials(disconnectedMaterial);
        }

        if(state == State.TwoLinks)
        {
            SyncMaterials(connectedMaterial);
        }
    }

    public void SyncMaterials(Material newMaterial)
    {
        foreach(Renderer r in myRenderers)
            r.material = newMaterial;
    }

    public void Reset()
    {
        state = State.NoLinks;
        source = null;
        destination = null;
    }
}
