using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class RoseSeedling : MonoBehaviour
{
    public GameObject seedling;
    public GameObject rose;

    public List<Transform> roseTransformList;
    
    public float seedlingSewTime;
    public float roseSewTime;
    
    public List<GameObject> roseClones = new List<GameObject>();
    
    private void SeedlingSew(GameObject _seedling,float time)
        {
            _seedling.transform.DOScale(1f, time);
        }
    
    [Button()]
        public void TestSeedlingSew()
        {
            SeedlingSew(seedling, seedlingSewTime);
        }
        
        
    private void RoseSew(GameObject _rose, float time, List<Transform> _roseTransformList, List<GameObject> _roseClones)
    {
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(Coroutine(time));
            var cloneRose = Instantiate(_rose, _roseTransformList[i]);
            _roseClones.Add(cloneRose);
        }
    }
    IEnumerator Coroutine(float time)
    {
        yield return new WaitForSeconds(time);
    }

    [Button()]
         public void TestRoseSew()
         {
             RoseSew(rose, roseSewTime, roseTransformList, roseClones);
         }
}
