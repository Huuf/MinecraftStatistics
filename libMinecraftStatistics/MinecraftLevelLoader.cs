using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

/**
 * Copyright Mojang AB. Converted/Modified by Huuf
 * 
 * Don't do evil.
 */
namespace libMinecraftStatistics {
  /// <summary>
  /// Set the maximum progress value and reset the value to 0
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="maxValue"></param>
  public delegate void SetProgressMaximumHandler(object sender, int maxValue);
  /// <summary>
  /// Set the current progress value
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="Value"></param>
  public delegate void SetProgressHandler(object sender, int Value);

  /// <summary>
  /// Load an Minecraft Level
  /// </summary>
  public class MinecraftLevelLoader {
    /// <summary>
    /// All map files for the current level
    /// </summary>
    string[] _MapFiles = null;

    /// <summary>
    /// The Map Seed
    /// </summary>
    long _Seed = 0;

    /// <summary>
    /// The Map Seed
    /// </summary>
    public long Seed {
      get { return _Seed; }
    }

    /// <summary>
    /// Set the maximum progress value and reset the value to 0
    /// </summary>
    public event SetProgressMaximumHandler SetProgressMaximum;
    /// <summary>
    /// Set the current progress value
    /// </summary>
    public event SetProgressHandler SetProgressHandler;

    /// <summary>
    /// Call the progress maximum value
    /// </summary>
    /// <param name="maxValue">The maximum value of progress</param>
    private void onSetProgressMaximum(int maxValue) {
      if (SetProgressMaximum != null)
        SetProgressMaximum(this, maxValue);
    }

    /// <summary>
    /// Call the progress value
    /// </summary>
    /// <param name="maxValue">The </param>
    private void onSetProgress(int progress) {
      if (SetProgressHandler != null)
        SetProgressHandler(this, progress);
    }

    /// <summary>
    /// Construct an Level Loader
    /// </summary>
    /// <param name="path">Path to an Minecraft Level</param>
    public MinecraftLevelLoader(string path) {
      if (!File.Exists(path + "\\level.dat")) throw new Exception("Invalid map directory");

      FileStream fs = new FileStream(path + "\\level.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
      byte[] uncompressed = GeneralFunctions.GZipDecompress(fs);
      fs.Close();

      if (uncompressed[0] == 0) return;

      int position = 1;
      string sTMP = GeneralFunctions.ReadUTF8(uncompressed, ref position);
      Tag tmp = Tag.newTag(uncompressed[0]);
      tmp.load(uncompressed, ref position, 0);

      _Seed = ((LongTag)((CompoundTag)((CompoundTag)tmp).value["Data"]).value["RandomSeed"]).value;

      _MapFiles = Directory.GetFiles(path + "\\region", "*.mca");
      for (int i = 0; i < (1 < _MapFiles.Length ? 1 : 0); i++) {
        StreamReader sr = new StreamReader(_MapFiles[i]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int j = 0; j < 32; j++) {
          for (int k = 0; k < 32; k++) {
            byte[] data = a.getChunkData(j, k);
            if (data != null) {

              if (data[0] != 0) {
                position = 1;
                sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
              }
            }
          }
        }
        sr.Close();
      }
    }

    /// <summary>
    /// Does a certain Biome Type Exist
    /// </summary>
    /// <param name="biome">The Biome to search for</param>
    /// <returns>True if the biome exists; else false</returns>
    public bool BiomeExists(enumBiomes biome) {
      onSetProgressMaximum(_MapFiles.Length);
      for (int ii = 0; ii < _MapFiles.Length; ii++) {
        onSetProgress(ii);
        StreamReader sr = new StreamReader(_MapFiles[ii]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int jj = 0; jj < 32; jj++) {
          for (int kk = 0; kk < 32; kk++) {
            byte[] data = a.getChunkData(jj, kk);
            if (data != null) {

              if (data[0] != 0) {
                int position = 1;
                string sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
                ByteArrayTag biomes = (ByteArrayTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["Biomes"];
                for (int j = 0; j < biomes.value.Length; j++) {
                  if (biomes.value[j] == (byte)biome) {
                    sr.Close();
                    return true;
                  }
                }

              }
            }
          }
        }
        sr.Close();
      }
      onSetProgress(_MapFiles.Length);

      return false;
    }

    /// <summary>
    /// Does a certain Biome Type Exist
    /// </summary>
    /// <param name="biome">The Biome to search for</param>
    /// <returns>True if the biome exists; else false</returns>
    public enumBiomes[] Biomes() {
      List<enumBiomes> lRet = new List<enumBiomes>();
      onSetProgressMaximum(_MapFiles.Length);
      for (int ii = 0; ii < _MapFiles.Length; ii++) {
        onSetProgress(ii);
        StreamReader sr = new StreamReader(_MapFiles[ii]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int jj = 0; jj < 32; jj++) {
          for (int kk = 0; kk < 32; kk++) {
            byte[] data = a.getChunkData(jj, kk);
            if (data != null) {

              if (data[0] != 0) {
                int position = 1;
                string sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
                ByteArrayTag biomes = (ByteArrayTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["Biomes"];
                for (int j = 0; j < biomes.value.Length; j++) {
                  if (!lRet.Contains((enumBiomes)biomes.value[j])) {
                    lRet.Add((enumBiomes)biomes.value[j]);
                  }
                }
              }
            }
          }
        }
        sr.Close();
      }
      onSetProgress(_MapFiles.Length);

      return lRet.ToArray();
    }

    /// <summary>
    /// On which level is a certain item?
    /// </summary>
    /// <param name="ID">The item to search for</param>
    /// <returns>An Dictionary where the key is the level, and the value is the amount</returns>
    public Dictionary<int, ulong> FindItemOnLevels(enumBlockTypes ID) {
      Dictionary<int, ulong> dRet = new Dictionary<int, ulong>();

      for (int i = 0; i < 256; i++) {
        dRet.Add(i, 0);
      }

      onSetProgressMaximum(_MapFiles.Length);
      for (int ii = 0; ii < _MapFiles.Length; ii++) {
        onSetProgress(ii);
        StreamReader sr = new StreamReader(_MapFiles[ii]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int jj = 0; jj < 32; jj++) {
          for (int kk = 0; kk < 32; kk++) {
            byte[] data = a.getChunkData(jj, kk);
            if (data != null) {

              if (data[0] != 0) {
                int position = 1;
                string sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
                ListTag sections = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["Sections"];
                int secX = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["xPos"]).value;
                int secZ = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["zPos"]).value;

                for (int j = 0; j < sections.value.Count; j++) {
                  CompoundTag section = (CompoundTag)sections.value[j];
                  ByteArrayTag blocks = (ByteArrayTag)section.value["Blocks"];
                  byte secY = ((ByteTag)section.value["Y"]).value;

                  if (section.value.ContainsKey("Add")) {
                    throw new NotImplementedException();
                  }
                  else {
                    for (int x = 0; x < 16; x++) {
                      for (int y = 0; y < 16; y++) {
                        for (int z = 0; z < 16; z++) {
                          int index = 16 * (y * 16 + z) + x;
                          if (blocks.value[index] == (int)ID) {
                            int yItem = secY * 16 + y;
                            if (yItem < 249) {
                              dRet[yItem] = dRet[yItem] + 1;
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      for (int i = 0; i < 256; i++) {
        if (dRet[i] == 0) {
          dRet.Remove(i);
        }
      }

      onSetProgress(_MapFiles.Length);

      return dRet;
    }

    /// <summary>
    /// Get all items in a chest, group it, and make it neat layout
    /// </summary>
    /// <param name="tileEntities">The tile entities </param>
    /// <param name="chest">The chest to look for and to make neat, results will store here.</param>
    private void MakeNeatChestContent(ListTag tileEntities, FoundItem chest) {
      if ((tileEntities == null) || (tileEntities.value.Count == 0)) return;
      for (int i = 0; i < tileEntities.value.Count; i++) {
        CompoundTag entity = (CompoundTag)tileEntities.value[i];
        if ((chest.x == ((IntTag)entity.value["x"]).value) && (chest.y == ((IntTag)entity.value["y"]).value) && (chest.z == ((IntTag)entity.value["z"]).value)) {
          ListTag items = (ListTag)entity.value["Items"];
          if (items.value.Count != 0) {
            Dictionary<string, int> dItems = new Dictionary<string, int>();
            for (int j = 0; j < items.value.Count; j++) {
              CompoundTag chestItem = (CompoundTag)items.value[j];
              string sId = ((StringTag)chestItem.value["id"]).value;
              if (chestItem.value.ContainsKey("tag")) {
                CompoundTag tag = (CompoundTag)chestItem.value["tag"];
                if (tag.value.ContainsKey("StoredEnchantments")) {
                  ListTag enchantments = (ListTag)((CompoundTag)chestItem.value["tag"]).value["StoredEnchantments"];
                  if (enchantments.value.Count > 0) {
                    sId += "{";
                    for (int k = 0; k < enchantments.value.Count; k++) {
                      enumEnchantments eEnch = (enumEnchantments)((ShortTag)((CompoundTag)enchantments.value[k]).value["id"]).value;
                      short lvl = ((ShortTag)((CompoundTag)enchantments.value[k]).value["lvl"]).value;
                      if (k > 0) sId += ", ";
                      sId += eEnch.ToString() + " [lvl " + lvl + "]";
                    }
                    sId += "}";
                  }
                }
              }
              if (!dItems.ContainsKey(sId)) {
                dItems.Add(sId, 0);
              }
              dItems[sId] = dItems[sId] + ((ByteTag)chestItem.value["Count"]).value;
            }
            string sData = "";
            foreach (string key in dItems.Keys) {
              if (sData != "") {
                sData += ", ";
              }
              sData += key + " (" + dItems[key] + ")";
            }
            chest.Data = dItems;
            chest.Extra = sData;
          }
        }

      }

    }

    /// <summary>
    /// Get the type of Spawner and add it to the spawner
    /// </summary>
    /// <param name="tileEntities">The tile entities</param>
    /// <param name="spawner">The spawner to look up</param>
    private void MakeNeatSpawner(ListTag tileEntities, FoundItem spawner) {
      if ((tileEntities == null) || (tileEntities.value.Count == 0)) return;
      for (int i = 0; i < tileEntities.value.Count; i++) {
        CompoundTag entity = (CompoundTag)tileEntities.value[i];
        if ((spawner.x == ((IntTag)entity.value["x"]).value) && (spawner.y == ((IntTag)entity.value["y"]).value) && (spawner.z == ((IntTag)entity.value["z"]).value)) {
          spawner.Extra = ((StringTag)entity.value["EntityId"]).value;
        }
      }
    }

    /// <summary>
    /// Find an item
    /// </summary>
    /// <param name="ID">The ID to find</param>
    /// <returns>All the locations of the items</returns>
    public FoundItem[] FindItems(enumBlockTypes ID) {
      List<FoundItem> lRet = new List<FoundItem>();

      onSetProgressMaximum(_MapFiles.Length);
      for (int ii = 0; ii < _MapFiles.Length; ii++) {
        onSetProgress(ii);
        StreamReader sr = new StreamReader(_MapFiles[ii]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int jj = 0; jj < 32; jj++) {
          for (int kk = 0; kk < 32; kk++) {
            byte[] data = a.getChunkData(jj, kk);
            if (data != null) {

              if (data[0] != 0) {
                int position = 1;
                string sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
                ListTag sections = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["Sections"];
                ListTag tileEntities = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["TileEntities"];
                int secX = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["xPos"]).value;
                int secZ = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["zPos"]).value;

                for (int j = 0; j < sections.value.Count; j++) {
                  CompoundTag section = (CompoundTag)sections.value[j];
                  ByteArrayTag blocks = (ByteArrayTag)section.value["Blocks"];
                  byte secY = ((ByteTag)section.value["Y"]).value;

                  if (section.value.ContainsKey("Add")) {
                    throw new NotImplementedException();
                  }
                  else {
                    for (int index = 0; index < blocks.value.Length; index++) {
                      if (blocks.value[index] == (int)ID) {
                        if ((secY * 16) + ((index >> 8) & 0xf) < 249) {
                          FoundItem fi = new FoundItem();
                          fi.x = (secX * 16) + (index & 0xf);
                          fi.y = (secY * 16) + ((index >> 8) & 0xf);
                          fi.z = (secZ * 16) + ((index >> 4) & 0xf);
                          fi.id = (enumBlockTypes)blocks.value[index];
                          switch (ID) {
                            case enumBlockTypes.chest:
                              MakeNeatChestContent(tileEntities, fi);
                              break;
                            case enumBlockTypes.mob_spawner:
                              MakeNeatSpawner(tileEntities, fi);
                              break;
                          }

                          lRet.Add(fi);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      onSetProgress(_MapFiles.Length);

      return lRet.ToArray();
    }

    /// <summary>
    /// Find multiple items in one go
    /// </summary>
    /// <param name="IDs">An array of IDs to find</param>
    /// <returns>An Dictionary of items, key is the item, value is an list of items</returns>
    public Dictionary<enumBlockTypes, List<FoundItem>> FindMultipleItems(enumBlockTypes[] IDs) {
      Dictionary<enumBlockTypes, List<FoundItem>> dRet = new Dictionary<enumBlockTypes, List<FoundItem>>();
      int[] iIds = new int[IDs.Length];
      for (int i = 0; i < IDs.Length; i++) {
        iIds[i] = (int)IDs[i];
        dRet.Add(IDs[i], new List<FoundItem>());
      }
      List<int> lIds = new List<int>(iIds);

      onSetProgressMaximum(_MapFiles.Length);
      for (int ii = 0; ii < _MapFiles.Length; ii++) {
        onSetProgress(ii);
        StreamReader sr = new StreamReader(_MapFiles[ii]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int jj = 0; jj < 32; jj++) {
          for (int kk = 0; kk < 32; kk++) {
            byte[] data = a.getChunkData(jj, kk);
            if (data != null) {

              if (data[0] != 0) {
                int position = 1;
                string sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
                ListTag sections = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["Sections"];
                ListTag tileEntities = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["TileEntities"];
                int secX = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["xPos"]).value;
                int secZ = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["zPos"]).value;

                for (int j = 0; j < sections.value.Count; j++) {
                  CompoundTag section = (CompoundTag)sections.value[j];
                  ByteArrayTag blocks = (ByteArrayTag)section.value["Blocks"];
                  byte secY = ((ByteTag)section.value["Y"]).value;

                  if (section.value.ContainsKey("Add")) {
                    throw new NotImplementedException();
                  }
                  else {
                    for (int index = 0; index < blocks.value.Length; index++) {
                      if (lIds.Contains(blocks.value[index])) {
                        if ((secY * 16) + ((index >> 8) & 0xf) < 249) {
                          FoundItem fi = new FoundItem();
                          fi.x = (secX * 16) + (index & 0xf);
                          fi.y = (secY * 16) + ((index >> 8) & 0xf);
                          fi.z = (secZ * 16) + ((index >> 4) & 0xf);
                          fi.id = (enumBlockTypes)blocks.value[index];
                          switch ((enumBlockTypes)fi.id) {
                            case enumBlockTypes.chest:
                              MakeNeatChestContent(tileEntities, fi);
                              break;
                            case enumBlockTypes.mob_spawner:
                              MakeNeatSpawner(tileEntities, fi);
                              break;
                          }

                          dRet[(enumBlockTypes)fi.id].Add(fi);
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      onSetProgress(_MapFiles.Length);

      return dRet;
    }

    /// <summary>
    /// Gets the item count of each item
    /// </summary>
    /// <returns>An Diction of items count, key is the item, value is the count</returns>
    public Dictionary<enumBlockTypes, ulong> GetItemCount() {
      Dictionary<enumBlockTypes, ulong> dRet = new Dictionary<enumBlockTypes, ulong>();

      onSetProgressMaximum(_MapFiles.Length);
      for (int ii = 0; ii < _MapFiles.Length; ii++) {
        onSetProgress(ii);
        StreamReader sr = new StreamReader(_MapFiles[ii]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int jj = 0; jj < 32; jj++) {
          for (int kk = 0; kk < 32; kk++) {
            byte[] data = a.getChunkData(jj, kk);
            if (data != null) {

              if (data[0] != 0) {
                int position = 1;
                string sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
                ListTag sections = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["Sections"];
                int secX = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["xPos"]).value;
                int secZ = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["zPos"]).value;

                for (int j = 0; j < sections.value.Count; j++) {
                  CompoundTag section = (CompoundTag)sections.value[j];
                  ByteArrayTag blocks = (ByteArrayTag)section.value["Blocks"];
                  byte secY = ((ByteTag)section.value["Y"]).value;

                  if (section.value.ContainsKey("Add")) {
                    throw new NotImplementedException();
                  }
                  else {
                    for (int index = 0; index < blocks.value.Length; index++) {
                      if ((secY * 16) + ((index >> 8) & 0xf) < 249) {

                        if (!dRet.ContainsKey((enumBlockTypes)blocks.value[index])) {
                          dRet.Add((enumBlockTypes)blocks.value[index], 0);
                        }
                        dRet[(enumBlockTypes)blocks.value[index]] = dRet[(enumBlockTypes)blocks.value[index]] + 1;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      onSetProgress(_MapFiles.Length);

      return dRet;
    }

    /// <summary>
    /// Get the item clusters
    /// </summary>
    /// <param name="searchFor">The items to look for</param>
    /// <returns>An List with all item clusters</returns>
    public List<List<FoundItem>> GetItemClusters(enumBlockTypes searchFor) {
      List<List<FoundItem>> lRet = new List<List<FoundItem>>();
      List<FoundItem> lAllOfKind = new List<FoundItem>();

      onSetProgressMaximum(_MapFiles.Length);
      for (int ii = 0; ii < _MapFiles.Length; ii++) {
        onSetProgress(ii);
        StreamReader sr = new StreamReader(_MapFiles[ii]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int jj = 0; jj < 32; jj++) {
          for (int kk = 0; kk < 32; kk++) {
            byte[] data = a.getChunkData(jj, kk);
            if (data != null) {

              if (data[0] != 0) {
                int position = 1;
                string sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
                ListTag sections = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["Sections"];
                int secX = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["xPos"]).value;
                int secZ = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["zPos"]).value;

                for (int j = 0; j < sections.value.Count; j++) {
                  CompoundTag section = (CompoundTag)sections.value[j];
                  ByteArrayTag blocks = (ByteArrayTag)section.value["Blocks"];
                  byte secY = ((ByteTag)section.value["Y"]).value;

                  if (section.value.ContainsKey("Add")) {
                    throw new NotImplementedException();
                  }
                  else {
                    for (int index = 0; index < blocks.value.Length; index++) {
                      if (blocks.value[index] == (byte)searchFor) {
                        int iX = (secX * 16) + (index & 0xf);
                        int iY = (secY * 16) + ((index >> 8) & 0xf);
                        int iZ = (secZ * 16) + ((index >> 4) & 0xf);
                        FoundItem fi = new FoundItem();
                        fi.x = iX;
                        fi.y = iY;
                        fi.z = iZ;
                        fi.id = searchFor;
                        lAllOfKind.Add(fi);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      onSetProgress(_MapFiles.Length);
      onSetProgressMaximum(lAllOfKind.Count);
      onSetProgress(lAllOfKind.Count);

      while (lAllOfKind.Count > 0) {
        onSetProgress(lAllOfKind.Count);
        List<FoundItem> lCurrent = new List<FoundItem>();
        lCurrent.Add(lAllOfKind[0]);
        lAllOfKind.RemoveAt(0);
        for (int i = 0; i < lAllOfKind.Count; i++) {
          FoundItem tmpItem = lAllOfKind[i];
          for (int x = tmpItem.x - 1; (i != -1) && (x <= tmpItem.x + 1); x++) {
            for (int y = tmpItem.y - 1; (i != -1) && (y <= tmpItem.y + 1); y++) {
              for (int z = tmpItem.z - 1; (i != -1) && (z <= tmpItem.z + 1); z++) {
                FoundItem TMPsearchFor = new FoundItem();
                TMPsearchFor.x = x;
                TMPsearchFor.y = y;
                TMPsearchFor.z = z;
                if (lCurrent.Contains(TMPsearchFor)) {
                  lCurrent.Add(lAllOfKind[i]);
                  lAllOfKind.RemoveAt(i);
                  i = -1;
                }
              }
            }
          }
        }

        lRet.Add(lCurrent);
      }

      return lRet;
    }

    /// <summary>
    /// Find all different types of items
    /// </summary>
    /// <returns>All the different items found</returns>
    public enumBlockTypes[] GetAllItems() {
      List<enumBlockTypes> lRet = new List<enumBlockTypes>();

      onSetProgressMaximum(_MapFiles.Length);
      for (int ii = 0; ii < _MapFiles.Length; ii++) {
        onSetProgress(ii);
        StreamReader sr = new StreamReader(_MapFiles[ii]);
        RegionFile a = new RegionFile(sr.BaseStream);
        for (int jj = 0; jj < 32; jj++) {
          for (int kk = 0; kk < 32; kk++) {
            byte[] data = a.getChunkData(jj, kk);
            if (data != null) {

              if (data[0] != 0) {
                int position = 1;
                string sTMP = GeneralFunctions.ReadUTF8(data, ref position);
                Tag tmp2 = Tag.newTag(data[0]);
                tmp2.load(data, ref position, 0);
                ListTag sections = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["Sections"];
                ListTag tileEntities = (ListTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["TileEntities"];
                int secX = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["xPos"]).value;
                int secZ = ((IntTag)((CompoundTag)((CompoundTag)tmp2).value["Level"]).value["zPos"]).value;

                for (int j = 0; j < sections.value.Count; j++) {
                  CompoundTag section = (CompoundTag)sections.value[j];
                  ByteArrayTag blocks = (ByteArrayTag)section.value["Blocks"];
                  byte secY = ((ByteTag)section.value["Y"]).value;

                  if (section.value.ContainsKey("Add")) {
                    throw new NotImplementedException();
                  }
                  else {
                    for (int index = 0; index < blocks.value.Length; index++) {
                      if (!lRet.Contains((enumBlockTypes)blocks.value[index])) {
                        lRet.Add((enumBlockTypes)blocks.value[index]);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }

      onSetProgress(_MapFiles.Length);

      return lRet.ToArray();
    }
  }
}
