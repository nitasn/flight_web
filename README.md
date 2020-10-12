# flight_web

This project consists of a WebPage client and an API Server.

The webpage draws a large map, that shows flights' paths;
It supports adding flights (by dragging a json in), showing details, etc.

All data is synced with the server; it filters out flights that are not in the air, calculates each flight's current lat/long, and sends that to the client. It also supports connecting to more servers to sync flights between them.

The server is a WebAPI built with C# ASP.NET.
The client uses Bootstrap and uses Google Maps API.

![alt text](https://github.com/nitasn/flight_web/blob/master/ScreenShot.png?raw=true)

It's a second year university project.
