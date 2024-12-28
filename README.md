# neredekal-casestudy
---

## **Solution Structure**

```plaintext
neredekalcase
│
├── src/
│   ├── Gateway/
│   │   ├── Gateway.API
│   │   └── Dockerfile
│   │
│   ├── HotelService/
│   │   ├── HotelService.API
│   │   ├── HotelService.Application
│   │   ├── HotelService.Domain
│   │   ├── HotelService.Infrastructure 
│   │   └── HotelService.Shared
│   │   └── Dockerfile
│   │
│   ├── ReportService/
│   │   ├── ReportService.API 
│   │   ├── ReportService.Application
│   │   ├── ReportService.Domain
│   │   ├── ReportService.Infrastructure
│   │   └── ReportService.Shared
│   │   └── Dockerfile
│   │
│   ├── UI/
│   │   ├── WebUI.Client (Blazor WebAssembly UI)
│   │   ├── WebUI.Shared
│   │   └── Dockerfile
│
├── Common/
│   ├── SharedKernel (RabbitMQ, BaseEntity)
│   └── EventBus (RabbitMQ, MediatR vb)
│
├── docker-compose.yml
├── logstash/
│   └── logstash.conf
│   
├── README.md
└── .gitignore
