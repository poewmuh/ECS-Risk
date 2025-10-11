using Risk.Gameplay.Configs;
using Risk.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Risk.Gameplay.Boot
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AllCharactersConfig _allCharactersConfig;
        [SerializeField] private CameraConfig _cameraConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_allCharactersConfig);
            builder.RegisterInstance(_cameraConfig);
            
            builder.Register<PlayerInputActions>(Lifetime.Singleton);
        }
    }
}
