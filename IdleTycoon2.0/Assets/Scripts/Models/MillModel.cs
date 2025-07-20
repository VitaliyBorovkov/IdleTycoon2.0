public class MillModel
{
    public bool IsProcessing { get; private set; }

    public void SetProcessing(bool value)
    {
        IsProcessing = value;
    }
}