using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

var csvPath = Path.Combine(AppContext.BaseDirectory, "passport-index-tidy.csv");
const string outputPath = "CountryRating.md";

// Parse CSV with CsvHelper
var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true };
using var reader = new StreamReader(csvPath);
using var csvReader = new CsvReader(reader, csvConfig);
var entries = csvReader.GetRecords<PassportEntry>()
    .Where(e => e.Requirement != "-1")
    .Select(e => (Passport: e.Passport.Trim(), Destination: e.Destination.Trim(), Requirement: e.Requirement.Trim()))
    .ToList();

static bool IsVisaFree(string requirement) =>
    requirement is "visa free" or "visa on arrival" or "eta"
    || int.TryParse(requirement, CultureInfo.InvariantCulture, out _);

// Precompute
var allCountries = entries.Select(e => e.Passport).Distinct().ToList();
var countryIndex = allCountries.Select((c, i) => (c, i)).ToDictionary(x => x.c, x => x.i);
var n = allCountries.Count;

// Build adjacency: visaFreeDestsIdx[i] = list of country indices that country i can visit visa-free
var visaFreeDestsIdx = new List<int>[n];
for (var i = 0; i < n; i++) visaFreeDestsIdx[i] = [];
foreach (var e in entries)
{
    if (IsVisaFree(e.Requirement) && countryIndex.TryGetValue(e.Destination, out var destIdx))
        visaFreeDestsIdx[countryIndex[e.Passport]].Add(destIdx);
}

// L1 (Reach): count of visa-free destinations, normalized to 0-100
var reachScores = new double[n];
for (var i = 0; i < n; i++)
    reachScores[i] = visaFreeDestsIdx[i].Count;

Normalize(reachScores);

// L2 (Depth): each destination weighted by its Reach score, normalized to 0-100
var depthScores = new double[n];
for (var i = 0; i < n; i++)
{
    double sum = 0;
    foreach (var dest in visaFreeDestsIdx[i])
        sum += reachScores[dest];
    depthScores[i] = sum;
}

Normalize(depthScores);

// Ranking
var reachRanked = DenseRank(reachScores);
var depthRanked = DenseRank(depthScores);
var reachRankMap = RankMap(reachRanked, n);
var depthRankMap = RankMap(depthRanked, n);

// Generate Markdown
var sb = new StringBuilder();
sb.AppendLine("# Passport Reach vs Depth");
sb.AppendLine();
sb.AppendLine($"> Generated on {DateTime.UtcNow:yyyy-MM-dd} | Data: [Passport Index Dataset](https://github.com/ilyankou/passport-index-dataset) | {n} countries");
sb.AppendLine();
sb.AppendLine("| # | Country | Reach Score | Reach Rank | Depth Score | Depth Rank | Delta |");
sb.AppendLine("|--:|---------|----------:|----------:|----------:|----------:|------:|");

foreach (var entry in depthRanked)
{
    var country = allCountries[entry.Index];
    var reachScore = reachScores[entry.Index];
    var reachRank = reachRankMap[entry.Index];
    var delta = reachRank - entry.Rank;
    sb.AppendLine($"| {entry.Rank} | {country} | {reachScore:F2} | {reachRank} | {entry.Score:F2} | {entry.Rank} | {DeltaStr(delta)} |");
}

sb.AppendLine();
sb.AppendLine("## Biggest Rank Changes (Reach → Depth)");
sb.AppendLine();
sb.AppendLine("| Country | Reach Rank | Depth Rank | Delta |");
sb.AppendLine("|---------|----------:|----------:|------:|");

var movers = Enumerable.Range(0, n)
    .Select(i => (Country: allCountries[i], Delta: reachRankMap[i] - depthRankMap[i]))
    .OrderByDescending(x => Math.Abs(x.Delta))
    .Take(20);

foreach (var m in movers)
    sb.AppendLine($"| {m.Country} | {reachRankMap[countryIndex[m.Country]]} | {depthRankMap[countryIndex[m.Country]]} | {DeltaStr(m.Delta)} |");

await File.WriteAllTextAsync(outputPath, sb.ToString());

Console.WriteLine($"Done! {n} countries ranked. Output: {outputPath}");
Console.WriteLine();
Console.WriteLine("Top 10:");
Console.WriteLine($"{"#",-4} {"Country",-28} {"Reach",-8} {"Depth",-8} {"Delta",6}");
Console.WriteLine(new string('-', 56));

foreach (var entry in depthRanked.Take(10))
{
    var country = allCountries[entry.Index];
    var reachRank = reachRankMap[entry.Index];
    var delta = reachRank - entry.Rank;
    Console.WriteLine($"{entry.Rank,-4} {country,-28} {reachRank,-8} {entry.Rank,-8} {DeltaStr(delta),6}");
}

return;

static void Normalize(double[] arr)
{
    var max = arr.Max();
    if (max > 0)
        for (var i = 0; i < arr.Length; i++)
            arr[i] = arr[i] / max * 100.0;
}

static List<(int Index, double Score, int Rank)> DenseRank(double[] s)
{
    var sorted = s.Select((v, i) => (Index: i, Score: v))
        .OrderByDescending(x => x.Score)
        .ToList();

    var ranked = new List<(int Index, double Score, int Rank)>(s.Length);
    for (var i = 0; i < sorted.Count; i++)
    {
        var rank = i == 0 ? 1
            : Math.Abs(sorted[i].Score - sorted[i - 1].Score) < 1e-9
                ? ranked[i - 1].Rank
                : ranked[i - 1].Rank + 1;
        ranked.Add((sorted[i].Index, sorted[i].Score, rank));
    }
    return ranked;
}

static int[] RankMap(List<(int Index, double Score, int Rank)> ranked, int size)
{
    var map = new int[size];
    foreach (var r in ranked) map[r.Index] = r.Rank;
    return map;
}

static string DeltaStr(int delta) => delta switch { > 0 => $"+{delta}", 0 => "0", _ => delta.ToString() };

sealed class PassportEntry
{
    public string Passport { get; set; } = "";
    public string Destination { get; set; } = "";
    public string Requirement { get; set; } = "";
}
