connection = new signalR.HubConnectionBuilder().withUrl("/ChatBox").build();
connection.start();
console.log(connection)

connection.on("ReceiveMessage", function (message) {
    var li = document.createElement("li");
    li.classList.add('sender');
    var p = document.createElement("p");
    p.textContent = message;
    li.appendChild(p);
    document.getElementsByClassName("forRealTimeChat").appendChild(li);
});

document.getElementById("realTimeChatSendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userConnectionId").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });

    var li = document.createElement("li");
    li.classList.add('replay');
    var p = document.createElement("p");
    p.textContent = message;
    li.appendChild(p);
    document.getElementById("forRealTimeChat").appendChild(li);

});