using System;
using System.ComponentModel.DataAnnotations.Schema;

public class Vehicle
{
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime MFDate { get; set; }
    public decimal? Price{get; set;}
    public int Quantity { get; set; }  
    [NotMapped]
    public int OperationType { get; set; }
}