using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ServiceHost.Areas.Administration.Pages;

public class IndexModel : PageModel
{
    public Chart DoughnutDataSet { get; set; }
    public List<Chart> BarLineDataSet { get; set; }

    public void OnGet()
    {
        BarLineDataSet =
        [
            new Chart("Apple", new List<int> { 100, 200, 250, 170, 50 }, ["#ffcdb2"],
                "#b5838d"),

            new Chart("Samsung", new List<int> { 200, 300, 350, 270, 100 },
                ["#ffc8dd"], "#ffafcc"),

            new Chart("Total", new List<int> { 300, 500, 600, 440, 150 },
                ["#0077b6"], "#023e8a")
        ];
        DoughnutDataSet = new Chart("Apple", data: new List<int> { 100, 200, 250, 170, 50 },
            borderColor: "#ffcdb2", backgroundColor: new[] { "#b5838d", "#ffd166", "#7f4f24", "#ef233c", "#003049" });
    }
}

public class Chart
{
    public Chart()
    {
    }

    public Chart(string label, List<int> data, string[] backgroundColor, string borderColor)
    {
        Label = label;
        Data = data;
        BackgroundColor = backgroundColor;
        BorderColor = borderColor;
    }

    [JsonProperty(PropertyName = "label")] public string Label { get; set; }

    [JsonProperty(PropertyName = "data")] public List<int> Data { get; set; }

    [JsonProperty(PropertyName = "backgroundColor")]
    public string[] BackgroundColor { get; set; }

    [JsonProperty(PropertyName = "borderColor")]
    public string BorderColor { get; set; }
}