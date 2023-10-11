using Microsoft.AspNetCore.Mvc;
using ZKBioClient;
using ZkbioCvApi.Model;

namespace ZkbioCvApi.Controllers
{
    [ApiController]
    [Route("department")]
    public class DepartmentController : Controller
    {
        private readonly ApiClientProvider apiClientProvider;

        public DepartmentController(ApiClientProvider apiClientProvider)
        {
            this.apiClientProvider = apiClientProvider;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(Department department)
        {
            ApiClient client = this.apiClientProvider.Get();
            DtoEditDepartment dto = new DtoEditDepartment
            {
                Name = department.name,
                Code = department.code,
                Parent = department.parentCode
            };

            var ok = await client.AddOrUpdateDepartment(dto);
            if (ok)
            {
                return this.StatusCode(StatusCodes.Status201Created);
            }

            throw new Exception();
        }

        [HttpDelete]
        [Route("delete/{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            ApiClient client = this.apiClientProvider.Get();
            var departments = await client.GetDepartments();
            var id = departments.Single(x => x.Code == code).Id;
            bool ok = await client.DeleteDepartment(id);
            if (ok)
            {
                return this.StatusCode(StatusCodes.Status204NoContent);
            }

            throw new Exception();
        }

        [HttpGet]
        [Route("get/{code}")]
        public async Task<IActionResult> Get(string code)
        {
            ApiClient client = this.apiClientProvider.Get();
            var departments = await client.GetDepartments();
            var department = departments.Single(x => x.Code == code);
            var result = Result<Department>.Success(new Department
            {
                name = department.Name,
                code = department.Code,
                parentCode = department.Parent
            });

            return this.Json(result);
        }
    }
}
