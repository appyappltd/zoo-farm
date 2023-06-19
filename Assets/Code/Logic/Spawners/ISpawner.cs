using System;

namespace Logic.Spawners
{
    public interface ISpawner<out T> : IDisposable
    {
        T Spawn();
    }
}