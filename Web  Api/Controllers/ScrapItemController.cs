//using Application;
//using Domain;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Web__Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ScrapItemController : ControllerBase
//    {
//        private readonly ScrapItemService _scrapItemService;

//        public ScrapItemController(ScrapItemService scrapItemService)
//        {

//            _scrapItemService = scrapItemService;
//        }



//        [HttpGet]
//        [Route("get")]
//        public async Task<List<WebScrapItem>> GetAll()
//        {
//            return await _scrapItemService.GetAll();
//        }

//        //[HttpGet("{id}")]
//        //Or
//        [HttpGet]
//        [Route("find/{id}")]
//        public async Task<WebScrapItem> GetUserById(int id)
//        {
//            return await _scrapItemService.Find(id);
//        }

//        [HttpPost]
//        [Route("add")]
//        public async Task Add(WebScrapItem scrapItem)
//        {
//            await _scrapItemService.Add(scrapItem);
//        }

//        [HttpDelete]
//        [Route("delete/{id}")]
//        public async Task Delete(int id)
//        {
//            await _scrapItemService.Delete(id);
//        }
//    }
//}
