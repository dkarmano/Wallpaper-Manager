// This source is subject to the Creative Commons Public License.
// Please see the README.MD file for more information.
// All other rights reserved.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

using Common;

namespace WallpaperManager.Models {
  /// <summary>
  ///   Contains settings related to a screen.
  /// </summary>
  /// <threadsafety static="true" instance="false" />
  public class ScreenSettings: INotifyPropertyChanged, ICloneable, IAssignable, IWeakEventListener {
    #region Property: Index
    /// <summary>
    ///   <inheritdoc cref="Index" select='../value/node()' />
    /// </summary>
    private readonly Int32 index;

    /// <summary>
    ///   Gets the index of the screen of which this instance defines settings for.
    /// </summary>
    /// <value>
    ///   The index of the screen of which this instance defines settings for.
    /// </value>
    public Int32 Index {
      get { return this.index; }
    }
    #endregion

    #region Property: CycleRandomly
    /// <summary>
    ///   <inheritdoc cref="CycleRandomly" select='../value/node()' />
    /// </summary>
    private Boolean cycleRandomly;

    /// <summary>
    ///   Gets or sets a <see cref="Boolean" /> indicating whether wallpapers will be cycled randomly on this screen or not.
    /// </summary>
    /// <value>
    ///   A <see cref="Boolean" /> indicating whether wallpapers will be cycled randomly on this screen or not.
    /// </value>
    public Boolean CycleRandomly {
      get { return this.cycleRandomly; }
      set {
        this.cycleRandomly = value;
        this.OnPropertyChanged("CycleRandomly");
      }
    }
    #endregion

    #region Property: StaticWallpaper
    /// <summary>
    ///   <inheritdoc cref="StaticWallpaper" select='../value/node()' />
    /// </summary>
    private Wallpaper staticWallpaper;

    /// <summary>
    ///   Gets the static <see cref="Wallpaper" /> which is used if <see cref="CycleRandomly" /> is set to <c>false</c>.
    /// </summary>
    /// <value>
    ///   The static <see cref="Wallpaper" /> which is used if <see cref="CycleRandomly" /> is <c>false</c>.
    /// </value>
    /// <exception cref="ArgumentNullException">
    ///   Attempted to set a <c>null</c> value.
    /// </exception>
    /// <seealso cref="CycleRandomly">CycleRandomly Property</seealso>
    /// <seealso cref="Wallpaper">Wallpaper Class</seealso>
    public Wallpaper StaticWallpaper {
      get { return this.staticWallpaper; }
      set {
        if (value == null) {
          throw new ArgumentNullException(ExceptionMessages.GetVariableCanNotBeNull());
        }

        this.staticWallpaper = value;
      }
    }
    #endregion

    #region Property: Margins
    /// <summary>
    ///   <inheritdoc cref="Margins" select='../value/node()' />
    /// </summary>
    private ScreenMargins margins;

    /// <summary>
    ///   Gets the margin definitions for this screen.
    /// </summary>
    /// <value>
    ///   The margin definitions for this screen.
    /// </value>
    /// <seealso cref="ScreenMargins">ScreenMargins Class</seealso>
    public ScreenMargins Margins {
      get { return this.margins; }
    }
    #endregion

    #region Properties: Bounds, BoundsWithMargin
    /// <summary>
    ///   <inheritdoc cref="Bounds" select='../value/node()' />
    /// </summary>
    private Rectangle bounds;

    /// <summary>
    ///   Gets the bounds of the assigned screen.
    /// </summary>
    /// <value>
    ///   The bounds of the assigned screen.
    /// </value>
    public Rectangle Bounds {
      get { return this.bounds; }
    }

    /// <summary>
    ///   <inheritdoc cref="BoundsWithMargin" select='../value/node()' />
    /// </summary>
    private Rectangle boundsWithMargin;

    /// <summary>
    ///   Gets the bounds of the assigned screen with their margin substracted.
    /// </summary>
    /// <value>
    ///   The bounds of the assigned screen with their margin substracted.
    /// </value>
    public Rectangle BoundsWithMargin {
      get { return this.boundsWithMargin; }
    }
    #endregion

    #region Property: TextOverlays
    /// <summary>
    ///   <inheritdoc cref="TextOverlays" select='../value/node()' />
    /// </summary>
    private ObservableCollection<WallpaperTextOverlay> textOverlays;

    /// <summary>
    ///   Gets the collection of <see cref="WallpaperTextOverlay" /> objects which should be applied on this screen.
    /// </summary>
    /// <value>
    ///   The collection of <see cref="WallpaperTextOverlay" /> objects which should be applied on this screen.
    /// </value>
    /// <seealso cref="WallpaperTextOverlay">WallpaperTextOverlay Class</seealso>
    public ObservableCollection<WallpaperTextOverlay> TextOverlays {
      get { return this.textOverlays; }
    }
    #endregion


    #region Methods: Constructor, RefreshBounds, Margins_PropertyChanged
    /// <summary>
    ///   Initializes a new instance of the <see cref="ScreenSettings" /> class for the screen with the given index.
    /// </summary>
    /// <param name="index">
    ///   The index of the screen of which this instance defines settings for.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   There is no screen with the given <paramref name="index" />.
    /// </exception>
    public ScreenSettings(Int32 index) {
      if (!index.IsBetween(0, Screen.AllScreens.Length - 1)) {
        throw new ArgumentOutOfRangeException(ExceptionMessages.GetValueOutOfRange(
          "index", index, "0", "Screen.AllScreens.Length"
        ));
      }

      this.index = index;
      this.cycleRandomly = true;
      this.staticWallpaper = null;
      this.staticWallpaper = new Wallpaper();

      this.margins = new ScreenMargins();
      PropertyChangedEventManager.AddListener(this.Margins, this, String.Empty);

      // Cache the Bounds and BoundsWithMargin rectangles for this first time.
      this.RefreshBounds();

      this.textOverlays = new ObservableCollection<WallpaperTextOverlay>();
    }

    /// <summary>
    ///   Refreshes the cached <see cref="Bounds" /> and recalculates <see cref="BoundsWithMargin" />.
    /// </summary>
    public void RefreshBounds() {
      this.bounds = Screen.AllScreens[this.Index].Bounds;
      this.Margins_PropertyChanged(this, null);
    }

    /// <summary>
    ///   Handles the <see cref="ScreenMargins.PropertyChanged" /> event of the <see cref="ScreenMargins" /> instance.
    /// </summary>
    /// <commondoc select='All/Methods/EventHandlers[@Params="Object,+EventArgs"]/*' />
    private void Margins_PropertyChanged(Object sender, PropertyChangedEventArgs e) {
      this.boundsWithMargin = new Rectangle(
        this.Bounds.X + this.Margins.Left, 
        this.Bounds.Y + this.Margins.Top,
        this.Bounds.Width - this.Margins.Right - this.Margins.Left,
        this.Bounds.Height - this.Margins.Bottom - this.Margins.Top
      );
    }
    #endregion

    #region ICloneable Implementation, IAssignable Implementation
    /// <inheritdoc />
    public virtual Object Clone() {
      ObservableCollection<WallpaperTextOverlay> overlayTextCollection = new ObservableCollection<WallpaperTextOverlay>();
      foreach (WallpaperTextOverlay overlayText in this.TextOverlays) {
        overlayTextCollection.Add((WallpaperTextOverlay)overlayText.Clone());
      }
      
      return new ScreenSettings(this.Index) {
        cycleRandomly = this.CycleRandomly,
        staticWallpaper = (Wallpaper)this.StaticWallpaper.Clone(),
        margins = (ScreenMargins)this.Margins.Clone(),        
        boundsWithMargin = this.BoundsWithMargin,
        textOverlays = overlayTextCollection,
      };
    }

    /// <summary>
    ///   Assigns all member values of this instance to the respective members of the given instance.
    /// </summary>
    /// <param name="other">
    ///   The target instance to assign to.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="other" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///   <paramref name="other" /> is not castable to the <see cref="ScreenSettings" /> type.
    /// </exception>
    public virtual void AssignTo(Object other) {
      if (other == null) {
        throw new ArgumentNullException(ExceptionMessages.GetVariableCanNotBeNull("other"));
      }

      ScreenSettings otherInstance = (other as ScreenSettings);
      if (otherInstance == null) {
        throw new ArgumentException(ExceptionMessages.GetTypeIsNotCastable("Object", "ScreenSettings", "other"));
      }

      otherInstance.CycleRandomly = this.CycleRandomly;
      this.StaticWallpaper.AssignTo(otherInstance.staticWallpaper);
      this.Margins.AssignTo(otherInstance.Margins);

      otherInstance.TextOverlays.Clear();
      for (Int32 i = 0; i < this.TextOverlays.Count; i++) {
        otherInstance.TextOverlays.Add(this.TextOverlays[i]);
      }
    }
    #endregion

    #region INotifyPropertyChanged Implementation
    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    public event PropertyChangedEventHandler PropertyChanged;

    /// <commondoc select='INotifyPropertyChanged/Methods/OnPropertyChanged/*' />
    protected virtual void OnPropertyChanged(String propertyName) {
      if (this.PropertyChanged != null) {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion

    #region IWeakEventListener Implementation
    /// <inheritdoc />
    public Boolean ReceiveWeakEvent(Type managerType, Object sender, EventArgs e) {
      if (managerType == typeof(PropertyChangedEventManager)) {
        this.Margins_PropertyChanged(sender, (PropertyChangedEventArgs)e);
        return true;
      }

      return false;
    }
    #endregion

    #region Methods: ToString
    /// <inheritdoc />
    public override String ToString() {
      return StringGenerator.FromListKeyed(
        new String[] { "Cycle Randomly", "Margins", "Static Wallpaper" },
        (IList<Object>)new Object[] { this.CycleRandomly, this.Margins, this.StaticWallpaper }
      );
    }
    #endregion
  }
}