namespace Te.EcsFeatureRunner.Feature
{
    public interface IEcsPhysicsUpdateFeature : IEcsFeature
    {
        public void PhysicsUpdate();
    }
    
    public interface IEcsPhysicsUpdateFeature<TWorld, TSystems> : IEcsFeature<TWorld>, IEcsPhysicsUpdateFeature
    {
        public TSystems PhysicsUpdateSystems { get; set; }
    }
}