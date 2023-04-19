"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/availableFlightsHub")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();
connection
    .start()
    .then(() => this.getConnectionId())
    .catch(err => console.error(err.toString()));

function getConnectionId(){
     connection.invoke("GetConnectionId")
        .then(function (connectionId) {
            publishAvailableFlightsToClient(connectionId);
        }
     );
}

function publishAvailableFlightsToClient(connectinoId) {
    $.ajax({
        type: "POST",
        url: "/home/PublishAvailableFlightsToClient",
        data: {
            "connectionId": connectinoId,
            "signalMethodName": "publishAvailableFlightsToClientSignal"
        },
        dataType: "text",
        success: function (msg) {
            console.log(msg);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
}

connection.on("publishAvailableFlightsToClientSignal", (jsonAirlineApiResult) => {
    var airlineApiResultsPlaceholder = $("#airlineApiResultsPlaceholder");  
    var htmlAirlineApiResult = renderAirlineApiResult(jsonAirlineApiResult);       
    airlineApiResultsPlaceholder.append(htmlAirlineApiResult);
});

function renderAirlineApiResult(jsonAirlineApiResult) {
    var htmlAvailableFlights = '';

    for (var counter = 0; counter < jsonAirlineApiResult.availableFlights.length; counter++) {
        var currentAvailableFlight = jsonAirlineApiResult.availableFlights[counter];
        htmlAvailableFlights +=
            `<ul class="list-group my-2 mx-2"> 
                <li class="list-group-item list-group-item-secondary">${currentAvailableFlight.flightNumber}</li> 
                <li class="list-group-item ">${currentAvailableFlight.boardingDateTime}</li>                
                <li class="list-group-item ">${currentAvailableFlight.sourceCity}</li>
                <li class="list-group-item ">${currentAvailableFlight.destinationCity}</li>
                <li class="list-group-item ">${currentAvailableFlight.availableSeatNumbers}</li>
                <li class="list-group-item ">${currentAvailableFlight.allowableCargoWeight}</li>
                <li class="list-group-item ">${currentAvailableFlight.sourceAirport}</li>
                <li class="list-group-item ">${currentAvailableFlight.destinationAirport}</li>
                <li class="list-group-item ">${currentAvailableFlight.price}</li>
            </ul>`;
    }

    var htmlAvailableFlightsConainer =
        `<div class="card-body">
            <div class="card">
                <div class="card-header">
                    <h3>
                        ${jsonAirlineApiResult.airlineName}
                    </h3>                
                </div> 
                    ${htmlAvailableFlights}
            </div>
        </div>`;

    return htmlAvailableFlightsConainer;
}