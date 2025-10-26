###### Assignment #2 
## Let's Play — **Ataxx**

> *Optional advanced variant:* if you want a more challenging and visually interesting geometry, you may implement **Hexxagon** — the same rules as Ataxx, but played on a **hexagonal grid** instead of a square one.  
> The rest of the requirements stay identical.


### Goals
After the second workshop, where you implemented your first MVC project, the goals of this assignment are:

1. To apply feedback and strengthen your MVC/MVP implementation skills on a more complex, strategic, and interactive game.
2. To practice decomposing a real game into maintainable components while following SOLID and clean code principles.
3. To design and test logic that involves state transitions, move validation, and AI-driven opponents.


### Overview
This assignment extends your previous MVC experience with a **real, playable board game — Ataxx**.

Ataxx is a **turn-based strategy game** played on a grid where each move can **clone** or **jump** your piece to a new position.  
All opponent pieces adjacent to the landing cell are **converted** to your color.  
The player who controls the most pieces at the end of the game wins.

You are free to reuse your workshop structure (e.g., DI container, command framework) but the project must demonstrate clean separation between **Model**, **View**, and **Controller** (or **Presenter**).

Spend some time playing the actual game to learn the rules and get the feel of the game. Ataxx version is [here](https://www.onlinesologames.com/ataxx), Hexxagon is [here](https://hexxagon.com/). They are pretty old-school in design, but that's maximum I could find online.

Also, I recommend watching [this part](https://youtu.be/Nsjsiz2A9mg?t=1893) of R. Martin talk explaining MVC/MVP and what went wrong on web with it. [Original M. Fowler article](https://martinfowler.com/eaaDev/uiArchs.html) explaining MVC/MVP evolution (ViewModels are there under the name of Presentation Model) can also help, but language can be a bit hard to grasp.


### Task

#### 1. Implement the Ataxx Game
- The board size should be **7×7** by default but configurable.
- Two players: **Player X** and **Player O** (human or bot).
- Each cell can be:
  - empty,
  - occupied by Player X,
  - occupied by Player O,
  - blocked (wall).
- The game must include **three predefined board layouts** with different wall configurations. Below is the sample; you can select your own variant 
  At the start of each session, **one layout is chosen randomly**.

#### Example wall variations
```
Layout 1 (classic)
  A B C D E F G
1 X . . . . . O
2 . . . . . . .
3 . . . . . . .
4 . . . . . . .
5 . . . . . . .
6 . . . . . . .
7 O . . . . . X

Layout 2 (cross)
  A B C D E F G
1 X . . # . . O
2 . . . # . . .
3 . . . # . . .
4 # # # . # # #
5 . . . # . . .
6 . . . # . . .
7 O . . # . . X

Layout 3 (center block)
  A B C D E F G
1 X . . . . . O
2 . . . # . . .
3 . . # . # . .
4 . # . . . # .
5 . . # . # . .
6 . . . # . . .
7 O . . . . . X
```


### 2. Game Rules
| Rule | Description |
|------|--------------|
| **Starting position** | Each corner has one piece (X in top-left & bottom-right, O in top-right & bottom-left). |
| **Turn** | Players alternate turns. |
| **Move** | A player may: <br>• **Clone**: move to an adjacent cell (orthogonal or diagonal). <br>• **Jump**: move two cells away (orthogonal or diagonal). <br>• Landing cell must be empty and not blocked. |
| **Conversion** | All opponent pieces adjacent (8 directions) to the landing cell become the moving player’s color. |
| **Walls** | Walls (#) are permanent obstacles that block movement and cannot be occupied. |
| **No moves** | If a player has no legal moves, their turn is skipped. |
| **End condition** | Game ends when the board is full or neither player can move. |
| **Winner** | Player with the most pieces on board. |


### 3. Game Modes
- **Player vs Player (PvP)**
- **Player vs Bot (PvE)** — simple bot that performs random valid moves.
- **Hard Bot (optional +1 point)** — bot that chooses the move maximizing its resulting piece count or uses minimax-like heuristic.


### 4. Views
You must implement **two views** to demonstrate the MVC/MVP pattern, with ability to **swap view** during gameplay (e.g. via special command)

#### Simple View
Plain grid with coordinates and minimal output.
```
Ataxx - Simple View
-------------------
  A B C D E F G
1 X . . . . . O
2 . . . . . . .
3 . . . . . . .
4 . . . . . . .
5 . . . . . . .
6 . . . . . . .
7 O . . . . . X

> move D3
```

#### Enhanced View
ASCII-art grid with colored or styled cells and real-time feedback.
```
Ataxx - Enhanced View
────────────────────────────────
Player X (You) vs Bot (O)
Turn 7

   A   B   C   D   E   F   G
  ┌───┬───┬───┬───┬───┬───┬───┐
1 │ X │   │   │   │   │   │ O │
  ├───┼───┼───┼───┼───┼───┼───┤
2 │   │ X │   │   │   │   │   │
  ├───┼───┼───┼───┼───┼───┼───┤
3 │   │   │ O │   │ X │   │   │
  └───┴───┴───┴───┴───┴───┴───┘

Available commands:
- move [coordinate] (e.g., D3)
- hint
- undo
- quit
```

### 5. Additional Requirements
| Feature | Description |
|----------|--------------|
| **New game** | Player can start a new game after the previous one without restarting the app. |
| **Hint system** | Show all valid moves for current player. |
| **Undo** | In PvE mode, player can undo their last move within 3 seconds. |
| **Time limit** | Each turn must be completed within 20 seconds; otherwise, a random move is made automatically. |
| **Statistics** | Store overall play statistics (games played, wins/losses, average move count, etc.) across sessions. |
| **Unit tests** | Cover critical logic: move validation, conversion, win detection, and no-move scenarios. |
| **Optional +1 point** | Hard Bot mode as described above. |


### 6. Grading Policy
| Criterion | Points |
|------------|--------|
| Basic logic (PvP) | 3 |
| Two views + switching | 1 |
| PvE with simple bot | 1 |
| Hints + undo | 2 |
| Time limit + statistics | 1 |
| **Total** | **8** |
| **+1 (bonus)** | Hard bot (heuristic/minimax) |


### 8. Extra Learning Materials
- [Ataxx — Wikipedia](https://en.wikipedia.org/wiki/Ataxx)  
- Martin Fowler — [Original article on MVC/MVP evolution](https://martinfowler.com/eaaDev/uiArchs.html)  
- Robert C. Martin — [MVC Explained in “Clean Architecture” Talk](https://youtu.be/Nsjsiz2A9mg?t=1893)  

### Notes
- Focus on **clarity and separation** between Model, View, and Controller.  
- Aim for **clean extensibility** (e.g., you should be able to swap Ataxx → Hexxagon with minimal controller changes).  

