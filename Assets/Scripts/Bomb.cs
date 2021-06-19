using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Rigidbody bombRigidbody;
    [SerializeField] private Explosion bombExplosion;
    private Transform bombIdleParent;    

    public void Throw(Bomb bomb, int forse, Transform parent)
    {       
        bombIdleParent = parent;
        bomb.transform.rotation = bombIdleParent.rotation;
        bomb.transform.parent = null;
        bomb.gameObject.SetActive(true);
        bomb.bombRigidbody.AddForce(bombIdleParent.forward * forse, ForceMode.Impulse);
        StartCoroutine(BombLifeTime(bomb));
    }
    private IEnumerator BombLifeTime(Bomb b)
    {
        yield return new WaitForSeconds(5);
        bombExplosion.Boom();
        ReturnToArray();        
        yield break;
    }

    private void ReturnToArray()
    {
        Player.instance.bombs.Add(this);
        transform.position = bombIdleParent.transform.position;
        transform.parent = bombIdleParent;
        transform.rotation = Quaternion.identity;
        bombRigidbody.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }


   
}
