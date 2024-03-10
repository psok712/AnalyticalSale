using AspNetAPI.Service;
using AspNetAPI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAPI.Controller;

[ApiController]
[Route("/api/request")]
public class RequestUserController : ControllerBase
{
    private readonly IRequestUserService _service = new RequestUserService();
    
    [HttpGet]
    [Route("/ads/{idProduct:long}")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Results<NotFound, Ok<double>> GetAverageDaySales([FromRoute]long idProduct)
    {
        return _service.GetAverageDaySales(idProduct);
    }
    
    [HttpGet]
    [Route("/prediction/{idProduct:long}&{days:long}")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Results<NotFound, Ok<double>> GetSalesPrediction([FromRoute]long idProduct, [FromRoute]long days)
    {
        return _service.GetSalesPrediction(idProduct, days);
    }
    
    [HttpGet]
    [Route("/demand/{idProduct:long}&{days:long}")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Results<NotFound, Ok<double>> GetSalesDemand([FromRoute]long idProduct, [FromRoute]long days)
    {
        return _service.GetSalesDemand(idProduct, days);
    }
    
}