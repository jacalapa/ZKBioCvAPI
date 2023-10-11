using Microsoft.AspNetCore.Mvc;
using ZKBioClient;
using ZkbioCvApi.Model;

namespace ZkbioCvApi.Controllers
{
    [ApiController]
    [Route("person")]
    public class PersonController : Controller
    {
        private readonly ApiClientProvider apiClientProvider;

        public PersonController(ApiClientProvider apiClientProvider)
        {
            this.apiClientProvider = apiClientProvider;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(Person person)
        {
            ApiClient client = this.apiClientProvider.Get();
            DtoEditPerson dto = new DtoEditPerson
            {
                Card = person.cardNo,
                DepartmentId = person.deptCode,
                FirstName = person.name,
                LastName = person.lastName,
                PersonId = person.pin,
                LevelIds = person.accLevelIds.Split(',')
            };

            var ok = await client.AddPersonAsync(dto);
            if (ok)
            {
                return this.StatusCode(StatusCodes.Status201Created);
            }

            throw new Exception();
        }

        [HttpDelete]
        [Route("delete/{pin}")]
        public async Task<IActionResult> Delete(string pin)
        {
            ApiClient client = this.apiClientProvider.Get();
            var persons = await client.GetPersonsAsync();
            var id = persons.Single(x => x.PersonId == pin).Id;
            bool ok = await client.DeletePersonAsync(id);
            if (ok)
            {
                return this.StatusCode(StatusCodes.Status204NoContent);
            }

            throw new Exception();
        }

        [HttpGet]
        [Route("get/{pin}")]
        public async Task<IActionResult> Get(string pin)
        {
            ApiClient client = this.apiClientProvider.Get();
            var persons = await client.GetPersonsAsync();
            var person = persons.Single(x => x.PersonId == pin);
            var result = Result<Person>.Success(new Person
            {
                name = person.FirstName,
                pin = person.PersonId,
                gender = person.Gender,
                mobilePhone = person.Phone,
                email = person.Email,
                lastName = person.LastName,
                cardNo = person.Card,
            });

            return this.Json(result);
        }
    }
}
