# Mercurial Source Control Plugin for MS Visual Studio 2022

```
Authors    Bernd Schrader (2008-2012), Dmitry Popov (2013-2014), Jan-Patrick Ahnen (2015-) and others
Licence   GNU General Public License (GPL) (v2.0)
URLs      https://github.com/HexWrench/VisualHG2015
          https://bitbucket.org/lmn/visualhg2/hg
          https://hg.codeplex.com/forks/lmn/visualhg2
          https://github.com/vitidev/VisualHgMod2
```

Use Visual Studio 2017 with SDK to build the solution.

* includes updates for VS2022
* project status icon

### Remarks

#### context menu

`VSCTCompile ` in `csproj` and `[ProvideMenuResource("Menus.ctmenu", 1)]` attribute as described [there](https://docs.microsoft.com/en-us/visualstudio/extensibility/internals/how-to-create-a-dot-vsct-file?view=vs-2022).

#### status icons

For some unknown reason, the method `IVsSccGlyphs.GetCustomGlyphList` is no longer called (bug???) and the standard method for using custom icons does not work.

And the status icons for git are very terrible. Therefore, the code must use the method `IVsSccGlyphs2.GetCustomGlyphMonikerList`

 icons from https://glyphlist.azurewebsites.net/knownmonikers/ are used, which are more or less normally displayed.

```
/// <summary>
/// This list of custom monikers will be appended to the standard moniker list
/// </summary>
List<ImageMoniker> monikers = new List<ImageMoniker>
{
    KnownMonikers.OnlineStatusBusy, //???
    KnownMonikers.OnlineStatusBusy, //Modified ++
    KnownMonikers.AddNoColor, //Added +++
    KnownMonikers.Cancel, //Removed
    KnownMonikers.OnlineStatusAvailable, //Clean
    KnownMonikers.OnlineStatusAway, //Missing
    KnownMonikers.Blank, //NotTracked
    KnownMonikers.OnlineStatusUnknown, //Ignored
    KnownMonikers.OnlineStatusOffline, //Renamed
    KnownMonikers.OnlineStatusOffline, //Copied
};
```