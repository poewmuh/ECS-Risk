using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Risk.Gameplay.ECS.Components.Spawn
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpawnRequestComponent : IComponent
    {
        public List<SpawnRequest> requests;
    }

    public struct SpawnRequest
    {
        public SpawnRequest(int enemyId)
        {
            this.enemyId = enemyId;
        }
        
        public int enemyId;
    }
}