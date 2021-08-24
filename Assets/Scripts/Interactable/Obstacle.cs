
using Runner.Player;
using UnityEngine;

namespace Runner.Interactable
{
    class Obstacle : Interactable
    {
        protected override void OnInteract(GameObject player)
        {
            player.GetComponent<PlayerController>().OnHitObstacle(gameObject);
        }
    }
}
