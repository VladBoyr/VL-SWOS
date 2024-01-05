# VL-SWOS
Проект для футбольной игры [Sensible World Of Soccer](https://ru.wikipedia.org/wiki/Sensible_Soccer) ([en](https://en.wikipedia.org/wiki/Sensible_World_of_Soccer))

# Structure of TEAM.xxx file
|Bytes|Description|
|---|---|
|2|Total number of teams in current TEAM.xxx file (Big Endian order)|
|||
||Team Structure|
|1|Country number = {xxx} in "TEAM.xxx"|
|1|Team Index in current TEAM.xxx file|
|2|Team General Id (Unique for each team) (Big Endian order)|
|1|_(in competitions mode)_ If 01 then team controled by CPU, 02 by PLAYER and 03 by COACH|
|16|Team Name|
|3|Unknown|
|1|Team Tactic|
|1|Division Number (If team isn't present in any league Then 04)|
|5|Home Kit Structure (1 - type of kit, 2-5 - colors)|
|5|Away Kit Structure (1 - type of kit, 2-5 - colors)|
|24|Coach Name|
|16|Players index order|
|||
||Player Structure|
|1|Player Nationality|
|1|Unknown|
|1|Player Number|
|22|Player Name|
|1|_(in career mode)_ Players Cards and Injures|
|1|Position & Hair/Face Color|
|1|Unknown|
|4|Player Skills|
|1|Player Value|
|1|_(in career mode)_ League Goals|
|1|_(in career mode)_ Cup Goals|
|1|_(in career mode)_ LeagueCup Goals|
|1|_(in career mode)_ EuroCup Goals|
|1|_(in career mode)_ 154 - RES Player, 155 - TRIAL Player, 0-127 - Increase Player Value, 128-255 - Decrease Player Value|
