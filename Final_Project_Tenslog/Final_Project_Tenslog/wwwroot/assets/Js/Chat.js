 connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});
console.log(connection)

connection.on("ReceiveMessage", function (time, myMesage, message) {
    var li = document.createElement("li");
    if (myMesage) {
        li.classList.add("sender");
    }
    else {
        li.classList.add("repaly");
    }
    var p = document.createElement("p");
    p.textContent = message;
    li.appendChild(p);

    var span = document.createElement("span");
    span.classList.add("time", "text-light");
    span.textContent = time;
    li.appendChild(span);

    var containerElement = document.getElementById("forRealTimeChat");
    containerElement.appendChild(liElement);
});

document.getElementById("realTimeChatSendButton").addEventListener("click", function (event) {
    var userId = document.getElementById("userId").value;
    var message = document.getElementById("realTimeChatInput").value;
    connection.invoke("SendMessage", userId,message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});