using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    bool paused;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject Diary;
    [SerializeField]
    GameObject DiaryHolder;
    [SerializeField]
    Image DiaryImage;
    [SerializeField]
    Image[] NotesHolders;
    [SerializeField]
    Sprite[] Notes;
    [SerializeField]
    EventSystem es;
    [SerializeField]
    GameObject GamePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void AddNote(int i)
    {
        NotesHolders[i].sprite = Notes[i];
    }
    public void OnClickNote()
    {
        
        string s = es.currentSelectedGameObject.name;
        int i = int.Parse(s)-1;
        if (NotesHolders[i].sprite == Notes[i])
        {
            DiaryImage.sprite = Notes[i];
            DiaryHolder.SetActive(true);
            foreach (GameObject g in NotesHolders.Select(_ => _.gameObject)) {
                g.GetComponent<Button>().interactable = true;
            }
            NotesHolders[i].gameObject.GetComponent<Button>().interactable = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            { 
                Time.timeScale = 0f; 
                paused = true;
                menu.SetActive(true);
                GamePanel.SetActive(false);
            }
            else if (Diary.activeSelf)
            {
                toMenu();
            }
            else
            {
                Continue();
            }
        }
    }
    public void toMenu()
    {
        Diary.SetActive(false);
        menu.SetActive(true);
    }
    public void Continue()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        GamePanel.SetActive(true);
    }
    public void Escape()
    {
        Application.Quit();
    }
    public void toDiary()
    {
        menu.SetActive(false );
        Diary.SetActive(true );
    }
}
