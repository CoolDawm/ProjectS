using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalEffectsManager : MonoBehaviour
{
    private Dictionary<string, ElementalEffect> effects = new Dictionary<string, ElementalEffect>();
    private List<HealthSystem> _targets=new List<HealthSystem>();
    GameObject _testSubject;
    void Start()
    {
        // Пример добавления эффектов
        effects.Add("Fire", new DamageEffect("Fire", 5f, 10f));
        effects.Add("Earth", new SlowEffect("Earth", 3f, 0.9f)); 
        effects.Add("Thunder", new StunEffect("Stun", 3f));
        _targets.AddRange(FindObjectsOfType<HealthSystem>());
        foreach (HealthSystem target in _targets)
        {
            target.OnTakeDamageWithElement+=ActivateEffect;
        }
        _testSubject = GameObject.FindGameObjectWithTag("Player");
    }
    public void AddTarget(HealthSystem target)
    {
        _targets.Add(target);
        target.OnTakeDamageWithElement += ActivateEffect;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ActivateEffect("Fire", _testSubject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ActivateEffect("Ice", _testSubject);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ActivateEffect("Stun", _testSubject);
        }
    }
    public void ActivateEffect(string effectKey, GameObject target)
    {
        if (effects.TryGetValue(effectKey, out ElementalEffect effect))
        {
            StartCoroutine(effect.ApplyEffect(target));
        }
        else
        {
            Debug.LogWarning($"Эффект {effectKey} не найден");
        }
    }
}
