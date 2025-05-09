💳 Virtual Card Application
This project allows users to apply for and receive a virtual card upon request. It's a full-stack web application built using React, ASP.NET, and PostgreSQL.

        🖼️ Preview
🔐 Login Page

📝 Virtual Card Application Page

💳 Virtual Card Example

🧠 Architecture Overview

React handles the frontend user interface

ASP.NET serves as the backend API

PostgreSQL stores user and card data



        🚀 Features
User registration and login

Apply for and receive a virtual card

Backend API for handling business logic

Secure storage of data in PostgreSQL


        🛠 Tech Stack
Frontend: React.js

Backend: ASP.NET (.NET 9)

Database: PostgreSQL



        📦 Installation & Setup
        🔧 Backend (ASP.NET)
Navigate to the backend project directory

Create a file named .env or use appsettings.Development.json to configure environment variables

Set up the following environment variables:

ini
Copy
Edit
ConnectionStrings__DefaultConnection=Host=YOUR_HOST;Port=5432;Database=YOUR_DB;Username=YOUR_USER;Password=YOUR_PASSWORD
Jwt__Key=YOUR_SECRET_KEY
Jwt__Issuer=YOUR_ISSUER
Jwt__Audience=YOUR_AUDIENCE
Run the application:

bash
Copy
Edit
dotnet run


        💻 Frontend (React)
Navigate to the frontend directory

Create a .env file in the root of the React project

Add the following environment variable:

bash
Copy
Edit
REACT_APP_API_BASE_URL=http://localhost:5000/api
Install dependencies:

bash
Copy
Edit
npm install
Start the development server:

bash
Copy
Edit
npm start


        🌐 API Endpoints
Method	Endpoint	Description
POST	/api/auth/signup	Register a new user
POST	/api/auth/login	User login
POST	/api/card/apply	Apply for a virtual card
GET	/api/card/{id}	Get virtual card details
🚧 Future Improvements
Email notifications

Card expiration & limits

Admin dashboard

Download or export card as image


        📄 License
This project is licensed under the MIT License.

Want help creating a .env.template file or generating placeholder images for your project? I can do that too!
