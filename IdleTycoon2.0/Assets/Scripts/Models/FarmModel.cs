public class FarmModel
{
    public int Level { get; private set; }

    public FarmModel()
    {
        Level = 1;
    }

    public void Upgrade()
    {
        Level++;
    }
}
