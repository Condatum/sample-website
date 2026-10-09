# NovelShelf Class Diagram

Planned database model for NovelShelf. The entity classes live in [`Models/Entities`](../Models/Entities). Until the database is added, the running app still stores the shelf in the browser through `Models/Novel.cs`.

![Class diagram](class-diagram.png)

PDF version: [class-diagram.pdf](class-diagram.pdf)

## Relationships

| Relationship | Multiplicity | Meaning |
| --- | --- | --- |
| User → LibraryEntry | 1 to 0..* | A user owns many shelf entries |
| Novel → LibraryEntry | 1 to 0..* | A novel can be on many users' shelves |
| Source → Novel | 1 to 0..* | A website hosts many novels |
| Novel ↔ Tag | 0..* to 0..* | Novels share genre tags (join table) |

`LibraryEntry` is the association class between `User` and `Novel`. It holds the reader's own status, chapter progress, rating and notes, so the same novel can be shared by many readers.

## Diagram source

```mermaid
classDiagram
    direction LR

    class User {
        +int Id
        +string Username
        +string Email
        +string PasswordHash
        +DateTimeOffset CreatedAt
        +ICollection~LibraryEntry~ LibraryEntries
    }

    class LibraryEntry {
        +int Id
        +int UserId
        +int NovelId
        +ReadingStatus Status
        +int CurrentChapter
        +string? LastReadUrl
        +int? Rating
        +string? Notes
        +DateTimeOffset AddedAt
        +DateTimeOffset UpdatedAt
        +double? Progress
        +Validate(ValidationContext) IEnumerable~ValidationResult~
    }

    class Novel {
        +int Id
        +string Title
        +string? Author
        +NovelKind Kind
        +string Url
        +string? CoverUrl
        +string? Synopsis
        +int? TotalChapters
        +DateTimeOffset CreatedAt
        +int SourceId
        +ICollection~Tag~ Tags
        +ICollection~LibraryEntry~ LibraryEntries
    }

    class Source {
        +int Id
        +string Name
        +string Host
        +string BaseUrl
        +ICollection~Novel~ Novels
        +NormalizeHost(string? url)$ string?
    }

    class Tag {
        +int Id
        +string Name
        +ICollection~Novel~ Novels
    }

    class NovelKind {
        <<enumeration>>
        WebNovel
        LightNovel
    }

    class ReadingStatus {
        <<enumeration>>
        Reading
        PlanToRead
        OnHold
        Completed
        Dropped
    }

    User "1" --> "0..*" LibraryEntry : owns
    Novel "1" --> "0..*" LibraryEntry : shelved as
    Source "1" --> "0..*" Novel : hosts
    Novel "0..*" -- "0..*" Tag : tagged with
    LibraryEntry ..> ReadingStatus
    Novel ..> NovelKind
```
