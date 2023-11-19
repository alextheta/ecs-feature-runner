using Te.EcsFeatureRunner.Feature;

namespace Te.EcsFeatureRunner
{
    public interface IEcsFeatureRunner
    {
        public IEcsFeatureRunner AddFeature<T>(T feature) where T : class, IEcsFeature;
        public void Init();
        public void Update();
        public void PhysicsUpdate();
        public void Destroy();
    }
}