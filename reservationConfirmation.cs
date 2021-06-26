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


    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    var connStr = "Server=tcp:hotelreserva.database.windows.net,1433;Initial Catalog=hoteldb;Persist Security Info=False;User ID=mbpatel;Password=mitca12@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    var text="" ;
    //int n=1;
    long n = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
    using (SqlConnection conn = new SqlConnection(connStr))
    {
        conn.Open();
        text = "insert into Reservation(cno, hotel_name, checkin, checkout)values("+n+",'"+ data.hotel_name+"',"+data.checkin+","+data.checkin+");";
        log.LogInformation($"{text}");   
        using (SqlCommand cmd = new SqlCommand(text, conn))
        {
            // Execute the command and log the # rows affected.
          var rows = await cmd.ExecuteNonQueryAsync();
          log.LogInformation($"{rows} rows were inserted");
        }
        for(int i=0; i<data.guests_list.Count;i=i+1){
        text = "insert into guest(cno, guest_name , gender)values("+n+",'"+data.guests_list[i].guest_name+"','"+data.guests_list[i].gender+"');";
        log.LogInformation($"{text}");   
        SqlCommand cmd = new SqlCommand(text, conn);
        var rows = await cmd.ExecuteNonQueryAsync();
        log.LogInformation($"{rows} rows were inserted");
        }
        
    }


    string responseMessage = "ok bye";
            return new OkObjectResult(responseMessage);
}
public class PostData
{
    public string name { get;set; }    
}



{
  "hotel_name": "Marriot",
   "checkin": "01-08-2021",
   "checkout": "12-08-2021",
  "guests_list": [
           { "guest_name" : "Mit",
             "gender": "Male"
           },
           { "guest_name" : "Neel",
             "gender": "Male"
           }
       ]
}



