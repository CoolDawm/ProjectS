using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementalEffect
{
    public string Name { get; private set; }
    public float Duration { get; protected set; }

    protected ElementalEffect(string name, float duration)
    {
        Name = name;
        Duration = duration;
    }

    public abstract IEnumerator ApplyEffect(GameObject target);
}

public class DamageEffect : ElementalEffect
{
    public float Damage { get; private set; }
    public DamageEffect(string name, float duration, float damage)
        : base(name, duration)
    {
        Damage = damage;
    }
    public override IEnumerator ApplyEffect(GameObject target)
    {
        ElementsStatusManager manager = target.GetComponent<ElementsStatusManager>();
        if (manager.isUnderEffect) yield break;
        Debug.Log($"{Name} применён к {target.name} с уроном {Damage}");
        HealthSystem healthSystem=target.GetComponent<HealthSystem>();
        manager.SetStatus("Fire",Color.red);
        while( Duration!=0 )
        {
            healthSystem.TakeDamage(Damage,Color.magenta);
            yield return new WaitForSeconds(1);
            Duration--;
        }
        manager.RemoveStatus("Fire");

        Debug.Log($"{Name} завершён у {target.name}");
    }
}

public class SlowEffect : ElementalEffect
{
    public float SlowPercentage { get; private set; }

    public SlowEffect(string name, float duration, float slowPercentage)
        : base(name, duration)
    {
        SlowPercentage = slowPercentage;
    }

    public override IEnumerator ApplyEffect(GameObject target)
    {
        ElementsStatusManager manager = target.GetComponent<ElementsStatusManager>();
        if (manager.isUnderEffect) yield break;
        Debug.Log($"{Name} применён к {target.name} со снижением скорости на {SlowPercentage * 100}%");
        manager.SetStatus("Earth", Color.blue);

        float oldSpead = target.GetComponent<Characteristics>().secondCharDic["MovementSpeed"];
        target.GetComponent<Characteristics>().charBuffBuffer["MovementSpeed"] += - (oldSpead * SlowPercentage);
        yield return new WaitForSeconds(Duration);
        target.GetComponent<Characteristics>().charBuffBuffer["MovementSpeed"] += (oldSpead * SlowPercentage);
        manager.RemoveStatus("Earth");

        Debug.Log($"{Name} снят с {target.name}");
    }
}

public class StunEffect : ElementalEffect
{
    public StunEffect(string name, float duration)
        : base(name, duration) { }

    public override IEnumerator ApplyEffect(GameObject target)
    {
        ElementsStatusManager manager = target.GetComponent<ElementsStatusManager>();
        if (manager.isUnderEffect) yield break;
        Debug.Log($"{Name} применён к {target.name}");
        manager.SetStatus("Thunder", Color.white);
        if (target.tag == "Player")
        {
            target.GetComponent<PlayerBehaviour>().Stun();

        }
        else
        {
            target.GetComponent<EnemyBehaviour>()?.Stun();

        }
        yield return new WaitForSeconds(Duration);
        if (target.tag == "Player")
        {
            target.GetComponent<PlayerBehaviour>().UnStun();

        }
        else
        {
            target.GetComponent<EnemyBehaviour>()?.UnStun();

        }
        manager.RemoveStatus("Thunder");
        Debug.Log($"{Name} завершён у {target.name}");
        
    }
}