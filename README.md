# Online Furniture Store 🛋

 This is a Furniture Store Website built using ASP.NET Core MVC. It allows users to browse products, add them to their wishlist and cart, place orders, make payments using PayPal, and manage their profiles. Admins have full control over managing products, categories, users, and orders, as well as assigning roles to users.

 ## Features
 - **User Functionality:**
   - **Sign Up / Login:** Users can create an account or log in with existing credentials.
   - **View Products:** Browse furniture products by category.
   - **Add to Wishlist:** Users can add products to their wishlist for later reference.
   - **Add to Cart:** Users can add products to their cart and manage quantities before checkout.
   - **Place Orders:** Users can proceed to checkout, provide delivery details, and confirm their orders.
   - **PayPal Payment Integration:** Users can make payments securely through PayPal integration.
   - **Order History:** Users can view their past orders and track delivery status.
   - **Product Reviews:** Users can leave reviews on products they have purchased.
   - **Profile Management:** Users can manage their profile information, such as name and address.
     
- **Admin Functionality:**
     - **Product Management:** Admins can perform CRUD (Create, Read, Update, Delete) operations on products.
     - **Category Management:** Admins can manage product categories.
     - **Order Management:** Admins can view and manage customer orders.
     - **User Management:** Admins can manage user accounts and their roles.
     - **Role Management:** Admins can assign roles (e.g., user, admin) to manage access levels.

## Architecture and Design Patterns
  - **Repository Pattern:** Used to handle data access logic, making the code more maintainable and testable.
  - **Unit of Work Pattern:** Ensures changes are committed to the database in a single transaction.
  - **Authentication:** Cookie-based authentication is used to secure user sessions.
    
## Technologies Used
  - **ASP.NET Core MVC:** For building the web application.
  - **Entity Framework Core:** As an ORM for database operations.
  - **PayPal SDK:** For integrating PayPal payments.
  - **Bootstrap:** For responsive front-end design.
  - **AJAX:** For partial page updates.
    
## Installation and Setup
1. Clone the repository:
   ```bash
          git clone https://github.com/saraelshaer/Online-Furniture-Store.git
2. Navigate to the project directory:
   ```bash
          cd Online-Furniture-Store
3. Set up the database connection string in appsettings.json.
4. Run the following command to apply migrations and seed the database:
   ```bash
           dotnet ef database update
5. Run the application:
   ```bash
           dotnet run


## Usage

  - **For Users:** Sign up, browse products, add to cart/wishlist, place orders, and manage your profile.
  - **For Admins:** Log in to access the admin dashboard for managing products, categories, orders, users, and roles.

## Contribution
Contributions are welcome! Please fork the repository and submit a pull request for any feature enhancements or bug fixes.

## Team members
- [Sara Elazb](https://github.com/SaraElazb)
- [Sara Elshaer](https://github.com/saraelshaer) 

  
 
