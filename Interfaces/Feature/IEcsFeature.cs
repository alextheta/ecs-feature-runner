namespace Te.EcsFeatureRunner.Feature
{
    public interface IEcsFeature
    {
    }
    
    public interface IEcsFeature<TWorld> : IEcsFeature
    {
        public TWorld World { get; set; }
    }
}