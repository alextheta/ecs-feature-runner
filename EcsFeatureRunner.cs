using System.Collections.Generic;
using Te.EcsFeatureRunner.Feature;

namespace Te.EcsFeatureRunner
{
    public abstract class EcsFeatureRunner : IEcsFeatureRunner
    {
        private List<IEcsInitFeature> _initFeatures;
        private List<IEcsUpdateFeature> _updateFeatures;
        private List<IEcsPhysicsUpdateFeature> _physicsUpdateFeatures;
        private List<IEcsDestroyFeature> _destroyFeatures;

        public virtual IEcsFeatureRunner AddFeature<T>() where T : class, IEcsFeature, new()
        {
            return AddFeature(new T());
        }

        public virtual IEcsFeatureRunner AddFeature<T>(T feature) where T : class, IEcsFeature
        {
            if (feature is IEcsInitFeature initFeature)
            {
                _initFeatures ??= new List<IEcsInitFeature>();
                _initFeatures.Add(initFeature);
            }

            if (feature is IEcsUpdateFeature updateFeature)
            {
                _updateFeatures ??= new List<IEcsUpdateFeature>();
                _updateFeatures.Add(updateFeature);
            }

            if (feature is IEcsPhysicsUpdateFeature physicsUpdateFeature)
            {
                _physicsUpdateFeatures ??= new List<IEcsPhysicsUpdateFeature>();
                _physicsUpdateFeatures.Add(physicsUpdateFeature);
            }

            if (feature is IEcsDestroyFeature destroyFeature)
            {
                _destroyFeatures ??= new List<IEcsDestroyFeature>();
                _destroyFeatures.Add(destroyFeature);
            }

            return this;
        }

        public virtual void Init()
        {
            if (_initFeatures == null)
            {
                return;
            }

            foreach (var feature in _initFeatures)
            {
                feature.Init();
            }
        }

        public virtual void Update()
        {
            if (_updateFeatures == null)
            {
                return;
            }

            foreach (var feature in _updateFeatures)
            {
                feature.Update();
            }
        }

        public virtual void PhysicsUpdate()
        {
            if (_physicsUpdateFeatures == null)
            {
                return;
            }

            foreach (var feature in _physicsUpdateFeatures)
            {
                feature.PhysicsUpdate();
            }
        }

        public virtual void Destroy()
        {
            if (_destroyFeatures == null)
            {
                return;
            }

            foreach (var feature in _destroyFeatures)
            {
                feature.Destroy();
            }

            _initFeatures = null;
            _updateFeatures = null;
            _physicsUpdateFeatures = null;
            _destroyFeatures = null;
        }
    }
}