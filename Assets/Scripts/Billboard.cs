using UnityEngine;



[ExecuteInEditMode]
// Tato trida zajisti, ze se dany gameobject nataci smerem ke kamere
public class Billboard : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
