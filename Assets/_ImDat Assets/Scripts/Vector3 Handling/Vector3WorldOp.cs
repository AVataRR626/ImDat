using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Vector3WorldOp : MonoBehaviour
{
    public enum Operation { Add, Sub, Cross, Proj, PlelPip};

    [Header("Basic Settings")]
    public Operation operation;
    public Vector3Relay result;
    public GameObject resultDisplay;
    public int maxSubjects = 2;
    public List<Vector3WorldBase> operands;

    [Header("Events")]
    public UnityEvent onFirstOperand;
    public UnityEvent onSecondOperand;
    public UnityEvent onMergeOperand;

    [Header("System Stuff - Usually Don't Touch")]
    public Vector3WorldBase clonePrefab;
    public Transform clonePoint;
    public bool cloneAllow = false;
    public float spawnClock = 0;
    public List<LineRendererLink> guideLines;


    public void OnDisable()
    {
        foreach (Vector3WorldBase v in operands)
        {
            if (v != null)
            {
                Debug.Log("Disabling: " + v.gameObject.name + " ----");
                v.gameObject.SetActive(false);
            }
        }
    }


    public void OnDestroy()
    {
        foreach (Vector3WorldBase v in operands)
        {
            if (v != null)
            {
                v.gameObject.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (result == null)
            result = GetComponent<Vector3Relay>();
    }
    void FixedUpdate()
    {
        TrackOperands();

        if(operands.Count < 2)
        {
            if(resultDisplay != null)
                resultDisplay.SetActive(false);
        }
        else if(operands.Count == 2)
        {
            if(operation != Operation.PlelPip)
                resultDisplay.SetActive(true);

            if (operation == Operation.Add)
                result.value = operands[0].value.value + operands[1].value.value;

            if(operation == Operation.Sub)
                result.value = operands[0].value.value - operands[1].value.value;

            if (operation == Operation.Cross)
                result.value = Vector3.Cross(operands[0].value.value, operands[1].value.value);

            if (operation == Operation.Proj)
                result.value = Vector3.Project(operands[0].value.value, operands[1].value.value);
        }
        else if (operands.Count == 3)
        {
            if(operation == Operation.PlelPip)
            {

                Vector3 AB = operands[0].value.value + operands[1].value.value;
                Vector3 AC = operands[0].value.value + operands[2].value.value;
                Vector3 BC = operands[1].value.value + operands[2].value.value;
                Vector3 finalCorner = AC + AB - operands[0].value.value;
                
                guideLines[0].linkPoint.localPosition = AC;
                guideLines[1].linkPoint.localPosition = AB;
                guideLines[4].linkPoint.localPosition = AC;
                guideLines[5].linkPoint.localPosition = finalCorner;
                guideLines[6].linkPoint.localPosition = AB;
                guideLines[7].linkPoint.localPosition = BC;
                guideLines[8].linkPoint.localPosition = BC;

            }
        }

        if(spawnClock > 0)
        {
            spawnClock -= Time.fixedDeltaTime;
        }
        else
        {
            spawnClock = 0;
        }
    }

    public void TrackOperands()
    {
        foreach (Vector3WorldBase v in operands)
        {
            v.transform.position = transform.position;

            if (v.trackRotation)
                v.transform.rotation = transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        //Debug.Log("Vector3WorldOps: " + other.name);

        if (operands.Count < maxSubjects)
        {
            Vector3WorldBase wb = other.GetComponent<Vector3WorldBase>();
            if (wb != null)
            {                
                if (!operands.Contains(wb))
                {
                    other.enabled = false;
                    wb.DetachHandle();
                    operands.Add(wb);

                    IRelayReferencePoint rrp = wb.GetComponent<IRelayReferencePoint>();

                    //cascade destroy commands to host operation
                    rrp.GetReferencePoint().GetComponent<DisableSync>().syncObjects.Add(this.gameObject);

                    //---- Handle Operations ----
                    if (operation == Operation.Add)
                    {
                        //align parallelogram graphics
                        

                        if (rrp != null)
                        {
                            guideLines[operands.Count - 1].linkPoint = rrp.GetReferencePoint();
                        }

                    }

                    if(operation == Operation.Sub)
                    {
                        if(operands.Count == 2)
                        {
                            //align direction graphics
                            IRelayReferencePoint[] rrps = new IRelayReferencePoint[2];
                            rrps[0] = operands[0].GetComponent<IRelayReferencePoint>();
                            rrps[1] = operands[1].GetComponent<IRelayReferencePoint>();

                            guideLines[0].linkPoint = rrps[0].GetReferencePoint();
                            guideLines[0].transform.parent = rrps[1].GetReferencePoint();
                            guideLines[0].transform.localPosition = Vector3.zero;
                        }
                        
                    }

                    if(operation == Operation.Proj)
                    {
                        if (operands.Count == 2)
                        {

                            SyncGuidelines_Proj();

                            
                            //align projection graphics
                            //IRelayReferencePoint[] rrp = new IRelayReferencePoint[2];
                            //rrp[0] = resultDisplay.GetComponent<IRelayReferencePoint>();
                            //rrp[1] = operands[0].GetComponent<IRelayReferencePoint>();

                        }
                    }

                    if(operation == Operation.PlelPip)
                    {
                        if (operands.Count == 3)
                        {

                            //align direction graphics
                            IRelayReferencePoint[] rrps = new IRelayReferencePoint[3];

                            for(int i = 0; i < 3; i++)
                                rrps[i] = operands[i].GetComponent<IRelayReferencePoint>();

                            /*
                            rrp[0] = operands[0].GetComponent<IRelayReferencePoint>();
                            rrp[1] = operands[1].GetComponent<IRelayReferencePoint>();
                            rrp[2] = operands[2].GetComponent<IRelayReferencePoint>();
                            */

                            Vector3 AB = operands[0].value.value + operands[1].value.value;
                            Vector3 AC = operands[0].value.value + operands[2].value.value;                            
                            Vector3 BC = operands[1].value.value + operands[2].value.value;
                            Vector3 finalCorner = AC + AB;

                            
                            guideLines[0].transform.parent = rrps[0].GetReferencePoint();
                            guideLines[0].transform.localPosition = Vector3.zero;
                            guideLines[0].linkPoint.localPosition = AC;

                            guideLines[1].transform.parent = rrps[0].GetReferencePoint();
                            guideLines[1].transform.localPosition = Vector3.zero;
                            guideLines[1].linkPoint.localPosition = AB;

                            guideLines[4].transform.parent = rrps[2].GetReferencePoint();
                            guideLines[4].transform.localPosition = Vector3.zero;
                            guideLines[4].linkPoint.localPosition = AC;

                            guideLines[5].linkPoint.localPosition = finalCorner;

                            guideLines[6].transform.parent = rrps[1].GetReferencePoint();
                            guideLines[6].transform.localPosition = Vector3.zero;
                            guideLines[6].linkPoint.localPosition = AB;

                            guideLines[7].transform.parent = rrps[1].GetReferencePoint();
                            guideLines[7].transform.localPosition = Vector3.zero;
                            guideLines[7].linkPoint.localPosition = BC;

                            guideLines[8].transform.parent = rrps[2].GetReferencePoint();
                            guideLines[8].transform.localPosition = Vector3.zero;
                            guideLines[8].linkPoint.localPosition = BC;
                        }
                    }

                    //---- Handle Events ----
                    if(operands.Count == 1)                    
                        onFirstOperand.Invoke();

                    if (operands.Count == 2)
                        onSecondOperand.Invoke();

                    onMergeOperand.Invoke();

                }
            }
        }
        
    }

    public void SyncGuidelines_Proj()
    {
        //align projection graphics
        IRelayReferencePoint[] rrp = new IRelayReferencePoint[2];
        rrp[0] = resultDisplay.GetComponent<IRelayReferencePoint>();
        rrp[1] = operands[0].GetComponent<IRelayReferencePoint>();

        guideLines[0].linkPoint = rrp[1].GetReferencePoint();
    }

    public void SpawnClone()
    {
        if (operands.Count == 2 && cloneAllow && spawnClock <= 0)
        {
            Vector3WorldBase newClone = Instantiate(clonePrefab, clonePoint.position, Quaternion.identity);
            newClone.referenceValue = result;
            spawnClock = 0.5f;
        }
    }

    public void CloneAllow()
    {
        cloneAllow = true;
    }

    public void CloneBlock()
    {
        cloneAllow = false;
    }

    public void SwapOperands()
    {
        if (operands.Count == 2)
        {
            Vector3WorldBase temp = operands[0];
            operands[0] = operands[1];
            operands[1] = temp;


            if(operation == Operation.Proj)
            {
                SyncGuidelines_Proj();
            }
        }

        Debug.Log("--- SWAP OPERANDS --- " + operands.Count);
    }

}
