using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class RareUpdateSystem : ISystem 
    {
        public World World { get; set;}
        protected abstract float updateTime { get;}
        protected abstract bool isHaveFirstTick { get;}

        private float _timer = 0f;

        public virtual void OnAwake()
        {
            if (isHaveFirstTick)
            {
                RareUpdate(0);
            }
        }
    
        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer >= updateTime)
            {
                _timer = 0f;
                RareUpdate(updateTime);
            }
        }

        protected abstract void RareUpdate(float time);

        public abstract void Dispose();
    }
}