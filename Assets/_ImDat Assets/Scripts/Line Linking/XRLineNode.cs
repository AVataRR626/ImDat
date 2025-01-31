using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(GridSnapper))]
public class XRLineNode : MonoBehaviour
{
    public XRLineLinker lineLinkTemplate;
    public List<XRLineLinker> lineLinks;
}
