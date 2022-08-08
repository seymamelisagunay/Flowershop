using UnityEngine;

public class ShelverHrController : MonoBehaviour
{
    public ShelverController prefab;
    public Transform firstPosition;
    public void Init(Vector3 createPosition)
    {
        Debug.Log("Test !"+createPosition);
        var clone = Instantiate(prefab);
        clone.transform.position = createPosition;
    }
}