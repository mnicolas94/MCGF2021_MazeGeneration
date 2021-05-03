using UnityEngine;

namespace Traps
{
    public class SpikesTrap : MonoBehaviour
    {
        public static int AnimationStateId = Animator.StringToHash("showing");
        
        [SerializeField] private Animator animator;
        [Space]
        [SerializeField] private float timeDown;
        [SerializeField] private float timeUp;
        [SerializeField] private float timeOffset;

        private bool _currentlyUp;
        private float _nextTimeToChange;

        private void Start()
        {
            _nextTimeToChange = timeOffset;
        }

        void Update()
        {
            if (Time.time >= _nextTimeToChange)
            {
                ToggleSpikesState();
                float cooldown = _currentlyUp ? timeUp : timeDown;
                _nextTimeToChange = Time.time + cooldown;
            }
        }

        private void ToggleSpikesState()
        {
            _currentlyUp = !_currentlyUp;
            animator.SetBool(AnimationStateId, _currentlyUp);
        }
    }
}
