using UnityEngine;

public interface IPlayerInput
{
    Vector2 Move { get; }
    bool InteractDown { get; }
}

public interface IPlayerMover
{
    float Speed { get; set; }
    void Move(Vector3 worldDir);
    void Face(Vector3 worldDir);
}

public interface IInventory
{
    bool CanAddBean { get; }
    bool PushBean(BeanBag b);
    bool PopBean(out BeanBag b);
    bool HasCup { get; }
    bool PushCup(CoffeeCup c);
    bool PopCup(out CoffeeCup c);
}

public interface IInteractable
{
    bool TryInteract(PlayerContext ctx);
}

public interface IResourceProvider
{
    BeanBag CreateBean(Vector3 hintPos);
}

public interface ICoffeeProcessor
{
    bool CanInsertBean { get; }
    void InsertBean();
    bool TryTakeCup(out CoffeeCup cup);
}

public interface IScoreService
{
    int Coins { get; }
    void Add(int amount);
}

public interface ICurrencyFX
{
    void FlyFromWorld(Vector3 worldPos, int amount, RectTransform target);
}

public interface ICustomerLifecycle
{
    void Served();
}

public interface ICustomerFactory
{
    CustomerInteractable Spawn();
}

public interface IPoolable
{
    void OnSpawnFromPool();
    void OnReturnToPool();
}

public interface IObjectPool<T> where T : Component
{
    T Get();
    void Release(T instance);
}

public interface IPoolService
{
    T Get<T>(T prefab) where T : Component;
    void Release<T>(T instance) where T : Component;
}

public interface IWeightedInteractable : IInteractable
{
    int GetPriority(PlayerContext ctx);
}

public interface IBeanReadable
{
    int BeanCount { get; }
}
