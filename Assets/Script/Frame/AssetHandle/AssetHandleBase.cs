namespace Frame
{
    public abstract class AssetHandleBase
    {
        public object Key;
        public bool Success;
        public float Progress => GetProgress();
        // 释放加载的资源
        public abstract void Release();
        public abstract float GetProgress();
    }
}