using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Character;
using Character.Data;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Battles
{
    public class BattleControllerUi : MonoBehaviour
    {
        public Action eventBattleEnded;
        
        [SerializeField] private GameObject canvas;
        [SerializeField] private Image blackBackground;
        [SerializeField] private float fadeBackgroundSpeed;
        [SerializeField] private ShowHidePanel panel;
        [SerializeField] private DialogueText text;
        [SerializeField] private Image playerImage;
        [SerializeField] private Image enemyImage;

        [SerializeField] private CharacterHealthUi enemyHealthUiPrefab;
        [SerializeField] private RectTransform enemyHealthContainer;

        private BattleController _controller;
        private CharacterData _playerData;
        private Health _playerHealth;
        private CharacterData _enemyData;
        private Health _enemyHealth;
        
        private bool _runningBattle;
        private bool _finishBattleOnNextInteraction;
        
        public bool IsRunningBattle => _runningBattle;

        private void Start()
        {
            _controller = new BattleController();
            panel.HidePanel();
        }

        private void Update()
        {
            if (IsRunningBattle && Input.GetButtonDown("Submit"))
            {
                if (text.IsRunning)
                {
                    text.ShowAll();
                }
                else
                {
                    if (_finishBattleOnNextInteraction)
                    {
                        EndBattle();
                        _finishBattleOnNextInteraction = false;
                    }
                    else
                    {
                        NextBattleStep();
                    }
                }
            }
        }

        public void StartBattle(CharacterData playerData, Health playerHealth, CharacterData enemyData, Health enemyHealth)
        {
            if (IsRunningBattle)
                return;
            _runningBattle = true;
            
            _playerData = playerData;
            _playerHealth = playerHealth;
            _enemyData = enemyData;
            _enemyHealth = enemyHealth;
            _controller.StartBattle(playerData, enemyData);
            
            // mostrar canvas
            ShowBlackBackground();
            panel.ShowPanel();
            text.Clear();
            
            // parar a todos los CharacterMovement
            StopCharacterMovement(playerHealth.gameObject);
            StopCharacterMovement(enemyHealth.gameObject);
            
            // iniciar música de batalla
            
            // poner sprites
            playerImage.sprite = playerData.BattlesData.BattleSprite;
            enemyImage.sprite = enemyData.BattlesData.BattleSprite;

            // poner la vida del enemigo
            InitializeEnemyHealth(enemyHealth);

            // mostrar mensaje de comienzo de batalla
            ShowBattleStartMessage(playerData, enemyData);
        }

        private void EndBattle()
        {
            // ocultar canvas
            HideBlackBackground();
            panel.HidePanel();
            
            EnableCharacterMovement(_playerHealth.gameObject);
            if (_enemyHealth != null)
                EnableCharacterMovement(_enemyHealth.gameObject);
            
            eventBattleEnded?.Invoke();

            _runningBattle = false;
        }

        private void ShowBlackBackground()
        {
            StartCoroutine(FadeBackgroundToValue(1));
        }

        private void HideBlackBackground()
        {
            StartCoroutine(FadeBackgroundToValue(0));
        }

        private IEnumerator FadeBackgroundToValue(float targetValue)
        {
            var color = blackBackground.color;
            float currentAlpha = color.a;

            while (Mathf.Abs(currentAlpha - targetValue) > 1E-3)
            {
                currentAlpha = Mathf.Lerp(currentAlpha, targetValue, fadeBackgroundSpeed);
                color.a = currentAlpha;
                blackBackground.color = color;
                yield return null;
            }
            
            color.a = targetValue;
            blackBackground.color = color;
        }

        private void StopCharacterMovement(GameObject characterObject)
        {
            var characterMovement = characterObject.GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                characterMovement.Stop();
                characterMovement.enabled = false;
            }
        }
        
        private void EnableCharacterMovement(GameObject characterObject)
        {
            var characterMovement = characterObject.GetComponent<CharacterMovement>();
            if (characterMovement != null)
            {
                characterMovement.enabled = true;
            }
        }

        private void ShowBattleStartMessage(CharacterData playerData, CharacterData enemyData)
        {
            string playerName = GetCharacterNameString(playerData);
            string enemyName = GetCharacterNameString(enemyData);
            string enemyMessage = enemyData.BattlesData.GetRandomStartDialogue();
            string message = $"{playerName} has encountered a wild {enemyName}.\nThe enemy says:\n\"{enemyMessage}\"";
            
            text.PutText(message);
        }

        private void NextBattleStep()
        {
            if (IsAnyCharacterDead())
            {
                var deadCharacter = GetDeadCharacter();
                HandleCharacterDeath(deadCharacter);
            }
            else
            {
                var battleStep = _controller.NextBattleStep();
                string description = GetBattleStepDescription(battleStep);
                text.PutText(description, () => HandleBattleStepDamage(battleStep));
            }
        }

        private void HandleBattleStepDamage(BattleStep battleStep)
        {
            if (!battleStep.missAttack)
            {
                var healthReceivingDamage = GetCharacterHealth(battleStep.target);
                healthReceivingDamage.DoDamage(battleStep.attack.Damage);
            }
        }

        private void HandleCharacterDeath(CharacterData deadCharacter)
        {
            string message = "";
            string characterName = GetCharacterNameString(deadCharacter);
            if (deadCharacter == _playerData)
            {
                message = $"Unfortunately, {characterName} died in the battle :(";
            }
            else
            {
                message = $"The {characterName} was defeated :D";
            }
            text.PutText(message);

            _finishBattleOnNextInteraction = true;
        }

        private Health GetCharacterHealth(CharacterData characterData)
        {
            if (characterData == _playerData)
            {
                return _playerHealth;
            }
            else
            {
                return _enemyHealth;
            }
        }

        private bool IsAnyCharacterDead()
        {
            return _playerHealth.IsDead || _enemyHealth.IsDead;
        }

        private CharacterData GetDeadCharacter()
        {
            if (_playerHealth.IsDead)
            {
                return _playerData;
            }
            if (_enemyHealth.IsDead)
            {
                return _enemyData;
            }

            return null;
        }

        private string GetBattleStepDescription(BattleStep step)
        {
            string executorName = GetCharacterNameString(step.executor);
            string targetName = GetCharacterNameString(step.target);

            string attackName = $"<i>{step.attack.AttackName}</i>";
            int damage = step.attack.Damage;
            bool attackMissed = step.missAttack;

            string description = $"{executorName} uses {attackName} on {targetName}.";
            if (attackMissed)
            {
                description += $"\n<color=#BBBBBB>It missed :(</color>";
            }
            else
            {
                description += $"\nIt dealt {damage} damage.";
                if (damage > 1)
                {
                    description += "\nIt is super effective!";
                }
            }

            return description;
        }

        private string GetCharacterNameString(CharacterData character)
        {
            Color nameColor = character.BattlesData.NameColor;
            string characterName = $"<color=#{ColorUtility.ToHtmlStringRGB(nameColor)}>{character.CharacterName}</color>";
            return characterName;
        }

        private void InitializeEnemyHealth(Health enemyHealth)
        {
            ClearEnemyHealth();
            var enemyHealthUi = Instantiate(enemyHealthUiPrefab, enemyHealthContainer);
            enemyHealthUi.SetCharacterHealth(enemyHealth);
        }

        private void ClearEnemyHealth()
        {
            var children = enemyHealthContainer.GetComponentsInChildren<CharacterHealthUi>();
            foreach (var item in children)
            {
                Destroy(item.gameObject);
            }
        }
    }
}