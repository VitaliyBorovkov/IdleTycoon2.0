public class BakeryModel
{
    //private int breadStorage = 0;
    public bool IsProcessing { get; private set; }

    public void SetProcessing(bool value)
    {
        IsProcessing = value;
    }

    //public void AddBread(int amount)
    //{
    //    breadStorage += amount;
    //}

    //public int GetBreadAmount()
    //{
    //    return breadStorage;
    //}

    //public void RemoveBread(int amount)
    //{
    //    breadStorage -= amount;
    //}
}