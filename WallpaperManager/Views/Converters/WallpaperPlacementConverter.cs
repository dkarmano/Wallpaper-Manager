// This source is subject to the Creative Commons Public License.
// Please see the README.MD file for more information.
// All other rights reserved.
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using WallpaperManager.Models;

namespace WallpaperManager.Views {
  /// <summary>
  ///   Converts a <see cref="WallpaperPlacement" /> value to a string.
  /// </summary>
  /// <threadsafety static="false" instance="false" />
  [ValueConversion(typeof(WallpaperPlacement), typeof(String))]
  public class WallpaperPlacementConverter: IValueConverter {
    #region Constants: UniformString, UniformToFillString, StretchString, CenterString, TileString
    /// <summary>
    ///   Represents the string representation for the <see cref="WallpaperPlacement.Uniform" /> value.
    /// </summary>
    private const String UniformString = "Uniform";

    /// <summary>
    ///   Represents the string representation for the <see cref="WallpaperPlacement.UniformToFill" /> value.
    /// </summary>
    private const String UniformToFillString = "Uniform to fill";

    /// <summary>
    ///   Represents the string representation for the <see cref="WallpaperPlacement.Stretch" /> value.
    /// </summary>
    private const String StretchString = "Stretch";

    /// <summary>
    ///   Represents the string representation for the <see cref="WallpaperPlacement.Center" /> value.
    /// </summary>
    private const String CenterString = "Center";

    /// <summary>
    ///   Represents the string representation for the <see cref="WallpaperPlacement.Tile" /> value.
    /// </summary>
    private const String TileString = "Tile";
    #endregion

    #region Methods: Convert, ConvertBack
    /// <summary>
    ///   Converts a <see cref="WallpaperPlacement" /> value to a string.
    /// </summary>
    /// <inheritdoc cref="IValueConverter.Convert" />
    public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture) {
      if (value == null) {
        return DependencyProperty.UnsetValue;
      }

      switch ((WallpaperPlacement)value) {
        case WallpaperPlacement.Uniform:
          return WallpaperPlacementConverter.UniformString;
        case WallpaperPlacement.UniformToFill:
          return WallpaperPlacementConverter.UniformToFillString;
        case WallpaperPlacement.Stretch:
          return WallpaperPlacementConverter.StretchString;
        case WallpaperPlacement.Center:
          return WallpaperPlacementConverter.CenterString;
        case WallpaperPlacement.Tile:
          return WallpaperPlacementConverter.TileString;
        default:
          return DependencyProperty.UnsetValue;
      }
    }

    /// <summary>
    ///   Converts a string value to <see cref="WallpaperPlacement" />.
    /// </summary>
    /// <inheritdoc cref="IValueConverter.ConvertBack" />
    public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
    #endregion
  }
}