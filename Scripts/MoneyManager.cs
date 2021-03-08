using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text moneyText;

    public static MoneyManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of MoneyManager found!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        ChangeMoney(0);
    }

    public void ChangeMoney(float money)
    {
        SaveManager.instance.save.money += money;
        SaveManager.instance.Save();
        moneyText.text = $"{GetMoney()} $";
    }

    public float GetMoney()
    {
        return SaveManager.instance.save.money;
    }

    public bool HasEnoughMoney(float money)
    {
        return SaveManager.instance.save.money >= money;
    }
}
