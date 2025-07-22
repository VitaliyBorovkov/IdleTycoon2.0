public class FarmModel
{
    public int Level { get; private set; }

    public FarmModel()
    {
        Level = 0;
    }

    public void Upgrade()
    {
        Level++;

    }

    public void SetLevel(int level)
    {
        Level = level;
    }

}
