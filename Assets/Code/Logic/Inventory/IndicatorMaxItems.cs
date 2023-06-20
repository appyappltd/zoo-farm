using Data.ItemsData;
using UnityEngine;

public class IndicatorMaxItems : MonoBehaviour
{
    [SerializeField] private Bubble _bubble;

    private Inventory _inventory;

    public void Awake()
    {
        _inventory = GetComponent<Inventory>();
        _inventory.AddItem += UpdateIndicator;
        _inventory.RemoveItem += () => UpdateIndicator();

        _bubble.gameObject.SetActive(false);
    }

    private void UpdateIndicator(HandItem item = null)
    {
        if (item && _inventory.IsMax)
            _bubble.gameObject.SetActive(true);
        else
            _bubble.gameObject.SetActive(false);
    }
}
