using UnityEngine;
using UnityEngine.UI; // 为了使用按钮

public class ARCameraToggler : MonoBehaviour
{
    private Camera mainCamera;
    private MonoBehaviour arCameraManager;
    private bool isARCameraActive = true;
    
    [SerializeField]
    private Button toggleButton; // 在Inspector中可以指定按钮

    void Start()
    {
        // 获取主相机
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("找不到主相机(Main Camera)！");
            return;
        }
        
        // 获取AR Camera Manager组件
        arCameraManager = mainCamera.GetComponent("ARCameraManager") as MonoBehaviour;
        // 注意：这里应该使用您AR框架中实际的AR相机管理器组件名称
        
        if (arCameraManager == null)
        {
            Debug.LogError("主相机上找不到AR Camera Manager组件！");
            return;
        }
        
    }

    // 公开此方法，以便可以从按钮点击事件调用
    public void ToggleARCamera()
    {
        if (arCameraManager != null)
        {
            isARCameraActive = !isARCameraActive;
            arCameraManager.enabled = isARCameraActive;
            Debug.Log("AR Camera Manager 已" + (isARCameraActive ? "启用" : "禁用"));
        }
    }
}