using Swos.Domain;
using Swos.Domain.Models;

internal class Program
{
    private static async Task Main()
    {
        var swosService = new SwosService();
        swosService.SetSwosPath(@"c:\Games\SW_SOCCR\DATA");

        var findPlayers = await swosService.FindPlayers(new SwosFindPlayerQuery
        {
            FileTypes = [SwosFileType.AnyTeam],
            MaxRating = 20,
            MinSkills = new Dictionary<SwosSkill, byte>()
            {
                { SwosSkill.Passing, 6 },
                { SwosSkill.Speed, 6 }
            }
        });

        foreach (var findPlayer in findPlayers)
        {
            var _ = await swosService.OpenSwosFile(findPlayer.SwosFile);

            Console.WriteLine($"File = {findPlayer.SwosFile}");

            Console.WriteLine($"Team Id = {findPlayer.TeamId}");
            Console.WriteLine($"Team Country = {await swosService.ReadTeamCountry(findPlayer.TeamId)}");
            Console.WriteLine($"Team Name = {await swosService.ReadTeamName(findPlayer.TeamId)}");

            Console.WriteLine($"Player Id = {findPlayer.PlayerId}");
            Console.WriteLine($"Player Name = {await swosService.ReadPlayerName(findPlayer.TeamId, findPlayer.PlayerId)}");
            Console.WriteLine($"Player Rating = {await swosService.ReadPlayerRating(findPlayer.TeamId, findPlayer.PlayerId)}");
            var (playerPosition, _) = await swosService.ReadPlayerPositionAndFace(findPlayer.TeamId, findPlayer.PlayerId);
            var skills = await swosService.ReadPlayerSkills(findPlayer.TeamId, findPlayer.PlayerId, playerPosition);
            Console.WriteLine($"Player Passing = {skills[SwosSkill.Passing].SkillValue}");
            Console.WriteLine($"Player Passing Primary = {skills[SwosSkill.Passing].PrimarySkill}");
            Console.WriteLine($"Player Speed = {skills[SwosSkill.Speed].SkillValue}");
            Console.WriteLine($"Player Speed Primary = {skills[SwosSkill.Speed].PrimarySkill}");
        }
    }
}