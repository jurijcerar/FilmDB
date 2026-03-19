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
