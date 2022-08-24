using GenericApi.Api.Models.v1;
using GenericApi.Data.Models.v1;
using GenericApi.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GenericApi.Api.Controllers.v1.Base
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public abstract class CrudController<T> : ControllerBase, ICrudController<T>
        where T : class, IRecord, new()
    {
        private readonly IRepository<T> _repo;

        public CrudController(IRepository<T> repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public virtual async Task<ActionResult<RestResponse<T>>> Create(IEnumerable<T> records)
        {
            try
            {
                var successfulRecords = await _repo.Create(records);
                var failedRecords = records.Except(successfulRecords ?? Enumerable.Empty<T>());
                return StatusCode(!failedRecords.Any() ? StatusCodes.Status201Created : StatusCodes.Status500InternalServerError, new RestResponse<T>()
                {
                    Message = failedRecords.Any() ? "Some records not created" : "Records created",
                    Success = !failedRecords.Any(),
                    SuccessfulRecords = successfulRecords ?? Enumerable.Empty<T>(),
                    FailedRecords = failedRecords
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet]
        public virtual async Task<ActionResult<RestResponse<T>>> Read()
        {
            try
            {
                var data = await _repo.Read();
                return StatusCode(data?.Any() ?? false ? StatusCodes.Status200OK : StatusCodes.Status404NotFound, new RestResponse<T>()
                {
                    Message = data?.Any() ?? false ? "Records found" : "No records found",
                    Success = data?.Any() ?? false,
                    SuccessfulRecords = data ?? Enumerable.Empty<T>(),
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpGet("{id:int}")]
        public virtual async Task<ActionResult<RestResponse<T>>> Read(int id)
        {
            try
            {
                var data = await _repo.Read(id);
                var result = new List<T>();
                if (data is not null) result.Add(data);
                return StatusCode((data is not null) ? StatusCodes.Status200OK: StatusCodes.Status404NotFound, new RestResponse<T>()
                {
                    Message = (data is not null) ? "Record found" : "Record not found",
                    Success = data is not null,
                    SuccessfulRecords = result,
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpPut]
        public virtual async Task<ActionResult<RestResponse<T>>> Update(IEnumerable<T> records)
        {
            try
            {
                var successfulRecords = await _repo.Update(records);
                var failedRecords = records.Except(successfulRecords ?? Enumerable.Empty<T>());
                return StatusCode(!failedRecords.Any() ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError, new RestResponse<T>()
                {
                    Message = failedRecords.Any() ? "Some records not updated" : "Records updated",
                    Success = !failedRecords.Any(),
                    SuccessfulRecords = successfulRecords ?? Enumerable.Empty<T>()
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        [HttpDelete]
        public virtual async Task<ActionResult<RestResponse<T>>> Delete(IEnumerable<T> records)
        {
            try
            {
                var successfulRecords = await _repo.Delete(records);
                var failedRecords = records.Except(successfulRecords ?? Enumerable.Empty<T>());
                return StatusCode(!failedRecords.Any() ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError, new RestResponse<T>()
                {
                    Message = failedRecords.Any() ? "Some records not deleted" : "Records deleted",
                    Success = !failedRecords.Any(),
                    SuccessfulRecords = successfulRecords ?? Enumerable.Empty<T>(),
                    FailedRecords = failedRecords ?? Enumerable.Empty<T>()
                });
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        protected virtual ActionResult Error(Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new RestResponse<T>()
            {
                Success = false,
                Message = "Exception caught",
                SuccessfulRecords = Enumerable.Empty<T>(),
                Exceptions = new List<string>() { ex.ToString() }
            });
        }
    }
}
