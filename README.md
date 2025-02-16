# Crypto Gateway Website

## 🚀 Overview

The **Crypto Gateway Website** is a secure, scalable platform that enables businesses to create payment links for invoices and orders. Customers can make payments seamlessly using QR codes with **Tether (USDT)** or other popular cryptocurrencies.

## 💡 Features

- **Business Registration:** Businesses can sign up and manage their crypto payments.
- **Payment Link Generation:** Easily create payment links for invoices or orders.
- **QR Code Payments:** Customers can pay instantly via QR codes.
- **Multi-Currency Support:** Accept payments in USDT and other cryptocurrencies.
- **Secure Transactions:** Implements robust security measures to ensure safe transactions.

## 🛠️ Tech Stack

- **Frontend:** Angular
- **Backend:** NodeJs & .NET Core
- **Database:** MS SQL Server
- **Crypto Integration:** TronWeb

## ⚙️ How It Works

1. **Business Registration:** Businesses sign up and create their accounts.
2. **Payment Link Creation:** They generate payment links for specific invoices/orders.
3. **Customer Payment:** Customers scan the QR code and pay using their preferred crypto wallet.
4. **Transaction Confirmation:** The system verifies and records the payment securely.

## 📦 Project Structure

```
/CryptoGatewaySample
├── Src
    ├── 0.ApplicationCore
        ├── CryptoGateway.DataAccess
        ├── CryptoGateway.Domain
        ├── CryptoGateway.DomainService
        ├── CryptoGateway.Framework
        ├── CryptoGateway.Infrastructure
        ├── CryptoGateway.Service
    ├── 1.EndPoints
        ├── CryptoGateway.BoApi
        ├── CryptoGateway.Job
        ├── CryptoGateway.NodeJsApi
        ├── CryptoGateway.PublicApi
        ├── CryptoGateway.WebApi
    ├── 2.Clients
        ├── CryptoGateway.Web
├── Tests
    ├── CryptoGateway.Domain.UnitTest
├── CryptoGateway.sln (Solution file)
└── README.md
```

## 🚀 Getting Started

1. **Clone the Repository:**\
   `git clone https://github.com/ehsanforoughi/CryptoGatewaySample.git`
2. **Setup Backend:**
   - Navigate to `/CryptoGateway`
   - Configure environment variables
   - Run `dotnet build` and `dotnet run`
3. **Setup Frontend:**
   - Navigate to `/ClientApp`
   - Run `npm install` and `ng serve`

## 📧 Contact

For any questions or collaborations, reach out at: [foroughi.ehsan@gmail.com]\
GitHub: [https://github.com/ehsanforoughi](https://github.com/ehsanforoughi)

---

*Built with ❤️ using .NET Core & Angular*

