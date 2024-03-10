using DataAccess.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAPI.Controller;

[ApiController]
[Route("/api/analysis")]
public class AnalyticalUserRequestController(IAnalysisSalesService service) : ControllerBase
{
    [HttpGet]
    [Route("/ads")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Results<Ok<double>, NotFound, BadRequest> GetAverageDaySales([FromQuery] long idProduct)
    {
        try
        {
            return TypedResults.Ok(service.GetAverageDaySales(idProduct));
        }
        catch (DayOutOfRangeException)
        {
            return TypedResults.BadRequest();
        }
        catch (ProductNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }
    
    [HttpGet]
    [Route("/prediction")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Results<Ok<double>, NotFound, BadRequest> GetSalesPrediction([FromQuery] long idProduct, [FromQuery] long days)
    {
        try
        {
            return TypedResults.Ok(service.GetSalesPrediction(idProduct, days));
        }
        catch (DayOutOfRangeException)
        {
            return TypedResults.BadRequest();
        }
        catch (ProductNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }
    
    [HttpGet]
    [Route("/demand")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Results<Ok<double>, NotFound, BadRequest> GetSalesDemand([FromQuery] long idProduct, [FromQuery] long days)
    {
        try
        {
            return TypedResults.Ok(service.GetSalesDemand(idProduct, days));
        }
        catch (DayOutOfRangeException)
        {
            return TypedResults.BadRequest();
        }
        catch (ProductNotFoundException)
        {
            return TypedResults.NotFound();
        }
    }
    
}