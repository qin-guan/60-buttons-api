# `60-buttons-api`

The API for [60-buttons](https://github.com/qin-guan/60-buttons).

Built with <3 using .NET 8 and SignalR.

## Why .NET?

Mostly for me to test out the new features in .NET 8 like Native AOT, Minimal APIs, and primary constructors.

Unfortunately SignalR does not support Native AOT yet so that was dropped.

It was also an opportunity for me to test out the new observability features! Metrics and traces from runtime, ASP.NET Core and Entity Framework Core are all exported to [Honeycomb](https://honeycomb.io).

Pretty cool for a project that took less than 2 days.