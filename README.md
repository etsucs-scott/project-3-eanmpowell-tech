# CSCI 1260 — Project

## Project Instructions
All project requirements, grading criteria, and submission details are provided on **D2L**.  
Refer to D2L as the *authoritative source* for this assignment.

This repository is intentionally minimal. You are responsible for:
- Creating the solution and projects
- Designing the class structure
- Implementing the required functionality

---

## Getting Started (CLI)

You may use **Visual Studio**, **VS Code**, or the **terminal**.

### Create a solution
```bash
dotnet new sln -n ProjectName
```

### Create a project (example: console app)
```bash
dotnet new console -n ProjectName.App
```

### Add the project to the solution
```bash
dotnet sln add ProjectName.App
```

### Build and run
```bash
dotnet build
dotnet run --project ProjectName.App
```

## Notes
- Commit early and commit often.
- Your repository history is part of your submission.
- Update this README with build/run instructions specific to your project.

## Minesweeper

### Board Sizes
- 8x8 → 10 mines  
- 12x12 → 25 mines  
- 16x16 → 40 mines  

---

### Input Commands
Use 0-indexed coordinates.

- `r row col` → Reveal a tile  
- `f row col` → Flag / unflag a tile  
- `q` → Quit  

Example:
r 3 4  
f 2 1  

---

### Seed Usage
- The game prompts for a seed (integer).
- If left blank, a seed is generated using the current time.
- The seed determines mine placement and board layout.
- The seed used is displayed during the game.
- Using the same seed will produce the same board every time.

---

### High Scores
- Stored in: `data/highscores.csv`
- File is created automatically if missing.

**Format:**
size,seconds,moves,seed,timestamp

**Example:**
8,45,32,12345,2026-04-08T12:30:00

**Rules:**
- Fastest time (seconds) determines high score
- Tie-breaker: fewer moves wins
- Only top 5 scores per board size are saved

---

### Board Symbols
- # → Hidden tile  
- f → Flagged tile  
- b → Bomb (when hit)  
- . → Empty revealed tile  
- 1–8 → Number of adjacent mines  

---

### Unit Tests
- Tests are written using xUnit
- Run tests using the Test Explorer
- Tests cover:
  - Board generation
  - Adjacent mine counts
  - Cascade reveal
  - Win/loss conditions
  - Flagging behavior