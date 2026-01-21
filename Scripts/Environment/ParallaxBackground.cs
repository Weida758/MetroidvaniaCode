using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX;
    private float cameraHalfWidth;
    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (!mainCamera) return;
        
        //camera half height * width/height = 1/2 * width = halfwidth
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeBackgroundWidth();
    }

    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentCameraPositionX;
        
        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

        foreach (ParallaxLayer parallaxLayer in backgroundLayers)
        {
            parallaxLayer.Move(distanceToMove);
            parallaxLayer.LoopBackGround(cameraLeftEdge, cameraRightEdge);
        }
    }


    private void InitializeBackgroundWidth()
    {
        foreach (ParallaxLayer parallaxLayer in backgroundLayers)
        {
            parallaxLayer.CalculateImageWidth();
        }
    }
}
