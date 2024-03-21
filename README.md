A logging service developed with large scale distributed systems in mind.

Consists of a connector to the logging system that is used to push logs from a system developed in any framework/programming language. This lends
itself to microservices architectures where each node can publish to the central logging system and thus all system logs can be managed 
from a single central place.

The main logging system is implemented using Asp.Net Core where logs published are streamed to a browser using
Asp.net SignalR server push technology.

The connector from the application publishes the logs to mongodb. The UI pulls the logs from mongodb and streams them to the browser if there active connections.

The problem is reduced to developing the connector once the logging system is in place. We currently have a .Net standard and a Java connector in place.


