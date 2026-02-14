# Depth Based Passport Rating

A .NET 10 console application that rates country passports using **Passport Depth** — a quality-weighted alternative to traditional passport rankings.

## The Problem with Traditional Rankings

Traditional passport indexes measure **Passport Reach** — simply counting how many countries a passport can access visa-free. This treats all destinations equally: the USA counts the same as a small island nation.

## Passport Depth: A Weighted Alternative

**Passport Depth** fixes this by weighting each destination by its own passport strength (Reach score). If Country A can visit Country B visa-free, Country B contributes its Reach score (not just 1 point) to Country A's Depth total.

**In short:** accessing a country with a strong passport is worth more than accessing one with a weak passport.

| Concept | What it measures |
|---------|-----------------|
| **Passport Reach** | How many countries you can visit visa-free |
| **Passport Depth** | How well-connected those destinations are, weighted by their Reach |

## Results

> Data source: [Passport Index Dataset](https://github.com/ilyankou/passport-index-dataset) | 199 countries analyzed

### Highlights

| Country | Reach Rank | Depth Rank | Change |
|---------|----------:|----------:|-------:|
| UAE | 1 | 19 | -18 |
| South Korea | 2 | 1 | +1 |
| Germany | 3 | 4 | -1 |
| Singapore | 3 | 9 | -6 |
| United States | 9 | 25 | -16 |

UAE drops significantly because many of its visa-free destinations have weaker passports. South Korea rises to #1 because its visa-free destinations are predominantly strong-passport countries.

### Ranking (sorted by Depth score)

| # | Country | Reach Score | Reach Rank | Depth Score | Depth Rank | Delta |
|--:|---------|----------:|----------:|----------:|----------:|------:|
| 1 | South Korea | 98.81 | 2 | 100.00 | 1 | +1 |
| 2 | Spain | 98.81 | 2 | 99.53 | 2 | 0 |
| 3 | Denmark | 98.21 | 3 | 99.34 | 3 | 0 |
| 4 | Finland | 98.21 | 3 | 99.12 | 4 | -1 |
| 4 | France | 98.21 | 3 | 99.12 | 4 | -1 |
| 4 | Germany | 98.21 | 3 | 99.12 | 4 | -1 |
| 4 | Italy | 98.21 | 3 | 99.12 | 4 | -1 |
| 4 | Norway | 98.21 | 3 | 99.12 | 4 | -1 |
| 5 | Sweden | 98.21 | 3 | 99.06 | 5 | -2 |
| 6 | Ireland | 97.62 | 4 | 99.01 | 6 | -2 |
| 7 | Japan | 97.62 | 4 | 99.00 | 7 | -3 |
| 8 | Luxembourg | 97.62 | 4 | 98.83 | 8 | -4 |
| 8 | Netherlands | 97.62 | 4 | 98.83 | 8 | -4 |
| 8 | Austria | 97.62 | 4 | 98.83 | 8 | -4 |
| 8 | Switzerland | 97.62 | 4 | 98.83 | 8 | -4 |
| 9 | Singapore | 98.21 | 3 | 98.70 | 9 | -6 |
| 10 | Belgium | 97.02 | 5 | 98.66 | 10 | -5 |
| 10 | Portugal | 97.02 | 5 | 98.66 | 10 | -5 |
| 11 | New Zealand | 96.43 | 6 | 98.32 | 11 | -5 |
| 12 | Greece | 96.43 | 6 | 98.26 | 12 | -6 |
| 13 | Poland | 96.43 | 6 | 97.98 | 13 | -7 |
| 14 | Hungary | 95.83 | 7 | 97.81 | 14 | -7 |
| 15 | United Kingdom | 96.43 | 6 | 97.80 | 15 | -9 |
| 16 | Czech Republic | 95.83 | 7 | 97.76 | 16 | -9 |
| 17 | Canada | 95.83 | 7 | 97.53 | 17 | -10 |
| 18 | Australia | 95.83 | 7 | 97.50 | 18 | -11 |
| 19 | United Arab Emirates | 100.00 | 1 | 97.38 | 19 | -18 |
| 20 | Liechtenstein | 94.64 | 9 | 97.37 | 20 | -11 |
| 21 | Malta | 95.83 | 7 | 97.32 | 21 | -14 |
| 22 | Croatia | 95.24 | 8 | 97.27 | 22 | -14 |
| 22 | Estonia | 95.24 | 8 | 97.27 | 22 | -14 |
| 22 | Slovakia | 95.24 | 8 | 97.27 | 22 | -14 |
| 23 | Iceland | 94.64 | 9 | 97.05 | 23 | -14 |
| 24 | Slovenia | 94.64 | 9 | 96.88 | 24 | -15 |
| 25 | United States | 94.64 | 9 | 96.73 | 25 | -16 |
| 26 | Latvia | 94.64 | 9 | 96.66 | 26 | -17 |
| 27 | Lithuania | 94.64 | 9 | 96.37 | 27 | -18 |
| 28 | Cyprus | 94.05 | 10 | 96.20 | 28 | -18 |
| 29 | Bulgaria | 94.05 | 10 | 96.01 | 29 | -19 |
| 29 | Romania | 94.05 | 10 | 96.01 | 29 | -19 |
| 30 | Monaco | 92.26 | 11 | 94.67 | 30 | -19 |
| 31 | Malaysia | 94.64 | 9 | 94.23 | 31 | -22 |
| 32 | Hong Kong | 90.48 | 12 | 93.08 | 32 | -20 |
| 33 | Chile | 88.69 | 14 | 92.33 | 33 | -19 |
| 34 | Brazil | 90.48 | 12 | 92.28 | 34 | -22 |
| 35 | Argentina | 89.88 | 13 | 91.94 | 35 | -22 |
| 36 | Andorra | 86.90 | 15 | 91.14 | 36 | -21 |
| 37 | San Marino | 86.90 | 15 | 90.43 | 37 | -22 |
| 38 | Israel | 85.71 | 16 | 89.95 | 38 | -22 |
| 39 | Barbados | 85.71 | 16 | 88.26 | 39 | -23 |
| 40 | Brunei | 84.52 | 17 | 87.83 | 40 | -23 |
| 41 | Bahamas | 83.93 | 18 | 86.66 | 41 | -23 |
| 42 | Uruguay | 81.55 | 19 | 86.45 | 42 | -23 |
| 43 | Mexico | 81.55 | 19 | 85.69 | 43 | -24 |
| 44 | Saint Vincent and the Grenadines | 81.55 | 19 | 84.55 | 44 | -25 |
| 45 | Vatican | 80.36 | 21 | 84.06 | 45 | -24 |
| 46 | Saint Kitts and Nevis | 80.95 | 20 | 83.76 | 46 | -26 |
| 47 | Peru | 78.57 | 23 | 83.18 | 47 | -24 |
| 48 | Costa Rica | 77.38 | 25 | 82.62 | 48 | -23 |
| 49 | Ukraine | 80.36 | 21 | 82.51 | 49 | -28 |
| 50 | Paraguay | 77.38 | 25 | 82.48 | 50 | -25 |
| 51 | Antigua and Barbuda | 79.76 | 22 | 81.88 | 51 | -29 |
| 52 | Seychelles | 80.36 | 21 | 81.44 | 52 | -31 |
| 53 | Trinidad and Tobago | 77.98 | 24 | 80.55 | 53 | -29 |
| 54 | Macao | 76.19 | 26 | 80.37 | 54 | -28 |
| 55 | Panama | 74.40 | 28 | 79.79 | 55 | -27 |
| 56 | Grenada | 75.60 | 27 | 78.63 | 56 | -29 |
| 57 | Saint Lucia | 76.19 | 26 | 78.58 | 57 | -31 |
| 58 | Mauritius | 76.19 | 26 | 78.13 | 58 | -32 |
| 59 | Colombia | 70.83 | 32 | 76.65 | 59 | -27 |
| 60 | Serbia | 73.81 | 29 | 76.36 | 60 | -31 |
| 61 | El Salvador | 70.24 | 33 | 76.24 | 61 | -28 |
| 62 | Dominica | 72.62 | 30 | 75.56 | 62 | -32 |
| 63 | Guatemala | 69.05 | 35 | 75.18 | 63 | -28 |
| 64 | Solomon Islands | 72.02 | 31 | 74.92 | 64 | -33 |
| 65 | Taiwan | 69.64 | 34 | 74.77 | 65 | -31 |
| 66 | Honduras | 68.45 | 36 | 74.40 | 66 | -30 |
| 67 | Georgia | 72.02 | 31 | 74.06 | 67 | -36 |
| 68 | North Macedonia | 69.64 | 34 | 73.26 | 68 | -34 |
| 69 | Montenegro | 69.64 | 34 | 72.64 | 69 | -35 |
| 70 | Tuvalu | 68.45 | 36 | 71.67 | 70 | -34 |
| 71 | Samoa | 68.45 | 36 | 71.44 | 71 | -35 |
| 72 | Tonga | 67.86 | 37 | 70.83 | 72 | -35 |
| 73 | Nicaragua | 65.48 | 40 | 70.72 | 73 | -33 |
| 74 | Kiribati | 66.67 | 38 | 69.96 | 74 | -36 |
| 75 | Marshall Islands | 66.07 | 39 | 69.92 | 75 | -36 |
| 76 | Venezuela | 66.67 | 38 | 69.21 | 76 | -38 |
| 77 | Bosnia and Herzegovina | 66.07 | 39 | 69.00 | 77 | -38 |
| 78 | Palau | 64.29 | 41 | 68.21 | 78 | -37 |
| 79 | Albania | 64.29 | 41 | 67.91 | 79 | -38 |
| 80 | Moldova | 65.48 | 40 | 67.67 | 80 | -40 |
| 81 | Micronesia | 63.10 | 42 | 66.81 | 81 | -39 |
| 82 | Russia | 69.64 | 34 | 58.05 | 82 | -48 |
| 83 | Turkey | 68.45 | 36 | 57.80 | 83 | -47 |
| 84 | Timor-Leste | 52.38 | 45 | 54.99 | 84 | -39 |
| 85 | Qatar | 63.10 | 42 | 53.29 | 85 | -43 |
| 86 | Kosovo | 47.02 | 52 | 50.45 | 86 | -34 |
| 87 | South Africa | 58.33 | 43 | 48.72 | 87 | -44 |
| 88 | Belize | 52.98 | 44 | 46.91 | 88 | -44 |
| 89 | Kuwait | 58.33 | 43 | 46.80 | 89 | -46 |
| 90 | Ecuador | 51.79 | 46 | 45.60 | 90 | -44 |
| 91 | Jamaica | 51.19 | 47 | 43.57 | 91 | -44 |
| 92 | Maldives | 51.19 | 47 | 42.97 | 92 | -45 |
| 93 | Guyana | 48.81 | 49 | 42.61 | 93 | -44 |
| 94 | Fiji | 48.81 | 49 | 42.22 | 94 | -45 |
| 95 | Vanuatu | 49.40 | 48 | 41.33 | 95 | -47 |
| 96 | Bahrain | 51.79 | 46 | 41.19 | 96 | -50 |
| 97 | Saudi Arabia | 51.79 | 46 | 41.18 | 97 | -51 |
| 98 | Thailand | 48.81 | 49 | 40.69 | 98 | -49 |
| 99 | Belarus | 49.40 | 48 | 40.02 | 99 | -51 |
| 100 | Nauru | 45.24 | 54 | 39.62 | 100 | -46 |
| 101 | Kazakhstan | 48.21 | 50 | 39.24 | 101 | -51 |
| 102 | Oman | 49.40 | 48 | 39.18 | 102 | -54 |
| 103 | Suriname | 43.45 | 56 | 37.83 | 103 | -47 |
| 104 | Botswana | 45.83 | 53 | 37.77 | 104 | -51 |
| 105 | Bolivia | 42.86 | 57 | 37.68 | 105 | -48 |
| 106 | Indonesia | 47.62 | 51 | 37.67 | 106 | -55 |
| 107 | China | 49.40 | 48 | 37.38 | 107 | -59 |
| 108 | Papua New Guinea | 43.45 | 56 | 36.11 | 108 | -52 |
| 109 | Lesotho | 43.45 | 56 | 35.52 | 109 | -53 |
| 110 | Azerbaijan | 44.05 | 55 | 34.76 | 110 | -55 |
| 111 | Dominican Republic | 42.26 | 58 | 34.74 | 111 | -53 |
| 112 | Swaziland | 42.26 | 58 | 34.42 | 112 | -54 |
| 113 | Armenia | 41.67 | 59 | 34.04 | 113 | -54 |
| 114 | Namibia | 42.86 | 57 | 33.82 | 114 | -57 |
| 115 | Mongolia | 39.29 | 62 | 33.24 | 115 | -53 |
| 116 | Malawi | 41.07 | 60 | 32.20 | 116 | -56 |
| 117 | Philippines | 39.29 | 62 | 31.76 | 117 | -55 |
| 118 | Kenya | 41.67 | 59 | 31.40 | 118 | -59 |
| 119 | Tanzania | 40.48 | 61 | 30.87 | 119 | -58 |
| 120 | Zambia | 39.29 | 62 | 30.82 | 120 | -58 |
| 121 | Morocco | 42.26 | 58 | 30.73 | 121 | -63 |
| 122 | Tunisia | 41.67 | 59 | 29.87 | 122 | -63 |
| 123 | Cuba | 36.90 | 66 | 29.37 | 123 | -57 |
| 124 | Kyrgyzstan | 38.10 | 64 | 29.18 | 124 | -60 |
| 125 | Uzbekistan | 37.50 | 65 | 29.09 | 125 | -60 |
| 126 | Gambia | 38.69 | 63 | 29.06 | 126 | -63 |
| 127 | Uganda | 38.10 | 64 | 28.90 | 127 | -63 |
| 128 | Ghana | 39.29 | 62 | 28.88 | 128 | -66 |
| 129 | Cape Verde | 38.69 | 63 | 28.11 | 129 | -66 |
| 130 | Sao Tome and Principe | 36.31 | 67 | 28.08 | 130 | -63 |
| 131 | Zimbabwe | 36.31 | 67 | 27.88 | 131 | -64 |
| 132 | Sierra Leone | 35.71 | 68 | 27.07 | 132 | -64 |
| 133 | Madagascar | 34.52 | 69 | 26.94 | 133 | -64 |
| 134 | India | 34.52 | 69 | 26.25 | 134 | -65 |
| 135 | Tajikistan | 34.52 | 69 | 25.87 | 135 | -66 |
| 136 | Benin | 35.71 | 68 | 25.83 | 136 | -68 |
| 137 | Rwanda | 38.10 | 64 | 25.79 | 137 | -73 |
| 138 | Gabon | 33.93 | 70 | 25.26 | 138 | -68 |
| 139 | Mozambique | 33.33 | 71 | 25.07 | 139 | -68 |
| 140 | Cambodia | 31.55 | 74 | 24.48 | 140 | -66 |
| 141 | Vietnam | 30.95 | 75 | 24.01 | 141 | -66 |
| 142 | Togo | 33.33 | 71 | 23.65 | 142 | -71 |
| 143 | Burkina Faso | 33.33 | 71 | 23.31 | 143 | -72 |
| 144 | Equatorial Guinea | 33.33 | 71 | 23.26 | 144 | -73 |
| 145 | Comoros | 30.95 | 75 | 23.23 | 145 | -70 |
| 146 | Guinea | 32.14 | 73 | 23.20 | 146 | -73 |
| 147 | Bhutan | 30.95 | 75 | 23.20 | 147 | -72 |
| 148 | Senegal | 32.74 | 72 | 22.82 | 148 | -76 |
| 149 | Mauritania | 31.55 | 74 | 22.52 | 149 | -75 |
| 150 | Egypt | 30.95 | 75 | 22.32 | 150 | -75 |
| 151 | Jordan | 29.76 | 77 | 22.22 | 151 | -74 |
| 152 | Algeria | 32.14 | 73 | 22.18 | 152 | -79 |
| 153 | Haiti | 29.17 | 78 | 22.09 | 153 | -75 |
| 154 | Niger | 31.55 | 74 | 21.97 | 154 | -80 |
| 155 | Turkmenistan | 29.76 | 77 | 21.94 | 155 | -78 |
| 156 | Ivory Coast | 31.55 | 74 | 21.73 | 156 | -82 |
| 157 | Chad | 29.76 | 77 | 21.25 | 157 | -80 |
| 158 | Laos | 28.57 | 79 | 21.02 | 158 | -79 |
| 159 | Guinea-Bissau | 29.76 | 77 | 20.89 | 159 | -82 |
| 160 | Angola | 29.76 | 77 | 20.84 | 160 | -83 |
| 161 | Mali | 30.36 | 76 | 20.66 | 161 | -85 |
| 162 | Djibouti | 27.98 | 80 | 20.41 | 162 | -82 |
| 163 | Cameroon | 28.57 | 79 | 20.36 | 163 | -84 |
| 164 | Burundi | 27.98 | 80 | 20.06 | 164 | -84 |
| 165 | Central African Republic | 29.17 | 78 | 20.02 | 165 | -87 |
| 166 | Sri Lanka | 25.60 | 84 | 19.97 | 166 | -82 |
| 167 | Lebanon | 26.79 | 82 | 19.22 | 167 | -85 |
| 168 | Congo | 27.98 | 80 | 19.14 | 168 | -88 |
| 169 | Liberia | 27.38 | 81 | 19.04 | 169 | -88 |
| 170 | Ethiopia | 26.19 | 83 | 18.95 | 170 | -87 |
| 171 | Myanmar | 25.00 | 85 | 18.69 | 171 | -86 |
| 172 | Iran | 25.60 | 84 | 18.32 | 172 | -88 |
| 173 | DR Congo | 25.60 | 84 | 18.00 | 173 | -89 |
| 174 | South Sudan | 25.60 | 84 | 17.85 | 174 | -90 |
| 175 | Nigeria | 25.60 | 84 | 17.53 | 175 | -91 |
| 176 | Bangladesh | 21.43 | 88 | 16.68 | 176 | -88 |
| 177 | Eritrea | 23.81 | 86 | 16.47 | 177 | -91 |
| 178 | Sudan | 23.81 | 86 | 16.35 | 178 | -92 |
| 179 | Nepal | 23.21 | 87 | 16.14 | 179 | -92 |
| 180 | Libya | 23.81 | 86 | 16.00 | 180 | -94 |
| 181 | North Korea | 23.81 | 86 | 15.94 | 181 | -95 |
| 182 | Palestine | 21.43 | 88 | 15.57 | 182 | -94 |
| 183 | Pakistan | 19.05 | 91 | 14.09 | 183 | -92 |
| 184 | Yemen | 20.83 | 89 | 14.07 | 184 | -95 |
| 185 | Somalia | 19.64 | 90 | 13.79 | 185 | -95 |
| 186 | Iraq | 18.45 | 92 | 12.95 | 186 | -94 |
| 187 | Syria | 16.67 | 93 | 11.79 | 187 | -94 |
| 188 | Afghanistan | 15.48 | 94 | 10.76 | 188 | -94 |

### Biggest Rank Changes (Reach → Depth)

| Country | Reach Rank | Depth Rank | Delta |
|---------|----------:|----------:|------:|
| North Korea | 86 | 181 | -95 |
| Somalia | 90 | 185 | -95 |
| Yemen | 89 | 184 | -95 |
| Afghanistan | 94 | 188 | -94 |
| Iraq | 92 | 186 | -94 |
| Libya | 86 | 180 | -94 |
| Palestine | 88 | 182 | -94 |
| Syria | 93 | 187 | -94 |
| Nepal | 87 | 179 | -92 |
| Pakistan | 91 | 183 | -92 |
| Sudan | 86 | 178 | -92 |
| Eritrea | 86 | 177 | -91 |
| Nigeria | 84 | 175 | -91 |
| South Sudan | 84 | 174 | -90 |
| DR Congo | 84 | 173 | -89 |
| Bangladesh | 88 | 176 | -88 |
| Congo | 80 | 168 | -88 |
| Iran | 84 | 172 | -88 |
| Liberia | 81 | 169 | -88 |
| Central African Republic | 78 | 165 | -87 |

## Access Types Counted as Visa-Free

- Visa free (unrestricted)
- Visa on arrival
- ETA (Electronic Travel Authorization)
- Numeric day allowances (e.g., 90 days)

Excluded: e-visa, visa required, no admission.

## Data Source

[Passport Index Dataset](https://github.com/ilyankou/passport-index-dataset) by Ilya Ilyankou (MIT license).

## Usage

```bash
dotnet run --project src
```

Output is written to `CountryRating.md`.

## Requirements

- .NET 10 SDK
