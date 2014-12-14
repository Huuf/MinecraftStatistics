using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MinecraftStatistics
{
  /// <summary>
  /// GUI for the user to access stats
  /// </summary>
  public partial class frmSelection : Form
  {
    /// <summary>
    /// Minecraft Level Loader, so accessible through threads
    /// </summary>
    libMinecraftStatistics.MinecraftLevelLoader _mll = null;

    /// <summary>
    /// Constructor
    /// </summary>
    public frmSelection()
    {
      InitializeComponent();
      
      cobStatistic.Items.AddRange(StatisticType.GetStatisticTypes());
      LoadBlockTypes();
      LoadBiomes();

      cobStatistic.SelectedIndex = 0;
      cobBiomes.SelectedIndex = 0;
      cobBlockTypes.SelectedIndex = 0;

      //cobLevelDirectory.Text = @"C:\adt-bundle-windows-x86_64-20140321\PreChunk";
      if (System.IO.Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\saves"))
      {
        string[] sLevels = System.IO.Directory.GetDirectories(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\saves");
        for (int i = 0; i < sLevels.Length; i++)
        {
          cobLevelDirectory.Items.Add(System.IO.Path.GetFileName(sLevels[i]));
        }
      }
    }

    /// <summary>
    /// When the user selects a new type of statistic
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cobStatistic_SelectedIndexChanged(object sender, EventArgs e) {
      if (cobStatistic.SelectedIndex < 0) return;

      StatisticType selected = (StatisticType)cobStatistic.Items[cobStatistic.SelectedIndex];
      if (selected == null) return;

      //See if the user needs to select a block type or biome
      lblBlockTypes.Enabled = cobBlockTypes.Enabled = selected.NeedsItem; 
      lblBiomes.Enabled = cobBiomes.Enabled = selected.NeedsBiome;
    }

    /// <summary>
    /// Load all the different block types from the enum, and make it look pretty
    /// </summary>
    private void LoadBlockTypes() {
      string[] sNames = Enum.GetNames(typeof(libMinecraftStatistics.enumBlockTypes));
      for (int i = 0; i < sNames.Length; i++) {
        string[] sParts = sNames[i].Split('_');
        string sName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", sParts).ToLower());
        cobBlockTypes.Items.Add(sName);
      }
    }

    /// <summary>
    /// Load all the different biomes from the enum, and make it look pretty
    /// </summary>
    private void LoadBiomes() {
      string[] sNames = Enum.GetNames(typeof(libMinecraftStatistics.enumBiomes));
      for (int i = 0; i < sNames.Length; i++) {
        string[] sParts = sNames[i].Split('_');
        string sName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", sParts).ToLower());
        cobBiomes.Items.Add(sName);
      }
    }

    /// <summary>
    /// Get the currently selected biome
    /// </summary>
    /// <returns></returns>
    private libMinecraftStatistics.enumBiomes getSelectedBiome() {
      string sCurrentSelectedBiome = cobBiomes.Text;

      string[] sNames = Enum.GetNames(typeof(libMinecraftStatistics.enumBiomes));
      for (int i = 0; i < sNames.Length; i++) {
        string[] sParts = sNames[i].Split('_');
        string sName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", sParts).ToLower());
        if (sName == sCurrentSelectedBiome) return (libMinecraftStatistics.enumBiomes)Enum.Parse(typeof(libMinecraftStatistics.enumBiomes), sNames[i]);
      }


      return libMinecraftStatistics.enumBiomes.ocean;
    }

    /// <summary>
    /// Get the currently selected block type
    /// </summary>
    /// <returns></returns>
    private libMinecraftStatistics.enumBlockTypes getSelectedBlockType() {
      string sCurrentSelectedBlock = cobBlockTypes.Text;

      string[] sNames = Enum.GetNames(typeof(libMinecraftStatistics.enumBlockTypes));
      for (int i = 0; i < sNames.Length; i++) {
        string[] sParts = sNames[i].Split('_');
        string sName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", sParts).ToLower());
        if (sName == sCurrentSelectedBlock) return (libMinecraftStatistics.enumBlockTypes)Enum.Parse(typeof(libMinecraftStatistics.enumBlockTypes), sNames[i]);
      }


      return libMinecraftStatistics.enumBlockTypes.air;
    }

    /// <summary>
    /// Generate the requested statistic
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnGenerate_Click(object sender, EventArgs e) {
      if (cobStatistic.SelectedIndex < 0) { txtResult.Text = "Please select an statistic!"; return; }

      StatisticType selected = (StatisticType)cobStatistic.Items[cobStatistic.SelectedIndex];
      if (selected == null) { txtResult.Text = "Please select an statistic!"; return; }

      btnGenerate.Enabled = false;

      libMinecraftStatistics.enumBlockTypes blockType = getSelectedBlockType();
      libMinecraftStatistics.enumBiomes biome = getSelectedBiome();

      string sPath = cobLevelDirectory.Text;

      _mll = new libMinecraftStatistics.MinecraftLevelLoader(sPath);
      _mll.SetProgressHandler += new libMinecraftStatistics.SetProgressHandler(_mll_SetProgressHandler);
      _mll.SetProgressMaximum += new libMinecraftStatistics.SetProgressMaximumHandler(_mll_SetProgressMaximum);
      txtSeed.Text = _mll.Seed.ToString();

      switch (selected.StatType) {
        case eStatisticType.MostCommon:
          new System.Threading.Thread(GetMostCommon).Start();
          return;
        case eStatisticType.ItemsCount:
          new System.Threading.Thread(GetItemCount).Start();
          return;
        case eStatisticType.ItemDistribution:
          new System.Threading.Thread(GetItemDistribution).Start(blockType);
          return;
        case eStatisticType.ItemLocations:
          new System.Threading.Thread(GetItemLocation).Start(blockType);
          return;
        case eStatisticType.Temples:
          new System.Threading.Thread(GetTemples).Start(biome);
          return;
        case eStatisticType.Biomes:
          new System.Threading.Thread(GetBiomes).Start();
          return;
        case eStatisticType.BiomeExists:
          new System.Threading.Thread(GetBiomeExists).Start(biome);
          return;
        case eStatisticType.NewBlockTypes:
          new System.Threading.Thread(GetNewItems).Start();
          return;
        case eStatisticType.NewBiomeTypes:
          new System.Threading.Thread(GetNewBiomes).Start();
          return;
        case eStatisticType.GetClusters:
          new System.Threading.Thread(GetItemClusters).Start(blockType);
          return;
        case eStatisticType.GetSpawners:
          new System.Threading.Thread(GetSpawners).Start();
          return;
        case eStatisticType.GetChests:
          new System.Threading.Thread(GetChests).Start();
          return;
      }
      btnGenerate.Enabled = true;
    }

    /// <summary>
    /// Set the maximum of the progressbar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="maxValue"></param>
    void _mll_SetProgressMaximum(object sender, int maxValue) {
      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          pgbProgress.Value = 0;
          pgbProgress.Maximum = maxValue;
        });
      }
      else {
        pgbProgress.Value = 0;
        pgbProgress.Maximum = maxValue;
      }
    }

    /// <summary>
    /// Set the current value of the progressbar
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="Value"></param>
    void _mll_SetProgressHandler(object sender, int Value) {
      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          pgbProgress.Value = Value;
        });
      }
      else {
        pgbProgress.Value = Value;
      }
    }

    /// <summary>
    /// Get the most common statistics
    /// </summary>
    private void GetMostCommon()
    {
      Dictionary<libMinecraftStatistics.enumBlockTypes, ulong> dItems = _mll.GetItemCount();

      string sTMP = "Item count:";
      foreach (libMinecraftStatistics.enumBlockTypes block in dItems.Keys)
      {
        sTMP += "\r\n-" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", block.ToString().Split('_')).ToLower()) + " - " + dItems[block];
      }

      {
        libMinecraftStatistics.enumBlockTypes item = libMinecraftStatistics.enumBlockTypes.diamond_ore;
        Dictionary<int, ulong> dLevels = _mll.FindItemOnLevels(item);

        sTMP += "\r\n\r\nLevel Distribution for item " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", item.ToString().Split('_')).ToLower());
        foreach (int level in dLevels.Keys)
        {
          sTMP += "\r\n-" + level.ToString().PadLeft(3, ' ') + " - " + dLevels[level];
        }
      }

      {
        libMinecraftStatistics.enumBlockTypes item = libMinecraftStatistics.enumBlockTypes.gold_ore;
        Dictionary<int, ulong> dLevels = _mll.FindItemOnLevels(item);

        sTMP += "\r\n\r\nLevel Distribution for item " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", item.ToString().Split('_')).ToLower());
        foreach (int level in dLevels.Keys)
        {
          sTMP += "\r\n-" + level.ToString().PadLeft(3, ' ') + " - " + dLevels[level];
        }
      }

      {
        libMinecraftStatistics.enumBlockTypes item = libMinecraftStatistics.enumBlockTypes.emerald_ore;
        Dictionary<int, ulong> dLevels = _mll.FindItemOnLevels(item);

        sTMP += "\r\n\r\nLevel Distribution for item " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", item.ToString().Split('_')).ToLower());
        foreach (int level in dLevels.Keys)
        {
          sTMP += "\r\n-" + level.ToString().PadLeft(3, ' ') + " - " + dLevels[level];
        }
      }


      {
        libMinecraftStatistics.FoundItem[] spawners = _mll.FindItems(libMinecraftStatistics.enumBlockTypes.mob_spawner);

        Dictionary<string, int> spawnerCount = new Dictionary<string, int>();

        for (int i = 0; i < spawners.Length; i++)
        {
          if (!spawnerCount.ContainsKey(spawners[i].Extra))
          {
            spawnerCount.Add(spawners[i].Extra, 0);
          }
          spawnerCount[spawners[i].Extra] = spawnerCount[spawners[i].Extra] + 1;
        }

        sTMP += "\r\n\r\nFound the following spawners:";
        foreach (string key in spawnerCount.Keys)
        {
          sTMP += "\r\nFound " + spawnerCount[key] + " of " + key + " spawners.";
        }
        sTMP += "\r\n\r\nLocation of spawners";
        for (int i = 0; i < spawners.Length; i++)
        {
          sTMP += "\r\n-" + spawners[i].x + " " + spawners[i].y + " " + spawners[i].z + " = " + spawners[i].Extra.Trim();
        }
      }

      {
        //Dictionary<string, int>
        libMinecraftStatistics.FoundItem[] chests = _mll.FindItems(libMinecraftStatistics.enumBlockTypes.chest);

        Dictionary<string, ulong> combinedChestContent = new Dictionary<string, ulong>();
        for (int i = 0; i < chests.Length; i++)
        {
          Dictionary<string, int> currentChest = (Dictionary<string, int>)chests[i].Data;
          foreach (string key in currentChest.Keys)
          {
            if (!combinedChestContent.ContainsKey(key))
            {
              combinedChestContent.Add(key, 0);
            }
            combinedChestContent[key] = combinedChestContent[key] + (ulong)currentChest[key];
          }
        }

        List<string> lSort = new List<string>();

        foreach (string key in combinedChestContent.Keys)
        {
          lSort.Add(key);
        }
        lSort.Sort();

        sTMP += "\r\n\r\nFound chests with the following contents:";
        for (int i = 0; i < lSort.Count; i++)
        {
          sTMP += "\r\n " + lSort[i] + " - " + combinedChestContent[lSort[i]];
        }

        sTMP += "\r\n\r\nAll chests + Items + Locations";
        for (int i = 0; i < chests.Length; i++)
        {
          sTMP += "\r\n-" + chests[i].x + " " + chests[i].y + " " + chests[i].z + " = " + chests[i].Extra.Trim();
        }

      }

      if (InvokeRequired)
      {
        BeginInvoke((MethodInvoker)delegate()
        {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else
      {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get all items + count
    /// </summary>
    private void GetItemCount()
    {
      Dictionary<libMinecraftStatistics.enumBlockTypes, ulong> dItems = _mll.GetItemCount();

      string sTMP = "Item count:";
      foreach (libMinecraftStatistics.enumBlockTypes block in dItems.Keys)
      {
        sTMP += "\r\n-" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", block.ToString().Split('_')).ToLower()) + " - " + dItems[block];
      }

      if (InvokeRequired)
      {
        BeginInvoke((MethodInvoker)delegate()
        {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else
      {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get the distribution of an item (on which level is the item?)
    /// </summary>
    /// <param name="oItem">An libMinecraftStatistics.enumBlockType to get the distribution for</param>
    private void GetItemDistribution(object oItem)
    {
      libMinecraftStatistics.enumBlockTypes item = (libMinecraftStatistics.enumBlockTypes)oItem;
      Dictionary<int, ulong> dLevels = _mll.FindItemOnLevels(item);

      string sTMP = "Level Distribution for item " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", item.ToString().Split('_')).ToLower());
      foreach (int level in dLevels.Keys)
      {
        sTMP += "\r\n-" + level.ToString().PadLeft(3, ' ') + " - " + dLevels[level];
      }

      if (InvokeRequired)
      {
        BeginInvoke((MethodInvoker)delegate()
        {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else
      {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get all the locations of a given item
    /// </summary>
    /// <param name="oItem">An libMinecraftStatistics.enumBlockType to get the locations for</param>
    private void GetItemLocation(object oItem)
    {
      libMinecraftStatistics.enumBlockTypes item = (libMinecraftStatistics.enumBlockTypes)oItem;
      libMinecraftStatistics.FoundItem[] items = _mll.FindItems(item);

      string sTMP = "Locations for item: " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", item.ToString().Split('_')).ToLower());
      for (int i = 0; i < items.Length; i++)
      {
        sTMP += "\r\n" + items[i].x + "\t" + items[i].y + "\t" + items[i].z;
      }

      if (InvokeRequired)
      {
        BeginInvoke((MethodInvoker)delegate()
        {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else
      {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get all possible temples
    /// </summary>
    private void GetTemples() {
      libMinecraftStatistics.enumBlockTypes[] templeItems = {
                                                              //Village?
                                                              libMinecraftStatistics.enumBlockTypes.tnt, //Desert temple
                                                              libMinecraftStatistics.enumBlockTypes.cauldron, //Witch hut
                                                              libMinecraftStatistics.enumBlockTypes.gold_block, //Ocean Monument
                                                              libMinecraftStatistics.enumBlockTypes.sponge, //Ocean Monument
                                                              libMinecraftStatistics.enumBlockTypes.end_portal_frame, //End Frame
                                                              libMinecraftStatistics.enumBlockTypes.sticky_piston //jungle temple
                                                            };

      Dictionary<libMinecraftStatistics.enumBlockTypes, List<libMinecraftStatistics.FoundItem>> dItems = _mll.FindMultipleItems(templeItems);

      //TODO Group items and determine stats per temple/monument/...
      string sTMP = "Possible Temples:";
      foreach (libMinecraftStatistics.enumBlockTypes block in dItems.Keys) {
        sTMP += "\r\n-" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", block.ToString().Split('_')).ToLower()) + " - " + dItems[block];
      }

      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get the biomes in the level
    /// </summary>
    private void GetBiomes() {
      libMinecraftStatistics.enumBiomes[] biomes = _mll.Biomes();

      string sTMP = "Biomes found:";
      for (int i = 0; i < biomes.Length; i++) {
        sTMP += "\r\n-" + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", biomes[i].ToString().Split('_')).ToLower());
      }

      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Check if a biome exists
    /// </summary>
    /// <param name="oItem">An libMinecraftStatistics.enumBiomes to see if it exists.</param>
    private void GetBiomeExists(object oBiome) {
      libMinecraftStatistics.enumBiomes biome = (libMinecraftStatistics.enumBiomes)oBiome;

      string sTMP = "Biome " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", biome.ToString().Split('_')).ToLower()) + " does ";

      if (_mll.BiomeExists(biome)) {
        sTMP += "exist.";
      }
      else {
        sTMP += "NOT exist.";
      }

      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get the biomes in the level
    /// </summary>
    private void GetNewBiomes() {
      libMinecraftStatistics.enumBiomes[] biomes = _mll.Biomes();

      List<libMinecraftStatistics.enumBiomes> lAllBiomes = new List<libMinecraftStatistics.enumBiomes>(biomes);
      for (int i = 0; i < lAllBiomes.Count; i++) {
        if (((int)lAllBiomes[i]).ToString() != lAllBiomes[i].ToString()) {
          lAllBiomes.RemoveAt(i);
          i--;
        }
      }


      string sTMP = "";
      if (lAllBiomes.Count == 0) {
        sTMP = "No new biomes found.";
      }
      else {
        sTMP = "The following new biome id's have been found:";
        for (int i = 0; i < lAllBiomes.Count; i++) {
          sTMP += "\r\n-" + lAllBiomes[i];
        }
      }

      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get the biomes in the level
    /// </summary>
    private void GetNewItems() {
      libMinecraftStatistics.enumBlockTypes[] items = _mll.GetAllItems();

      List<libMinecraftStatistics.enumBlockTypes> lAllItems = new List<libMinecraftStatistics.enumBlockTypes>(items);
      for (int i = 0; i < lAllItems.Count; i++) {
        if (((int)lAllItems[i]).ToString() != lAllItems[i].ToString()) {
          lAllItems.RemoveAt(i);
          i--;
        }
      }


      string sTMP = "";
      if (lAllItems.Count == 0) {
        sTMP = "No new item types found.";
      }
      else {
        sTMP = "The following new item id's have been found:";
        for (int i = 0; i < lAllItems.Count; i++) {
          sTMP += "\r\n-" + lAllItems[i];
        }
      }

      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get all the item clusters
    /// </summary>
    /// <param name="oItem">An libMinecraftStatistics.enumBlockType to get the chunks for</param>
    private void GetItemClusters(object oItem) {
      libMinecraftStatistics.enumBlockTypes item = (libMinecraftStatistics.enumBlockTypes)oItem;
      List<List<libMinecraftStatistics.FoundItem>> llfi = _mll.GetItemClusters(item);

      string sTMP = "Cluster sizes for the following item: " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", item.ToString().Split('_')).ToLower());
      List<int> lCount = new List<int>();

      for (int i = 0; i < llfi.Count; i++) {
        if (!lCount.Contains(llfi[i].Count)) {
          lCount.Add(llfi[i].Count);
        }
      }
      lCount.Sort();
      Dictionary<int, uint> dRes = new Dictionary<int, uint>();
      for (int i = 0; i < lCount.Count; i++) {
        dRes.Add(lCount[i], 0);
      }

      for (int i = 0; i < llfi.Count; i++) {
        dRes[llfi[i].Count] = dRes[llfi[i].Count] + 1;
      }

      for (int i = 0; i < lCount.Count; i++) {
        sTMP += "\r\n" + lCount[i] + "\t" + dRes[llfi[i].Count];
      }

      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get all the spawners
    /// </summary>
    private void GetSpawners() {
      libMinecraftStatistics.FoundItem[] spawners = _mll.FindItems(libMinecraftStatistics.enumBlockTypes.mob_spawner);

      Dictionary<string, int> spawnerCount = new Dictionary<string, int>();

      for (int i = 0; i < spawners.Length; i++) {
        if (!spawnerCount.ContainsKey(spawners[i].Extra)) {
          spawnerCount.Add(spawners[i].Extra, 0);
        }
        spawnerCount[spawners[i].Extra] = spawnerCount[spawners[i].Extra] + 1;
      }

      string sTMP = "Found the following spawners:";
      foreach (string key in spawnerCount.Keys) {
        sTMP += "\r\nFound " + spawnerCount[key] + " of " + key + " spawners.";
      }
      sTMP += "\r\n\r\nLocation of spawners";
      for (int i = 0; i < spawners.Length; i++) {
        sTMP += "\r\n-" + spawners[i].x + " " + spawners[i].y + " " + spawners[i].z + " = " + spawners[i].Extra.Trim();
      }

      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    /// <summary>
    /// Get all chests + contents
    /// </summary>
    private void GetChests() {
      //Dictionary<string, int>
      libMinecraftStatistics.FoundItem[] chests = _mll.FindItems(libMinecraftStatistics.enumBlockTypes.chest);

      Dictionary<string, ulong> combinedChestContent = new Dictionary<string, ulong>();
      for (int i = 0; i < chests.Length; i++) {
        Dictionary<string, int> currentChest = (Dictionary<string, int>)chests[i].Data;
        foreach (string key in currentChest.Keys) {
          if (!combinedChestContent.ContainsKey(key)) {
            combinedChestContent.Add(key, 0);
          }
          combinedChestContent[key] = combinedChestContent[key] + (ulong)currentChest[key];
        }
      }

      List<string> lSort = new List<string>();

      foreach (string key in combinedChestContent.Keys) {
        lSort.Add(key);
      }
      lSort.Sort();

      string sTMP = "Found chests with the following contents:";
      for (int i = 0; i < lSort.Count; i++) {
        sTMP += "\r\n " + lSort[i] + " - " + combinedChestContent[lSort[i]];
      }

      sTMP += "\r\n\r\nAll chests + Items + Locations";
      for (int i = 0; i < chests.Length; i++) {
        sTMP += "\r\n-" + chests[i].x + " " + chests[i].y + " " + chests[i].z + " = " + chests[i].Extra.Trim();
      }


      if (InvokeRequired) {
        BeginInvoke((MethodInvoker)delegate() {
          txtResult.Text = sTMP;
          btnGenerate.Enabled = true;
        });
      }
      else {
        txtResult.Text = sTMP;
        btnGenerate.Enabled = true;
      }
    }

    private void btnBrowseDir_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      fbd.Description = "Select Level Folder";
      fbd.ShowNewFolderButton = false;

      if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        if (System.IO.File.Exists(fbd.SelectedPath + "\\level.dat"))
        {
          cobLevelDirectory.Text = fbd.SelectedPath;
        }
        else
        {
          MessageBox.Show("Could not find Level.dat, are you sure that is an minecraft level directory?", "Minecraft Statistics", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        
      }
    }
  }
}
