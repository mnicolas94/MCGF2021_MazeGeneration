using UnityEngine;

namespace Character.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterMovement characterMovement;

        [SerializeField] private MovementOptions movementOptions;
    
        private void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            var dir = movementOptions.GetTransformedMovementDirection(h, v);
        
            characterMovement.Move(dir);
        }
    }
}
