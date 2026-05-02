using System.Collections.Generic;
using System.Linq;
using FishingGame.FishSystem;

namespace FishingGame
{
    public interface IInventoryComponent
    {
        int GetTotalPrice();
        int GetFishCount();
    }

    public class FishStack : IInventoryComponent
    {
        public const int MaxCapacity = 4;

        private readonly List<Fish> _fish;

        public string FishTypeName { get; }
        public IReadOnlyList<Fish> Fish => _fish;
        public bool IsFull => _fish.Count >= MaxCapacity;
        public bool IsEmpty => _fish.Count == 0;

        public FishStack(Fish firstFish)
        {
            FishTypeName = firstFish.Name;
            _fish = new List<Fish>();
            AddFish(firstFish);
        }

        public bool CanAccept(Fish fish)
        {
            return fish.Name == FishTypeName && !IsFull;
        }

        public bool AddFish(Fish fish)
        {
            if (!CanAccept(fish) && !IsEmpty)
            {
                return false;
            }

            _fish.Add(fish);
            return true;
        }

        public int GetTotalPrice()
        {
            return _fish.Sum(fish => fish.GetTotalPrice());
        }

        public int GetFishCount()
        {
            return _fish.Sum(fish => fish.GetFishCount());
        }
    }

    public class Inventory : IInventoryComponent
    {
        private readonly List<FishStack> _stacks;

        public int TotalFish => GetFishCount();
        public int OccupiedSlots => _stacks.Count;
        public IReadOnlyList<FishStack> Stacks => _stacks;

        public Inventory()
        {
            _stacks = new List<FishStack>();
        }

        public void AddFish(Fish fish)
        {
            FishStack availableStack = _stacks.FirstOrDefault(stack => stack.CanAccept(fish));

            if (availableStack != null)
            {
                availableStack.AddFish(fish);
                return;
            }

            _stacks.Add(new FishStack(fish));
        }

        public List<Fish> GetAllFish()
        {
            return _stacks.SelectMany(stack => stack.Fish).ToList();
        }

        public int GetFishCount(string fishName)
        {
            return _stacks
                .Where(stack => stack.FishTypeName == fishName)
                .Sum(stack => stack.GetFishCount());
        }

        public List<Fish> GetFishByRarity(FishRarity rarity)
        {
            return _stacks
                .SelectMany(stack => stack.Fish)
                .Where(fish => fish.Rarity == rarity)
                .ToList();
        }

        public int SellAll()
        {
            int totalPrice = GetTotalPrice();
            Clear();
            return totalPrice;
        }

        public void Clear()
        {
            _stacks.Clear();
        }

        public int GetTotalPrice()
        {
            return _stacks.Sum(stack => stack.GetTotalPrice());
        }

        public int GetFishCount()
        {
            return _stacks.Sum(stack => stack.GetFishCount());
        }
    }
}