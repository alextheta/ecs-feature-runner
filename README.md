# ECS feature runner
It happend that way that grouping ECS system sets by so-called 'features' considered a good practice to organize your code to make it more scalable and maintainable. Unfortunately, some ECS frameworks are lacking of this must-have feature, so this tiny project is aims to solve this issue

## Getting started with `EscFeatureRunner`
1. Begin by creating an instance of the `EcsFeatureRunner` class:
```csharp
private IEcsFeatureRunner _featureRunner;

...

_featureRunner = new EcsFeatureRunner();
```
2. Add all your features:
```csharp
_featureRunner.AddFeature(new HealthFeature())
              .AddFeature(new MovementFeature())
              .AddFeature(new NetworkFeature());
```
3. Call `EcsFeatureRunner` callbacks in your environment. For Unity, here's an example:
```csharp
private void Start()
{
    _featureRunner.Init();
}

private void Update()
{
    _featureRunner.Update();
}

private void FixedUpdate()
{
    _featureRunner.PhysicsUpdate();
}

private void OnDestroy()
{
    _featureRunner.Destroy();
}
```

## Creating features
To create a feature, implement some of the following interfaces:
* `IEcsInitFeature`: Contains an `Init` method called on `EcsFeatureRunner.Init()`.
* `IEcsUpdateFeature`: Contains an `Update` method called on `EcsFeatureRunner.Update()`.
* `IEcsPhysicsUpdateFeature`: Contains a `PhysicsUpdate` method called on `EcsFeatureRunner.PhysicsUpdate()`.
* `IEcsDestroyFeature`: Contains a `Destroy` method called on `EcsFeatureRunner.Destroy()`.

Extend `EcsFeatureRunner` with your own feature types if you need more types of callbacks.

## Generic features
Some ECS frameworks share similarities in their API design. They typically offer a class for managing the game world and another class for handling component storage. For instance, both [LeoECS Lite](https://github.com/Leopotam/ecslite) and [Morpeh](https://github.com/scellecs/morpeh) provide `EcsWorld` and `World` as the game world classes, and `IEcsPool` and `Stash` as component storage.
To enhance the convenience of working with these frameworks, you can create features more comfortably by utilizing generic versions of the `IEcsFeature` interfaces. These interfaces include additional fields that contribute to an improved development experience.

### Example of generic features for LeoECS Lite
1. Feature interfaces:
```csharp
using Leopotam.EcsLite;

public interface ILeoLiteEcsInitFeature : IEcsInitFeature<EcsWorld>
{
}

public interface ILeoLiteEcsUpdateFeature : IEcsUpdateFeature<EcsWorld, IEcsSystems>
{
}

public interface ILeoLiteEcsPhysicsUpdateFeature : IEcsPhysicsUpdateFeature<EcsWorld, IEcsSystems>
{
}

public interface ILeoLiteEcsDestroyFeature : IEcsDestroyFeature<EcsWorld>
{
}
```
2. Example of feature:
```csharp
public class LeoEcsFeature : ILeoLiteEcsInitFeature, ILeoLiteEcsUpdateFeature, ILeoLiteEcsPhysicsUpdateFeature, ILeoLiteEcsDestroyFeature
{
    public EcsWorld World { get; set; } // provided by generic IEcsFeature
    public IEcsSystems UpdateSystems { get; set; } // provided by generic IEcsUpdateFeature
    public IEcsSystems PhysicsUpdateSystems { get; set; } // provided by generic IEcsPhysicsUpdateFeature

    public void Init()
    {
        World = new EcsWorld();
        UpdateSystems = new EcsSystems(World);
        PhysicsUpdateSystems = new EcsSystems(World);

        UpdateSystems.Add(new SomeSystem())
                     .Add(new AnotherSystem())
                     .Add(new CleanupSystem());

        PhysicsUpdateSystems.Add(new SomePhysicsSystem())
                            .Add(new AnimationSystem())
                            .Add(new YetAnotherSystem());

        UpdateSystems.Init();
        PhysicsUpdateSystems.Init();
    }

    public void Update()
    {
        UpdateSystems.Run();
    }

    public void PhysicsUpdate()
    {
        PhysicsUpdateSystems.Run();
    }

    public void Destroy()
    {
        UpdateSystems.Destroy();
        PhysicsUpdateSystems.Destroy();
        World.Destroy();
    }
}
```

### Example of generic features for Morpeh
1. Feature interfaces:
```csharp
using Scellecs.Morpeh;

public interface IMorpehEcsInitFeature : IEcsInitFeature<World>
{
}

public interface IMorpehEcsUpdateFeature : IEcsUpdateFeature<World, SystemsGroup>
{
}

public interface IMorpehEcsDestroyFeature : IEcsDestroyFeature<World>
{
}
```
2. Example of feature:
```csharp
// Morpeh world can handle IFixedSystem in the same system container with ISystem
public class MorpehEcsFeature : IMorpehEcsInitFeature, IMorpehEcsUpdateFeature, IEcsPhysicsUpdateFeature, IMorpehEcsDestroyFeature
{
    public World World { get; set; } // provided by generic IEcsFeature
    public SystemsGroup UpdateSystems { get; set; } // provided by generic IEcsUpdateFeature

    public void Init()
    {
        World = World.Create();
        UpdateSystems = World.CreateSystemsGroup();

        UpdateSystems.AddSystem(new SomeSystem());
        UpdateSystems.AddSystem(new AnotherSystem());
        UpdateSystems.AddSystem(new CleanupSystem());

        UpdateSystems.AddSystem(new SomePhysicsFixedSystem());
        UpdateSystems.AddSystem(new AnimationFixedSystem());
        UpdateSystems.AddSystem(new YetAnotherFixedSystem());

        World.AddSystemsGroup(0, UpdateSystems);
    }

    public void Update()
    {
        World.Update(Time.deltaTime);
    }

    public void PhysicsUpdate()
    {
        World.FixedUpdate(Time.deltaTime);
    }

    public void Destroy()
    {
        World.Dispose();
    }
}
```
