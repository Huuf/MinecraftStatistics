using System;
using System.Collections.Generic;
using System.Text;

/**
 * Copyright Mojang AB. Converted/Modified by Huuf
 * 
 * Don't do evil.
 */
namespace libMinecraftStatistics {

  class EndTag : Tag {

    public override void load(byte[] input, ref int position, int unknown) {
    }

    public override string ToString() {
      return "END";
    }
  }

  class ByteTag : Tag {
    public byte value = 0;

    public override void load(byte[] input, ref int position, int unknown) {
      value = input[position];
      position++;
    }

    public override string ToString() {
      return value + "b";
    }
  }

  class ShortTag : Tag {
    public short value;

    public override void load(byte[] input, ref int position, int unknown) {
      value = (short)((input[position] << 8) | input[position + 1]);
      position += 2;
    }

    public override string ToString() {
      return value + "s";
    }
  }

  class IntTag : Tag {
    public int value;

    public override void load(byte[] input, ref int position, int unknown) {
      value = (int)((input[position + 0] << 24) | (input[position + 1] << 16) + (input[position + 2] << 8) + input[position + 3]);
      position += 4;
    }

    public override string ToString() {
      return value.ToString();
    }
  }

  class LongTag : Tag {
    public long value;

    public override void load(byte[] input, ref int position, int unknown) {
      byte[] bData = new byte[8];
      Array.Copy(input, position, bData, 0, 8);
      position += 8;
      if (BitConverter.IsLittleEndian) {
        Array.Reverse(bData);
      }

      value = BitConverter.ToInt64(bData, 0);
    }

    public override string ToString() {
      return value.ToString();
    }
  }

  class FloatTag : Tag {
    public float value;

    public override void load(byte[] input, ref int position, int unknown) {
      int tmp = (int)((input[position + 0] << 24) | (input[position + 1] << 16) + (input[position + 2] << 8) + input[position + 3]);
      byte[] bytes = BitConverter.GetBytes(tmp);
      value = BitConverter.ToSingle(bytes, 0);
      position += 4;
    }

    public override string ToString() {
      return value.ToString() + "f";
    }
  }

  class DoubleTag : Tag {
    public double value;

    public override void load(byte[] input, ref int position, int unknown) {
      long tmp = (long)((input[position + 0] << 56) + (input[position + 1] << 48) | (input[position + 2] << 40) | (input[position + 3] << 32) |
        (input[position + 4] << 24) | (input[position + 5] << 16) | (input[position + 6] << 8) | input[position + 7]);
      byte[] bytes = BitConverter.GetBytes(tmp);
      value = BitConverter.ToDouble(bytes, 0);
      position += 8;
    }

    public override string ToString() {
      return value.ToString() + "d";
    }
  }

  class ByteArrayTag : Tag {
    public byte[] value;

    public override void load(byte[] input, ref int position, int unknown) {
      int length = (int)((input[position + 0] << 24) | (input[position + 1] << 16) + (input[position + 2] << 8) + input[position + 3]);
      position += 4;
      value = new byte[length];
      Array.Copy(input, position, value, 0, length);
      position += length;
    }

    public override string ToString() {
      return "[" + value.Length + " bytes]";
    }
  }

  class IntArrayTag : Tag {
    public int[] value;

    public override void load(byte[] input, ref int position, int unknown) {
      int length = (int)((input[position + 0] << 24) | (input[position + 1] << 16) + (input[position + 2] << 8) + input[position + 3]);
      position += 4;
      value = new int[length];
      for (int i = 0; i < length; i++) {
        value[i] = (int)((input[position + 0] << 24) | (input[position + 1] << 16) + (input[position + 2] << 8) + input[position + 3]);
        position += 4;
      }
    }

    public override string ToString() {
      string tmp = "[";
      for (int i = 0; i < value.Length; i++) {
        tmp += value[i] + ",";
      }
      return tmp + "]";
    }
  }

  class StringTag : Tag {
    public string value = "";

    public override void load(byte[] input, ref int position, int unknown) {
      ushort length = (ushort)((input[position] << 8) | input[position + 1]);
      value = "";

      if (length > 0)
        value = Encoding.UTF8.GetString(input, position + 2, length);

      position += 2 + length;
    }

    public override string ToString() {
      return "\"" + value.Replace("\"", "\\\"") + "\"";
    }
  }

  class ListTag : Tag {
    public List<Tag> value = new List<Tag>();

    public override void load(byte[] input, ref int position, int depth) {
      byte bType = input[position];
      position++;
      int Count = (int)((input[position + 0] << 24) | (input[position + 1] << 16) + (input[position + 2] << 8) + input[position + 3]);
      position += 4;

      for (int i = 0; i < Count; i++) {
        Tag tmp = Tag.newTag(bType);
        tmp.load(input, ref position, depth + 1);
        value.Add(tmp);
      }
    }

    public override string ToString() {
      string ret = "[";
      for (int i = 0; i < value.Count; i++) {
        ret += i + ":" + value[i].ToString() + ",";
      }
      return ret + "]";
    }
  }

  class CompoundTag : Tag {
    public Dictionary<string, Tag> value = new Dictionary<string, Tag>();

    public override void load(byte[] input, ref int position, int depth) {
      value.Clear();
      byte bType = 0;
      while ((bType = input[position]) != 0) {
        position++;
        string name = GeneralFunctions.ReadUTF8(input, ref position);
        Tag tmp = Tag.newTag(bType);
        tmp.load(input, ref position, depth + 1);
        value.Add(name, tmp);
      }
      position++;
    }

    public override string ToString() {
      string ret = "{";
      foreach (string key in value.Keys) {
        ret += key + ":" + value[key].ToString() + ",";
      }
      return ret + "}";
    }
  }

  abstract class Tag {
    public abstract void load(byte[] input, ref int position, int depth);

    public static Tag newTag(byte type) {
      switch (type) {
        case 0:
          //END
          return new EndTag();
        case 1:
          //Byte
          return new ByteTag();
        case 2:
          //Short
          return new ShortTag();
        case 3:
          //Int
          return new IntTag();
        case 4:
          //Long
          return new LongTag();
        case 5:
          //Float
          return new FloatTag();
        case 6:
          //Double
          return new DoubleTag();
        case 7:
          //Byte[]
          return new ByteArrayTag();
        case 8:
          //String
          return new StringTag();
        case 9:
          //List
          return new ListTag();
        case 10:
          //Compound
          return new CompoundTag();
        case 11:
          //Int[]
          return new IntArrayTag();
      }
      return null;
    }
  }
}
