# VL-SWOS
Project for [Sensible World Of Soccer](https://en.wikipedia.org/wiki/Sensible_World_of_Soccer) / Проект для футбольной игры [Sensible World Of Soccer](https://ru.wikipedia.org/wiki/Sensible_Soccer)

# Structure of TEAM.xxx file
|Bytes|Description|
|---|---|
|2|Total number of teams in current TEAM.xxx file (big-endian)|
|||
||Team Structure|
|1|Country number = {xxx} in "TEAM.xxx"|
|1|Team Index in current TEAM.xxx file|
|2|Team General Id (Unique for each team) (big-endian)|
|1|_(in competitions AND career mode)_ If 01 then team controled by CPU, 02 by PLAYER and 03 by COACH|
|16|Team Name|
|3|Unknown|
|1|Team Tactic|
|1|Division Number (If team isn't present in any league then 04)|
|5|Home Kit Structure (1 - type of kit, 2-5 - colors)|
|5|Away Kit Structure (1 - type of kit, 2-5 - colors)|
|24|Coach Name|
|16|Players index order|
|||
||Player Structure|
|1|Player Nationality|
|1|_(in career mode)_ Real Player Value. Player Value tends to this value. When a player is not playing (injured or carded), this value is set to zero. When a player is placed on the wrong position (for example, CF on CD), this value is decreased.|
|1|Player Number|
|22|Player Name|
|1|Unknown|
|1|Position & Hair/Face Color|
|1|_(in career mode)_ Players Cards and Injures|
|4|Player Skills|
|1|Player Value|
|1|_(in career mode)_ League Goals|
|1|_(in career mode)_ Cup Goals|
|1|_(in career mode)_ LeagueCup Goals|
|1|_(in career mode)_ EuroCup Goals|
|1|_(in career mode)_ 154 - RES Player, 155 - TRIAL Player, 0-127 - Increase Player Value, 128-255 - Decrease Player Value|

# Structure of CAREER file (*.car)
|Offset|Offset (hex)|Bytes|Description|
|---|---|---|---|
|0|0000|2|Total number of teams playing in EUROCUPS (little-endian OR 1 byte). 80 teams = 16 EUROPEAN CHAMPIONS CUP + 32 EUROPEAN CUP-WINNERS CUP + 32 UEFA CUP|
|2|0002|54720|Teams playing in the EUROCUPS (80 teams * 684 bytes)|
|54724|D5C4|1|Number of already played seasons (max 20 seasons). Set this value to zero If you want to play indefinitely.|
|54748|D5DC|4|Bank Account of team (little-endian). Max = 100 millions.|
|55098|D73A|2|MR./MS. of Player-Coach|
|55103|D73F|8|First Name of Player-Coach|
|55112|D748|12|Surname of Player-Coach|
|55125|D755|1|Nationality of Player-Coach|
|56128||30|Players index order (30 players). If player doesn't exists then 255 (FF)|
|56160||76|Team structure (but Global Id isn't the same)|
|56236||1140|All players in team (30 players * 38 bytes)|
|57452||1|Total number of players in team (max 30 players)|
