using System.Collections.Generic;
using Character.Data;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogues/Dialogue", order = 0)]
    public class Dialogue : DialogueSequenceBase
    {
        [SerializeField] private CharacterData speaker;
        [SerializeField] private string text;

        public CharacterData Speaker => speaker;
        public string Text => text;
        
        public override List<Dialogue> Dialogues()
        {
            return new List<Dialogue>{this};
        }
    }
}