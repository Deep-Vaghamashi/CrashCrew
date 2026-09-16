using UnityEngine;
using Unity.Netcode;

public class PlayerCollisionDamage : NetworkBehaviour
{
    [SerializeField] private float minimumImpactSpeed = 3f;
    [SerializeField] private float damageMultiplier = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer)
            return;

        PlayerHealth otherHealth =
            collision.gameObject.GetComponent<PlayerHealth>();

        if (otherHealth == null)
            return;

        // Get the velocity of this car at the moment of impact.
        float impactSpeed = GetComponent<Rigidbody>().linearVelocity.magnitude;

        if (impactSpeed < minimumImpactSpeed)
            return;

        float damage =
            (impactSpeed - minimumImpactSpeed) * damageMultiplier;

        otherHealth.TakeDamage(damage);
    }
}