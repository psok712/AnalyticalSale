namespace Domain.Models;

public record Sale(    
    long Id,
    DateTime Date,
    long Sales,
    long Stock
);