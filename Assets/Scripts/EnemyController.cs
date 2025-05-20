using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    GameObject Ear;
    [SerializeField]
    GameObject Eye;
    public bool earspawned;
    public bool eyespawned;

    public bool room2 = false;
    [SerializeField]
    GameObject spEye;
    [SerializeField]
    GameObject spEar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SpawnEar(float x, GameObject player)
    {
        if(!earspawned) {
            earspawned = true;
            int xpos = (Mathf.Abs(x + 10) > Mathf.Abs(x - 55)) ? -10 : 55;
            print($"xpos = {xpos}; left = {Mathf.Abs(x + 10)}, right = {Mathf.Abs(x - 55)}");
            int ypos = room2 ? 11 : -1;
            GameObject enemy = Instantiate(Ear, new Vector3(xpos, ypos, 0), Quaternion.identity);
                enemy.GetComponent<Enemy>().player = player;
            if (xpos < 0)
            {
                enemy.GetComponent<SpriteRenderer>().flipX = true;
            }
            spEar = enemy;
            StartCoroutine(waitEar());
        }
    }
    public void SpawnEye(float x, GameObject player)
    {
        if (!eyespawned)
        {
            eyespawned = true;
            int xpos = (Mathf.Abs(x + 10) > Mathf.Abs(x - 55)) ? -10 : 55;
            print($"xpos = {xpos}; left = {Mathf.Abs(x + 10)}, right = {Mathf.Abs(x - 55)}");
            int ypos = room2 ? 10 : 0;
            GameObject enemy = Instantiate(Eye, new Vector3(xpos, ypos, 0), Quaternion.identity);
                enemy.GetComponent<Enemy>().player = player;
            if (xpos < 0)
            {
                enemy.GetComponent<SpriteRenderer>().flipX = true;
            }
            spEye = enemy;
            StartCoroutine(waitEye());

        }

    }
    public void Restart()
    {
        Destroy(spEar);
        Destroy(spEye);
        earspawned = false;
        eyespawned = false;
    }
    IEnumerator waitEye() {
        yield return new WaitWhile(() => spEye != null);
        eyespawned = false ;
    }
    IEnumerator waitEar() {
        yield return new WaitWhile(() => spEar != null);
        earspawned = false ;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
