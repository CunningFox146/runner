using System;

namespace Runner.Managers.ObjectPool
{
    public interface IPoolReaction
    {
        public void ObjectPooled(bool isInPool);
    }
}