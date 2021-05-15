using UnityEngine;
using UnityEngine.Tilemaps;

namespace Items.Implementations
{
    public class MinimapContainer : MonoBehaviour
    {
        [SerializeField] private GameObject minimapCamera;
        [SerializeField] private GameObject minimapTilemap;

        public void EnableMinimap()
        {
            minimapCamera.SetActive(true);
            minimapTilemap.SetActive(true);
        }
    }
}