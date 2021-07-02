using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Rigidbody bombRigidbody;
    [SerializeField] private Explosion bombExplosion;
    [SerializeField] private GameObject bombBody;
    [SerializeField] private GameObject bombWFXExpl;
    [SerializeField] private AudioSource bombSound;
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
        yield return new WaitForSeconds(4);
        bombWFXExpl.transform.parent = null;
        bombWFXExpl.transform.rotation = Quaternion.identity;
        bombWFXExpl.SetActive(true);
        bombExplosion.Boom();
        bombSound.Play();
        bombBody.SetActive(false);
        yield return new WaitForSeconds(5.5f);
        ReturnToArray();        
        yield break;
    }

    private void ReturnToArray()
    {
        Player.instance.bombs.Add(this);
        bombWFXExpl.transform.parent = transform;
        bombWFXExpl.transform.position = transform.position;
        bombWFXExpl.SetActive(false);
        bombBody.SetActive(true);
        transform.position = bombIdleParent.transform.position;
        transform.parent = bombIdleParent;
        transform.rotation = Quaternion.identity;
        bombRigidbody.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }


   
}
