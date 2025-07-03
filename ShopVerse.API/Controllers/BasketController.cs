using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopVerse.API.Errors;
using ShopVerse.Core.Dtos.Basket;
using ShopVerse.Core.Entities;
using ShopVerse.Core.Respository.Interfaces;

namespace ShopVerse.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket (string? id)
        {
            if(id is null) return BadRequest(new ApiErrorResponse(400, "Basket ID cannot be null."));
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket is null) new CustomerBasket() { Id = id};
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket (CustomerBasketDto model)
        {
          var basket = await  _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(model));
            if(basket is null) return BadRequest(new ApiErrorResponse(400, "Basket could not be created or updated."));
            return Ok(basket);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string? id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
