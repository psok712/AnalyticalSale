using DataAccess.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAPI.Controller;

[ApiController]
[Route("/api/analysis")]
public class AnalyticalController(IAnalyticalSaleService service) : ControllerBase
{
    [HttpGet]
    [Route("/ads")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public Results<Ok<double>, NotFound, BadRequest> GetAverageDaySales([FromQuery] long productId)
    {
        try
        {
            return TypedResults.Ok(service.GetAverageDaySales(productId));
        }
        catch (DayOutOfRangeException)
        {
            return TypedResults.BadRequest();
        }
        catch (ProductNotFoundException)
        {
            return TypedResults.NotFound();
        }
        catch (Exception)
        {
            return TypedResults.BadRequest();
        }
    }
    
    [HttpGet]
    [Route("/prediction")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Results<Ok<double>, NotFound, BadRequest> GetSalesPrediction(
        [FromQuery] long productId, 
        [FromQuery] long days)
    {
        try
        {
            return TypedResults.Ok(service.GetSalesPrediction(productId, days));
        }
        catch (DayOutOfRangeException)
        {
            return TypedResults.BadRequest();
        }
        catch (ProductNotFoundException)
        {
            return TypedResults.NotFound();
        }
        catch (Exception)
        {
            return TypedResults.BadRequest();
        }
    }
    
    [HttpGet]
    [Route("/demand")]
    [ProducesResponseType<double>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Results<Ok<double>, NotFound, BadRequest> GetSalesDemand(
        [FromQuery] long productId, 
        [FromQuery] long days)
    {
        try
        {
            return TypedResults.Ok(service.GetSalesDemand(productId, days));
        }
        catch (DayOutOfRangeException)
        {
            return TypedResults.BadRequest();
        }
        catch (ProductNotFoundException)
        {
            return TypedResults.NotFound();
        }
        catch (Exception)
        {
            return TypedResults.BadRequest();
        }
    }
    
}