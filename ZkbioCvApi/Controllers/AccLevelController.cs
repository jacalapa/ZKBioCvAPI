using Microsoft.AspNetCore.Mvc;
using ZKBioClient;
using ZkbioCvApi.Model;

namespace ZkbioCvApi.Controllers
{
    [ApiController]
    [Route("accLevel")]
    public class AccLevelController : Controller
    {
        private readonly ApiClientProvider apiClientProvider;

        public AccLevelController(ApiClientProvider apiClientProvider)
        {
            this.apiClientProvider = apiClientProvider;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            ApiClient client = this.apiClientProvider.Get();
            var levels = await client.GetLevelsAsync();

            var result = Result<AccLevel[]>.Success(levels.Select(level => new AccLevel
            {
                id = level.Id,
                name = level.Name
            }).ToArray());

            return this.Json(result);
        }
    }
}
