using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Script
{
    public class AreaPositionSelector : MonoBehaviour
    {
        public enum Shape
        {
            Circle,
            Rectangle
        }

        public Vector3 Position => transform.position;
        public Shape shape;
        [ShowIf("shape",Shape.Circle)]
        public float radius;
        [ShowIf("shape",Shape.Rectangle)]
        public Vector3 scale;
        
        public Vector3 GetPosition()
        {
            var position = transform.position;

            Vector3 resultVector;
            if (shape == Shape.Circle)
            {
                var centerX = position.x;
                var centerZ = position.z;
                var randomX = Random.Range(centerX - radius, centerX + radius);
                var randomZ = Random.Range(centerZ - radius, centerZ + radius);
                resultVector = new Vector3(randomX, position.y, randomZ);
            }
            else
            {
                var centerX = position.x;
                var centerZ = position.z;
                var centerY = position.y;
                var randomX = Random.Range(centerX - scale.x/2f, centerX+scale.x/2f);
                var randomZ = Random.Range(centerZ - scale.z/2f, centerZ+scale.z/2f);
                var randomY = Random.Range(centerY - scale.y/2f, centerY+scale.y/2f);
                resultVector = new Vector3(randomX, randomY, randomZ);
            }
            
            return resultVector;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            if (shape == Shape.Circle)
                Gizmos.DrawWireSphere(transform.position, radius);
            else
                Gizmos.DrawWireCube(transform.position, scale);
        }
#endif
    }
}