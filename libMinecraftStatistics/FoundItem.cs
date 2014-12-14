using System;
using System.Collections.Generic;
using System.Text;

namespace libMinecraftStatistics {
  /// <summary>
  /// Item search result
  /// </summary>
  public class FoundItem {
    /// <summary>
    /// X Coordinate
    /// </summary>
    public int x;
    /// <summary>
    /// Y Coordinate
    /// </summary>
    public int y;
    /// <summary>
    /// Z Coordinate
    /// </summary>
    public int z;
    /// <summary>
    /// Item ID
    /// </summary>
    public enumBlockTypes id;
    /// <summary>
    /// Extra information
    /// </summary>
    public string Extra;
    /// <summary>
    /// Extra information in easy format
    /// </summary>
    public object Data;

    /// <summary>
    /// Determines whether the specified System.Object is equal to the current libMinecraftStatistics.FoundItem.
    /// </summary>
    /// <param name="obj">The System.Object to compare with the current libMinecraftStatistics.FoundItem.</param>
    /// <returns>true if the specified System.Object is equal to the current libMinecraftStatistics.FoundItem;
    /// otherwise, false.</returns>
    public override bool Equals(object obj) {
      if ((obj == null) && (this == null)) return true;
      if ((obj == null)) return false;
      FoundItem x = (FoundItem)obj;
      if (x != null) {
        return (x.x == this.x) && (x.y == this.y) && (x.z == this.z);

      }
      return false;
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() {
      if ((Extra == null) || (Extra == "")) {
        return x + " " + y + " " + z + ":" + id;
      }
      else {
        return (x + " " + y + " " + z + ":" + id + (" " + Extra.Trim())).Trim();
      }
    }

    /// <summary>
    /// Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>A hash code for the current libMinecraftStatistics.FoundItem.</returns>
    public override int GetHashCode() {
      return ToString().GetHashCode();
    }
  }
}
