using GenericApi.Api.Controllers.v1.Base;
using GenericApi.Api.Models.v1;
using GenericApi.Data.Models.v1;
using GenericApi.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GenericApi.Api.Controllers.v1
{
    public class PersonController : CrudController<Person>
    {
        public PersonController(IRepository<Person> repo) : base(repo) { }
    }
}
