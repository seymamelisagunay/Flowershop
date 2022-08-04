using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(menuName = "Configs/SoundItem", order = 1)]
    public class SoundItem : ScriptableObject
    {
        public List<SoundController> items;
        private SoundController _lastPlayedItem;

        public SoundController Get()
        {
            if (items.Count == 1) return items[0];

            SoundController selectedItem = null;
            if (_lastPlayedItem == null)
            {
                selectedItem = items[Random.Range(0, items.Count - 1)];
                _lastPlayedItem = selectedItem;
                return selectedItem;
            }

            var repeatCount = 0;
            do
            {
                selectedItem = items[Random.Range(0, items.Count - 1)];
                repeatCount++;
            } while (_lastPlayedItem == selectedItem && repeatCount < 5);

            _lastPlayedItem = selectedItem;
            return selectedItem;
        }
    }
}