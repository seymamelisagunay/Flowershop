using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    public class SoundParent : MonoBehaviour
    {
        public void LoadPrefab(string key, Dictionary<string, int> lastPlayedIndex,
            Action<SoundController> onLoad)
        {
            StartCoroutine(LoadCoroutine(key, lastPlayedIndex, onLoad));
        }

        private IEnumerator LoadCoroutine(string key, Dictionary<string, int> lastPlayedIndex,
            Action<SoundController> onLoad)
        {
            var task = Resources.LoadAsync<SoundItem>($"SoundControllers/{key}");
            yield return task;

            if (task.asset)
            {
                var soundItem = (SoundItem) task.asset;
                var randomIndex = 0;

                if (soundItem.items.Count == 1)
                {
                    soundItem.items[0].key = key;
                    onLoad.Invoke(soundItem.items[0]);
                }
                else if (lastPlayedIndex.TryGetValue(key, out var index))
                {
                    var tryAmount = 5;
                    do
                    {
                        randomIndex = Random.Range(0, soundItem.items.Count);
                        tryAmount--;
                        if (tryAmount <= 0) break;
                    } while (randomIndex == index);

                    lastPlayedIndex[key] = randomIndex;
                    soundItem.items[randomIndex].key = key;
                    onLoad.Invoke(soundItem.items[randomIndex]);
                }
                else
                {
                    randomIndex = Random.Range(0, soundItem.items.Count);
                    lastPlayedIndex.Add(key, randomIndex);
                    soundItem.items[randomIndex].key = key;
                    onLoad.Invoke(soundItem.items[randomIndex]);
                }
            }
            else
            {
                onLoad.Invoke(null);
            }
        }
    }
}