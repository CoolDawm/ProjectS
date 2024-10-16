using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : CharacterSlot
{
    public Transform weaponHolder;
    public bool isPrimarySlot; 
    private GameObject currentWeaponInstance;
    private AbilitiesHolder _playerAbilityHolder;
    private GameObject _player;
    private Weapon _currentWeapon;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerAbilityHolder = _player.GetComponent<AbilitiesHolder>();
    }

    public override bool CanAcceptItem(Item item)
    {
        Debug.Log(item is Weapon);
        return item is Weapon;
    }

    public override void AddItem(Item newItem)
    {
        base.AddItem(newItem);
        Weapon newWeapon = newItem as Weapon;
        if (newWeapon != null)
        {
            _currentWeapon = newWeapon;
            weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder").transform;

            if (isPrimarySlot && newWeapon.weaponPrefab != null)
            {
                if (currentWeaponInstance != null)
                {
                    Destroy(currentWeaponInstance);
                    RemoveWeaponAttributes();
                    _player.GetComponent<Characteristics>().SetWeaponDamage(1);

                }
                // »спользуем локальные координаты дл€ смещени€
                currentWeaponInstance = Instantiate(newWeapon.weaponPrefab, weaponHolder);
                currentWeaponInstance.transform.localPosition = newWeapon.offset;
                currentWeaponInstance.transform.localRotation = Quaternion.identity;
                currentWeaponInstance.transform.GetChild(0).GetComponent<Renderer>().material.color = Random.ColorHSV();
                _playerAbilityHolder.ChangeAbility(newWeapon.GetAbility(), 0);
                _player.GetComponent<Characteristics>().SetWeaponDamage(newWeapon.damage);
                AddWeaponAttributes(newWeapon);
            }
            else if (!isPrimarySlot && newWeapon.weaponPrefab != null)
            {
                if (currentWeaponInstance != null)
                {
                    InstantiateWeapon();
                    RemoveWeaponAttributes();
                }
            }
        }
        
    }

    public override void ClearSlot()
    {
        ClearInstantiatedWeapon();
        base.ClearSlot();
    }
    public void ClearInstantiatedWeapon()
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
            RemoveWeaponAttributes();
            _player.GetComponent<Characteristics>().SetWeaponDamage(1);

        }
    }
    public Weapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public void AddWeaponAttributes(Weapon newWeapon)
    {
        _player.GetComponent<Characteristics>().AddAttributes(newWeapon.CharacteristicsBonuses());
    }

    public void RemoveWeaponAttributes()
    {
        if (_currentWeapon != null)
        {
            _player.GetComponent<Characteristics>().RemoveAttributes(_currentWeapon.CharacteristicsBonuses());
        }
    }

    public void InstantiateWeapon()
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            _player.GetComponent<Characteristics>().SetWeaponDamage(1);

        }
        if (_currentWeapon != null && _currentWeapon.weaponPrefab != null)
        {
            currentWeaponInstance = Instantiate(_currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
            currentWeaponInstance.transform.localPosition = _currentWeapon.offset;
            AddWeaponAttributes(_currentWeapon);
            currentWeaponInstance.transform.GetChild(0).GetComponent<Renderer>().material.color = Random.ColorHSV();
            _playerAbilityHolder.ChangeAbility(_currentWeapon.GetAbility(), 0);
            _player.GetComponent<Characteristics>().SetWeaponDamage(_currentWeapon.damage);

        }
    }
}
