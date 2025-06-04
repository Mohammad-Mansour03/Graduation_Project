# Hojozaty - Venue Booking App

Hojozaty is a modern and user-friendly venue booking application designed to simplify the process of reserving spaces for events, meetings, and gatherings. With a sleek interface and powerful functionality, Hojozaty ensures seamless communication between venue owners and customers.

## Features

### For Users:
- **Search for Venues**: Filter venues by location, date, price, capacity, and type.
- **View Venue Details**: Access comprehensive information, including photos, amenities, and reviews.
- **Instant Booking**: Book venues in real-time or send inquiries to venue owners.
- **Manage Bookings**: Track upcoming and past bookings with a personalized dashboard.

### For Venue Owners:
- **List Venues**: Add venue details, including availability, pricing, and policies.
- **Manage Bookings**: Accept or decline booking requests.
- **Analytics**: View booking statistics and revenue reports.

### General Features:
- **Secure Payments**: Supports multiple payment gateways.
- **User Reviews and Ratings**: Build trust with genuine feedback from customers.

## Technologies Used

- **Frontend**: .NET MAUI using XAML.
- **Backend**:C# with MVVM design pattern.
- **Database**: Supabase.

## Getting Started

### Prerequisites
1. Install .NET SDK: [Download here](https://dotnet.microsoft.com/download)
2. Install Visual Studio or configure VS Code for .NET MAUI development.
3. Set up SQL Server or Azure SQL Database.

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/hojozaty.git
   cd hojozaty
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Set up the database:
   - Run the SQL scripts in the `/Database` folder to initialize the database.
   - Update the connection string in `appsettings.json`.
4. Run the app:
   ```bash
   dotnet build
   dotnet run
   ```

### Running the App
- Use the terminal to run the app locally.
- Access the app on your emulator, device, or desktop.

## Contribution
We welcome contributions from the community! To contribute:
1. Fork the repository.
2. Create a new branch for your feature or bug fix:
   ```bash
   git checkout -b feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add feature or fix description"
   ```
4. Push your changes:
   ```bash
   git push origin feature-name
   ```
5. Create a pull request.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.


