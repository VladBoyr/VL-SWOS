# Design of Career Mod

# Next Season procedures
### 1. Teams are ranked by average player value (only best 16 players).
Weight of GK/DEF's player value higher than FWD because there are fewer high-value GK than high-value FWD.<br/><br/>
AC MILAN's rank = 0<br/>
URALMASH's rank = 851

### 2. Teams plan their transfers.
Transfers Count = random[6..16]<br/><br/>
Choose Transfer Player's Team
|Condition|Transfer Player's Team|
|---|---|
|New Team Rank < 200|New Team Rank + random[0..400]|
|New Team Rank > Last Team Rank - 200|New Team Rank + random[-400..0]|
|Otherwise|New Team Rank + random[-200..200]|

Choose Transfer Player
|Condition|Transfer Player|
|---|---|
|ABS(New Team Rank - Transfer Player's Team Rank) <= 50|random[0..15] best player|
|(New Team Rank - Transfer Player's Team Rank) > 50|random[8..15] best player|
|(Transfer Player's Team Rank - New Team Rank) > 50|random[0..7] best player|

Choose New Transfer Player's Value<br/>
_(New Value depends on Player Position.)_
|Condition|New Value|
|---|---|
|(Old Value - 20) > Avg Player Value (New Team)|Avg Player Value (New Team) + random[10..20]|
|(Old Value - 10) > Avg Player Value (New Team)|Old Value + random[-10..0]|
|Old Value > Avg Player Value (New Team)|Old Value + random[-5..5]|
|(Old Value + 10) > Avg Player Value (New Team)|Old Value + random[0..10]|
|Otherwise|Avg Player Value (New Team) + random[-10..0]|

### 3. Transfer bidding.
Transfer Player's Team selects 6 best transfer offers (with highest player value) to 6 different players.

### 4. Changing Players skills and value.
|Condition|New Player Value|
|---|---|
|Old Player Value < 12|Old Player Value + random[-3..7]|
|Otherwise|Old Player Value + random[-5..5]|

Older version - New Player Value doesn't depends on player age but should.

### 5. Junior players become to problem positions. 

# Skill's weight for Player position
|Position|P|V|H|T|C|S|F|
|---|---|---|---|---|---|---|---|
|RB/LB|6|6|1|6|6|6|1|
|D|5|5|5|6|5|5|1|
|RW/LW|6|6|1|6|6|6|1|
|M|6|6|1|6|6|6|1|
|A|1|6|6|1|6|6|6|

Primary Skills Count = random[0..7]<br/>
Best skills set as primary. If skills equals then random skill set as primary.
