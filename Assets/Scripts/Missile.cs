using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private Explosion missileExpl;
    [SerializeField] private Rigidbody missileRGB;
    [SerializeField] private float shootForce;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject missileBody;
    [SerializeField] private GameObject missileVFXExplosion;
    private Transform targetPos;

    private void Update()
    {
        MissileFly();
        MissileRotation();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Player"))
        {          
            missileExpl.Boom();
            Destroy(missileBody);
            StartCoroutine(ExplosionVFXDuration());
        }
    }

    public void InitMissile(Transform target, Transform pos)
    {
        targetPos = target;
        transform.position = pos.position;
        transform.rotation = pos.rotation;
        transform.parent = null;
    }

    public void MissileRotation()
    {
        var Pos = targetPos.position - transform.position;
        var Dir = Vector3.RotateTowards(transform.forward, Pos, rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(Dir);
    }

    private void MissileFly()
    {
        missileRGB.AddForce(transform.forward*shootForce, ForceMode.Force);
    } 
    private IEnumerator ExplosionVFXDuration()
    {
        missileVFXExplosion.SetActive(true);
        missileVFXExplosion.transform.parent = null;
        yield return new WaitForSeconds(6);
        Destroy(missileVFXExplosion);
        Destroy(gameObject);
    }
}
