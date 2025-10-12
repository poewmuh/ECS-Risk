using Risk.Gameplay.Configs;
using Risk.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Risk.Gameplay.Boot
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AllHeroesConfig _allHeroesConfig;
        [SerializeField] private AllEnemysConfig _allEnemysConfig;
        [SerializeField] private CameraConfig _cameraConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_allHeroesConfig);
            builder.RegisterInstance(_allEnemysConfig);
            builder.RegisterInstance(_cameraConfig);
            
            builder.Register<PlayerInputActions>(Lifetime.Singleton);
        }
    }
}
