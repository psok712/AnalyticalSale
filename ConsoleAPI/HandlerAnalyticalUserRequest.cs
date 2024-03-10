using System.Globalization;
using AnalysisSales.Interfaces;
using DataAccess.Exceptions;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Service;

namespace AnalysisSales;

public class HandlerAnalyticalUserRequest : IHandlerAnalyticalUserRequest
{
    private readonly String _pathFileSalesHistory = @"..\..\..\..\TestsFileJson\HistorySales.json";
    private readonly String _pathFileSeasonalityProducts = @"..\..\..\..\TestsFileJson\SeasonalityProducts.json";
    
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
        var service = new AnalysisSalesService(
            new SaleHistoryRepository(_pathFileSalesHistory),
            new SeasonalityProductsRepository(_pathFileSeasonalityProducts)
        );
        
        Console.WriteLine("Enter your request: ");
        RequestUser requestUser = CreateUser(Console.ReadLine()!);
        Console.WriteLine(ResultAnalyticalUserRequest(requestUser, service));
    }

    private String ResultAnalyticalUserRequest(RequestUser requestUser, IAnalysisSalesService service)
    {
        switch (requestUser.Command)
        {
            case CommandRequestEnum.Ads:
                return "ADS: " + service.GetAverageDaySales(
                    requestUser.IdProduct).ToString(CultureInfo.InvariantCulture);
            case CommandRequestEnum.Demand:
                return "Demand: " + service.GetSalesDemand(
                    requestUser.IdProduct, requestUser.Days).ToString(CultureInfo.InvariantCulture);
            case CommandRequestEnum.Prediction:
                return "Prediction: " + service.GetSalesPrediction(
                    requestUser.IdProduct, requestUser.Days).ToString(CultureInfo.InvariantCulture);
            default:
                return "Sorry, there is no such command.";
        }
    }
    
    private RequestUser CreateUser(string strCommand) 
    {
        var arrCommand = strCommand.Split();

        if (arrCommand.Length > 1 && long.TryParse(arrCommand[1], out long idProduct))
        {
            switch (arrCommand.Length)
            {
                case 2: return new RequestUser(DefineAnalyticalUserRequest(arrCommand[0]), idProduct);
                case 3:
                {
                    if (long.TryParse(arrCommand[2], out long days) 
                        && (DefineAnalyticalUserRequest(arrCommand[0]) == CommandRequestEnum.Demand 
                        || DefineAnalyticalUserRequest(arrCommand[0]) == CommandRequestEnum.Prediction))
                    {
                        return new RequestUser(DefineAnalyticalUserRequest(arrCommand[0]), idProduct, days);
                    }

                    break;
                }
            }
        }
        
        return new RequestUser(CommandRequestEnum.None, -1);
    }
    
    private CommandRequestEnum DefineAnalyticalUserRequest(string strCommand)
    {
        switch (strCommand.ToLower())
        {
            case "ads": return CommandRequestEnum.Ads;
            case "prediction": return CommandRequestEnum.Prediction;
            case "demand": return CommandRequestEnum.Demand;
            default: return CommandRequestEnum.None;
        }
    }
}