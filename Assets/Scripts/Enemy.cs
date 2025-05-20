using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    int x = 1;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(transform.position.x > 0)
        {
            x = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(x, 0, 0) * Time.deltaTime;
        Vector3 dir = transform.position - player.transform.position;
        if(transform.transform.position.x + x <= -9 || transform.position.x + x >= 55)
        {
            Destroy(gameObject);
        }
        if(dir.magnitude < 7)
        {
            print("Near");
            
        }
    }
}
