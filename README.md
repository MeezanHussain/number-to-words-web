# Number to Words Web Application

This project implements a simple web application that converts numerical
amounts into their written English representation. It has been developed
in **C#** using the .NET 6 minimal API model and serves a small HTML
interface so that the conversion routine can be exercised interactively
via a web browser.

## Features

- Server‑side conversion: A C# class (`NumberToWordsConverter`) turns
  decimal values into words without relying on any external libraries or
  advanced language features. The algorithm splits numbers into
  manageable three‑digit groups (hundreds, thousands, millions, etc.)
  and processes each group independently. Cents are treated as
  a two‑digit integer and appended after the word “and”.
- Web API: A minimal API endpoint (`/api/convert`) accepts an
  `amount` query parameter and returns the amount in words. Invalid
  input produces a meaningful error message.
- Interactive UI: The root page serves a small HTML form allowing
  users to enter an amount (e.g. `123.45`) and immediately see the
  converted words.

## Why this approach?

Several different architectures could satisfy the requirement of
converting numbers to words and providing a web interface:

1. **Console application with a self‑hosted web server:** A simple
   console program could listen for HTTP requests using
   `HttpListener`. While functional, it lacks the conveniences and
   security provided by a modern web framework.

2. **ASP.NET MVC:** A traditional ASP.NET MVC application could serve
   Razor pages and controllers. However, MVC introduces significant
   boilerplate for routing, controllers and views that is unnecessary
   for a single‑endpoint application.

3. **Minimal API (chosen):** The .NET 9 minimal API provides
   a lightweight, modern way to build HTTP services using just a few
   lines of code. It supports serving static files (our HTML page),
   routing and dependency injection out of the box, without the
   overhead of controllers or startup classes. For a small utility
   service, this approach strikes a balance between simplicity and
   maintainability.

## How the number‑to‑words algorithm works

The conversion algorithm avoids advanced language features to make the
logic explicit:

1. **Splitting dollars and cents:** The input is parsed into
   a `decimal` value, then split into integer (`dollars`) and
   fractional (`cents`) parts. Negative amounts are prefaced with
   the word “MINUS”.

2. **Three‑digit groups:** The integer part is divided into groups of
   three digits (units, thousands, millions, etc.). Each group is
   processed independently to generate words for the hundreds, tens
   and ones places.

3. **Handling teens and tens:** The converter stores arrays of
   pre‑computed strings for numbers zero through nineteen and for the
   tens (twenty, thirty, etc.). Numbers from 20–99 are constructed
   by combining the appropriate tens value with the ones value.

4. **Group names:** An array of group names (`THOUSAND`, `MILLION`,
   `BILLION`, etc.) is used to append the appropriate magnitude after
   processing each three‑digit group.

5. **Cents:** The fractional part is multiplied by 100 and treated
   as a whole number between 0 and 99. It is converted using the same
   logic as a three‑digit group and appended with “CENT(S)” after
   the dollars part.

This approach is straightforward to follow, easy to unit test and does
not rely on any external libraries or the `System.Globalization` number
formatting utilities. By avoiding features such as LINQ or generics,
the algorithm remains accessible to readers with a basic understanding
of C#.

## Build and run instructions

The project uses the standard .NET project structure and requires
.NET 6 or later to build and run.

To build and run the application:

```bash
# Navigate into the project directory
cd NumberToWordsWeb

# Restore any missing packages (handled automatically in newer SDKs)
dotnet restore

# Build the project
dotnet build

# Run the application; by default this binds to http://localhost:5000
dotnet run
```

Once running, navigate a browser to `http://localhost:5000/` to view the
web interface. Enter numerical amounts (e.g. `123.45`) and click
**Convert** to see the text representation.

The API endpoint can also be called directly:

```
GET http://localhost:5000/api/convert?amount=789.01
```

## Tests

Automated testing is supported via a companion console test harness project:

### Structure

```
NumberToWordsWeb.Tests
│   NumberToWordsWeb.Tests.csproj
│   Program.cs   – Executes automated test cases
```

### Running tests

1. Open a terminal and navigate to the `NumberToWordsWeb.Tests` directory:

   ```bash
   cd NumberToWordsWeb.Tests
   ```

2. Run:

   ```bash
   dotnet run
   ```

3. The program will execute a set of predefined test cases (from the `TestPlan.md`)
   and display **PASS/FAIL** results for each input.

The test harness ensures:

- Accuracy of conversion for whole numbers, decimals, large values, and edge cases.
- Output matches the format and wording described in the requirements.

## File structure

```
number-to-words-web
│
├── NumberToWordsWeb
│   │   NumberToWordsWeb.csproj
│   │   Program.cs
│   │   NumberToWordsConverter.cs
│   │   README.md
│   │   TestPlan.md
│   └── wwwroot/index.html
│
└── NumberToWordsWeb.Tests
    │   NumberToWordsWeb.Tests.csproj
    │   Program.cs
```

## Limitations and future improvements

The converter currently supports magnitudes up to trillions. If
extremely large numbers (e.g. quadrillions) are required, extend
the `Groups` array accordingly.
Only two decimal places are considered for cents. Additional
precision could be handled by changing the multiplier (e.g. to 1000
for milli‑units) and renaming “CENTS” appropriately.
The HTML interface is intentionally simple. It could be enhanced
with input validation, real‑time updates or additional styling.

---
