using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEngine;

namespace Risk.Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Risk/DifficultyConfig", fileName = "DifficultyConfig")]
    public class DifficultyConfig : ScriptableObject
    {
        [SerializeField] private List<DifficultyLevel> _difficultyLevels = new();
        
        public DifficultyLevel GetDifficultyByTime(float timeOnMap)
        {
            foreach (var difficultyLevel in _difficultyLevels)
            {
                if (timeOnMap >= difficultyLevel.StartTime && timeOnMap < difficultyLevel.EndTime)
                {
                    return difficultyLevel;
                }
            }
            
            Debug.LogError($"[DifficultyConfig] GetDifficultyByTime failed time on map: {timeOnMap}");
            return null;
        }
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            _difficultyLevels = _difficultyLevels.OrderBy(l => l.StartTime).ToList();

            for (int i = 0; i < _difficultyLevels.Count; i++)
            {
                var next = i + 1 < _difficultyLevels.Count ? _difficultyLevels[i + 1].StartTime : float.MaxValue;
                _difficultyLevels[i].SetEndTime(next);
                _difficultyLevels[i].SetLevel(i);
            }
        }
#endif
    }
    
    [System.Serializable]
    public class DifficultyLevel
    {
        [SerializeField, ReadOnly] private int _level;
        [SerializeField] private float _startTime;
        [SerializeField, ReadOnly] private float _endTime;
        [SerializeField] private List<DifficultyWave> _waveData;

        public int Level => _level;
        public float StartTime => _startTime;
        public float EndTime => _endTime;
        public List<DifficultyWave> WaveData => _waveData;

        public void SetLevel(int level)
        {
            _level = level;
        }
        
        public void SetEndTime(float endTime)
        {
            _endTime = endTime;
        }
    }
    
    [System.Serializable]
    public class DifficultyWave
    {
        [SerializeField] private List<WaveConfig> _waves;
        [SerializeField] private float _spawnInterval;
        
        public List<WaveConfig> Waves => _waves;
        public float SpawnInterval => _spawnInterval;
    }
    
    [System.Serializable]
    public class WaveConfig
    {
        [SerializeField] private int _enemyId;
        [SerializeField] private int _count;
        
        public int EnemyId => _enemyId;
        public int Count => _count;
    }
}
