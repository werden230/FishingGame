using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FishingGame.FishSystem
{
    public interface IGameLogger
    {
        void Info(string message);
        void Error(string message, Exception exception);
    }

    public sealed class FileGameLogger : IGameLogger
    {
        private static readonly object SyncRoot = new object();
        private readonly string _logFilePath;

        public FileGameLogger(string baseDirectory)
        {
            string logsDirectory = Path.Combine(baseDirectory, "Logs");
            Directory.CreateDirectory(logsDirectory);

            string fileName = $"fishinggame-{DateTime.Now:yyyy-MM-dd}.log";
            _logFilePath = Path.Combine(logsDirectory, fileName);
        }

        public void Info(string message)
        {
            Write("INFO", message);
        }

        public void Error(string message, Exception exception)
        {
            string fullMessage = $"{message} | {exception.GetType().Name}: {exception.Message}";
            Write("ERROR", fullMessage);
        }

        private void Write(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            string line = $"[{timestamp}] [{level}] {message}";

            lock (SyncRoot)
            {
                File.AppendAllText(_logFilePath, line + Environment.NewLine);
            }

            Console.WriteLine(line);
        }
    }

    public sealed class LoggingFishFactoryProxy : IFishFactory
    {
        private readonly IFishFactory _innerFactory;
        private readonly IGameLogger _logger;
        private readonly BiomeType _biomeType;

        public LoggingFishFactoryProxy(IFishFactory innerFactory, IGameLogger logger, BiomeType biomeType)
        {
            _innerFactory = innerFactory ?? throw new ArgumentNullException(nameof(innerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _biomeType = biomeType;
        }

        public Fish CreateRandomFish()
        {
            try
            {
                Fish fish = _innerFactory.CreateRandomFish();
                _logger.Info($"CreateRandomFish biome={_biomeType} result={FormatFish(fish)}");
                return fish;
            }
            catch (Exception ex)
            {
                _logger.Error($"CreateRandomFish failed biome={_biomeType}", ex);
                throw;
            }
        }

        public Fish CreateFishByRarity(FishRarity rarity)
        {
            try
            {
                Fish fish = _innerFactory.CreateFishByRarity(rarity);
                _logger.Info($"CreateFishByRarity biome={_biomeType} requested={rarity} result={FormatFish(fish)}");
                return fish;
            }
            catch (Exception ex)
            {
                _logger.Error($"CreateFishByRarity failed biome={_biomeType} requested={rarity}", ex);
                throw;
            }
        }

        public List<Fish> GetAvailableFish()
        {
            try
            {
                List<Fish> fishList = _innerFactory.GetAvailableFish();
                _logger.Info($"GetAvailableFish biome={_biomeType} count={fishList.Count}");
                return fishList;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAvailableFish failed biome={_biomeType}", ex);
                throw;
            }
        }

        private static string FormatFish(Fish fish)
        {
            if (fish == null)
            {
                return "null";
            }

            return $"{fish.Name}/{fish.Rarity}/price={fish.Price}/size={fish.Size}/weight={fish.Weight:F2}";
        }
    }
}
