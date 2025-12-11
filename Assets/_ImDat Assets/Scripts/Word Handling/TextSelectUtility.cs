using UnityEngine;

public class TextSelectUtility : MonoBehaviour
{
    public TMPro.TMP_InputField myInputField;
    public string selectedText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myInputField.onTextSelection.AddListener(GetSelectedText);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetSelectedText(string str, int start, int end)
    {
        selectedText = str.Substring(Mathf.Min(start, end), Mathf.Abs(end - start));
    }
}
