using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPartyApi.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Day
{
    public string datetime { get; set; }
    public double tempmax { get; set; }
    public double tempmin { get; set; }
    public double temp { get; set; }
    public double humidity { get; set; }
    public string sunrise { get; set; }
    public string sunset { get; set; }
}

public class Weather
{

    public List<Day> days { get; set; }
}



