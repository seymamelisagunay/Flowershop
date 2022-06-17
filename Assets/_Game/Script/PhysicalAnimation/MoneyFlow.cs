using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class MoneyFlow : MonoBehaviour
{
    public Transform start;
    public Transform restart;
    public int count;
    public GameObject prefab;
    

    public List<GameObject> clones = new List<GameObject>();
    public float flowTime;

    private GameObject _money;

    private void ObjectGeneration(int _count, GameObject _prefab, Transform _start)
    {
        _count = count;
        _prefab = prefab;
        _start = start;
        for (int i = 0; i < _count; i++)
        {
            var cloneObjectPrefab = Instantiate(_prefab, _start);
            clones.Add(cloneObjectPrefab);
            StartCoroutine(Coroutine());
        }
    }

    private void ObjectFlow(Transform _start)
    {
        _start = start;
        
            foreach (var _clones in clones)
            {
                _clones.transform.DOMove(_start.position, 0.2f);
                StartCoroutine(Coroutine());
                //Destroy(_clones);
            }
        
    }
    [Button]
        public void Test()
        {
            ObjectGeneration(count, prefab, start);
            ObjectFlow(start);
            Debug.Log("testTEST");
        }
    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
