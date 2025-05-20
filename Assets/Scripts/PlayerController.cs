using System;
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
    bool vision = false;
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
    [SerializeField]
    EnemyController enemys;
    [SerializeField]
    Animator animator;
    [SerializeField]
    AudioSource step;
    [SerializeField]
    AudioSource reznya;
    [SerializeField]
    AudioSource flashOn;
    [SerializeField]
    AudioSource flashOff;
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
        if (text.Contains("!!!"))
        {
            canvas.Pause();
            enemys.Restart();
            transform.position = new Vector3(0, -1.77f, 0);
            taked = new bool[6] {false,false,false,false,false,false };
            foreach (var item in FindObjectsByType<Interactable>(FindObjectsSortMode.None))
            {
                if(item.sp !=  null) 
                    item.sp.enabled = true;
            }
        }
    }
    IEnumerator eyeAgr()
    {
        while (true)
        {
            if (flashB && !vision)
            {
                eye = eye + 10 > 100 ? 100 : eye + 10;
                eyeText.text = $"Глаз {eye}% ";
                if(eye == 100)
                {
                    vision = true;
                    enemys.SpawnEye(transform.position.x, gameObject);
                    StartCoroutine(waitEye());
                }
            }
            else if(!vision)
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
    IEnumerator waitEar()
    {
        yield return new WaitWhile(() => enemys.earspawned);
        steps = false;
        ear = 0;
        earText.text = $"Ухо {ear}% ";

    }
    IEnumerator waitEye()
    {
        yield return new WaitWhile(() => enemys.eyespawned);
        vision = false;
        eye = 0;
        eyeText.text = $"Глаз {eye}% ";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !Inside)
        {
            float x = Input.GetAxis("Horizontal");
            if(transform.position.x + (x + 0.5f) < 55 && transform.position.x + (x-0.5f) > -10)
                transform.position += new Vector3(x, 0, 0) * speed * Time.deltaTime;
            if (x < 0)
            {
                animator.SetBool("Left", true);
                animator.SetBool("Right", false);
                if(!step.isPlaying)
                step.Play();
            }
            else if( x > 0) 
            {
                animator.SetBool("Right", true) ;
                animator.SetBool("Left", false);
                if(!step.isPlaying)
                    step.Play();
            }

            if (x >= 1 || x <= -1)
            {

                if (!steps)
                {
                    ear = ear + 10 > 100 ? 100 : ear + 10;
                    earText.text = $"Ухо {ear}% ";
                    steps = true;
                    if (ear == 100)
                    {
                        enemys.SpawnEar(transform.position.x, gameObject);
                        StartCoroutine(waitEar());
                    }
                    else
                    {

                        StartCoroutine(earDelay());
                    }
                    
                }

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                flash.SetActive(!flash.activeSelf);
                flashB = !flashB;
                if(flashB)
                    flashOn.Play();
                else flashOff.Play();
            }
        }
        else
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
            step.Pause();
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
                            enemys.room2 = true;
                            enemys.Restart();
                        }
                        else if(interactable.name == "Дверь2")
                        {
                            canvas.Win();
                        }
                    }
                    else if(!Inside)
                    {
                        interactable.OpenClose();
                        ear = ear + 25 > 100 ? 100 : ear + 25;
                        earText.text = $"Ухо {ear}% ";
                        steps = true;
                        if (ear == 100)
                        {
                            enemys.SpawnEar(transform.position.x, gameObject);
                            StartCoroutine(waitEar());
                        }
                        else
                        {

                            StartCoroutine(earDelay());
                        }
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
                            else
                            {
                                StartCoroutine(popUp("Это ключ, наверное он что-то открывает"));
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
                    GetComponent<SpriteRenderer>().sortingOrder = -1;
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
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        Inside = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!Inside)
            {
                StartCoroutine(popUp("Ты погиб!!!"));
                reznya.Play();
            }
        }
        else
        {
            interactable = collision.GetComponent<Interactable>();

        }
        print(collision.tag);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print(collision.tag + " вышел");
        if(interactable  == collision.gameObject.GetComponent<Interactable>()) 
            interactable = null;
    }
}
