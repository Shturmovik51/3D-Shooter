using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private int explRadius;
    [SerializeField] private int explPower;
    [SerializeField] private int explDamage;
    private Rigidbody entityRigidbody;

    private void Start()
    {
        entityRigidbody = GetComponent<Rigidbody>();
    }

    public void Boom()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explRadius);

        foreach (var hit in colliders)
        {
            Rigidbody hitRB = hit.GetComponent<Rigidbody>();
            if (hitRB != null && hitRB != entityRigidbody)
            {               

                hitRB.gameObject.isStatic = false;
                hitRB.isKinematic = false;
                hitRB.AddExplosionForce(explPower, explosionPos, explRadius, 3.0f, ForceMode.Impulse);

            }

            if (GameManager.instance.healthContainer.ContainsKey(hit.gameObject))
            {
                var healh = GameManager.instance.healthContainer[hit.gameObject];
                healh.TakeDamage(explDamage);
            }
        }
    }
}
