namespace Te.EcsFeatureRunner.Feature
{
    public interface IEcsUpdateFeature : IEcsFeature
    {
        public void Update();
    }
    
    public interface IEcsUpdateFeature<TWorld, TSystems> : IEcsFeature<TWorld>, IEcsUpdateFeature
    {
        public TSystems UpdateSystems { get; set; }
    }
}