using GenericApi.Api.Models.v1;
using Microsoft.AspNetCore.Mvc;

namespace GenericApi.Api.Controllers.v1.Base
{
    public interface ICrudController<T>
    {
        public Task<ActionResult<RestResponse<T>>> Create(IEnumerable<T> records);
        public Task<ActionResult<RestResponse<T>>> Read();
        public Task<ActionResult<RestResponse<T>>> Read(int id);
        public Task<ActionResult<RestResponse<T>>> Update(IEnumerable<T> records);
        public Task<ActionResult<RestResponse<T>>> Delete(IEnumerable<T> records);
    }
}
