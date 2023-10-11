using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZKBioClient;
using ZkbioCvApi.Model;

namespace ZkbioCvApi.Controllers
{
    [Route("door")]
    [ApiController]
    public class DoorController : Controller
    {
        private readonly ApiClientProvider apiClientProvider;

        public DoorController(ApiClientProvider apiClientProvider)
        {
            this.apiClientProvider = apiClientProvider;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get(string id)
        {
            ApiClient client = this.apiClientProvider.Get();
            var doors = await client.GetDoorsAsync();
            var door = doors.Single(x => x.Id == id);
            var result = Result<Door>.Success(new Door
            {
                id = door.Id,
                name = door.DoorName,
                deviceId = door.OwnedDevice
            });

            return this.Json(result);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            ApiClient client = this.apiClientProvider.Get();
            var doors = await client.GetDoorsAsync();
            
            var result = Result<Door[]>.Success(doors.Select(door => new Door
            {
                id = door.Id,
                name = door.DoorName,
                deviceId = door.OwnedDevice
            }).ToArray());

            return this.Json(result);
        }

        [HttpPost]
        [Route("remoteCloseByName")]
        public async Task<IActionResult> RemoteCloseByName(string name)
        {
            ApiClient client = this.apiClientProvider.Get();
            var doors = await client.GetDoorsAsync();
            var door = doors.Single(x => x.DoorName == name);
            var ok = await client.RemoteClosingAsync(door.Id);
            if (ok)
            {
                return this.StatusCode(StatusCodes.Status201Created);
            }

            throw new Exception();
        }

        [HttpPost]
        [Route("remoteCloseById")]
        public async Task<IActionResult> RemoteCloseById(string id)
        {
            ApiClient client = this.apiClientProvider.Get();
            var ok = await client.RemoteClosingAsync(id);
            if (ok)
            {
                return this.StatusCode(StatusCodes.Status201Created);
            }

            throw new Exception();
        }

        [HttpPost]
        [Route("remoteOpenByName")]
        public async Task<IActionResult> RemoteOpenByName(string name, int interval)
        {
            ApiClient client = this.apiClientProvider.Get();
            var doors = await client.GetDoorsAsync();
            var door = doors.Single(x => x.DoorName == name);
            var ok = await client.RemoteOpeningAsync(door.Id, interval);
            if (ok)
            {
                return this.StatusCode(StatusCodes.Status201Created);
            }

            throw new Exception();
        }

        [HttpPost]
        [Route("remoteOpenById")]
        public async Task<IActionResult> RemoteOpenById(string id, int interval)
        {
            ApiClient client = this.apiClientProvider.Get();
            var ok = await client.RemoteOpeningAsync(id, interval);
            if (ok)
            {
                return this.StatusCode(StatusCodes.Status201Created);
            }

            throw new Exception();
        }
    }
}
