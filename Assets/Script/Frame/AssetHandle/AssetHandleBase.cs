namespace Frame
{
    public abstract class AssetHandleBase
    {
        public object Key;
        public abstract void Release();
    }
}