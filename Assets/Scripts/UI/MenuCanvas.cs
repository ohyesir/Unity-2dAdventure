using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCanvas : MonoBehaviour
{
    EventSystem m_EventSystem;
    public GameObject newGameButton;

    private void OnEnable()
    {
        m_EventSystem = EventSystem.current;
        
        m_EventSystem.SetSelectedGameObject(newGameButton);//游戏运行时只会有一个eventsystem  

        // Debug.Log("Current selected GameObject : " + m_EventSystem.currentSelectedGameObject);
    }

    public void ExitGame()
    {
        Debug.Log ("quti");
        Application.Quit();
    }
}
