using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftStatistics
{
  enum eStatisticType
  {
    MostCommon,
    ItemsCount,
    ItemDistribution,
    ItemLocations,
    Temples,
    Biomes,
    BiomeExists,
    NewBlockTypes,
    NewBiomeTypes,
    GetClusters,
    GetSpawners,
    GetChests
  }
  class StatisticType
  {
    private string _name;
    private bool _needsItem;
    private bool _needsBiome;
    private eStatisticType _statType;
    private static StatisticType[] _cachedStatTypes = null;

    public static StatisticType[] GetStatisticTypes()
    {
      if (_cachedStatTypes == null)
      {
        _cachedStatTypes = new StatisticType[12];
        _cachedStatTypes[0] = new StatisticType("Most common", eStatisticType.MostCommon, false, false);
        _cachedStatTypes[1] = new StatisticType("Count all items", eStatisticType.ItemsCount, false, false);
        _cachedStatTypes[2] = new StatisticType("Item Distribution", eStatisticType.ItemDistribution, true, false);
        _cachedStatTypes[3] = new StatisticType("Item Locations", eStatisticType.ItemLocations, true, false);
        _cachedStatTypes[4] = new StatisticType("Temple Locations", eStatisticType.Temples, false, false);
        _cachedStatTypes[5] = new StatisticType("Biomes", eStatisticType.Biomes, false, false);
        _cachedStatTypes[6] = new StatisticType("Check if a biome exists", eStatisticType.BiomeExists, false, true);
        _cachedStatTypes[7] = new StatisticType("Check for new block types", eStatisticType.NewBlockTypes, false, false);
        _cachedStatTypes[8] = new StatisticType("Check for new biome types", eStatisticType.NewBiomeTypes, false, false);
        _cachedStatTypes[9] = new StatisticType("Get cluster size", eStatisticType.GetClusters, true, false);
        _cachedStatTypes[10] = new StatisticType("Get spawners", eStatisticType.GetSpawners, false, false);
        _cachedStatTypes[11] = new StatisticType("Get chests", eStatisticType.GetChests, false, false);
      }
      return _cachedStatTypes;
    }

    public string Name { get { return _name; } }
    public bool NeedsItem { get { return _needsItem; } }
    public bool NeedsBiome { get { return _needsBiome; } }
    public eStatisticType StatType { get { return _statType; } }

    private StatisticType(string name, eStatisticType statType, bool needsItem, bool needsBiome)
    {
      _name = name;
      _statType = statType;
      _needsItem = needsItem;
      _needsBiome = needsBiome;
    }

    public override string ToString()
    {
      return _name;
    }
  }
}
