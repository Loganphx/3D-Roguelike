using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Damagable _damagable;

    private Canvas _canvas;
    private Image _healthBarFill;

    private void Awake()
    {
        _damagable = GetComponent<Damagable>();
        _canvas = GetComponentInChildren<Canvas>();
        _healthBarFill = _canvas.transform.Find("Panel_HealthBar").Find("Panel_Fill").GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (_damagable.CurrentHealth != _damagable.MaxHealth)
        {
            // Hey we're damaged
            if (!_canvas.enabled) _canvas.enabled = true;
            _healthBarFill.fillAmount = _damagable.CurrentHealth / (float)_damagable.MaxHealth;
        }
        else
        {
            if (_canvas.enabled) _canvas.enabled = false;
        }
    }
}
