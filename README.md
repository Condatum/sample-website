# NovelShelf

A centralized library for **web novels and light novels**. Add novels from any site (Royal Road, Webnovel, NovelUpdates, Syosetu, …) to one bookshelf instead of juggling dozens of browser tabs and apps.

Built with **C# Blazor** (interactive server) and **Tailwind CSS v4**, in a matte black theme.

## Features

- **Library**: every novel on one shelf, with search (title, author, tag, site), status tabs, type filter and sorting
- **Progress tracking**: chapter counter with progress bar, `+1 ch` from the shelf, and a *Continue* link back to the chapter you were on
- **Add / edit novels**: paste a link and the source site is detected and the title filled in; set type, status, rating, tags, notes and a cover
- **Sources**: see which websites your novels come from
- **Backup**: export and import your shelf as JSON

There is no database yet. The library is stored in the browser's `localStorage`.

## Run Locally

Requires the .NET 10 SDK **and ASP.NET Core runtime**, plus Node.js (for Tailwind).

```bash
npm install
dotnet run
```

`dotnet build` recompiles `Styles/tailwind.css` into `wwwroot/app.css` automatically when `node_modules` is present. While editing styles you can also run:

```bash
npm run watch:css
```

## Class Diagram

See [docs/class-diagram.md](docs/class-diagram.md) for the planned database model (User, Source, Novel, Tag, LibraryEntry).

## Project Structure

```
Components/
  Layout/    MainLayout (responsive nav), BrandMark
  Pages/     Home (library), AddNovel, NovelDetails, Sources, About
  Shared/    NovelCard, NovelCover, NovelForm, StatusBadge
Models/      Novel (browser storage), enums
  Entities/  User, Source, Novel, Tag, LibraryEntry (database models)
Services/    LibraryService (localStorage), SampleNovels
Styles/      tailwind.css (theme tokens + component classes)
```
