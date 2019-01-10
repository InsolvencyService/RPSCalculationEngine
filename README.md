# CalculationsEngine
CalculationsEngine is a Rest Api that performs the redundancy payments calculations based on business and legal requirements. There are one or more endpoints for each calculation; all accept json payloads, perform calculations and return results in json. 

# API Endpoints
The GET methods on all calculations serves as a ping function to test the endpoints. The POST methods that are listed below accepts the JSON request model, performs the calculation and returns the response in JSON format. The following are the endpoints that are available in calculationsEngine 

| Path                        | Calculation |  Description                               |
| --------------------------- |:--------:| ------------------------------------------ |
|/appa | Appa | Calculates Arrears of pay and protective award. Each component can be calculated independent of each other |
|/apportionment | Apportionment | Calculates the apportionment percentage for the given claim amount |
|/ba| Basic award| Calculates the NI & tax for the given basic award amount |
|/hol | Holiday| Calculates Holiday Pay Accrued and the holiday taken not paid claims. Each component can be calculated independent of each other |
|/notice |Notice | Calculates Compensatory notice pay and the notice worked not paid claims. Each component can be calculated independent of each other|
|/pnd | Projected notice date | Calculates the projected notice date for the given length of service |
|/rp | Redundancy payments | Calculates the redundancy payment for the claimant |
|/ront | Refund of notional tax | Calculates the notional tax refund and the compensatory notice paid after the refund |

# Prerequisites
* Visual studio 2017
* Net Core 2.1

# Getting Started
Clone the repository on your development machine using the link https://github.com/InsolvencyService/RPSCalculationEngine.

Open the solution in Visual studio. Make Insolvency.CalculationEngine.Redundancy.API as start-up project and hit run. This will allow to run project and open a swagger generated page(configured on http://localhost:58065/swagger/)  that shows the interface definitions and the json models for the request and response payloads.

# License
This project is licensed under the MIT License - see the LICENSE.md file for details

