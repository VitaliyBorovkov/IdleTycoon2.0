public class BakeryModel
{
    public bool IsProcessing { get; private set; }

    public void SetProcessing(bool value)
    {
        IsProcessing = value;
    }
}