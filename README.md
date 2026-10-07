# DON'T LOOK AT IT

A 3D survival game for the finale of the Unity course.

---

# I. IDEA

* Game lấy cảm hứng từ các game survival/co-op như **MUCK**, kết hợp với các yếu tố horror và exploration.
* Người chơi được đưa đến một thế giới bí ẩn, nơi phải thu thập tài nguyên, chế tạo công cụ, xây dựng căn cứ và chiến đấu với các sinh vật để sống sót.
* Tuy nhiên, thế giới này tồn tại một thực thể đặc biệt.

> **ONE RULE: DON'T LOOK AT IT.**

* Thực thể này được gọi là **The Entity**.
* Cứ mỗi 2 ngày, The Entity sẽ xuất hiện vào ban đêm.
* Người chơi không được nhìn trực tiếp vào nó.
* Nếu nhìn vào The Entity, camera sẽ bị kéo về phía nó và người chơi sẽ liên tục mất HP.
* Nếu người chơi cố tình tấn công The Entity khi chưa có vật phẩm đặc biệt, The Entity sẽ ngay lập tức giết người chơi.
* Người chơi phải khám phá thế giới, thu thập các vật liệu đặc biệt và chế tạo một cặp kính có khả năng chống lại ảnh hưởng của The Entity.
* Chỉ khi có kính, người chơi mới có thể nhìn trực tiếp và **gây damage lên The Entity**.
* Mục tiêu cuối cùng của game là đánh bại The Entity và phá đảo.

---

# II. CONCEPT

## 1. Tổng quan game

* **Title:** DON'T LOOK AT IT
* **Genre:** 3D Survival / Crafting / Horror / Action
* **Art style:** Low Poly / Stylized
* **Camera:** First Person
* **Players:** 1–4
* **World:** Procedurally Generated Survival World
* **One rule:** DON'T LOOK AT IT

### Bối cảnh

Người chơi tỉnh dậy trên một hòn đảo xa lạ.

Không biết mình đến đây bằng cách nào.

Không biết tại sao nơi này không có con người.

Không biết những công trình bỏ hoang thuộc về ai.

Nhưng càng khám phá thế giới, người chơi càng nhận ra rằng nơi này từng có một nền văn minh.

Và nền văn minh đó đã biến mất.

Thứ duy nhất còn lại là những dòng chữ được khắc trên các công trình:

```text
DON'T LOOK AT IT.
```

---

# 2. Thành phần trong game

## 2.1. Thành phần UI

### Màn hình chính

* **New Game:** Tạo world mới.
* **Continue:** Tiếp tục world hiện tại.
* **Settings:** Điều chỉnh âm thanh, đồ họa và gameplay.
* **Quit:** Thoát game.

---

### World Creation

* **World Name**
* **World Seed**
* **Difficulty**
* **Number of Players**

Cấu trúc:

```text
World Creation {
    WorldName,
    Seed,
    Difficulty,
    MaxPlayers
}
```

---

### Trong game

UI chính:

* HP
* Stamina
* Hunger
* Hotbar
* Crosshair
* Interaction Prompt
* Day / Night
* Current Day
* Entity Warning
* Inventory
* Crafting Menu

---

### Pause Menu

```text
Pause
├── Continue
├── Settings
└── Leave World
```

---

## 2.2. Thành phần chính trong game

### Player

Người chơi là nhân vật chính.

Chỉ số cơ bản:

* HP
* Max HP
* Stamina
* Hunger
* Defense
* Attack
* Movement Speed

---

### Enemy

Các sinh vật sống trong thế giới.

Chỉ số cơ bản:

* HP
* ATK
* DEF
* SPD
* Attack Range
* Attack Speed
* Detection Range

---

### Boss — The Entity

Boss cuối của game.

Chỉ số:

* HP
* Max HP
* Phase
* Damage
* Attack Range
* Corruption
* Look Damage
* Invulnerable State

---

### Resource

Các tài nguyên được sử dụng để:

* Craft
* Build
* Upgrade
* Cook
* Create Weapons

Ví dụ:

* Wood
* Stone
* Iron
* Coal
* Food
* Obsidian
* Ancient Metal
* Moon Glass
* Eye Fragment
* Entity Residue

---

### Item

Các vật phẩm người chơi có thể nhặt hoặc chế tạo.

Ví dụ:

* Axe
* Pickaxe
* Sword
* Bow
* Food
* Armor
* Potion
* The Glasses

---

### Weapon

Vũ khí dùng để chiến đấu.

Ví dụ:

* Sword
* Spear
* Bow
* Crossbow
* Magic Weapon

---

# III. CƠ CHẾ TRONG GAME

## 3.1. World

Thế giới là một map 3D có thể khám phá.

Cấu trúc cơ bản:

```text
World
│
├── Forest
├── Mountain
├── Cave
├── Ruins
├── Volcano
├── Ocean
└── Special Areas
```

Mỗi khu vực có:

* Resource
* Enemy
* Structures
* Loot
* Secrets

---

# 3.2. Day / Night System

Thế giới có chu kỳ ngày và đêm.

```text
DAY
 ↓
SUNSET
 ↓
NIGHT
 ↓
SUNRISE
 ↓
DAY
```

Mỗi ngày có một khoảng thời gian cố định.

Ví dụ:

```text
Day
06:00 → 18:00

Night
18:00 → 06:00
```

---

# 3.3. Entity Night

The Entity xuất hiện:

> **Mỗi 2 ngày vào ban đêm.**

Ví dụ:

```text
DAY 1
  ↓
NIGHT 1
  ↓
DAY 2
  ↓
ENTITY NIGHT
  ↓
DAY 3
  ↓
NIGHT 3
  ↓
DAY 4
  ↓
ENTITY NIGHT
```

---

# 3.4. Entity Appearance

Khi Entity xuất hiện:

* bầu trời thay đổi
* ánh sáng giảm
* môi trường trở nên yên tĩnh
* nhạc nền thay đổi
* âm thanh đặc biệt xuất hiện
* The Entity xuất hiện ở một vị trí rất xa

Không có thông báo:

```text
BOSS SPAWNED
```

Thay vào đó, người chơi phải tự nhận ra.

---

# 3.5. Looking At The Entity

Đây là mechanic quan trọng nhất của game.

Người chơi được xác định đang nhìn Entity bằng:

```text
Camera
   │
   ▼
Raycast
   │
   ▼
Entity Collider
```

Có thể kết hợp với góc nhìn:

```text
Camera Forward
       │
       ▼
    Entity
```

Nếu Entity nằm trong vùng nhìn hợp lệ:

```text
IsLookingAtEntity = true
```

---

# 3.6. Entity Look Effect

Nếu người chơi nhìn Entity khi chưa có kính:

```text
LOOK AT ENTITY
      │
      ▼
Camera Pull
      │
      ▼
Visual Distortion
      │
      ▼
HP Drain
      │
      ▼
Death
```

---

### Effect 1 — Camera Pull

Camera bị kéo dần về phía Entity.

Không quay ngay lập tức.

```text
Current Rotation
       │
       ▼
Target Rotation
       │
       ▼
Smooth Rotation
```

Có thể sử dụng:

```csharp
Quaternion.Slerp()
```

---

### Effect 2 — HP Drain

Khi người chơi nhìn Entity:

```text
HP -= LookDamage × DeltaTime
```

Ví dụ:

```text
LookDamage = 5 HP / second
```

---

### Effect 3 — Visual Distortion

Màn hình có thể xuất hiện:

* Blur
* Vignette
* Camera Shake
* Chromatic Aberration
* Screen Distortion

---

# 3.7. Entity Attack Restriction

Đây là luật quan trọng nhất của Boss.

## Khi chưa có The Glasses:

> **The Entity không thể bị damage.**

Mọi loại tấn công đều vô hiệu.

```text
Sword       → 0 Damage
Bow         → 0 Damage
Spear       → 0 Damage
Projectile  → 0 Damage
Magic       → 0 Damage
Explosion   → 0 Damage
```

---

## Nếu người chơi cố tình tấn công Entity

```text
Player Attack
      │
      ▼
Entity
      │
      ▼
Has Glasses?
   │          │
  NO         YES
   │          │
   ▼          ▼
INSTANT      DAMAGE
DEATH        ENTITY
```

---

### Instant Death

Nếu:

```text
HasGlasses == false
```

và người chơi gây damage lên Entity:

```text
PlayerDeath()
```

Người chơi chết ngay lập tức.

Không có combat.

Không có damage exchange.

Không có cơ hội sống sót.

---

# 3.8. The Glasses

The Glasses là vật phẩm quan trọng nhất trong progression.

Cấu trúc:

```text
Glasses {
    IsEquipped,
    EntityProtection,
    EntityVision,
    EntityDamagePermission
}
```

---

### Không có kính

```text
LOOK
 ↓
CAMERA PULL
 ↓
HP DRAIN
```

Tấn công:

```text
ATTACK ENTITY
 ↓
INSTANT DEATH
```

---

### Có kính

```text
LOOK
 ↓
ENTITY REVEALED
 ↓
NO CAMERA CONTROL
 ↓
ENTITY CAN BE DAMAGED
```

---

# 3.9. Glasses Crafting

Người chơi phải thu thập các nguyên liệu đặc biệt.

Ví dụ:

```text
Moon Glass
     +
Obsidian Lens
     +
Eye Fragment
     +
Ancient Metal
     +
Entity Residue
     │
     ▼
 THE GLASSES
```

---

# 3.10. Resource Gathering

Người chơi có thể thu thập tài nguyên từ thế giới.

Ví dụ:

```text
Tree
 ↓
Axe
 ↓
Wood
```

```text
Rock
 ↓
Pickaxe
 ↓
Stone
```

```text
Iron Ore
 ↓
Pickaxe
 ↓
Iron Ore
 ↓
Furnace
 ↓
Iron
```

---

# 3.11. Inventory

Inventory chứa các Item người chơi sở hữu.

Cấu trúc:

```text
Inventory {
    Slots[],
    MaxSlots
}
```

Mỗi slot:

```text
ItemSlot {
    Item,
    Amount
}
```

---

# 3.12. Crafting

Người chơi sử dụng tài nguyên để tạo Item.

Ví dụ:

```text
Wood + Stone
      │
      ▼
Crafting Table
      │
      ▼
Wooden Pickaxe
```

---

# 3.13. Crafting Recipe

Cấu trúc:

```text
Recipe {
    Result,
    ResultAmount,
    Materials[]
}
```

Ví dụ:

```text
Iron Sword {
    Iron: 3,
    Wood: 1
}
```

---

# 3.14. Combat

Combat sử dụng hệ thống vũ khí.

Các loại:

* Melee
* Ranged
* Special

Flow:

```text
Player
  │
  ▼
Attack
  │
  ▼
Check Hit
  │
  ▼
Target
  │
  ▼
Calculate Damage
  │
  ▼
Target HP -= Damage
```

---

# 3.15. Damage System

Công thức cơ bản:

```text
Final Damage = ATK - DEF
```

Nếu:

```text
Final Damage < 1
```

thì:

```text
Final Damage = 1
```

---

# 3.16. Enemy AI

Enemy có các trạng thái:

```text
Idle
  ↓
Patrol
  ↓
Detect Player
  ↓
Chase
  ↓
Attack
  ↓
Dead
```

Flow:

```text
Player Detected?
     │
 ┌───┴───┐
No      Yes
│        │
▼        ▼
Patrol  Chase
         │
         ▼
    In Attack Range?
       │       │
      No      Yes
       │       │
       ▼       ▼
     Chase   Attack
```

---

# 3.17. Hunger System

Người chơi cần ăn để duy trì Hunger.

```text
Hunger
  │
  ▼
Time passes
  │
  ▼
Hunger decreases
```

Nếu Hunger quá thấp:

* giảm Stamina
* giảm Movement Speed
* cuối cùng mất HP

---

# 3.18. Stamina

Stamina được sử dụng khi:

* Sprint
* Jump
* Dodge
* Special Attack

```text
Sprint
  │
  ▼
Stamina -= Cost × DeltaTime
```

Khi không sử dụng:

```text
Stamina += Regen × DeltaTime
```

---

# 3.19. Building

Người chơi có thể xây dựng căn cứ.

Các thành phần:

* Floor
* Wall
* Door
* Roof
* Chest
* Workbench
* Furnace
* Campfire

Flow:

```text
Build Mode
    │
    ▼
Select Structure
    │
    ▼
Check Materials
    │
    ▼
Place
    │
    ▼
Consume Materials
```

---

# IV. GAME ELEMENTS

## 1. Player

```text
Player {
    HP,
    MaxHP,
    Stamina,
    MaxStamina,
    Hunger,
    MaxHunger,
    Attack,
    Defense,
    MovementSpeed,
    Inventory,
    Equipment
}
```

---

## 2. Enemy

```text
Enemy {
    Name,
    HP,
    MaxHP,
    ATK,
    DEF,
    SPD,
    AttackRange,
    AttackSpeed,
    DetectionRange,
    LootTable
}
```

---

## 3. Entity

```text
Entity {
    HP,
    MaxHP,
    Phase,
    IsSpawned,
    IsVulnerable,
    HasTrueForm,
    LookDamage,
    CorruptionDamage,
    AttackPatterns
}
```

---

## 4. Glasses

```text
Glasses {
    EntityProtection,
    EntityVision,
    EntityDamagePermission,
    Durability
}
```

---

## 5. Resource

```text
Resource {
    Name,
    Amount,
    Rarity,
    RespawnTime
}
```

---

## 6. Item

```text
Item {
    Name,
    Type,
    StackSize,
    Rarity
}
```

---

## 7. Weapon

```text
Weapon {
    Name,
    Damage,
    AttackSpeed,
    AttackRange,
    Durability,
    Type
}
```

---

# V. WORLD

## 1. Biomes

Thế giới bao gồm nhiều biome.

### Forest

Tài nguyên:

* Wood
* Stone
* Food
* Herbs

Enemy:

* Wolf
* Goblin
* Corrupted Deer

---

### Mountain

Tài nguyên:

* Iron
* Coal
* Crystal

Enemy:

* Rock Golem
* Mountain Beast

---

### Volcano

Tài nguyên:

* Obsidian
* Magma Crystal
* Ancient Metal

Enemy:

* Fire Elemental
* Lava Golem

---

### Ancient Ruins

Tài nguyên:

* Ancient Metal
* Moon Glass
* Lore
* Rare Items

Enemy:

* Skeleton
* Ancient Guardian
* Corrupted Knight

---

# VI. LEVEL / PROGRESSION

## 1. Survival Progression

```text
Start World
     ↓
Gather Resources
     ↓
Craft Tools
     ↓
Build Base
     ↓
Explore
     ↓
Fight Enemies
     ↓
Find Special Materials
     ↓
Craft Glasses
     ↓
Fight Entity
     ↓
Defeat Entity
     ↓
Complete Game
```

---

# 2. Progression Stages

## Stage 1 — SURVIVE

Mục tiêu:

* tìm thức ăn
* thu thập gỗ
* thu thập đá
* tạo công cụ
* xây nơi trú ẩn

---

## Stage 2 — EXPLORE

Mục tiêu:

* khám phá biome
* tìm cave
* tìm ruins
* tìm dungeon
* tìm lore

---

## Stage 3 — DISCOVER

Người chơi phát hiện:

> The Entity xuất hiện mỗi 2 ngày.

Và:

> Không được nhìn vào nó.

---

## Stage 4 — PREPARE

Người chơi bắt đầu tìm nguyên liệu để chế tạo:

> **The Glasses**

---

## Stage 5 — REVEAL

Người chơi chế tạo thành công kính.

The Entity trở thành:

> **một thực thể có thể bị tấn công.**

---

## Stage 6 — FINAL BATTLE

Người chơi phải đợi Entity Night tiếp theo.

Sau đó:

```text
Equip Glasses
      ↓
Find Entity
      ↓
Look At Entity
      ↓
Entity Reveals
      ↓
Boss Fight
```

---

# VII. THE ENTITY

## 1. Entity State

```text
EntityState {
    Dormant,
    Appearing,
    Watching,
    Vulnerable,
    Attacking,
    PhaseTransition,
    Dead
}
```

---

# 2. Entity Before Glasses

```text
IsVulnerable = false
```

Người chơi:

```text
Attack
  ↓
0 Damage
```

Nếu thực sự gây damage event lên Entity:

```text
Player
  ↓
Invalid Attack
  ↓
Instant Death
```

---

# 3. Entity After Glasses

```text
IsVulnerable = true
```

Người chơi có thể:

```text
Attack
  ↓
Calculate Damage
  ↓
Entity HP -= Damage
```

---

# 4. Entity Boss Phases

## Phase 1 — The Watcher

Entity ở trên bầu trời.

Các weak point xuất hiện.

Người chơi sử dụng kính để nhìn và tấn công.

---

## Phase 2 — The Fall

Entity rơi xuống mặt đất.

Tạo:

* Shockwave
* Area Damage
* Environmental Destruction

---

## Phase 3 — The Eye

Entity chuyển sang combat trực tiếp.

Weak Point chính:

```text
THE EYE
```

---

## Phase 4 — Don't Look Away

Entity bắt đầu teleport nếu người chơi không nhìn nó.

```text
Player Looks Away
       ↓
Timer
       ↓
Too Long?
       ↓
Entity Teleport
       ↓
Regenerate HP
```

---

## Phase 5 — The Glasses Break

Kính bắt đầu bị phá hủy.

```text
Boss Low HP
     ↓
Glasses Durability ↓
     ↓
Cracks
     ↓
Final Phase
```

Người chơi phải giết Entity trước khi kính vỡ hoàn toàn.

---

# VIII. ENTITY NIGHT

## 1. Cycle

```text
Day 1
  ↓
Night
  ↓
Day 2
  ↓
Entity Night
  ↓
Day 3
  ↓
Night
  ↓
Day 4
  ↓
Entity Night
```

---

## 2. Entity Night Flow

```text
Sunset
   ↓
World becomes darker
   ↓
Environmental Audio changes
   ↓
Entity Spawn
   ↓
Player can see Entity
   ↓
Don't Look At It
   ↓
Entity disappears
```

---

# IX. ECONOMY

Game không sử dụng tiền tệ truyền thống.

Nền kinh tế dựa trên:

* Resources
* Rare Materials
* Loot

---

## 1. Common Resources

```text
Wood
Stone
Fiber
Food
```

---

## 2. Rare Resources

```text
Iron
Coal
Crystal
Obsidian
```

---

## 3. Special Resources

```text
Ancient Metal
Moon Glass
Eye Fragment
Entity Residue
```

---

## 4. Glasses Cost

Ví dụ:

```text
The Glasses

Moon Glass       ×2
Obsidian Lens    ×2
Eye Fragment     ×1
Ancient Metal    ×3
Entity Residue   ×1
```

---

# X. UI / UX

## 1. Main Menu

```text
Main Menu
├── New Game
├── Continue
├── Settings
└── Quit
```

---

## 2. HUD

```text
┌─────────────────────────────┐
│ HP      ██████████████      │
│ Stamina ████████████        │
│ Hunger  ██████████          │
│                             │
│                             │
│            +                │
│                             │
│                             │
│ [1][2][3][4][5][6][7][8]   │
└─────────────────────────────┘
```

---

## 3. Entity Warning

Không hiển thị boss health bar khi Entity chưa thể bị đánh.

Thay vào đó:

```text
...
```

hoặc:

```text
DON'T LOOK.
```

---

## 4. Boss HUD

Chỉ xuất hiện khi người chơi đã có kính và Entity trở nên vulnerable.

```text
THE ENTITY

████████████████████
```

---

# XI. GAME FLOW

```text
                         START
                           │
                           ▼
                      Main Menu
                           │
                ┌──────────┴──────────┐
                ▼                     ▼
            New Game              Continue
                │                     │
                ▼                     ▼
          Create World             Load World
                │                     │
                └──────────┬──────────┘
                           ▼
                       GAME WORLD
                           │
                           ▼
                    Explore / Gather
                           │
                           ▼
                        Craft
                           │
                           ▼
                        Build
                           │
                           ▼
                     Fight Enemies
                           │
                           ▼
                     Day / Night
                           │
                           ▼
                 Every 2 Days → Entity
                           │
                           ▼
                 Player Sees Entity
                           │
                ┌──────────┴──────────┐
                ▼                     ▼
           Has Glasses?              No
                │                     │
               Yes                    ▼
                │                Don't Attack
                ▼                     │
         Entity Vulnerable             │
                │                      ▼
                ▼                Attack Entity?
          Fight Entity             │         │
                │                 No        Yes
                │                  │         │
                │                  ▼         ▼
                │               Survive   Instant Death
                │
                ▼
          Entity Defeated
                │
                ▼
            Game Complete
```

---

# XII. SAVE DATA

Game sử dụng một file save cho mỗi world.

```text
WorldData {
    WorldName,
    Seed,
    CurrentDay,
    PlayerData,
    Inventory,
    WorldState,
    EntityState
}
```

---

## Player Data

```text
PlayerData {
    Position,
    Rotation,
    HP,
    Hunger,
    Inventory,
    Equipment,
    HasGlasses
}
```

---

## World Data

```text
WorldData {
    Seed,
    CurrentDay,
    CurrentTime,
    DiscoveredLocations[],
    BuiltStructures[],
    CollectedResources[]
}
```

---

# XIII. DEVELOPMENT SCOPE

## 1. MVP

### Player

* [ ] First Person Controller
* [ ] Movement
* [ ] Sprint
* [ ] Jump
* [ ] Stamina
* [ ] HP
* [ ] Hunger
* [ ] Inventory

---

### World

* [ ] Terrain
* [ ] Forest
* [ ] Resource Nodes
* [ ] Day / Night
* [ ] Basic Procedural Generation

---

### Survival

* [ ] Gathering
* [ ] Crafting
* [ ] Food
* [ ] Building
* [ ] Inventory

---

### Combat

* [ ] Melee Weapon
* [ ] Ranged Weapon
* [ ] Enemy AI
* [ ] Damage System
* [ ] Enemy Loot

---

### Entity

* [ ] Entity Spawn
* [ ] Entity Night
* [ ] Look Detection
* [ ] Camera Pull
* [ ] HP Drain
* [ ] Instant Death
* [ ] Entity Invulnerability

---

### Glasses

* [ ] Rare Materials
* [ ] Glasses Recipe
* [ ] Glasses Item
* [ ] Entity Protection
* [ ] Entity Vulnerability

---

### Final Boss

* [ ] Boss Model
* [ ] Boss HP
* [ ] Boss AI
* [ ] Phase System
* [ ] Weak Points
* [ ] Boss Attacks
* [ ] Final Phase
* [ ] Boss Death
* [ ] Ending

---

# XIV. CORE SYSTEM SUMMARY

Project tập trung vào 6 hệ thống chính:

## 1. Survival

Người chơi phải thu thập tài nguyên, ăn uống và sống sót.

---

## 2. Crafting

Tài nguyên được sử dụng để tạo:

* Tools
* Weapons
* Armor
* Food
* Special Items

---

## 3. Exploration

Người chơi khám phá thế giới để tìm:

* Resources
* Ruins
* Dungeons
* Lore
* Special Materials

---

## 4. Entity System

The Entity xuất hiện mỗi 2 ngày.

Nếu nhìn vào:

```text
Camera Pull
+
HP Drain
+
Visual Distortion
```

---

## 5. Glasses System

The Glasses thay đổi luật của Entity.

```text
WITHOUT GLASSES
        ↓
ENTITY INVULNERABLE
        ↓
ATTACK = INSTANT DEATH
```

Sau khi có kính:

```text
WITH GLASSES
        ↓
ENTITY VULNERABLE
        ↓
ATTACK = DAMAGE
        ↓
BOSS FIGHT
```

---

## 6. Final Progression

```text
SURVIVE
   ↓
EXPLORE
   ↓
GATHER
   ↓
CRAFT
   ↓
DISCOVER
   ↓
FIND SPECIAL MATERIALS
   ↓
CRAFT THE GLASSES
   ↓
WAIT FOR ENTITY NIGHT
   ↓
LOOK AT IT
   ↓
FIGHT THE ENTITY
   ↓
DEFEAT THE ENTITY
   ↓
COMPLETE GAME
```

---

# XV. CORE DESIGN RULES

Game có 5 nguyên tắc thiết kế chính:

### 1. Survival

Người chơi phải sống sót.

### 2. Exploration

Thế giới phải khiến người chơi muốn khám phá.

### 3. Discovery

Không phải mọi thứ đều được giải thích.

### 4. Fear

Người chơi phải sợ việc nhìn vào Entity.

### 5. Progression

Người chơi phải chuẩn bị đủ trước khi đối đầu với Boss.

---

# XVI. THE ONE RULE

Tất cả gameplay cuối cùng đều quay về một luật:

```text
┌─────────────────────────────┐
│                             │
│      DON'T LOOK AT IT       │
│                             │
└─────────────────────────────┘
```

Nhưng để phá đảo game:

```text
DON'T LOOK AT IT
       ↓
GET THE GLASSES
       ↓
LOOK AT IT
       ↓
KILL IT
```

> **You spend the entire game avoiding the thing you eventually have to look at.**

---

# XVII. PROJECT STATUS

**Status:** Pre-production

**Engine:** Unity

**Genre:** 3D Survival / Crafting / Horror / Action

**Players:** 1–4

**Camera:** First Person

**World:** Procedurally Generated

**Main Objective:** Defeat The Entity

**Special Item:** The Glasses

**Final Boss:** The Entity

**ONE RULE:**

# DON'T LOOK AT IT.
