# MicroMart E-Commerce  - Overview of the project

## Requirement Overview

### Functional Requirements:
- Users can browse a catalog of products.
- Users can add products to their shopping cart.
- Users can place orders from their shopping cart.
- Products in the cart have a time limit of **10 minutes**. If the user does not complete the purchase within this period, the cart will be cleared, and the stock will be reset.
- A successful order placement will trigger:
  - Stock updates.
  - An email notification to the user.

### Notes:
- Any user can view the product catalog.
- Any user can add products to their shopping cart.
- Only authenticated users can place an order.

## Tech Stack
- **Backend:** .NET
- **API Gateway:** Ocelot
- **Authentication:** Keycloak (Ongoing)
- **Database:** PostgreSQL
- **Cache:** Redis
- **Message Broker:** RabbitMQ
- **Containerization:** Docker
- **Logging & Monitoring:** ELK Stack (Ongoing) 

## Microservices Responsibilities

### 1. **Authentication Service**
- User registration and creation.
- Authentication and authorization.
- Account verification.
- Password recovery.
- Managing roles and permissions. (Ongoing)

### 2. **User Service**
- Managing user profiles.
- Automatically creating user profiles upon successful authentication.
- Interfacing with authentication service for user registration.

### 3. **Product Service**
- Creating and managing product listings.
- Inventory management.
- Fetching all available products.
- Fetching detailed product information along with available stock.
- Updating and deleting products.

### 4. **Inventory Service**
- Maintaining product inventory.
- Managing stock history.

### 5. **Cart Service**
- Creating carts for both authenticated and anonymous users.
- Holding stock for **10-15 minutes** before auto-reverting inventory if not purchased.
- Displaying all available carts per user or session.

### 6. **Order Service**
- Creating new orders.
- Triggering the email service upon order completion.
- Clearing the cart after order placement.
- Updating stock through the inventory service.
- Recording order details in the user service.

### 7. **Notification Service**
- Sending order confirmation and notification emails.
- Storing email history for tracking purposes.

---
### Contributions

### License
This project is licensed under the [MIT License](LICENSE).

For more details, refer to the repository documentation.

