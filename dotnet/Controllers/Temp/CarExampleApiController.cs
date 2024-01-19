using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Example;
using Sabio.Models.Requests.Example;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;

namespace Sabio.Web.Api.Controllers.Temp
{
    [AllowAnonymous]
    [Route("api/cars")]
    public class CarExampleApiController : BaseApiController
    {
        private IAuthenticationService<int> _authService;

        private static List<Car> _cars = new List<Car>()
        {
            new Car(){ Id = GetRandomId(), Make = "Toyota",Model = "Camry ", Year = 2020},
            new Car(){ Id = GetRandomId(), Make = "Toyota",Model = "Camry ", Year = 2020},
            new Car(){ Id = GetRandomId(), Make = "Toyota",Model = "Camry ", Year = 2020},
            new Car(){ Id = GetRandomId(), Make = "Toyota",Model = "Camry ", Year = 2020},
            new Car(){ Id = GetRandomId(), Make = "Toyota",Model = "Camry ", Year = 2020}
        };

        public CarExampleApiController(
             IAuthenticationService<int> authService
            , ILogger<CarExampleApiController> logger) : base(logger)
        {

            _authService = authService;

        }

        [HttpGet]
        public ActionResult<ItemsResponse<Car>> GetAllCars()
        {
            int code = 200;
            BaseResponse response = null;//do not declare an instance.

            try
            {
                List<Car> list = _cars;

                if (list == null || list.Count == 0)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Car> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }


            return StatusCode(code, response);
        }

        [HttpGet("{id}")]
        public ActionResult<ItemResponse<Car>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Car widget = _cars.Where(c => c.Id == id).FirstOrDefault();

                if (widget == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Car> { Item = widget };
                }
            }
            catch (Exception ex)
            {

                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);

        }
        [HttpPost]
        public ActionResult<ItemResponse<int>> Post(CarAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                Car newCar = new Car() { Id = GetRandomId(), Make = model.Make, Model = model.Model, Year = model.Year };
                
                _cars.Add(newCar);
                
                ItemResponse<int> response = new ItemResponse<int>();
                response.Item = newCar.Id;

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id}")]
        public ActionResult<SuccessResponse> Put(CarUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;//

            try
            {
                int index = _cars.FindIndex(c => c.Id == model.Id);
                if (index > -1)
                {
                    _cars[index].Make = model.Make;
                    _cars[index].Model = model.Model;
                    _cars[index].Year = model.Year;
                }

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpDelete("{id}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
           
            int code = 200;
            BaseResponse response = null;//do not declare an instance.

            try
            {
                int index = _cars.FindIndex(c => c.Id == id);
                if (index > -1)
                {
                    _cars.RemoveAt(index);
                }

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        private static int GetRandomId()
        {
            return new Random().Next(1, 10000);
        }
    }
}
