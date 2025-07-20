using UnityEngine;

[CreateAssetMenu(fileName = "BreadSellSettings", menuName = "ScriptableObjects/Bakery/Bread Sell Settings")]
public class BreadSellSettings : ScriptableObject
{
    public float interval = 5f;
    public int breadPerSale = 1;
    public int moneyPerBread = 10;
    public int xpPerBread = 2;
}