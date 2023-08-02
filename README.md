# XM-Test-Task

## 1. Installation
- You only need to install .Net 6.0
https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## 2. Running

2.1 If you're using an IDE:
- Run the XM_Test_Task configuration from launchSettings.json and skip step 2.2.

2.2 If you're not using an IDE:
- Using CLI go to the root folder of the repo on your local machine, then go to the "XM-Test-Task" project - ```cd XM-Test-Task```
- Run ```dotnet run``` - this should restore and build the project in debug configuration.

The app now should be started and running on your localhost on port 5001 using HTTPS, or port 5000 using HTTP.

## 3. Usage
- Swagger is located at ```{host}/swagger/index.html```
- Endpoint #1 - getting aggregated btc-usd price by specific time point - located at ```{host}/BitcoinPricesFetch/GetBySpecificTime/{ticks}```. Ticks should be in the Unix epoch format - ```[ 0 .. 253402297199 ]```
- Endpoint #2 - getting a list of btc-usd prices from data storage by time range - ```{host}/BitcoinPricesFetch/GetByRange?StartTicks={StartTicks}&EndTicks={EndTicks}``` Both query parameters should also be in the Unix epoch format - ```[ 0 .. 253402297199 ]```
