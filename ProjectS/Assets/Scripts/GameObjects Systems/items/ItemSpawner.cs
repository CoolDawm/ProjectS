using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemGenerator itemGenerator;
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public KeyCode spawnKey = KeyCode.E; // Клавиша для генерации предмета
    public float spawnRadius = 4f;
    private bool playerInRange = false;
    private void Start()
    {
        itemGenerator=FindObjectOfType<ItemGenerator>();
    }
    public ItemComponent GenerateItem(GameObject player, float charIncrease, float offset)
    {
        Item newItem = itemGenerator.GenerateRandomWeapon(charIncrease); 
        Vector3 spawnPosition = player.transform.position + player.transform.forward * offset;

        GameObject itemObject = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        itemObject.GetComponent<ItemComponent>().SetItem(newItem);
        return itemObject.GetComponent<ItemComponent>();
    }

    public void GenerateRewards(GameObject player)
    {
        float distanceBetweenItems = 2.0f; // Расстояние между предметами
        ItemComponent item1 = GenerateItem(player, 1, distanceBetweenItems);
        ItemComponent item2 = GenerateItem(player, 1.15f, distanceBetweenItems * 2);
        ItemComponent item3 = GenerateItem(player, 1.3f, distanceBetweenItems * 3);

        item1.isRewardToChoose = true;
        item2.isRewardToChoose = true;
        item3.isRewardToChoose = true;

        item1.onChoose += item2.DestroyNotChosenItem;
        item1.onChoose += item3.DestroyNotChosenItem;
        item2.onChoose += item1.DestroyNotChosenItem;
        item2.onChoose += item3.DestroyNotChosenItem;
        item3.onChoose += item2.DestroyNotChosenItem;
        item3.onChoose += item1.DestroyNotChosenItem;
    }

}
