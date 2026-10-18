# GCC - DON'T LOOK AT IT

A 3D survival game for the finale of the Unity course.

---

# I. IDEA

* Game lấy cảm hứng từ các game survival như **MUCK**, kết hợp với các yếu tố horror, exploration, crafting và combat.
* Người chơi được đưa đến một thế giới bí ẩn, nơi phải thu thập tài nguyên, chế tạo công cụ, xây dựng căn cứ và chiến đấu với các sinh vật để sống sót.
* Thế giới được tạo bằng procedural generation, cho phép người chơi khám phá những khu vực và tài nguyên khác nhau.
* Tuy nhiên, thế giới này tồn tại một thực thể đặc biệt.

> **ONE RULE: DON'T LOOK AT IT.**

* Thực thể này được gọi là **The Entity**.
* Cứ mỗi 2 ngày, The Entity sẽ xuất hiện vào ban đêm.
* Người chơi không được nhìn trực tiếp vào nó khi chưa có vật phẩm bảo vệ.
* Nếu nhìn vào The Entity, camera sẽ bị kéo về phía nó và người chơi sẽ liên tục mất HP.
* Nếu người chơi cố tình tấn công The Entity khi chưa có vật phẩm đặc biệt, The Entity sẽ ngay lập tức giết người chơi.
* Người chơi phải khám phá thế giới, thu thập các vật liệu đặc biệt và chế tạo một cặp kính có khả năng chống lại ảnh hưởng của The Entity.
* Chỉ khi có kính, người chơi mới có thể nhìn trực tiếp và gây damage lên The Entity.
* Trong quá trình sinh tồn, quái vật sẽ ngày càng mạnh hơn và xuất hiện đông hơn.
* Người chơi phải liên tục nâng cấp trang bị, thu thập tài nguyên và chuẩn bị cho những mối đe dọa ở giai đoạn sau.
* Mục tiêu cuối cùng của game là đánh bại The Entity và phá đảo.

---

# II. CONCEPT

## 1. Tổng quan game

* **Title:** DON'T LOOK AT IT
* **Genre:** 3D Survival / Crafting / Horror / Action
* **Art Style:** Low Poly / Stylized
* **Camera:** First Person
* **Players:** 1–4 (planned)
* **World:** Procedurally Generated Survival World
* **Difficulty:** Dynamic Difficulty Scaling
* **Main Boss:** The Entity
* **Special Item:** The Glasses
* **One Rule:** DON'T LOOK AT IT

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
WorldCreation {
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
* Survival Time
* Difficulty Level
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
* Max HP
* ATK
* DEF
* SPD
* Attack Range
* Attack Speed
* Detection Range

Các loại enemy dự kiến:

* **QuaiDanhGan:** Enemy chuyên tấn công cận chiến.
* **QuaiDanhXa:** Enemy chuyên tấn công từ xa.

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

Mỗi khu vực có thể chứa:

* Resource
* Enemy
* Structures
* Loot
* Secrets

### Procedural Generation

* Địa hình được tạo tự động bằng Perlin Noise.
* Thế giới được chia thành các chunk.
* Mỗi chunk có mesh và collider riêng.
* Chunk được tải hoặc giải phóng dựa trên vị trí người chơi.
* Seed được sử dụng để kiểm soát việc tạo địa hình và tài nguyên.

### Chunk Management

```text
Player Position
      │
      ▼
Calculate Current Chunk
      │
      ▼
Check Nearby Chunks
      │
      ├── Missing Chunk → Generate
      │
      └── Far Chunk → Unload
```

Mục tiêu tối ưu:

* Giảm số lượng chunk cần xử lý đồng thời.
* Hạn chế tạo toàn bộ thế giới ngay từ đầu.
* Ưu tiên các chunk cần thiết cho gameplay.
* Giảm tải CPU và GPU khi thế giới mở rộng.

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

Ví dụ:

```text
Day
06:00 → 18:00

Night
18:00 → 06:00
```

Các thông số thời gian sẽ được điều chỉnh trong quá trình phát triển.

### Day / Night Effects

* Ánh sáng môi trường thay đổi.
* Tầm nhìn của người chơi thay đổi.
* Âm thanh môi trường thay đổi.
* Mức độ nguy hiểm có thể tăng vào ban đêm.
* The Entity xuất hiện theo chu kỳ riêng.

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

### Entity Night Rules

* The Entity xuất hiện theo chu kỳ 2 ngày.
* Người chơi cần chuẩn bị trước khi màn đêm đặc biệt bắt đầu.
* Môi trường có thể thay đổi khi Entity xuất hiện.
* Người chơi phải tuân thủ luật không nhìn trực tiếp vào Entity nếu chưa có kính.
* Cơ chế tăng độ khó thông thường không được tự ý thay đổi chu kỳ xuất hiện của Entity.

---

# 3.4. Entity Appearance

Khi Entity xuất hiện:

* Bầu trời thay đổi.
* Ánh sáng giảm.
* Môi trường trở nên yên tĩnh.
* Nhạc nền thay đổi.
* Âm thanh đặc biệt xuất hiện.
* The Entity xuất hiện ở một vị trí xa.

Không nhất thiết hiển thị thông báo:

```text
BOSS SPAWNED
```

Thay vào đó, người chơi phải tự nhận ra những dấu hiệu bất thường của thế giới.

---

# 3.5. Looking At The Entity

Đây là mechanic quan trọng nhất của game.

Người chơi được xác định đang nhìn Entity bằng camera và hệ thống kiểm tra tầm nhìn.

```text
Camera
   │
   ▼
Raycast
   │
   ▼
Entity Collider
   │
   ▼
Check Glasses
   │
   ├── No Glasses → Apply Entity Effects
   │
   └── Has Glasses → Allow Entity Vision
```

Có thể kết hợp với kiểm tra góc nhìn và khoảng cách để xác định người chơi có đang nhìn Entity hay không.

---

# 3.6. Entity Look Effect

Nếu người chơi nhìn Entity khi chưa có kính:

```text
LOOK AT ENTITY
      │
      ▼
CAMERA PULL
      │
      ▼
VISUAL DISTORTION
      │
      ▼
HP DRAIN
      │
      ▼
DEATH
```

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

### Effect 2 — HP Drain

Khi người chơi nhìn Entity:

```text
HP -= LookDamage * Time.deltaTime;
```

Ví dụ:

```text
LookDamage = 5 HP / second
```

### Effect 3 — Visual Distortion

Các hiệu ứng dự kiến:

* Blur
* Vignette
* Camera Shake
* Chromatic Aberration
* Screen Distortion

---

# 3.7. Entity Attack Restriction

Đây là luật quan trọng nhất của Boss.

## Khi chưa có The Glasses

> **The Entity không thể bị damage theo cách thông thường.**

```text
Sword       → 0 Damage
Bow         → 0 Damage
Spear       → 0 Damage
Projectile  → 0 Damage
Magic       → 0 Damage
Explosion   → 0 Damage
```

Nếu người chơi cố tình tấn công Entity khi chưa có kính:

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

Cơ chế phát hiện hành vi tấn công Entity cần được triển khai riêng trong hệ thống combat.

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

### Có kính

```text
LOOK
 ↓
ENTITY REVEALED
 ↓
NO FORCED CAMERA PULL
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

Công thức chế tạo chính thức sẽ được cân bằng dựa trên độ khó thu thập tài nguyên và tiến trình của người chơi.

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

Tài nguyên đặc biệt có thể được đặt tại các khu vực nguy hiểm để khuyến khích khám phá.

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

Các tính năng dự kiến:

* Nhặt vật phẩm.
* Sử dụng vật phẩm.
* Sắp xếp vật phẩm.
* Chuyển vật phẩm vào hotbar.
* Quản lý số lượng vật phẩm.

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

# 3.13. Combat

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

Hệ thống combat phải tương thích với cơ chế tăng độ khó để các chỉ số của kẻ địch được áp dụng nhất quán.

---

# 3.14. Damage System

Công thức sát thương cơ bản dự kiến:

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

Công thức này có thể được điều chỉnh trong quá trình cân bằng game.

---

# 3.15. Enemy AI

Enemy có các trạng thái cơ bản:

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

### QuaiDanhGan

* Phát hiện người chơi trong phạm vi nhất định.
* Tiếp cận người chơi.
* Tấn công ở cự ly gần.
* Chịu ảnh hưởng của trọng lực và địa hình.

### QuaiDanhXa

* Phát hiện và theo dõi người chơi.
* Tấn công từ xa.
* Sử dụng projectile hoặc cơ chế bắn tương đương.
* Cần duy trì khoảng cách chiến đấu phù hợp.

---

# 3.16. Dynamic Difficulty System

Độ khó của game tăng dần theo thời gian sống sót và số ngày trong game.

Mục tiêu là tạo áp lực ngày càng lớn mà không khiến độ khó tăng đột ngột hoặc mất cân bằng.

Hệ thống gồm hai yếu tố:

```text
REAL-TIME SURVIVAL
         +
     IN-GAME DAYS
         │
         ▼
 DIFFICULTY MANAGER
         │
         ├── Enemy Stats
         │
         ├── Spawn Count
         │
         └── Spawn Interval
```

### 3.16.1. Real-Time Difficulty

Thời gian sống sót được tính từ lúc bắt đầu game hoặc bắt đầu một world mới.

```text
Survival Time
      │
      ▼
Calculate Time Factor
      │
      ▼
Update Difficulty
```

Khi người chơi sống sót lâu hơn:

* Enemy HP tăng dần.
* Enemy Damage tăng dần.
* Enemy Movement Speed tăng dần.
* Số lượng enemy trong mỗi đợt tăng dần.
* Khoảng thời gian giữa các đợt spawn giảm dần.

### 3.16.2. Day-Based Difficulty

Số ngày trong game bổ sung một hệ số độ khó.

```text
Current Day
     │
     ▼
Calculate Day Factor
     │
     ▼
Combine With Time Factor
     │
     ▼
Final Difficulty
```

Ví dụ:

* Những ngày đầu: Enemy có chỉ số thấp và xuất hiện ít.
* Những ngày tiếp theo: Enemy mạnh hơn và spawn thường xuyên hơn.
* Giai đoạn sau: Người chơi phải chuẩn bị vũ khí và tài nguyên tốt hơn để sống sót.

### 3.16.3. Continuous Scaling

Độ khó được cập nhật theo thời gian thay vì chỉ tăng tại các mốc cố định.

Ví dụ:

```text
Time = 0 minutes
Enemy HP = 100%

Time = 5 minutes
Enemy HP > 100%

Time = 10 minutes
Enemy HP > Previous HP

Time = 20 minutes
Enemy HP > Previous HP
```

Các chỉ số thực tế phải tuân theo giới hạn được cấu hình để tránh tăng vô hạn.

### 3.16.4. Enemy Stat Scaling

Các chỉ số được điều chỉnh:

| Stat | Effect |
|---|---|
| Max HP | Enemy khó bị tiêu diệt hơn |
| Attack Damage | Enemy gây nhiều sát thương hơn |
| Movement Speed | Enemy tiếp cận nhanh hơn |
| Detection Range | Enemy phát hiện người chơi từ xa hơn |

Công thức thiết kế dự kiến:

```text
Final Enemy HP =
Base HP * Health Multiplier

Final Enemy Damage =
Base Damage * Damage Multiplier

Final Enemy Speed =
Base Speed * Speed Multiplier
```

Các hệ số được tính bởi `DifficultyManager.cs`.

**Important:** Chỉ áp dụng hệ số khi khởi tạo hoặc cập nhật có kiểm soát. Không nhân chỉ số liên tục mỗi frame.

### 3.16.5. Spawn Scaling

Hệ thống spawn điều chỉnh hai thông số chính:

```text
Spawn Count
Spawn Interval
```

Khi độ khó tăng:

```text
Difficulty ↑
    │
    ├── Spawn Count ↑
    │
    └── Spawn Interval ↓
```

Ví dụ minh họa:

| Giai đoạn | Số quái mỗi đợt | Thời gian giữa các đợt |
|---|---:|---:|
| Early Game | 2–3 | 60 giây |
| Mid Game | 4–5 | 40 giây |
| Late Game | 6–8 | 25 giây |
| End Game | 8–10 | 15 giây |

Các giá trị trên là mục tiêu cân bằng ban đầu, không phải thông số cố định.

### 3.16.6. Maximum Enemy Limit

Để đảm bảo hiệu năng, hệ thống phải giới hạn số lượng enemy tồn tại đồng thời.

```text
Can Spawn?
    │
    ▼
Current Enemy Count < Max Enemy Count?
    │
 ┌──┴──┐
Yes    No
 │      │
 ▼      ▼
Spawn  Wait
```

Ngay cả khi thời gian spawn đã đến, hệ thống không được tạo thêm quái nếu đã đạt giới hạn.

### 3.16.7. Difficulty Manager

`DifficultyManager.cs` chịu trách nhiệm:

* Theo dõi thời gian sống sót.
* Nhận số ngày hiện tại từ Day/Night System.
* Tính hệ số độ khó tổng hợp.
* Cung cấp hệ số HP, damage và tốc độ.
* Cung cấp thông tin để điều chỉnh spawn count.
* Cung cấp thông tin để điều chỉnh spawn interval.
* Giới hạn độ khó tối đa theo cấu hình.

`EnemySpawner.cs` chịu trách nhiệm:

* Tạo enemy.
* Điều chỉnh số lượng enemy mỗi đợt.
* Điều chỉnh khoảng thời gian giữa các đợt.
* Kiểm tra giới hạn enemy tồn tại đồng thời.
* Lấy hệ số độ khó từ `DifficultyManager`.

```text
DifficultyManager
       │
       ▼
 Calculate Difficulty
       │
       ├─────────────────┐
       ▼                 ▼
 Enemy AI          Enemy Spawner
       │                 │
       ▼                 ▼
 Enemy Stats       Spawn Count
                   Spawn Interval
```

### 3.16.8. Difficulty Design Rules

* Độ khó tăng liên tục, không nhảy vọt bất thường.
* Thời gian thực và số ngày cùng ảnh hưởng đến độ khó.
* Không thay đổi chu kỳ Entity Night mỗi 2 ngày.
* Các enemy được spawn sau phải nhận chỉ số phù hợp với độ khó hiện tại.
* Không tăng chỉ số vô hạn.
* Không spawn vượt giới hạn enemy tối đa.
* Đảm bảo người chơi có cơ hội thu thập tài nguyên và nâng cấp trang bị trước khi đối mặt với thử thách cao hơn.

---

# 3.17. Hunger System

Người chơi cần ăn để duy trì Hunger.

```text
Hunger
  │
  ▼
Time Passes
  │
  ▼
Hunger Decreases
```

Nếu Hunger quá thấp, các hiệu ứng dự kiến:

* Giảm Stamina.
* Giảm Movement Speed.
* Cuối cùng mất HP.

---

# 3.18. Stamina

Stamina được sử dụng khi:

* Sprint.
* Jump.
* Dodge.
* Special Attack.

```text
Sprint
  │
  ▼
Stamina -= Cost * DeltaTime
```

Khi không sử dụng:

```text
Stamina += Regen * DeltaTime
```

---

# 3.19. Building

Người chơi có thể xây dựng căn cứ.

Các thành phần dự kiến:

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

Enemy có thể nhận hệ số điều chỉnh từ hệ thống độ khó.

```text
Enemy Base Stats
       │
       ▼
Difficulty Multipliers
       │
       ▼
Final Enemy Stats
```

---

## 3. Difficulty Manager

```text
DifficultyManager {
    SurvivalTime,
    CurrentDay,
    TimeDifficultyFactor,
    DayDifficultyFactor,
    FinalDifficulty,
    HealthMultiplier,
    DamageMultiplier,
    SpeedMultiplier,
    SpawnCountMultiplier,
    SpawnIntervalMultiplier
}
```

Các giá trị được tính toán bởi hệ thống và cung cấp cho những thành phần liên quan.

---

## 4. Enemy Spawner

```text
EnemySpawner {
    EnemyPrefabs[],
    BaseSpawnCount,
    BaseSpawnInterval,
    MaxAliveEnemies,
    CurrentAliveEnemies,
    SpawnCount,
    SpawnInterval
}
```

---

## 5. Entity

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

## 6. Glasses

```text
Glasses {
    EntityProtection,
    EntityVision,
    EntityDamagePermission,
    Durability
}
```

---

## 7. Resource

```text
Resource {
    Name,
    Amount,
    Rarity,
    RespawnTime
}
```

---

## 8. Item

```text
Item {
    Name,
    Type,
    StackSize,
    Rarity
}
```

---

## 9. Weapon

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

Thế giới được định hướng bao gồm nhiều biome.

### Forest

Tài nguyên:

* Wood
* Stone
* Food
* Herbs

Enemy dự kiến:

* Wolf
* Goblin
* Corrupted Deer

### Mountain

Tài nguyên:

* Iron
* Coal
* Crystal

Enemy dự kiến:

* Rock Golem
* Mountain Beast

### Volcano

Tài nguyên:

* Obsidian
* Magma Crystal
* Ancient Metal

Enemy dự kiến:

* Fire Elemental
* Lava Golem

### Ancient Ruins

Tài nguyên:

* Ancient Metal
* Moon Glass
* Lore
* Rare Items

Enemy dự kiến:

* Skeleton
* Ancient Guardian
* Corrupted Knight

Các biome và loại enemy trên thuộc định hướng thiết kế, có thể được bổ sung trong những giai đoạn phát triển sau.

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
Survive Longer
     ↓
Difficulty Increases
     ↓
Find Special Materials
     ↓
Craft The Glasses
     ↓
Fight The Entity
     ↓
Defeat The Entity
     ↓
Complete Game
```

---

# 2. Progression Stages

## Stage 1 — SURVIVE

Mục tiêu:

* Tìm thức ăn.
* Thu thập gỗ.
* Thu thập đá.
* Tạo công cụ cơ bản.
* Xây nơi trú ẩn.

Enemy ở giai đoạn này có chỉ số tương đối thấp.

---

## Stage 2 — EXPLORE

Mục tiêu:

* Khám phá biome.
* Tìm cave.
* Tìm ruins.
* Tìm dungeon.
* Tìm lore và tài nguyên hiếm.

Người chơi bắt đầu gặp những loại enemy nguy hiểm hơn.

---

## Stage 3 — ESCALATE

Thời gian sống sót ngày càng dài.

```text
Survival Time ↑
      +
Current Day ↑
      │
      ▼
Difficulty ↑
```

Người chơi phải đối mặt với:

* Enemy có HP cao hơn.
* Enemy gây sát thương lớn hơn.
* Enemy di chuyển nhanh hơn.
* Các đợt spawn đông hơn.
* Khoảng thời gian nghỉ giữa các đợt ngắn hơn.

---

## Stage 4 — DISCOVER

Người chơi phát hiện:

> The Entity xuất hiện mỗi 2 ngày.

Và:

> Không được nhìn vào nó.

Người chơi bắt đầu tìm hiểu cách đối phó với Entity.

---

## Stage 5 — PREPARE

Người chơi cần:

* Nâng cấp trang bị.
* Tìm nguyên liệu hiếm.
* Chuẩn bị vũ khí.
* Tìm tài liệu hoặc manh mối.
* Chế tạo The Glasses.

Độ khó sinh tồn tiếp tục tăng trong quá trình chuẩn bị.

---

## Stage 6 — REVEAL

Người chơi chế tạo thành công kính.

The Entity trở thành:

> **Một thực thể có thể bị tấn công.**

---

## Stage 7 — FINAL BATTLE

Người chơi chờ Entity Night tiếp theo.

```text
Equip Glasses
      ↓
Wait For Entity Night
      ↓
Find Entity
      ↓
Look At Entity
      ↓
Entity Becomes Vulnerable
      ↓
Boss Fight
      ↓
Defeat Entity
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

## 2. Entity Before Glasses

```text
IsVulnerable = false
```

Người chơi:

```text
Attack
  ↓
0 Damage
```

Nếu kích hoạt cơ chế trừng phạt khi tấn công Entity:

```text
Player
  ↓
Invalid Attack
  ↓
Instant Death
```

---

## 3. Entity After Glasses

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

## 4. Entity Boss Phases

### Phase 1 — The Watcher

Entity ở trên bầu trời.

Các weak point xuất hiện.

Người chơi sử dụng kính để nhìn và tấn công.

### Phase 2 — The Fall

Entity rơi xuống mặt đất.

Các đòn tấn công dự kiến:

* Shockwave.
* Area Damage.
* Environmental Destruction.

### Phase 3 — The Eye

Entity chuyển sang combat trực tiếp.

Weak Point chính:

```text
THE EYE
```

### Phase 4 — Don't Look Away

Entity có thể teleport nếu người chơi không nhìn nó trong thời gian quá lâu.

```text
Player Looks Away
       ↓
Timer
       ↓
Too Long?
       ↓
Entity Teleport
       ↓
Possible HP Regeneration
```

### Phase 5 — The Glasses Break

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

Người chơi phải đánh bại Entity trước khi kính vỡ hoàn toàn.

Các phase này là thiết kế mục tiêu cho boss cuối, chưa mặc định đã được triển khai trong game.

---

# VIII. ENTITY NIGHT

## 1. Cycle

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

## 2. Entity Night Flow

```text
Sunset
   ↓
World Becomes Darker
   ↓
Environmental Audio Changes
   ↓
Entity Appears
   ↓
Player Must Avoid Direct Vision
   ↓
Entity Disappears
```

---

# IX. ECONOMY

Game không sử dụng tiền tệ truyền thống.

Nền kinh tế dựa trên:

* Resources.
* Rare Materials.
* Loot.

## 1. Common Resources

```text
Wood
Stone
Fiber
Food
```

## 2. Rare Resources

```text
Iron
Coal
Crystal
Obsidian
```

## 3. Special Resources

```text
Ancient Metal
Moon Glass
Eye Fragment
Entity Residue
```

## 4. Glasses Cost

Ví dụ công thức chế tạo:

```text
The Glasses

Moon Glass       ×2
Obsidian Lens    ×2
Eye Fragment     ×1
Ancient Metal    ×3
Entity Residue   ×1
```

Công thức chính thức sẽ được xác định khi hệ thống crafting và resource progression được hoàn thiện.

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
│ HP       ████████████       │
│ Stamina  ██████████         │
│ Hunger   ████████           │
│ Day      04                 │
│ Difficulty:  NORMAL         │
│                             │
│              +              │
│                             │
│                             │
│ [1][2][3][4][5][6][7][8]   │
└─────────────────────────────┘
```

---

## 3. Difficulty Indicator

Thông tin độ khó có thể được hiển thị trên HUD hoặc trong menu thống kê.

Ví dụ:

```text
SURVIVAL TIME: 20:00
CURRENT DAY: 5
DIFFICULTY: HARD
```

Các mức độ khó hiển thị chỉ phục vụ UI; hệ thống tính toán bên trong vẫn có thể tăng liên tục thay vì nhảy giữa các cấp cố định.

---

## 4. Entity Warning

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

## 5. Boss HUD

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
                 Survival Time Increases
                           │
                           ▼
                   Difficulty Increases
                           │
                           ▼
                   Spawn Pressure Increases
                           │
                           ▼
                     Day / Night
                           │
                           ▼
                 Every 2 Days → Entity
                           │
                           ▼
                  Find Special Materials
                           │
                           ▼
                    Craft The Glasses
                           │
                           ▼
                     Fight The Entity
                           │
                           ▼
                    Defeat The Entity
                           │
                           ▼
                     Game Complete
```

---

# XII. SAVE DATA

Game được định hướng sử dụng dữ liệu lưu riêng cho mỗi world.

```text
WorldData {
    WorldName,
    Seed,
    CurrentDay,
    CurrentTime,
    SurvivalTime,
    PlayerData,
    Inventory,
    WorldState,
    EntityState
}
```

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

## Difficulty Data

```text
DifficultyData {
    SurvivalTime,
    CurrentDay,
    DifficultySettings
}
```

Độ khó cần được tính lại nhất quán khi tải world. Nếu muốn tiếp tục đúng tiến trình cũ, dữ liệu thời gian sống sót và ngày hiện tại phải được lưu hoặc khôi phục từ world state.

---

# XIII. DEVELOPMENT SCOPE

## 1. Player

- [x] First Person Controller
- [x] Basic Movement
- [x] Camera Look
- [ ] Sprint Polish
- [ ] Jump Polish
- [ ] Stamina
- [x] Basic HP
- [ ] Hunger
- [ ] Inventory

## 2. World

- [x] Basic Terrain Generation
- [x] Chunk-Based World
- [x] Basic Resource Spawning
- [ ] Additional Biomes
- [ ] Day / Night
- [ ] LOD
- [ ] Advanced World Optimization

## 3. Survival

- [ ] Gathering Interaction
- [ ] Crafting
- [ ] Food
- [ ] Building
- [ ] Inventory Management
- [ ] Save / Load World

## 4. Combat

- [ ] Complete Melee Enemy AI
- [ ] Complete Ranged Enemy AI
- [ ] Damage Balancing
- [ ] Weapon System
- [ ] Enemy Loot
- [ ] Enemy Spawn Management

## 5. Dynamic Difficulty

- [ ] Create `DifficultyManager.cs`
- [ ] Track Survival Time
- [ ] Connect In-Game Day System
- [ ] Calculate Combined Difficulty
- [ ] Scale Enemy HP
- [ ] Scale Enemy Damage
- [ ] Scale Enemy Movement Speed
- [ ] Increase Spawn Count
- [ ] Reduce Spawn Interval
- [ ] Set Maximum Alive Enemy Limit
- [ ] Balance Early / Mid / Late Game

## 6. Entity

- [ ] Entity Spawn
- [ ] Entity Night
- [ ] Look Detection
- [ ] Camera Pull
- [ ] HP Drain
- [ ] Visual Distortion
- [ ] Instant Death Rule
- [ ] Entity Invulnerability

## 7. Glasses

- [ ] Rare Materials
- [ ] Glasses Recipe
- [ ] Glasses Item
- [ ] Entity Protection
- [ ] Entity Vulnerability

## 8. Final Boss

- [ ] Boss Model
- [ ] Boss HP
- [ ] Boss AI
- [ ] Phase System
- [ ] Weak Points
- [ ] Boss Attacks
- [ ] Final Phase
- [ ] Boss Death
- [ ] Ending

---

# XIV. CORE SYSTEM SUMMARY

Project tập trung vào 7 hệ thống chính.

## 1. Survival

Người chơi phải thu thập tài nguyên, quản lý nhu cầu và sống sót.

## 2. Crafting

Tài nguyên được sử dụng để tạo tools, weapons, armor, food và special items.

## 3. Exploration

Người chơi khám phá thế giới để tìm resources, ruins, dungeons, lore và special materials.

## 4. Combat

Người chơi chiến đấu với nhiều loại enemy có hành vi và cách tấn công khác nhau.

## 5. Dynamic Difficulty

Thời gian sống sót và số ngày trong game quyết định độ khó.

```text
Survival Time ↑
Current Day ↑
      │
      ▼
Enemy Stats ↑
Spawn Count ↑
Spawn Interval ↓
```

## 6. Entity System

The Entity xuất hiện mỗi 2 ngày.

Nếu nhìn vào khi chưa có kính:

```text
Camera Pull
+
HP Drain
+
Visual Distortion
```

## 7. Glasses System

The Glasses thay đổi luật của Entity.

```text
WITHOUT GLASSES
        ↓
ENTITY INVULNERABLE
        ↓
INVALID ATTACK = INSTANT DEATH
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

# XV. CORE DESIGN RULES

Game có 6 nguyên tắc thiết kế chính.

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

### 6. Escalation

Thế giới phải trở nên nguy hiểm hơn theo thời gian.

Độ khó tăng dần nhằm tạo áp lực sinh tồn, đồng thời khuyến khích người chơi khám phá, thu thập tài nguyên và nâng cấp trang bị.

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
SURVIVE
       ↓
EXPLORE
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

**Status:** In Development

**Engine:** Unity

**Language:** C#

**Genre:** 3D Survival / Crafting / Horror / Action

**Camera:** First Person

**World:** Procedurally Generated

**Difficulty:** Real-Time + Day-Based Scaling

**Main Objective:** Defeat The Entity

**Special Item:** The Glasses

**Final Boss:** The Entity

**Core Gameplay:** Explore, Gather, Craft, Survive, Fight

**ONE RULE:**

# DON'T LOOK AT IT.
