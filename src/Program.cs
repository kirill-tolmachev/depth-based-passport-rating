using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

var csvPath = Path.Combine(AppContext.BaseDirectory, "passport-index-tidy.csv");
const string outputPath = "CountryRating.md";
const int maxDepth = 100;

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

// Iterative depth computation using normalized scores (doubles)
// Level 1: each country's score = count of visa-free destinations
var scores = new double[n];
for (var i = 0; i < n; i++)
    scores[i] = visaFreeDestsIdx[i].Count;

// Normalize helper
static void NormalizeInPlace(double[] arr)
{
    var max = arr.Max();
    if (max > 0)
        for (var i = 0; i < arr.Length; i++)
            arr[i] = arr[i] / max * 100.0;
}

NormalizeInPlace(scores);

// Dense ranking from scores
static List<(int Index, double Score, int Rank)> DenseRankArray(double[] s)
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

// Extract rank map from ranked list
static int[] RankMap(List<(int Index, double Score, int Rank)> ranked, int size)
{
    var map = new int[size];
    foreach (var r in ranked) map[r.Index] = r.Rank;
    return map;
}

// Store key levels for output
var levelScores = new Dictionary<int, double[]>(); // level -> normalized scores
var levelRanks = new Dictionary<int, int[]>();       // level -> rank per country

levelScores[1] = (double[])scores.Clone();
var ranked1 = DenseRankArray(scores);
levelRanks[1] = RankMap(ranked1, n);

// Track convergence
int? convergedAt = null;
var prevOrder = DenseRankArray(scores).Select(r => r.Index).ToArray();

Console.WriteLine("Computing depths...");

for (var depth = 2; depth <= maxDepth; depth++)
{
    var newScores = new double[n];
    for (var i = 0; i < n; i++)
    {
        double sum = 0;
        foreach (var dest in visaFreeDestsIdx[i])
            sum += scores[dest];
        newScores[i] = sum;
    }

    NormalizeInPlace(newScores);

    // Check convergence: has the ordering stabilized?
    var currentOrder = DenseRankArray(newScores).Select(r => r.Index).ToArray();
    if (convergedAt == null && currentOrder.SequenceEqual(prevOrder))
    {
        convergedAt = depth;
        Console.WriteLine($"  Ranking converged at depth {depth}");
    }
    prevOrder = currentOrder;

    // Store key levels
    if (depth is 2 or 3 or 10 or 50 or 100)
    {
        levelScores[depth] = (double[])newScores.Clone();
        levelRanks[depth] = RankMap(DenseRankArray(newScores), n);
    }

    scores = newScores;

    if (depth % 10 == 0)
        Console.WriteLine($"  Depth {depth} done");
}

if (convergedAt == null)
    Console.WriteLine("  Ranking did NOT fully converge within 100 iterations");

// Format delta string
static string DeltaStr(int delta) => delta switch { > 0 => $"+{delta}", 0 => "0", _ => delta.ToString() };


// Generate Markdown
var sb = new StringBuilder();
sb.AppendLine("# Passport Reach vs Passport Depth: Multi-Level Analysis");
sb.AppendLine();
sb.AppendLine($"> Generated on {DateTime.UtcNow:yyyy-MM-dd} | Data source: [Passport Index Dataset](https://github.com/ilyankou/passport-index-dataset)");
sb.AppendLine();

sb.AppendLine("## Methodology");
sb.AppendLine();
sb.AppendLine("- **Passport Reach (Level 1)**: Traditional passport strength — count of visa-free destinations (each = 1 point).");
sb.AppendLine("- **Passport Depth (Level 2+)**: Each destination weighted by its previous-level score. Measures not just *where* you can go, but how well-connected those destinations are.");
sb.AppendLine("- **Level N**: Each destination weighted by its Level N-1 score. Scores normalized to 0–100 at each level.");
sb.AppendLine("- **Access types counted**: visa free, visa on arrival, ETA, and numeric day allowances.");
sb.AppendLine($"- **Countries analyzed**: {allCountries.Count}");
sb.AppendLine($"- **Max depth**: {maxDepth}");
if (convergedAt != null)
    sb.AppendLine($"- **Ranking converged at depth {convergedAt}** — deeper levels produce the same ordering.");
else
    sb.AppendLine("- Ranking did not fully converge within 100 iterations.");
sb.AppendLine();

// Combined comparison table: Level 1, 2, 3, 10, 50, 100
var keyLevels = new[] { 1, 2, 3, 10, 50, 100 }.Where(l => levelScores.ContainsKey(l)).ToArray();

sb.AppendLine("## Combined Ranking (All Levels)");
sb.AppendLine();

// Build header
var header = new StringBuilder("| # | Country |");
foreach (var l in keyLevels)
    header.Append($" L{l} Score | L{l} Rank |");
header.Append(" Delta (L1→L100) |");
sb.AppendLine(header.ToString());

var separator = new StringBuilder("|--:|---------|");
foreach (var _ in keyLevels)
    separator.Append("--------:|--------:|");
separator.Append("---------------:|");
sb.AppendLine(separator.ToString());

// Sort by final level (100)
var finalLevel = keyLevels.Last();
var finalRanked = DenseRankArray(levelScores[finalLevel]);

foreach (var entry in finalRanked)
{
    var country = allCountries[entry.Index];
    var row = new StringBuilder($"| {entry.Rank} | {country} |");
    foreach (var l in keyLevels)
    {
        var score = levelScores[l][entry.Index];
        var rank = levelRanks[l][entry.Index];
        row.Append($" {score:F2} | {rank} |");
    }
    var l1Rank = levelRanks[1][entry.Index];
    var lFinalRank = levelRanks[finalLevel][entry.Index];
    var delta = l1Rank - lFinalRank;
    row.Append($" {DeltaStr(delta)} |");
    sb.AppendLine(row.ToString());
}
sb.AppendLine();

// Convergence journey: show how max delta between consecutive levels shrinks
sb.AppendLine("## Convergence Analysis");
sb.AppendLine();
sb.AppendLine("Maximum rank change between consecutive levels:");
sb.AppendLine();
sb.AppendLine("| From → To | Max Rank Change | Country |");
sb.AppendLine("|-----------|----------------:|---------|");

var prevLevelRanks = levelRanks[1];
foreach (var l in keyLevels.Skip(1))
{
    var curRanks = levelRanks[l];
    var maxDelta = 0;
    var maxCountry = "";
    for (var i = 0; i < n; i++)
    {
        var d = Math.Abs(prevLevelRanks[i] - curRanks[i]);
        if (d > maxDelta) { maxDelta = d; maxCountry = allCountries[i]; }
    }
    sb.AppendLine($"| L{keyLevels.First(k => levelRanks.ContainsKey(k) && levelRanks[k] == prevLevelRanks)} → L{l} | {maxDelta} | {maxCountry} |");
    prevLevelRanks = curRanks;
}
sb.AppendLine();

// Biggest movers L1 -> L100
sb.AppendLine("## Biggest Rank Changes (Reach → Depth)");
sb.AppendLine();
var movers = Enumerable.Range(0, n)
    .Select(i => (Country: allCountries[i], Delta: levelRanks[1][i] - levelRanks[finalLevel][i]))
    .OrderByDescending(x => Math.Abs(x.Delta))
    .Take(20)
    .ToList();

sb.AppendLine("| Country | L1 Rank | L100 Rank | Delta |");
sb.AppendLine("|---------|--------:|----------:|------:|");
foreach (var m in movers)
    sb.AppendLine($"| {m.Country} | {levelRanks[1][countryIndex[m.Country]]} | {levelRanks[finalLevel][countryIndex[m.Country]]} | {DeltaStr(m.Delta)} |");

var output = sb.ToString();
await File.WriteAllTextAsync(outputPath, output);

Console.WriteLine();
Console.WriteLine($"Done! Output written to {outputPath}");
Console.WriteLine();

// Console top 10 comparison
Console.WriteLine("Top 10 comparison across levels:");
var colWidth = 28;
var headerLine = "#".PadRight(4);
foreach (var l in keyLevels)
    headerLine += " " + ("L" + l).PadRight(colWidth);
Console.WriteLine(headerLine);
Console.WriteLine(new string('-', 4 + keyLevels.Length * (colWidth + 1)));

for (var row = 0; row < 10; row++)
{
    var line = (row + 1).ToString().PadRight(4);
    foreach (var l in keyLevels)
    {
        var ranked = DenseRankArray(levelScores[l]);
        if (row < ranked.Count)
        {
            var entry = ranked[row];
            var label = $"{allCountries[entry.Index]} ({entry.Score:F1})";
            line += " " + label.PadRight(colWidth);
        }
    }
    Console.WriteLine(line);
}

sealed class PassportEntry
{
    public string Passport { get; set; } = "";
    public string Destination { get; set; } = "";
    public string Requirement { get; set; } = "";
}
