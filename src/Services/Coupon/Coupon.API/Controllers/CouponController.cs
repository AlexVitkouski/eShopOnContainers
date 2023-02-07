using Coupon.API.Dtos;
using Coupon.API.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coupon.API.Controllers;

[Route("api/v1/[controller]")]
[Authorize]
[ApiController]
public class CouponController : ControllerBase
{

    private readonly ICouponRepository _couponRepository;
    private readonly IMapper<CouponDto, Infrastructure.Models.Coupon> _mapper;

    public CouponController(ICouponRepository couponRepository, IMapper<CouponDto, Infrastructure.Models.Coupon> mapper)
    {
        _couponRepository = couponRepository;
        _mapper = mapper;
    }

    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CouponDto>> GetCouponByCodeAsync(string code)
    {
        var coupon = await _couponRepository.FindCouponByCodeAsync(code);

        if (coupon is null || coupon.Consumed)
        {
            return NotFound();
        }

        var couponDto = _mapper.Translate(coupon);

        return couponDto;
    }

    [HttpGet("buyerId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<double>> GetLoyaltyDicsountAsync(int buyerId)
    {
        var loyaltyTier = await _couponRepository.FindLoyaltyTierForBuyerAsync(buyerId);

        if (loyaltyTier == null)
        {
            return NotFound();
        }

        return loyaltyTier.Discount;
    }
}