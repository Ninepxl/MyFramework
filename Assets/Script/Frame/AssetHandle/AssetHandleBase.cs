namespace Frame
{
    public enum AssetType
    {
        Common = 0,
        GameObject = 2,
    }
    public abstract class AssetHandleBase
    {
        public AssetType CurType;
        // 资源的标签名 (Addressable)
        public object Key;
        // 加载是否成功
        public bool Success;
        public bool IsDone;
        // 加载进度
        public float Progress => GetProgress();
        // 释放加载的资源
        public abstract void Release();
        // 加载进度
        public abstract float GetProgress();
    }
}