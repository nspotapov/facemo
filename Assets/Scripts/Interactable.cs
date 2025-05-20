using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    Sprite Close;
    [SerializeField]
    Sprite Open;
    [SerializeField]
    public SpriteRenderer sp;
    [SerializeField]
    AudioSource audio1;
    [SerializeField]
    AudioSource audio2;
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
            sp.enabled = false;
        }
            audio1.Play();
        
    }
    public bool OpenDoor(bool fkey, bool skey)
    {
        if(gameObject.name == "Дверь1" && fkey)
        {
            audio2.Play();
            return true;
        }
        if (gameObject.name == "Дверь2" && skey)
        {
            audio2.Play();
            return true;
        }
        audio1.Play();
        return false;
    }
    public void OpenClose()
    {
        if(opened)
        {
            sp.sprite = Close;
            opened = false;
            audio2.Play();
        }
        else
        {
            opened = true;
            sp.sprite = Open;
            audio1.Play();
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
