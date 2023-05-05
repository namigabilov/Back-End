let connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.start();