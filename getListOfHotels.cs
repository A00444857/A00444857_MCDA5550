#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Collections.Generic;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string name = req.Query["name"];

    List<Hotel> Hotels = dbConnect();

    return new OkObjectResult(Hotels);
}

    public static List<Hotel> dbConnect()
    {
        var connStr = "Server=tcp:hotelreserva.database.windows.net,1433;Initial Catalog=hoteldb;Persist Security Info=False;User ID=mbpatel;Password=mitca12@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
             List<Hotel> characters = new List<Hotel>();
    
             using (SqlConnection conn = new SqlConnection(connStr))
             {
                 conn.Open();
                 var text = "SELECT * FROM [HotelList]";
    
                 using (SqlCommand cmd = new SqlCommand(text, conn))
                 {
                     using (SqlDataReader reader = cmd.ExecuteReader())
                     {
                         while (reader.Read())
                         {
                             characters.Add(new Hotel(){ Hotel_Name = (string) reader.GetValue(0), Price = (int) reader.GetValue(1),Availability = (bool) reader.GetValue(2) });
                         }
                     }
                 }
             }
             return characters;
    }

    public class Hotel
    {
        public string Hotel_Name { get; set; }
        public int Price { get; set; }
        public bool Availability { get; set; }
    }
