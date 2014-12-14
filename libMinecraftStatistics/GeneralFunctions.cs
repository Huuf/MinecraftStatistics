using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace libMinecraftStatistics {
  /// <summary>
  /// General functions for the Anvil stuff
  /// </summary>
  static class GeneralFunctions {
    /// <summary>
    /// Deflate an GZip Stream into an byte array.
    /// </summary>
    /// <param name="compressed">The compressed GZip Stream</param>
    /// <returns>An byte[] of decompressed bytes</returns>
    public static byte[] GZipDecompress(Stream compressed) {
      byte[] buffer = new byte[4096];
      using (GZipStream gzs = new GZipStream(compressed, CompressionMode.Decompress))
      using (MemoryStream uncompressed = new MemoryStream()) {
        for (int r = -1; r != 0; r = gzs.Read(buffer, 0, buffer.Length))
          if (r > 0) uncompressed.Write(buffer, 0, r);
        return uncompressed.ToArray();
      }
    }

    /// <summary>
    /// Deflate an stream using Deflate Algorithm
    /// </summary>
    /// <param name="compressed">The compressed Defalted Stream</param>
    /// <returns>An byte[] of decompressed bytes</returns>
    public static byte[] DeflateDecompress(Stream compressed) {
      byte[] buffer = new byte[compressed.Length];
      using (DeflateStream dfs = new DeflateStream(compressed, CompressionMode.Decompress))
      using (MemoryStream uncompressed = new MemoryStream()) {
        for (int r = -1; r != 0; r = dfs.Read(buffer, 0, buffer.Length))
          if (r > 0) uncompressed.Write(buffer, 0, r);
        return uncompressed.ToArray();
      }
    }

    /// <summary>
    /// Read an UTF8 String
    /// </summary>
    /// <param name="data">The data to read the string from</param>
    /// <param name="position">The current position in the data stream</param>
    /// <returns>An string</returns>
    public static string ReadUTF8(byte[] data, ref int position) {
      ushort length = (ushort)((data[position] << 8) | data[position + 1]);
      string sRet = "";

      if (length > 0)
        sRet = Encoding.UTF8.GetString(data, position + 2, length);

      position += 2 + length;
      return sRet;
    }

    /// <summary>
    /// Read an Int from an byte[]
    /// </summary>
    /// <param name="data">The data to read the int from</param>
    /// <param name="position">The current position in the data stream</param>
    /// <returns>An Int</returns>
    public static int ReadInt(byte[] data, ref int position) {
      int ret = (int)((data[position + 0] << 24) | (data[position + 1] << 16) + (data[position + 2] << 8) + data[position + 3]);
      position += 4;
      return ret;
    }

    /// <summary>
    /// Read an Int from stream
    /// </summary>
    /// <param name="stream">The stream to read the int from</param>
    /// <returns>An Int</returns>
    public static int ReadInt(Stream stream) {
      byte[] intBytes = new byte[4];
      int i = 0;
      stream.Read(intBytes, 0, 4);
      return ReadInt(intBytes, ref i);
    }
  }
}
