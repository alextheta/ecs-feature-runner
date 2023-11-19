namespace Te.EcsFeatureRunner.Feature
{
    public interface IEcsDestroyFeature : IEcsFeature
    {
        public void Destroy();
    }

    public interface IEcsDestroyFeature<TWorld> : IEcsFeature<TWorld>, IEcsDestroyFeature
    {
    }
}