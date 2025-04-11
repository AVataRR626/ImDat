using UnityEngine;
using UnityEngine.UI;

public class TransparencyController : MonoBehaviour
{
    public Slider transparencySlider; // 透明度滑块
    public GameObject targetObject;   // 目标物体
    
    private Material material;        // 物体材质
    
    void Start()
    {
        // 获取物体的材质
        if (targetObject != null && targetObject.GetComponent<Renderer>() != null)
        {
            material = targetObject.GetComponent<Renderer>().material;
            
            // 设置滑块初始值
            if (transparencySlider != null)
            {
                Color currentColor = material.color;
                transparencySlider.value = currentColor.a;
                transparencySlider.onValueChanged.AddListener(UpdateTransparency);
            }
        }
        else
        {
            Debug.LogError("目标物体不存在或没有Renderer组件");
        }
    }
    
    // 更新透明度的方法
    public void UpdateTransparency(float value)
    {
        if (material != null)
        {
            // 获取当前颜色
            Color color = material.color;
            // 更新透明度（Alpha值）
            color.a = value;
            // 应用新颜色到材质
            material.color = color;
        }
    }
}