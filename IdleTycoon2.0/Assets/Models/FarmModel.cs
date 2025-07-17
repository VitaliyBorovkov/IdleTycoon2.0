public class FarmModel
{
    public int Level { get; private set; }
    public float ProductionInterval { get; private set; }
    public int GrainPerCycle { get; private set; }

    public FarmModel(int level, float interval, int grainPerCycle)
    {
        Level = level;
        ProductionInterval = interval;
        GrainPerCycle = grainPerCycle;
    }

    public void Upgrade(int newLevel, float newInterval, int newGrainPerCycle)
    {
        Level = newLevel;
        ProductionInterval = newInterval;
        GrainPerCycle = newGrainPerCycle;
    }
}
