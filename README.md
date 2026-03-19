# FilmDB — Film Database

A personal film catalogue application built with C# and WPF (.NET Framework).  
Browse, add, edit, rate, and organise your movie collection — with XML import/export and autosave.

---

## Features

- **Browse & search** — a filterable list shows your films at a glance with poster thumbnail, title, and rating
- **Detailed view** — click any film to see directors, writers, actors, description, and star rating
- **Add / Edit films** — a dedicated dialog lets you set poster image, crew, genres, rating, and favourite status
- **Favourite flag** — mark films with a ⭐ star; favourites are highlighted in the list
- **Genre management** — create and manage your own genre tags in Settings, then assign them per film
- **Import / Export** — load and save your entire collection as XML
- **Autosave** — optionally save automatically every N minutes to `Saves/autosave.xml`
- **Two layouts** — swap between default (list left, detail right) and alternate (detail left, list right)
- **Resizable panels** — drag the orange splitter to adjust column widths

---

## Getting Started

### Requirements

- Windows 10 / 11
- Visual Studio 2019 or later
- .NET Framework 4.x
- [Xceed WPF Toolkit](https://github.com/xceedsoftware/wpftoolkit) (managed via `packages.config`)

### Build & Run

```
git clone <repo-url>
cd "FilmDB"
# Open FilmDB.sln in Visual Studio, then Build → Run (F5)
```

NuGet will restore the Xceed WPF Toolkit dependency automatically on first build.

---

## Usage

| Action | How |
|---|---|
| Select a film | Click once in the list |
| Edit a film | Double-click in the list |
| Add a film | Menu → Films → Add… (or right-click the list) |
| Remove a film | Menu → Films → Remove (or right-click the list) |
| Toggle favourite | Click the ⭐ button in the detail panel |
| Rate a film | Choose a rating in the UC widget and click Send |
| Import collection | Menu → File → Import |
| Export collection | Menu → File → Export |
| Configure autosave | Menu → Tools → Settings → Autosave tab |
| Manage genres | Menu → Tools → Settings → Genres tab |
| Switch layout | Menu → Layout → Default / Alternate |

---

## Project Structure

```
FilmDB/
├── Class1.cs              # Film model (INotifyPropertyChanged)
├── MainWindow.xaml(.cs)   # Main window — list + detail view
├── Window1.xaml(.cs)      # Settings dialog (genres + autosave)
├── Window2.xaml(.cs)      # Add / Edit film dialog
├── UC.xaml(.cs)           # Rating user control
├── Resources/
│   ├── UI_Dictionary.xaml # Shared styles (main windows)
│   └── UB_Dictionary.xaml # Styles for the UC rating widget
└── Slike/                 # Sample poster images + default poster
```

---

## Bug Fixes (vs original)

| # | Location | Bug |
|---|---|---|
| 1 | `Window2.xaml.cs` | `Delete_Writer` and `Delete_Actor` were checking `ListView_Directors.SelectedIndex` — deletions in Writers and Actors lists did nothing |
| 2 | `Window2.xaml.cs` | `Delete_Genre` in the edit dialog also checked `ListView_Directors.SelectedIndex` |
| 3 | `Window2.xaml.cs` | `Image_Click` signature was `RoutedEventArgs` but wired to `MouseDown` — should be `MouseButtonEventArgs` |
| 4 | `Window2.xaml.cs` | `Accept_Button` showed two sequential MessageBoxes for the same validation error (one standard + one Xceed) |
| 5 | `Window2.xaml.cs` | `OpenFileDialog` image filter used commas (`*.GIF,*.PNG`) instead of semicolons, breaking PNG/GIF selection |
| 6 | `MainWindow.xaml.cs` | Autosave wrote to `Saves/autosave.xml` without creating the directory first — crash if folder absent |
| 7 | `MainWindow.xaml.cs` | `Change_Film` (double-click) didn't guard against `SelectedIndex == -1` — would crash with `ArgumentOutOfRangeException` |
| 8 | `MainWindow.xaml.cs` | `UC_OnGetRating` and `Main_Favorite` didn't guard against no film selected — same crash |
| 9 | `MainWindow.xaml.cs` | `Import` re-bound `ListView1.ItemsSource` but didn't re-apply the search filter to the new collection view |
| 10 | `Window1.xaml.cs` | `Int32.Parse` on the autosave time field — crash if the field contains non-numeric text |
| 11 | All windows | Window titles were `"MainWindow"`, `"Window1"`, `"Window2"` |
| 12 | `Window2.xaml` | Hardcoded size `Height="999.066" Width="1231.262"` — opened partly off-screen on normal monitors |

---

## License

Academic / personal project.
