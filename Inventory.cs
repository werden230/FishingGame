using System.Collections.Generic;
using System.Linq;
using FishingGame.FishSystem;

namespace FishingGame
{
    public class Inventory
    {
        private List<Fish> _fishList;
        private Dictionary<string, int> _fishCount;
        
        public int TotalFish => _fishList.Count;
        
        public Inventory()
        {
            _fishList = new List<Fish>();
            _fishCount = new Dictionary<string, int>();
        }
        
        public void AddFish(Fish fish)
        {
            _fishList.Add(fish);
            
            if (_fishCount.ContainsKey(fish.Name))
                _fishCount[fish.Name]++;
            else
                _fishCount[fish.Name] = 1;
        }
        
        public List<Fish> GetAllFish()
        {
            return new List<Fish>(_fishList);
        }
        
        public int GetFishCount(string fishName)
        {
            return _fishCount.ContainsKey(fishName) ? _fishCount[fishName] : 0;
        }
        
        public List<Fish> GetFishByRarity(FishRarity rarity)
        {
            return _fishList.Where(f => f.Rarity == rarity).ToList();
        }
        
        public void Clear()
        {
            _fishList.Clear();
            _fishCount.Clear();
        }
    }
}