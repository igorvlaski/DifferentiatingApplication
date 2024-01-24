# DifferentiatingApplication

Overview
The DifferentiatingApplication is an ASP.NET Core Web API designed for storing and comparing base64 encoded binary data. Users can store data on 'left' and 'right' sides for a given ID and then retrieve differences, if any, between them. The application features endpoints for saving data and retrieving computed differences, making it an effective tool for data comparison tasks.

Usage and Development
To use the application, simply send a PUT request to /v1/diff/{id}/{side} with base64 encoded data, and use GET /v1/diff/{id} to fetch the comparison result, which can either be 'Equals', 'SizeDoNotMatch', or 'ContentDoNotMatch' with detailed diffs. Built with ASP.NET Core and Entity Framework Core, the application includes a DiffService for business logic and a DiffController for handling API requests. For development and testing, it’s equipped with unit and integration tests which can be run using standard .NET testing tools.
