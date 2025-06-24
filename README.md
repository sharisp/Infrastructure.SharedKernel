# 🛠️ Infrastructure.Shared.Kernel

A lightweight shared infrastructure kernel for .NET 8 projects, encapsulating reusable infrastructure concerns like `DbContext` base logic, soft-delete filtering, user context access, and domain event support.

---

## 📦 Installation

Install via NuGet:Andrew.Infrastructure.SharedKernel 


## 📁 Features

### 🔧 `AppHelper.cs`

Reads configuration values from `appsettings.json` and provides convenient static access helpers.

> ✅ Simplifies configuration binding and access across different layers.

---

### 🗃️ `BaseDbContext.cs`

A custom `DbContext` base class that:

* Automatically applies \[soft delete filters] using `IsDel` flag.

> ✅ Perfect base for all EF Core contexts in DDD-style applications.

---

### 👤 `CurrentUser.cs`

Provides a service to access **current logged-in user** context:

* `UserId`

Typically bound to `HttpContext` via `IHttpContextAccessor`.

> ✅ Useful for audit tracking and per-user operations.

---

### 📑 `IUnitOfWork` + `UnitOfWork.cs`

Handles transactional consistency and domain event publishing. Features:

* Automatic audit data filling
* Dispatches MediatR events for created/modified entities
* Centralized `SaveChangesAsync`

> ✅ Works seamlessly with your `BaseDbContext` and aggregates.

---

---

## 🧠 Design Principles

* 🔁 **Reusable** across services
* 📦 Packaged as **shared NuGet**
* 👥 Designed for **DDD + Clean Architecture**
* 🧪 Unit-testable components


```

---

## 📄 License

MIT License.

---

## 👤 Author

Created by **Andrew Wang**.
Contributions and issues welcome!
