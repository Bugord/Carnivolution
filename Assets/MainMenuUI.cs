using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Texture2D hoverSprite;
    [SerializeField] private Texture2D regularSprite;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        Cursor.SetCursor(regularSprite, Vector2.zero, CursorMode.Auto);
        DontDestroyOnLoad(gameObject);
    }

    public void OnHover()
    {
        Cursor.SetCursor(hoverSprite, Vector2.zero, CursorMode.Auto);
    }

    public void OnHoverLeave()
    {
        Cursor.SetCursor(regularSprite, Vector2.zero, CursorMode.Auto);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("EditorScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void AmbientToggle(bool toggle)
    {
        audioMixer.SetFloat("AmbientSound", toggle ? 1 : 0);
    }
    
    public void SoundsToggle(bool toggle)
    {
        audioMixer.SetFloat("ActionSounds", toggle ? 1 : 0);
    }
}
