using System;
using System.Collections;
using Character;
using UnityEngine;

namespace Puzzles.Spiders
{
    public class SpidersPuzzleController : MonoBehaviour
    {
        [SerializeField] private string spidersTag;
        [SerializeField] private float timeAfterBattleToNotifyPuzzleSolved;
        
        
        private int _cantSpiders;
        
        private IEnumerator Start()
        {
            yield return null;

            var spiders = GameObject.FindGameObjectsWithTag(spidersTag);
            _cantSpiders = spiders.Length;
            foreach (var spider in spiders)
            {
                var spiderHealth = spider.GetComponent<Health>();
                spiderHealth.eventDied += OnSpiderDead;
            }
        }

        private void OnSpiderDead()
        {
            _cantSpiders--;

            if (IsPuzzleSolved())
            {
                var coroutine = WaitForBattleToEnd(GameManager.Instance.NotifyPuzzleSolved);
                StartCoroutine(coroutine);
            }
        }

        private IEnumerator WaitForBattleToEnd(Action onBattleEnd)
        {
            while (GameManager.Instance.BattleController.IsRunningBattle)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(timeAfterBattleToNotifyPuzzleSolved);
            
            onBattleEnd?.Invoke();
        }

        public bool IsPuzzleSolved()
        {
            return _cantSpiders <= 0;
        }
    }
}