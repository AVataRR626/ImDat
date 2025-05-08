using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WordRig : MonoBehaviour
{
    [Header("Primary Settings")]
    public string text;

    [Header("Cloning Settings")]
    public WordRig wordRigPrefab;
    public Transform clonePoint;
    

    [Header("System Stuff (usually don't touch)")]
    public GameObject myHandle;
    public TextMeshProUGUI textDisplayTMPG;
    public TMP_InputField keyboardInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textDisplayTMPG.text = text;
        keyboardInput.text = text;

        if(Application.isPlaying)
            myHandle.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(keyboardInput.isActiveAndEnabled)
        {
            text = keyboardInput.text;
        }
        
        textDisplayTMPG.text = text;
    }

    public void Clone()
    {
        WordRig newWord = Instantiate(wordRigPrefab,clonePoint.position,clonePoint.rotation);
        newWord.text = text;
    }
}
