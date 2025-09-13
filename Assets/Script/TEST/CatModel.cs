public sealed class CatModel
{
    public int Level { get; private set; }
    public CatModel(int level)
    {
        Level = level;
    }
}