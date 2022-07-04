using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CamFocus : MonoBehaviour
{
    public Transform target;
     [Button()]
     void CameraFocus () { 
         Vector3 pointOnside = target.position + new Vector3 (target.localScale.x * 4.78f, 14f, target.localScale.z * -8);
         float aspect = (float)Screen.width / (float)Screen.height;
         float maxDistance = (target.localScale.y * 0.5f) / Mathf.Tan (Mathf.Deg2Rad * (Camera.main.fieldOfView / aspect)); 
         Camera.main.transform.position = Vector3.MoveTowards (pointOnside, target.position, -maxDistance);
         Camera.main.transform.LookAt (target.position);
     
     }
}
