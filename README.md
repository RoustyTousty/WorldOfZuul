# Clean Hands - World of Zuul

> A text-based investigative adventure game inspired by the Tangentopoli scandal

**Course:** Software Engineering, 1st Semester  
**Institution:** SDU (University of Southern Denmark)  
**Project Type:** Educational Game Development

---

## 📖 About the Game

**Clean Hands** (Mani Pulite) is an investigative text-based adventure game set in Milan, 1992, during the Tangentopoli corruption scandal. Players take on the role of a detective navigating through various locations, examining clues, and uncovering evidence of political and corporate corruption.

The game features:
- Interactive environments with detailed object examination
- Puzzle-solving through code-breaking and item collection
- Dynamic dialogue with NPCs
- Inventory management system
- Multiple interconnected locations and rooms

### Key Locations

- **ENI Executive Office** - A lavish office filled with secrets, safes, hidden compartments, and incriminating evidence
- **Court House** - Navigate through the judicial system
- **City** - The central hub connecting various investigation points

---

## 🎮 Gameplay

### Available Commands

| Command | Usage | Description |
|---------|-------|-------------|
| `look` | `look` | Examine your current surroundings |
| `move` | `move [ExitName]` | Move through an exit to another location |
| `back` | `back` | Return to the previous room |
| `inspect` | `inspect [Object]` | Examine an object or item closely |
| `open` | `open [Object]` | Open drawers, safes, doors, etc. |
| `take` | `take [ItemName]` | Pick up an item |
| `drop` | `drop [ItemName]` | Drop an item from inventory |
| `use` | `use [ItemName]` | Use an item from your inventory |
| `read` | `read [Object]` | Read documents or signs |
| `answer` | `answer [Object]` | Answer phone calls |
| `push` | `push [Object]` | Push objects like paintings |
| `talk` | `talk [NPCName]` | Initiate conversation with NPCs |
| `inventory` | `inventory` | View your current inventory |
| `help` | `help` | Display the help menu |
| `quit` | `quit` | Exit the game |

### Sample Gameplay

```
> look
--- ENI Executive Office - Milan, 1992 ---
You stand inside a lavish yet oppressive office...

> inspect desk
A heavy mahogany desk, lacquered to a mirror sheen...

> open drawer center
Enter the code for the center drawer: 7-4-2
You open the center drawer...

> take envelope
You picked up the sealed envelope.

> inventory
Inventory:
 - sealed envelope
```

---

## 🛠️ Technical Details

### Technology Stack

- **Language:** C# 10
- **Framework:** .NET 6.0
- **Platform:** Windows Console Application
- **Architecture:** Object-Oriented Design

### Project Structure

```
WorldOfZuul/
├── commands/           # Command parsing and processing
│   ├── Command.cs
│   ├── CommandWords.cs
│   └── Parser.cs
├── Entities/          # Player and NPC classes
│   ├── Player.cs
│   ├── Npc.cs
│   └── Dialogue/
├── World/             # Game world and room management
│   ├── Map.cs
│   ├── Room.cs
│   ├── Location.cs
│   ├── Exit.cs
│   ├── InteractiveObject.cs
│   ├── ENIRoomBuilder.cs
│   └── MontedisonRoomBuilder.cs
├── Items/             # Item and inventory system
│   ├── Item.cs
│   ├── Inventory.cs
│   └── types/
├── Events/            # Event management system
│   ├── EventManager.cs
│   └── IEvent.cs
├── Data/              # JSON world data
│   └── World.json
├── Game.cs            # Main game loop
└── Program.cs         # Entry point
```

### Key Design Patterns

- **Command Pattern** - For processing player commands
- **Builder Pattern** - For constructing complex room interactions (ENIRoomBuilder)
- **State Pattern** - Room states track puzzle progress and flags
- **Observer Pattern** - Event system for triggers and responses

---

## 🚀 Getting Started

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- Windows OS (for optimal console rendering)
- Terminal with Unicode support

### Building the Project

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd WorldOfZuul
   ```

2. **Build the solution**
   ```powershell
   dotnet build WorldOfZuul/WorldOfZuul.sln -c Debug
   ```

3. **Run the game**
   ```powershell
   dotnet run --project WorldOfZuul/WorldOfZuul/WorldOfZuul.csproj
   ```

   Or navigate to the build output:
   ```powershell
   cd WorldOfZuul/WorldOfZuul/bin/Debug/net6.0
   ./WorldOfZuul.exe
   ```

### Quick Start

```powershell
# From the repository root
cd WorldOfZuul/WorldOfZuul
dotnet run
```

---

## 🎯 Features

### Interactive Objects

The game features a rich interactive environment with:
- **Desks** with multiple drawers requiring combination codes
- **Safes** with dial locks
- **Paintings** that reveal hidden compartments
- **Phones** for answering mysterious calls
- **Folders and documents** containing evidence

### Puzzle Mechanics

- **Code-based locks** - Enter correct combinations to unlock drawers and safes
- **Item dependencies** - Use found items to unlock new areas
- **Evidence collection** - Gather documents to progress the investigation
- **Hidden secrets** - Push paintings to reveal compartments

### UI Features

- **ASCII Command Box** - Always-visible command reference in the top-right corner
- **Bottom-aligned input** - Clean prompt positioning at the screen bottom
- **Dynamic screen clearing** - Responsive output rendering
- **Color-coded text** - Enhanced readability (where supported)

---

## 📚 Learning Objectives

This project demonstrates understanding of:

1. **Object-Oriented Programming**
   - Encapsulation, inheritance, and polymorphism
   - Class design and relationships
   - Interface implementation

2. **Software Architecture**
   - Separation of concerns
   - Modular design
   - Design patterns application

3. **Data Structures**
   - Collections (Lists, Dictionaries)
   - Arrays and custom data types
   - Graph structures (room navigation)

4. **File I/O & Serialization**
   - JSON data loading
   - Configuration management

5. **User Experience Design**
   - Command-line interface design
   - Input parsing and validation
   - Feedback and error handling

---

## 🐛 Known Issues

- Console window resizing may affect command box rendering
- Some terminals may not render Unicode box-drawing characters correctly
- Long text outputs may cause scrolling issues in smaller windows

---

## 🔮 Future Enhancements

- Save/load game state functionality
- Additional investigation locations
- More complex dialogue trees
- Achievement system
- Multiple endings based on evidence collected

---

## 👥 Authors

SDU Software Engineering Students - 1st Semester Project

---

## 📝 License

This project is created for educational purposes as part of the SDU Software Engineering curriculum.

---

## 🙏 Acknowledgments

- Inspired by the classic "World of Zuul" educational game
- Historical context based on the real Mani Pulite (Clean Hands) investigation in 1990s Italy
- SDU Faculty for project guidance and requirements

---

**Happy Investigating! 🕵️**
