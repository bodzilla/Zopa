Run instructions:
- Set "Zopa.Console" as start-up project
- Compile and build application
- Modify appsettings.json as required
- Run using: "dotnet .\Zopa.Console.dll [market_file_path] [loan_amount]"

Packages used:
- CsvHelper (fairly supported community-driven project)
- Microsoft.Extensions.Logging
- Microsoft.Extensions.DependencyInjection
- NUnit3TestAdapter (for unit-testing)

Development approach:
- TDD (although I wrote my code before-hand)
- Any contract length / loan increments / loan ranges can be used
- Lender / quotes are immutable
- The decimal point constraints are defined in the front end as I percieved this to be a UI choice rather than a business constraint, therefore a different front end could use the same data but with it's own constaint rules

Would like to improve by:
- Pushing as a serverless cloud app (i.e. Azure Function, AWS Lambda)
- Using BDD approach to testing with relevant frameworks
- Using a NoSQL database as data store
- Using async approach
- Custom exceptions
- Comments in unit tests (lack of time)