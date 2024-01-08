using Swos.Domain;
using Swos.Domain.Models;

namespace SWOS.CareerMod
{
    public class CountryFacesExtractor
    {
        private readonly ISwosService swosService;

        public CountryFacesExtractor(ISwosService swosService)
        {
            this.swosService = swosService;
        }

        public async Task Extract(string swosPath)
        {
            var countryFaces = new Dictionary<SwosCountry, Dictionary<SwosFace, int>>();

            swosService.SetSwosPath(swosPath + @"\DATA\");

            foreach (var swosFile in new SwosFileType[] { SwosFileType.Club, SwosFileType.NationalTeam, SwosFileType.CustomTeam }.GetSwosFiles())
            {
                var teamsCount = await swosService.OpenSwosFile(swosFile.Value);
                for (int teamId = 0; teamId < teamsCount; teamId++)
                    for (int playerId = 0; playerId < SwosTeam.PlayersCount; playerId++)
                    {
                        var country = await swosService.ReadPlayerCountry(teamId, playerId);
                        var (_, playerFace) = await swosService.ReadPlayerPositionAndFace(teamId, playerId);
                        countryFaces.TryAdd(country, []);
                        countryFaces[country].TryAdd(playerFace, 0);
                        countryFaces[country][playerFace]++;
                    }
                swosService.CloseSwosFile();
            }
            
            if (!Directory.Exists(swosPath + $@"\COUNTRY\"))
                Directory.CreateDirectory(swosPath + $@"\COUNTRY\");

            foreach (var (country, faces) in countryFaces)
            {
                await File.WriteAllLinesAsync(swosPath + $@"\COUNTRY\{(byte)country:D3}.fac", faces.Select(x => $"{x.Key}={x.Value}"));
            }
        }
    }
}