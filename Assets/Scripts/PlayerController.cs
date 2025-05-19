using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject flash;
    [SerializeField]
    bool flashB = false;
    bool steps = false;
    public bool Inside = false;
    public bool[] taked =
    {
        false, false, false, false, false, false
    };
    //  ключ1, ключ2, запи1, запи2, запи3, запи4
    [SerializeField]
    Interactable interactable;
    [SerializeField]
    TextMeshProUGUI earText;
    [SerializeField]
    TextMeshProUGUI eyeText;
    int ear = 0, eye = 0;
    [SerializeField]
    CanvasController canvas;
    [SerializeField]
    TextMeshProUGUI popup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(eyeAgr());
    }
    IEnumerator popUp(string text)
    {
        popup.text = text;
        yield return new WaitForSeconds(2);
        popup.text = "";
    }
    IEnumerator eyeAgr()
    {
        while (true)
        {
            if (flashB)
            {
                eye = eye + 10 > 100 ? 100 : eye + 10;
                eyeText.text = $"Глаз {eye}% ";
            }
            else
            {
                eye = eye - 5 < 0 ? 0 : eye - 5;
                eyeText.text = $"Глаз {eye}% ";
            }

            if (!steps)
            {
                ear = ear - 5 < 0 ? 0 : ear - 5;
                earText.text = $"Ухо {ear}% ";
            }
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator earDelay()
    {
        yield return new WaitForSeconds(1);
        steps = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !Inside)
        {
            float x = Input.GetAxis("Horizontal");
            if(transform.position.x + (x + 0.5f) < 55 && transform.position.x + (x-0.5f) > -10)
                transform.position += new Vector3(x, 0, 0) * speed * Time.deltaTime;
            if(x >= 1 || x <= -1)
            {
                if(!steps)
                {
                    ear = ear + 10 > 100 ? 100 : ear + 10;
                    earText.text = $"Ухо {ear}% ";
                    steps = true;
                    StartCoroutine(earDelay());
                }

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                flash.SetActive(!flash.activeSelf);
                flashB = !flashB;
            }
        }
        if (Input.anyKeyDown)
        {
            float y = Input.GetAxis("Vertical");
            if (Input.GetKeyDown(KeyCode.E) && interactable != null)
            {
                if (interactable.tag != "Брать")
                {
                    if (interactable.CompareTag("Дверь"))
                    {
                        if (!interactable.OpenDoor(taked[0], taked[1]))
                        {
                            StartCoroutine(popUp("Кажется дверь закрыта, нужен ключ"));
                        }
                        else if(interactable.name == "Дверь1")
                        {
                            StartCoroutine(popUp("переход в следующую комнатушку"));
                            transform.localPosition = new Vector3(0, 11, 0);
                        }
                    }
                    else if(!Inside)
                    {
                        interactable.OpenClose();
                        ear = ear + 25 > 100 ? 100 : ear + 25;
                        earText.text = $"Ухо {ear}% ";
                        steps = true;
                        StartCoroutine(earDelay());
                    }
                    
                }
                else
                {
                    for(int i = 0; i < taked.Length; i++)
                    {
                        if (interactable.item[i])
                        { 
                            taked[i] = true;
                            if(i >=2)
                            {
                                canvas.AddNote(i - 2);
                                StartCoroutine(popUp("вы нашли записку, картинка позже, посмотри в дневнике")); //TODO перенести в канвас
                            }
                            
                        }
                    }
                    interactable.Take();
                }
            }

            if (y < 0 && Inside)
            {
                if (interactable.tag == "Шкаф")
                {
                    interactable.Out();
                    Inside = false;
                    GetComponent<SpriteRenderer>().sortingOrder = 2;
                    print("out");
                }
            }
            if (y > 0 && interactable.tag == "Шкаф")
            {
                if (interactable.opened)
                {
                    interactable.Hide();
                    GetComponent<SpriteRenderer>().sortingOrder = 0;
                    Inside = true;
                    flashB = false;
                    flash.SetActive(false);
                    print("hide");
                }
                else
                {
                    StartCoroutine(DelayHide());
                    print("delayhide");
                }
            }
        }
           

    }
    IEnumerator DelayHide()
    {
        interactable.OpenClose();
        yield return new WaitForSeconds(0.5f);
        interactable.Hide();
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        Inside = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactable = collision.GetComponent<Interactable>();
        print(collision.tag);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print(collision.tag + " вышел");
        if(interactable  == collision.gameObject.GetComponent<Interactable>()) 
            interactable = null;
    }
}
