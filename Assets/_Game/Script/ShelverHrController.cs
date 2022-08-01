using UnityEngine;

public class ShelverHrController : MonoBehaviour
{
    public ShelverController prefab;

    public void Init()
    {
        var clone = Instantiate(prefab);
    }
}