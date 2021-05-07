using System;
using Character;
using UnityEngine;
using Utils;

namespace Battles
{
    public class EnemyBattleStarter : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private CharacterBehaviour character;

        private void OnTriggerEnter2D(Collider2D other)
        {
            int otherLayer = other.gameObject.layer;
            if (playerMask.IsLayerInMask(otherLayer))
            {
                var playerCharacterBehaviour = other.GetComponent<CharacterBehaviour>();
                if (playerCharacterBehaviour != null)
                {
                    var playerData = playerCharacterBehaviour.CharacterData;
                    var playerHealth = playerCharacterBehaviour.CharacterHealth;
                    var enemyData = character.CharacterData;
                    var enemyHealth = character.CharacterHealth;

                    if (!GameManager.Instance.BattleController.IsRunningBattle)
                    {
                        GameManager.Instance.BattleController.StartBattle(
                            playerData,
                            playerHealth,
                            enemyData,
                            enemyHealth);
                    }
                }
            }
        }
    }
}