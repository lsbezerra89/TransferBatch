# Transfer Batch Processor

This is a console application developed in C# that processes a batch of transfers from a CSV file, calculates commissions, and displays the result in the console.

## Project Structure

The project follows a Domain-Driven Design (DDD) architecture, with separation of responsibilities into the following folders:

- **Domain**: Contains the main entity (`Transfer`) and a DTO class (`CommissionDTO`).
- **Services**: 
	- Business rules for commission calculation (`TransferService`).
	- CSV file reading and manipulation (`FileService`).
- **App**: Presentation layer, containing the console project and dependency injection class.
- **Tests**: Unit tests to validate business logic.

## Requirements

- .NET 8.0

## Build the Project

- To build the project, run the following command:
```
dotnet build
```

- Run the application by passing the path to your CSV file, as shown in the example:
```
dotnet run --project TransferBatch path/sample.transfers.csv
OR
TransferBatch.App.exe "PATH\sample.csv"
```



## Contact

- **Name**: Leandro Bezerra
- **Email**: leandro.santos.bezerra@hotmail.com
- **Linkedin**: https://www.linkedin.com/in/leandro-bezerra-68a057194/
- **GitHub**: https://github.com/lsbezerra89
