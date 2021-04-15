using UnityEngine;

namespace Animations
{
    public class AnimateScale : StateMachineBehaviour
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private bool showing;

        private bool _initialized;
        private Transform _targetTransform;
        private SpriteRenderer _targetRenderer;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_initialized)
            {
                _initialized = true;
                _targetTransform = animator.transform;
                _targetRenderer = animator.GetComponent<SpriteRenderer>();
            }

            if (showing)
            {
                _targetTransform.localScale = Vector3.zero;
            }
            _targetRenderer.enabled = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float normalizedTime = stateInfo.normalizedTime;
            float curveValue = curve.Evaluate(normalizedTime);

            var localScale = _targetTransform.localScale;
            localScale.x = curveValue;
            localScale.y = curveValue;
            localScale.z = curveValue;
            _targetTransform.localScale = localScale;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _targetRenderer.enabled = showing;
            _targetTransform.localScale = Vector3.one;
        }
    }
}
