# ASP.NET Core Back-End API - javaHate

Welcome to the JavaHate Back-End API! This is a simple project built using **ASP.NET Core** for managing resources via RESTful API endpoints.

## How to Run

1. Clone the repository.
2. Open the project in your preferred IDE.
3. Update the database using the .NET CLI:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

4. Run the application using the .NET CLI:

    ```bash
    dotnet run
    ```

## View Endpoints

1. Start the project
2. Go to localhost:{Port}/swagger

## Testing

1. Make sure you have all dependencies installed:

    ```bash
    dotnet restore
    ```

2. run them with the command:

    ```bash
    dotnet test
    ```

Enjoy using the API!
