using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    Sprite Close;
    [SerializeField]
    Sprite Open;
    [SerializeField]
    SpriteRenderer sp;
    public bool opened = false;
    public bool[] item = { false, false, false, false, false, false };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Take()
    {
        if(item[0] || item[1])
        {
            Destroy(gameObject);
        }
    }
    public bool OpenDoor(bool fkey, bool skey)
    {
        if(gameObject.name == "Дверь1" && fkey)
        {
            return true;
        }
        if (gameObject.name == "Дверь2" && skey)
        {
            return true;
        }
        return false;
    }
    public void OpenClose()
    {
        if(opened)
        {
            sp.sprite = Close;
            opened = false;
        }
        else
        {
            opened = true;
            sp.sprite = Open;
        }
    }
    public void Hide()
    {
        if (opened)
        {
            opened = false;
            sp.sprite = Close;
        }
    }
    public void Out()
    {
        opened = true ;
        sp.sprite = Open ;
    }
}
