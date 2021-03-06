using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        public static int SpeedHashId = Animator.StringToHash("speed");

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
    
        [SerializeField] private float speed;
    
        public float Speed => speed;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 dir)
        {
            if (!enabled)
                return;
            _rb.velocity = dir.normalized * speed;

            if (dir.x < 0)
            {
                spriteRenderer.flipX = true;
            } else if (dir.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        
        public void Stop()
        {
            Move(Vector2.zero);
        }

        private void Update()
        {
            animator.SetFloat(SpeedHashId, _rb.velocity.magnitude);
        }

    }
}
