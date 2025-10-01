# Daily Journal

A minimal journaling app for Windows. One file per day. That's it.

## What It Does

- Creates a new journal entry file for each day
- Lets you write in today's entry
- Auto-saves every 2 seconds
- Once the day passes, entries become read-only
- No formatting, no features, no distractions

## Requirements

- Windows
- .NET Framework (WPF)

## How to Use

1. Run the app
2. Start writing
3. That's it

Your entries are automatically saved. Yesterday's entries are locked and can only be read.

## Storage

Journal files are stored locally on your machine in `Documents\DailyJournal` . One text file per day.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Building from Source

### Prerequisites:
- Visual Studio 2022
- .NET 9

This is a WPF application. Open the solution in Visual Studio and build.
