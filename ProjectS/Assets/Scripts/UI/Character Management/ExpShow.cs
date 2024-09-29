using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpShow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _expText;
    [SerializeField]
    private Image _expBarSprite;
    private PlayerLevelSystem _playerLevelSystem;
    // Start is called before the first frame update
    void Start()
    {
        _playerLevelSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLevelSystem>();
        _playerLevelSystem.onExpChange += UpdateExpBar;
    }
    public void UpdateExpBar(float maxExp, float currentExp)
    {
        _expBarSprite.fillAmount = currentExp / maxExp;
        _expText.text = $"{currentExp}/{maxExp}";
    }

}
