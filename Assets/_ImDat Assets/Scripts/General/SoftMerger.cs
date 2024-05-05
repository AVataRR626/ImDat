using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftMerger : MonoBehaviour
{
    public string checkTag = "Soft Mergable";
    public int maxSubjects = 2;
    public List<Transform> subjects;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform s in subjects)
        {
            if(s != null)
            {
                s.transform.position = transform.position;
                s.transform.rotation = transform.rotation;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (subjects.Count < maxSubjects)
        {
            if (collision.gameObject.CompareTag(checkTag))
            {
                Collider c = collision.collider;
                c.isTrigger = true;
                subjects.Add(c.transform);
            }
        }
    }
}
