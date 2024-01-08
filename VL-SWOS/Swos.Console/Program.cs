using Swos.Domain;
using Swos.Domain.Models;

internal class Program
{
    private static async Task Main()
    {
        var swosService = new SwosService();
        swosService.SetSwosPath(@"c:\Games\SW_SOCCR\DATA");

        /*
        var _ = await swosService.OpenSwosFile("team.031");
        await swosService.WritePlayerRating(20, 1, 255);
        swosService.CloseSwosFile();
        */

        var findTeams = await swosService.FindTeams(new SwosFindTeamQuery
        {
            FileTypes = [SwosFileType.Club],
            GlobalIds = new int[]
            {
                1777
            }
        });

        foreach (var findTeam in findTeams)
        {
            var _ = await swosService.OpenSwosFile(findTeam.SwosFile);
            var team = await swosService.ReadTeam(findTeam.TeamId);
            Console.WriteLine($"File = {findTeam.SwosFile}");
            Console.WriteLine($"Team Id = {findTeam.TeamId}");
            Console.WriteLine($"Team Global Id = {team.GlobalId}");
            Console.WriteLine($"Team Country = {team.Country}");
            Console.WriteLine($"Team Name = {team.Name}");
            Console.WriteLine();
        }

        var findPlayers = await swosService.FindPlayers(new SwosFindPlayerQuery
        {
            FileTypes = [SwosFileType.Club],
            MinPrice = 7000000,
            Positions = new SwosPosition[]
            {
                SwosPosition.A
            },
            MinSkills = new Dictionary<SwosSkill, byte>()
            {
                { SwosSkill.Shooting, 7 },
                { SwosSkill.Speed, 7 },
                { SwosSkill.BallControl, 7 }
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
            Console.WriteLine($"Passing = {skills[SwosSkill.Passing].SkillValue}");
            Console.WriteLine($"Shooting = {skills[SwosSkill.Shooting].SkillValue}");
            Console.WriteLine($"Heading = {skills[SwosSkill.Heading].SkillValue}");
            Console.WriteLine($"Tackling = {skills[SwosSkill.Tackling].SkillValue}");
            Console.WriteLine($"BallControl = {skills[SwosSkill.BallControl].SkillValue}");
            Console.WriteLine($"Speed = {skills[SwosSkill.Speed].SkillValue}");
            Console.WriteLine($"Finishing = {skills[SwosSkill.Finishing].SkillValue}");
            Console.WriteLine();
        }
    }
}