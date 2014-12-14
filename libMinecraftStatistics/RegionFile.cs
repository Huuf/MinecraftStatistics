using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
/**
 * Copyright Mojang AB. Converted/Modified by Huuf
 * 
 * Don't do evil.
 */

namespace libMinecraftStatistics {
  /// <summary>
  /// Minecraft anvil region file loader
  /// </summary>
  class RegionFile {
    /// <summary>
    /// The anvil file opened
    /// </summary>
    private Stream _file;
    /// <summary>
    /// The sector offsets
    /// </summary>
    private int[] _sectorOffsets;
    /// <summary>
    /// Last time the chunk got modified
    /// </summary>
    private int[] _chunkTimestamps;
    /// <summary>
    /// Is the sector free
    /// </summary>
    private List<bool> _sectorFree;

    /// <summary>
    /// Constructor for the anvil file
    /// </summary>
    /// <param name="file">The file to read from</param>
    public RegionFile(Stream file) {
      this._sectorOffsets = new int[1024];
      this._chunkTimestamps = new int[1024];
      this._file = file;

      if (_file.Length < 4096) {
        throw new Exception();
      }

      if ((_file.Length & 0xFFF) != 0) {
        throw new Exception();
      }

      int i = (int)(_file.Length / 4096);
      _sectorFree = new List<bool>(i);
      for (int j = 0; j < i; j++) {
        _sectorFree.Add(true);
      }
      _sectorFree[0] = false;
      _sectorFree[1] = false;

      _file.Position = 0;
      int k;

      for (int j = 0; j < 1024; j++) {
        k = GeneralFunctions.ReadInt(_file);
        this._sectorOffsets[j] = k;
        if ((k != 0) && ((k >> 8) + (k & 0xFF) <= this._sectorFree.Count)) {
          for (int m = 0; m < (k & 0xFF); m++) {
            _sectorFree[(k >> 8) + m] = false;
          }
        }
      }

      for (int j = 0; j < 1024; j++) {
        k = GeneralFunctions.ReadInt(_file);
        this._chunkTimestamps[j] = k;
      }
    }

    /// <summary>
    /// Retrieve the chunk data from a certain chunk
    /// </summary>
    /// <param name="x">X position of the chunk</param>
    /// <param name="y">Y position of the chunk</param>
    /// <returns></returns>
    public byte[] getChunkData(int x, int y) {
      if (outOfBounds(x, y)) {
        return null;
      }
      try {
        int i = getOffset(x, y);
        if (i == 0) {
          return null;
        }
        int j = i >> 8;
        int k = i & 0xFF;
        if (j + k > this._sectorFree.Count) {
          return null;
        }
        this._file.Position = j * 4096;
        int m = GeneralFunctions.ReadInt(_file);
        if (m > 4096 * k) {
          return null;
        }
        if (m <= 0) {
          return null;
        }
        int n = this._file.ReadByte();
        byte[] arrayOfByte;
        if (n == 1) {
          arrayOfByte = new byte[m - 1];
          this._file.Read(arrayOfByte, 0, m - 1);
          MemoryStream ms = new MemoryStream(arrayOfByte);
          return GeneralFunctions.GZipDecompress(ms);
        }
        if (n == 2) {
          this._file.Position += 2;
          arrayOfByte = new byte[m - 1 - 2];
          this._file.Read(arrayOfByte, 0, m - 1 - 2);
          MemoryStream ms = new MemoryStream(arrayOfByte);
          return GeneralFunctions.DeflateDecompress(ms);
        }
        return null;
      }
      catch (IOException) { }
      return null;
    }

    /// <summary>
    /// Is the given coordinate out of range
    /// </summary>
    /// <param name="x">The X to check</param>
    /// <param name="y">The Y to check</param>
    /// <returns>True if the coordinate is out of bounds; else false</returns>
    private bool outOfBounds(int x, int y) {
      return (x < 0) || (x >= 32) || (y < 0) || (y >= 32);
    }

    /// <summary>
    /// Get the offset of a certain Chunk
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <returns>The offset of the given sector</returns>
    private int getOffset(int x, int y) {
      return this._sectorOffsets[(x + y * 32)];
    }

    /// <summary>
    /// Close the open file.
    /// </summary>
    public void close() {
      if (this._file != null) {
        this._file.Close();
        this._file = null;
      }
    }
  }
}
