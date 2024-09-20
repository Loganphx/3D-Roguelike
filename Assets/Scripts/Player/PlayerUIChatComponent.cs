using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIChatComponent
{
    private int _totalMessages;
  
    private Transform _viewPortContentTransform;
    private Scrollbar _scrollbar;
    private List<TMP_Text> chatMessages = new List<TMP_Text>();


    public PlayerUIChatComponent(GameObject viewPortContent, Transform scrollbarTransform)
    {
        _viewPortContentTransform = viewPortContent.transform;
        _scrollbar = scrollbarTransform.GetComponent<Scrollbar>();
        
        foreach (Transform textBoxTransform in _viewPortContentTransform)
        {
            chatMessages.Add(textBoxTransform.GetComponent<TMP_Text>());
        }
    }
    public void HandleMessage(string message)
    {
        // Only loop when chat is full
    
        if (_totalMessages == chatMessages.Count)
        {
            var text = chatMessages[0];
            text.transform.SetParent(null);
            text.transform.SetParent(_viewPortContentTransform);
            text.SetText(message);
            
            chatMessages.RemoveAt(0);
            chatMessages.Add(text);

            _scrollbar.value = 0;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollbar.handleRect);
        }
        else
        {
            // 0
            chatMessages[_totalMessages].SetText(message);
            _totalMessages++;

            _scrollbar.value = 1 - (_totalMessages / (float)chatMessages.Count);
        }
    }
}