using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Vector3WorldOp : MonoBehaviour
{
    public enum Operation { Add, Sub, Cross, Proj };

    public Operation operation;
    public Vector3Relay result;
    public GameObject resultDisplay;
    public int maxSubjects = 2;
    public List<Vector3WorldBase> operands;

    [Header("System Stuff - Usually Don't Touch")]
    public Vector3WorldBase clonePrefab;
    public Transform clonePoint;
    public bool cloneAllow = false;
    public float spawnClock = 0;
    public List<LineRendererLink> guideLines;

    public void OnDestroy()
    {
        foreach (Vector3WorldBase v in operands)
        {
            if (v != null)
                Destroy(v.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (result == null)
            result = GetComponent<Vector3Relay>();
    }
    void Update()
    {
        TrackOperands();

        if(operands.Count < 2)
        {
            resultDisplay.SetActive(false);
        }
        else if(operands.Count == 2)
        {
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

        if(spawnClock > 0)
        {
            spawnClock -= Time.deltaTime;
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

                    if (operation == Operation.Add)
                    {
                        //align parallelogram graphics
                        Vector3RelaySetterPosDelta rps = wb.GetComponent<Vector3RelaySetterPosDelta>();
                        guideLines[operands.Count - 1].linkPoint = rps.referencePoint;
                    }

                    if(operation == Operation.Sub)
                    {
                        if(operands.Count == 2)
                        {
                            //align direction graphics
                            Vector3RelaySetterPosDelta [] rps = new Vector3RelaySetterPosDelta[2];
                            rps[0] = operands[0].GetComponent<Vector3RelaySetterPosDelta>();
                            rps[1] = operands[1].GetComponent<Vector3RelaySetterPosDelta>();

                            guideLines[0].linkPoint = rps[0].referencePoint;
                            guideLines[0].transform.parent = rps[1].referencePoint;
                            guideLines[0].transform.localPosition = Vector3.zero;
                        }
                        
                    }

                    if(operation == Operation.Proj)
                    {
                        if (operands.Count == 2)
                        {
                            //align projection graphics
                            Vector3RelaySetterPosDelta[] rps = new Vector3RelaySetterPosDelta[2];
                            rps[0] = resultDisplay.GetComponent<Vector3RelaySetterPosDelta>();
                            rps[1] = operands[0].GetComponent<Vector3RelaySetterPosDelta>();

                            guideLines[0].linkPoint = rps[1].referencePoint;
                        }
                    }
                }
            }
        }
        
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
        }

        Debug.Log("--- SWAP OPERANDS --- " + operands.Count);
    }

}
