using System.Globalization;
using AnalysisSales.Models;
using DataAccess.Exceptions;
using Domain.Interfaces;
using Service;

namespace AnalysisSales.Controller;

public class AnalyticalController
{
    public void RunApplication()
    {
        do
        {
            try
            {
                InputAnalyticalUserRequest();
            }
            catch (ProductNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DayOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                Console.WriteLine("An unknown error has occurred.");
            }

            Console.WriteLine("Do you want to continue writing queries? " +
                             "If yes, then press any other button, otherwise spacebar.");
            
        } while (Console.ReadKey(true).Key != ConsoleKey.Spacebar);
    }
    
    private void InputAnalyticalUserRequest()
    {
        Console.WriteLine("Enter your request: ");
        var saleAnalyticRequest = CreateUser(Console.ReadLine()!);
        Console.WriteLine(ResultAnalyticalUserRequest(saleAnalyticRequest, new AnalyticalSaleService()));
    }

    private string ResultAnalyticalUserRequest(SaleAnalyticRequest saleAnalyticRequest, IAnalyticalSaleService service)
    {
        switch (saleAnalyticRequest.SaleAnalytic)
        {
            case SaleAnalyticCommand.Ads:
                return "ADS: " + service.GetAverageDaySales(
                    saleAnalyticRequest.ProductId).ToString(CultureInfo.InvariantCulture);
            case SaleAnalyticCommand.Demand:
                return "Demand: " + service.GetSalesDemand(
                    saleAnalyticRequest.ProductId, saleAnalyticRequest.Days).ToString(CultureInfo.InvariantCulture);
            case SaleAnalyticCommand.Prediction:
                return "Prediction: " + service.GetSalesPrediction(
                    saleAnalyticRequest.ProductId, saleAnalyticRequest.Days).ToString(CultureInfo.InvariantCulture);
            default:
                return "Sorry, there is no such command.";
        }
    }
    
    private SaleAnalyticRequest CreateUser(string strCommand) 
    {
        var arrCommand = strCommand.Split();

        if (arrCommand.Length > 1 && long.TryParse(arrCommand[1], out long productId))
        {
            switch (arrCommand.Length)
            {
                case 2: return new SaleAnalyticRequest(DefineAnalyticalUserRequest(arrCommand[0]), productId);
                case 3:
                {
                    if (long.TryParse(arrCommand[2], out long days) 
                        && (DefineAnalyticalUserRequest(arrCommand[0]) == SaleAnalyticCommand.Demand 
                        || DefineAnalyticalUserRequest(arrCommand[0]) == SaleAnalyticCommand.Prediction))
                    {
                        return new SaleAnalyticRequest(DefineAnalyticalUserRequest(arrCommand[0]), productId, days);
                    }

                    break;
                }
            }
        }
        
        return new SaleAnalyticRequest(SaleAnalyticCommand.None, -1);
    }
    
    private SaleAnalyticCommand DefineAnalyticalUserRequest(string strCommand)
    {
        switch (strCommand.ToLower())
        {
            case "ads": return SaleAnalyticCommand.Ads;
            case "prediction": return SaleAnalyticCommand.Prediction;
            case "demand": return SaleAnalyticCommand.Demand;
            default: return SaleAnalyticCommand.None;
        }
    }
}