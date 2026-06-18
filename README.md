# CPM-WIN

A Windows desktop application for generating random game variants and tracking player performance for NES Monopoly challenges.

## Overview

CPM-WIN (Computer Player Monopoly - Windows) is a companion tool designed for NES Monopoly players who want to add variety and challenge to their gameplay. The application generates random opponents, game rules, and tracks your win/loss statistics over time.

## Features

### 🎲 Random Challenge Generation
- **Random Opponent Selection**: Automatically selects computer opponents from the 8 NES Monopoly characters (Arthur, Gertrude, Erwin, Maude, Carmen, Isaac, Penelope, Ollie)
- **Custom Game Rules**: Generates random gameplay variants and special rules to increase difficulty
- **Level Progression System**: 
  - Progress through 7 difficulty levels based on your wins
  - Level number = number of computer players (Level 1 = 1 computer + you, Level 7 = 7 computers + you)
  - Win a game to advance to the next level
  - Progress automatically saves and loads between sessions
  - Lose a game or complete Level 7 to reset to Level 1

### 📊 Performance Tracking
- Maintains detailed game logs with timestamps and results
- Generates player reports including:
  - Total games played
  - Win/loss records
  - Win rate percentage
  - Longest win streak
  - Current level progression

### ⏱️ Built-in Game Timer
- Visual timer to track game duration
- Optional auto-start with 1-minute delay
- Speech synthesis for elapsed time announcements at configurable intervals

### 🔊 Speech Synthesis
- Text-to-speech functionality for time announcements
- Customizable speech rate and volume
- Audible notifications for game events

## System Requirements

- **OS**: Windows 7 or later
- **Framework**: .NET Framework 4.8.1
- **Additional**: Speech synthesis capabilities (Windows Speech API)

## Installation

1. Download the latest release from the [Releases](https://github.com/HiTechCharles/CPM-WIN/releases) page
2. Extract the files to your preferred location
3. Run `CPM-WIN.exe`

### Building from Source

1. Clone the repository:
   ```bash
   git clone https://github.com/HiTechCharles/CPM-WIN.git
   ```

2. Open `CPM-WIN.sln` in Visual Studio 2019 or later

3. Restore NuGet packages (if any)

4. Build the solution (F6 or Build → Build Solution)

5. The executable will be in `bin\Debug` or `bin\Release`

## Usage

### First Time Setup

**Before starting your first game, you must set your player name:**

1. Launch CPM-WIN
2. Go to the menu and select "Edit Human Name"
3. Enter your name and click OK
4. Your name is automatically saved for all future games

### Starting a New Challenge

1. Click "New Game" to generate a new challenge setup
   - The app automatically loads your saved level progress
   - Level determines the number of computer opponents you'll face
2. The application will display:
   - Number of players (computer + you)
   - Randomly selected opponents (your name appears in a random position)
   - A special game rule or variant
3. Start your NES Monopoly game with the generated parameters

### Using the Timer

- Click "Start Timer" to begin tracking game time
- The timer can auto-start after a 2-minute delay
- Speech announcements occur at regular intervals

### Logging Results

- Enter your total assets in the Assets field when the game ends
- Close the application or start a new game to save results
- Results are automatically saved to log files
- **Level Progression**:
  - Win (positive assets): Advance to the next level
  - Loss (zero or negative assets): Reset to Level 1
  - Complete Level 7: Reset to Level 1 to start over
  - Your progress is saved in `Level Progress.txt` and loads automatically

### Viewing Reports

- Access the "Reports" menu to view:
  - Player statistics
  - Game history
  - Level progression

## Data Storage

The application stores data in your Documents folder:
- **Location**: `Documents\CPM\`
- **Files**:
  - `Player Name.txt` - Your saved player name (required for new games)
  - `Last Game.txt` - Most recent game details
  - `Full Log.txt` - Complete game history
  - `Player Report.txt` - Statistical summary
  - `Level Progress.txt` - Current difficulty level

## NES Monopoly Characters

The application includes all 8 computer players from NES Monopoly:
- **Arthur** - The businessman
- **Gertrude** - The grandmother
- **Erwin** - The professor
- **Maude** - The sophisticated lady
- **Carmen** - The socialite
- **Isaac** - The working man
- **Penelope** - The young woman
- **Ollie** - The child

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

Copyright © 2026 HiTechCharles

This project is provided as-is for personal use and educational purposes.

## Acknowledgments

- Inspired by NES Monopoly (1991) by Sculptured Software
- Built with Windows Forms and .NET Framework

## Support

For issues, questions, or suggestions:
- Open an issue on [GitHub Issues](https://github.com/HiTechCharles/CPM-WIN/issues)
- Contact: [HiTechCharles](https://github.com/HiTechCharles)

---

**Note**: This application is a companion tool for NES Monopoly gameplay and is not affiliated with or endorsed by Hasbro, Parker Brothers, or any official Monopoly rights holders.
