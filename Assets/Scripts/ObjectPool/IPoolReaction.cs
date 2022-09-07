namespace Runner.ObjectPool
{
    public interface IPoolReaction
    {
        public void ObjectPooled(bool isInPool);
    }
}