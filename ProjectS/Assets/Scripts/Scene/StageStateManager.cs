using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStateManager : MonoBehaviour
{
    [SerializeField]
    private List<SpawnerBuildingBehaviour> _spawnersList=new List<SpawnerBuildingBehaviour>();
    [SerializeField]
    private GameObject _expItemPrefab;
    [SerializeField]
    private float _expOnWin;
    private ItemSpawner _itemSpawner;
    private int _destroyedSpawnersCounter = 0;//Kostil - 10 spawners on level so technicaly every 10 destroyed- 1 reward
    private void Start()
    {
        _itemSpawner=FindObjectOfType<ItemSpawner>();
    }
    public void UpdateSpawnersList(SpawnerBuildingBehaviour spawner)
    {
        _spawnersList.Add(spawner);
        spawner.onDestroy += RemoveSpawnerFromList;
        _expOnWin+=50;
    }
    private void RemoveSpawnerFromList(SpawnerBuildingBehaviour spawner)
    {
        _spawnersList.Remove(spawner);
        _destroyedSpawnersCounter++;
        CheckForWin();
    }
    private void CheckForWin()
    {
        if (_destroyedSpawnersCounter>=10)
        {
            _destroyedSpawnersCounter = 0;
            SpawnExpItem();
            _itemSpawner.GenerateRewards(GameObject.FindGameObjectWithTag("Player"));
        }

    }
    public void IncreaseExp(float amount)
    {
        _expOnWin += amount;
    }
    protected void SpawnExpItem()
    {
        Item newItem = new Item();
        newItem.itemName = $"ExpScroll ({_expOnWin})";
        newItem.isStackable = true;
        newItem.SetExpAmount(_expOnWin);
        Vector3 randomOffset = Random.insideUnitSphere * 4;
        randomOffset.y = 0;

        Vector3 spawnPosition = GameObject.FindGameObjectWithTag("Player").transform.position + randomOffset;
        GameObject itemObject = Instantiate(_expItemPrefab, spawnPosition, Quaternion.identity);
        itemObject.GetComponent<ItemComponent>().SetItem(newItem);
    }

}
