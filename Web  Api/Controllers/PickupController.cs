//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Domain;
//using Application;

//namespace Web__Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PickupController : ControllerBase
//    {
//        private readonly PickupService _pickupService;

//        public PickupController(PickupService pickupService)
//        {
//            _pickupService = pickupService;
//        }

//        [HttpPost]
//        [Route("Add")]
//        public async Task AddPickup(WebPickUp pickup)
//        {
//            await _pickupService.Add(pickup);
//        }

//        [HttpGet]
//        [Route("get")]
//        public async Task<List<WebPickUp>> GetAll()
//        {
//            return await _pickupService.GetAll();
//        }

//        //[HttpGet("{id}")]
//        //Or
//        [HttpGet]
//        [Route("find/{id}")]
//        public async Task<WebPickUp> GetUserById(int id)
//        {
//            return await _pickupService.Find(id);
//        }

      
//        [HttpDelete]
//        [Route("delete/{id}")]
//        public async Task Delete(int id)
//        {
//            await _pickupService.Delete(id);
//        }
//    }
//}
