using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AbilitesManager : MonoBehaviour
{
    public float areaOfEffectRadius;
    public GameObject projectilePrefab;
    //Abilities
    public SlowAura slowAura;
    public DamageUpAura damageUpAura;
    public SpeedScream speedScream;
    public RangeAOE rangeAoe;
    public FrostBeam frostBeam;
    public SimpleMelee simpleMelee;
    public SimpleRange simpleRange;
    //
    private FloatTextManager _floatingTextManager;
    private Utilities _utilities=new Utilities();

    public void Start()
    {
        _floatingTextManager = GameObject.FindGameObjectWithTag("FloatingTextManager").GetComponent<FloatTextManager>();
        slowAura=GetComponent<SlowAura>();
        damageUpAura=GetComponent<DamageUpAura>();
        speedScream=GetComponent<SpeedScream>();
        rangeAoe=GetComponent<RangeAOE>();
        frostBeam=GetComponent<FrostBeam>();
    }
    public void MeleeAbility(float meleeRange,float meleeDamage,GameObject attacker,float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            _floatingTextManager.ShowFloatingText(attacker.transform,"Failed"); 
            return;
        }
        simpleMelee.Melee(meleeRange,meleeDamage,attacker);
    }

    public void SlowingAura(GameObject emmiter,String aim,float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            _floatingTextManager.ShowFloatingText(emmiter.transform,"Failed"); 
            return;
        }
        slowAura.StartEmitting(emmiter, aim);
        _floatingTextManager.ShowFloatingText(emmiter.transform,"Slow Aura"); 
    } 
    public void DamageUpAura(GameObject emmiter,String aim,float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            _floatingTextManager.ShowFloatingText(emmiter.transform,"Failed"); 
            return;
        }
        damageUpAura.StartEmitting(emmiter, aim);
        _floatingTextManager.ShowFloatingText(emmiter.transform,"Damage Up Aura"); 
    } 
    public void SpeedScream(GameObject emmiter,String aim,float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            _floatingTextManager.ShowFloatingText(emmiter.transform,"Failed"); 
            return;
        }
        speedScream.StartEmitting(emmiter, aim);
        _floatingTextManager.ShowFloatingText(emmiter.transform,"Speed Scream"); 
    }

    public void RangeAbility(float projectileLifeTime, float projectileSpeed, Transform shootingPosition, float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            _floatingTextManager.ShowFloatingText(shootingPosition.gameObject.GetComponentInParent<Transform>(),
                "Failed");
            return;
        }

        simpleRange.Shoot(projectileLifeTime, projectileSpeed, shootingPosition);

    }

    public void AoeAbility(float aoeDamage, float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            return;
        }
        rangeAoe.AoeAbility(aoeDamage);
    }

    public void Shield(GameObject abilityObject, float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            _floatingTextManager.ShowFloatingText(abilityObject.transform, "Failed");
            return;
        }

        abilityObject.GetComponent<HealthSystem>().ShieldCharge(100);
    }

    public void MeleeAoe(float damage,GameObject user,float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            _floatingTextManager.ShowFloatingText(user.transform,"Failed"); 
            return;
        }
        if (user.tag == "Player")
        {
            Collider[] colliders = Physics.OverlapSphere(user.transform.position, areaOfEffectRadius, LayerMask.GetMask("Enemy"));
            foreach (Collider collider in colliders)
            {
                collider.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("Boooooom");
            Collider[] colliders = Physics.OverlapSphere(user.transform.position, areaOfEffectRadius);
            foreach (Collider collider in colliders)
            {
                
                collider.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
            }
        }
        
    }

    public void StartFrostBeam(Transform point, float damagePerSecond, float duration, float beamRange, string aim, float chance)
    {
        if (!_utilities.CalculateChance(chance))
        {
            _floatingTextManager.ShowFloatingText(point.gameObject.GetComponentInParent<Transform>(), "Failed");
            return;
        }

        frostBeam.StartFrostBeam(point, damagePerSecond, duration, beamRange, aim);
    }
}
