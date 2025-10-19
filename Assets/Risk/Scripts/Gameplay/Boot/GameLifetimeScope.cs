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
        [SerializeField] private DifficultyConfig _difficultyConfig;
        [SerializeField] private AllWeaponsConfig _allWeaponsConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_allHeroesConfig);
            builder.RegisterInstance(_allEnemysConfig);
            builder.RegisterInstance(_cameraConfig);
            builder.RegisterInstance(_difficultyConfig);
            builder.RegisterInstance(_allWeaponsConfig);
            
            builder.Register<PlayerInputActions>(Lifetime.Singleton);
        }
    }
}
