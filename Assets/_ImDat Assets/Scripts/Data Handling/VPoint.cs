/*
 * ImDat - Immersive Data Visualisation 
 *  
 * The atomic data "pixel!" Da-El?
 * 
 * - Matt Cabanag, 24 Apr 2024
 *  
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPoint : MonoBehaviour
{
    public Vector3 rotationSpeed;
    public Vector3 oscilationSpeed;
    public Color primaryColour;
    public GameObject primaryGraphics;

    [Header("System Stuff [Dont Touch! (usually)]")]
    public string albedoKey;
    public Renderer primaryRenderer;

    // Start is called before the first frame update
    void Start()
    {
        primaryRenderer = primaryGraphics.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Instanced shader value changing
        //myRenderer.sharedMaterial.SetFloat(highlightKey, highlightValue);
        primaryRenderer.material.SetColor(albedoKey, primaryColour);

    }
}
