"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/flightTicketsHub")
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
            publishFlightTicketsToClient(connectionId);
        }
     );
}

function publishFlightTicketsToClient(connectinoId) {
    $.ajax({
        type: "POST",
        url: "/home/PublishFlightTicketsToClient",
        data: {
            "connectionId": connectinoId,
            "signalMethodName": "publishFlightTicketsToClientSignal"
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

connection.on("publishFlightTicketsToClientSignal", (jsonAirlineApiResult) => {
    var airlineApiResultsPlaceholder = $("#airlineApiResultsPlaceholder");  
    var htmlAirlineApiResult = renderAirlineApiResult(jsonAirlineApiResult);       
    airlineApiResultsPlaceholder.append(htmlAirlineApiResult);
});

function renderAirlineApiResult(jsonAirlineApiResult) {
    var htmlFlightTickets = '';

    for (var counter = 0; counter < jsonAirlineApiResult.flightTickets.length; counter++) {
        var currentFlightTicket = jsonAirlineApiResult.flightTickets[counter];
        htmlFlightTickets +=
            `<ul class="list-group my-2 mx-2"> 
                <li class="list-group-item list-group-item-secondary">${currentFlightTicket.flightNumber}</li> 
                <li class="list-group-item ">${currentFlightTicket.boardingDateTime}</li>                
                <li class="list-group-item ">${currentFlightTicket.sourceCity}</li>
                <li class="list-group-item ">${currentFlightTicket.destinationCity}</li>
                <li class="list-group-item ">${currentFlightTicket.availableSeatNumbers}</li>
                <li class="list-group-item ">${currentFlightTicket.allowableCargoWeight}</li>
                <li class="list-group-item ">${currentFlightTicket.sourceAirport}</li>
                <li class="list-group-item ">${currentFlightTicket.destinationAirport}</li>
                <li class="list-group-item ">${currentFlightTicket.price}</li>
            </ul>`;
    }

    var htmlFlightTicketsConainer =
        `<div class="card-body">
            <div class="card">
                <div class="card-header">
                    <h3>
                        ${jsonAirlineApiResult.airlineName}
                    </h3>                
                </div> 
                    ${htmlFlightTickets}
            </div>
        </div>`;

    return htmlFlightTicketsConainer;
}