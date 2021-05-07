using Character.Data;
using UnityEngine;

namespace Battles
{
    public class BattleController
    {
        private CharacterData _player;
        private CharacterData _enemy;

        private CharacterData _nextPlayerToAttack;
        
        public void StartBattle(CharacterData player, CharacterData enemy)
        {
            _player = player;
            _enemy = enemy;
            
            SelectFirstCharacterToAttack();
        }

        private void SelectFirstCharacterToAttack()
        {
            float randomValue = Random.value;
            _nextPlayerToAttack = _player.Statistics.FirstHitChance <= randomValue ? _player : _enemy;
        }

        private void ToggleNextCharacterToAttack()
        {
            _nextPlayerToAttack = GetTargetCharacter();
        }

        private CharacterData GetTargetCharacter()
        {
            return _nextPlayerToAttack == _player ? _enemy : _player;
        }

        public BattleStep NextBattleStep()
        {
            var attack = _nextPlayerToAttack.BattlesData.GetRandomAttack();
            bool missAttack = Random.value < _nextPlayerToAttack.Statistics.HitChance;
            
            var battleStep = new BattleStep
            {
                executor = _nextPlayerToAttack,
                target = GetTargetCharacter(),
                attack = attack,
                missAttack = missAttack
            };
            
            ToggleNextCharacterToAttack();

            return battleStep;
        }
    }

    public struct BattleStep
    {
        public CharacterData executor;
        public CharacterData target;
        public CharacterAttack attack;
        public bool missAttack;
    }
}