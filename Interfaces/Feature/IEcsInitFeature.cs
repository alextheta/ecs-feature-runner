namespace Te.EcsFeatureRunner.Feature
{
    public interface IEcsInitFeature : IEcsFeature
    {
        public void Init();
    }
    
    public interface IEcsInitFeature<TWorld> : IEcsFeature<TWorld>, IEcsInitFeature
    {
    }
}