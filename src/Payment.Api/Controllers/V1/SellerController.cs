using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.ViewModels;
using Payment.Business.Interfaces.Notifications;
using Payment.Business.Interfaces.Services;
using Payment.Business.Models;

namespace Payment.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SellerController : MainController
    {
        private readonly IMapper _mapper;
        private readonly ISellerService _sellerService;

        public SellerController(INotifier notifier, IMapper mapper, ISellerService sellerService) : base(notifier)
        {
            _mapper = mapper;
            _sellerService = sellerService;
        }


        [HttpPost]
        public async Task<ActionResult> AddSeller(SellerRequest sellerViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _sellerService.Add(_mapper.Map<Seller>(sellerViewModel));
            return CustomResponse(sellerViewModel);
        }
    }



}
