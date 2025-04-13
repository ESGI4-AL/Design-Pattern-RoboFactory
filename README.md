# Welcome to RoboFactory

## Project Overview
This project implements a Robot Factory Management System in C#, with a focus on applying design patterns. The system tracks inventory, processes assembly orders, and manages the production of different robot models.

## Functional Requirements

### Robot Models
The factory currently produces three robot models:
- **XM-1**: Core_CM1, Generator_GM1, Arms_AM1, Legs_LM1
- **RD-1**: Core_CD1, Generator_GD1, Arms_AD1, Legs_LD1
- **WI-1**: Core_CI1, Generator_GI1, Arms_AI1, Legs_LI1

Each robot consists of:
- 1 main module (requires System_SB1 installation)
- 1 generator
- 1 grasping module
- 1 movement module

### Available Parts
- **Main Modules**: Core_CM1, Core_CD1, Core_CI1
- **Generators**: Generator_GM1, Generator_GD1, Generator_GI1
- **Grasping Modules**: Arms_AM1, Arms_AD1, Arms_AI1
- **Movement Modules**: Legs_LM1, Legs_LD1, Legs_LI1
- **System**: System_SB1 (compatible with all main modules)

### User Commands

#### 1. View Inventory
```
STOCKS
```
Displays the complete inventory of the factory, including robots and parts.

#### 2. Check Required Parts
```
NEEDED_STOCKS [quantity] [robot_model], [quantity] [robot_model], ...
```
Lists all parts needed to produce the requested robots.

#### 3. Get Assembly Instructions
```
INSTRUCTIONS [quantity] [robot_model], [quantity] [robot_model], ...
```
Returns the sequence of assembly instructions needed to produce the requested robots.

#### 4. Verify Order
```
VERIFY [quantity] [robot_model], [quantity] [robot_model], ...
```
Checks if an order can be fulfilled based on current inventory. Returns:
- `AVAILABLE`: Order can be fulfilled
- `UNAVAILABLE`: Insufficient parts in inventory
- `ERROR [message]`: Invalid order

#### 5. Process Order
```
PRODUCE [quantity] [robot_model], [quantity] [robot_model], ...
```
Processes an order, updates inventory, and returns:
- `STOCK_UPDATED`: Order successfully processed
- `ERROR [message]`: Order cannot be processed

### Assembly Instructions

The following instructions are used during robot assembly:
- `PRODUCING [robot_model]`: Start production of a robot
- `GET_OUT_STOCK [quantity] [part]`: Remove parts from inventory
- `INSTALL [system] [part]`: Install system on part
- `ASSEMBLE [result] [part1] [part2]`: Assemble two parts
- `FINISHED [robot_model]`: Complete production of a robot


## Example Usage

### 1. View Inventory (STOCKS)
```
STOCKS
```

Output:
```
5 XM-1
3 RD-1
2 WI-1
10 Core_CM1
8 Core_CD1
7 Core_CI1
15 Generator_GM1
12 Generator_GD1
9 Generator_GI1
15 Arms_AM1
12 Arms_AD1
9 Arms_AI1
15 Legs_LM1
12 Legs_LD1
9 Legs_LI1
20 System_SB1
```

### 2. Check Required Parts (NEEDED_STOCKS)
```
NEEDED_STOCKS 2 XM-1, 1 RD-1
```

Output:
```
2 XM-1 :
2 Core_CM1
2 Generator_GM1
2 Arms_AM1
2 Legs_LM1
1 RD-1 :
1 Core_CD1
1 Generator_GD1
1 Arms_AD1
1 Legs_LD1
Total :
2 Core_CM1
2 Generator_GM1
2 Arms_AM1
2 Legs_LM1
1 Core_CD1
1 Generator_GD1
1 Arms_AD1
1 Legs_LD1
```

### 3. Get Assembly Instructions (INSTRUCTIONS)
```
INSTRUCTIONS 1 XM-1
```

Output:
```
PRODUCING XM-1
GET_OUT_STOCK 1 Core_CM1
GET_OUT_STOCK 1 Generator_GM1
GET_OUT_STOCK 1 Arms_AM1
GET_OUT_STOCK 1 Legs_LM1
INSTALL System_SB1 Core_CM1
ASSEMBLE TMP1 Core_CM1 Generator_GM1
ASSEMBLE TMP1 Arms_AM1
ASSEMBLE TMP3 [TMP1,Arms_AM1] Legs_LM1
FINISHED XM-1
```

### 4. Verify Order (VERIFY)
#### a. Valid Order with Sufficient Inventory
```
VERIFY 1 XM-1, 1 RD-1
```

Output:
```
AVAILABLE
```

#### b. Valid Order with Insufficient Inventory
```
VERIFY 10 XM-1, 10 RD-1, 10 WI-1
```

Output:
```
UNAVAILABLE
```

#### c. Invalid Order
```
VERIFY 1 XM-1, 1 Cat
```

Output:
```
ERROR `Cat` is not a recognized robot
```

### 5. Process Order (PRODUCE)
#### a. Successful Production
```
PRODUCE 1 XM-1
```

Output:
```
STOCK_UPDATED
```

#### b. Failed Production (Insufficient Inventory)
```
PRODUCE 100 XM-1
```

Output:
```
ERROR Insufficient inventory to produce 100 XM-1
```

#### c. Invalid Production Request
```
PRODUCE 1 Unknown-Robot
```

Output:
```
ERROR `Unknown-Robot` is not a recognized robot
```
