// This source is subject to the Creative Commons Public License.
// Please see the README.MD file for more information.
// All other rights reserved.
using System;
using System.Collections.Generic;

using Common;

using WallpaperManager.Data;

namespace WallpaperManager.Business {
  /// <summary>
  ///   Instanced when a <see cref="WallpaperChanger" /> instance requests <see cref="Wallpaper" /> objects for a 
  ///   new cycle.
  /// </summary>
  /// <seealso cref="WallpaperChanger">WallpaperChanger Class</seealso>
  /// <seealso cref="Wallpaper">Wallpaper Class</seealso>
  /// <threadsafety static="true" instance="false" />
  public class RequestWallpapersEventArgs: EventArgs {
    #region Property: Wallpapers
    /// <summary>
    ///   <inheritdoc cref="Wallpapers" select='../value/node()' />
    /// </summary>
    private readonly List<Wallpaper> wallpapers;

    /// <summary>
    ///   Gets the collection of <see cref="Wallpaper" /> objects to be provided for cycling.
    /// </summary>
    /// <value>
    ///   The collection of <see cref="Wallpaper" /> objects to be provided for cycling.
    /// </value>
    /// <exception cref="ArgumentNullException">
    ///   Attempted to set a <c>null</c> value.
    /// </exception>
    /// <seealso cref="Wallpaper">Wallpaper Class</seealso>
    public List<Wallpaper> Wallpapers {
      get { return this.wallpapers; }
    }
    #endregion

    #region Method: Constructor, ToString
    /// <summary>
    ///   Initializes a new instance of the <see cref="RequestWallpapersEventArgs" /> class.
    /// </summary>
    public RequestWallpapersEventArgs() {
      this.wallpapers = new List<Wallpaper>(50);
    }

    /// <inheritdoc />
    public override String ToString() {
      return StringGenerator.FromListKeyed(
        new String[] { "Wallpapers" },
        (IList<Object>)new Object[] { this.Wallpapers.Count }
      );
    }
    #endregion
  }
}