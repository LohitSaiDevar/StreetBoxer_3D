using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public ShakeData MyShake;
    private void Start()
    {
        
    }

    public void ShakeCamera()
    {
        CameraShakerHandler.Shake(MyShake);
    }
}
