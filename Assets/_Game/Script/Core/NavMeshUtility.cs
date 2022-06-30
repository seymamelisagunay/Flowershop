using UnityEngine;
using UnityEngine.AI;

namespace Sources.Utility
{
    public class NavMeshUtility
    {
        private NavMeshDataInstance _dataInstance;

        public NavMeshUtility(NavMeshData data, Vector3 position)
        {
            if (data)
                _dataInstance = NavMesh.AddNavMeshData(data, position, Quaternion.identity);
        }

        public Vector3 GetPosition(Vector3 position, float range)
        {
            var finalPosition = position;
            for (var i = 0; i < 15; i++)
            {
                var temp = position;
                var targetDelta = new Vector3(Random.Range(-1 * range, range),
                    0,
                    Random.Range(-1 * range, range));

                temp += targetDelta;

                if (NavMesh.SamplePosition(temp, out var hit, range, NavMesh.AllAreas))
                {
                    finalPosition = hit.position;
                    return finalPosition;
                }
            }

            return finalPosition;
        }
       
        public bool SamplePosition(Vector3 startPosition, out NavMeshHit hit, int distance)
        {
            var value = NavMesh.SamplePosition(startPosition, out hit, distance, NavMesh.AllAreas);
            return value;
        }

        public bool CalculatePath(Vector3 startPosition, Vector3 targetPosition, NavMeshPath path)
        {
            return NavMesh.CalculatePath(startPosition, targetPosition, NavMesh.AllAreas, path);
        }

        public void RemoveData()
        {
            _dataInstance.Remove();
        }
    }
}