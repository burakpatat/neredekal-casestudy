#### NeredeKal CaseStudy ();

Mikroservis mimarisi ile yazılan bu proje, **otel yönetimi** ve **raporlama** işlemleri için iki mikroservis içermektedir. Mikroservisler arasında `RabbitMQ` aracılığıyla iletişim sağlanmaktadır. `HotelService`, otel ve iletişim bilgilerini yönetirken, `ReportService`, konum bazlı raporları asenkron olarak üretir.

 #### Ana İşlevler :
- **Otel Yönetimi**: Otel oluşturma, kaldırma, iletişim bilgisi ekleme, iletişim bilgisi kaldırma, Otel yetkililerinin listelenmesi, Otel ile ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi
- **Raporlama**: Konum bazlı raporlar (otel sayısı, telefon numarası sayısı).

#### Mesajlaşma Sistemi
Uygulama, **RabbitMQ** kullanarak mikroservisler arasında asenkron veri iletimi sağlar. **HotelService**, **ReportService**'e rapor talepleri gönderirken RabbitMQ kullanır ve **ReportService**, gelen talepleri asenkron olarak alıp işler.
**HotelService**, rapor taleplerini **RabbitMQ**'ya gönderir. Kullanıcı bir rapor talep ettiğinde, **HotelService** bu talebi RabbitMQ kuyruğuna gönderir. Bu işlem, `ReportRequestedEvent` adıyla yapılan bir mesajlaşma ile gerçekleşir.

#### Kullanılan Teknolojiler

- **Backend Framework**: `.NET Core 9`
- `MongoDB`: ReportService
- `PostgreSQL`: HotelService
- **Mesajlaşma Sistemi**: `RabbitMQ`
- **Loglama ve İzleme**: `ELK Stack` (Elasticsearch, Kibana)
- **API Gateway**: `Ocelot`
- **Containerization**: `Docker Compose`

**Domain-Driven-Design**, **Event-Driven-Architecture**, **CQRS**, **Mediator**, **Generic Repository**, **UnitOfWork** 

#### Çalıştırma
`docker-compose.yaml` içerisinde yazılan image'ler ile PostgreSQL ve ELK Stack kurulumları yapabilirsiniz, proje de RabbitMQ ve MongoDB **Cloud** tarafında kullanılmaktadır. appsettings.json içinde konfigürasyonlardan bakabilirsiniz.

Proje konumundan 
```bash 
  docker-compose up -d
```
komutu ile servisleri ayağa kaldırabilirsiniz,
HotelService içinde ki PostgreSQL konfigurasyonu 

```bash 
  "PostgreSqlConnection": "Host=localhost;Port=5432;Database=neredekalHotelDb;Username=poisondev;Password=poisondevdocker"
```

`Gateway` `localhost:5284` dizini üzerinde ki swagger dökümanı ile Service dökümanları arası geçiş yapabilirsiniz

**Configure Startup Project**'den Gateway.API HotelService.API ve ReportService.API projelerine Start verip projeyi ayağa kaldırabilirsiniz.

#### Portlar
- Gateway => 5284

- HotelService.API => 5284

- Gateway => 5055

- ReportService.API => 5284

- PostgreSQL => 5284  - ElasticSearch => 9200  - Kibana => 5601


![Ekran görüntüsü 2025-01-03 164118](https://github.com/user-attachments/assets/68fb850c-8677-4b14-9b82-1e12ec2c4e13)

![Ekran görüntüsü 2025-01-03 163953](https://github.com/user-attachments/assets/f3a629c9-61fd-421d-9551-b9313e0350fc)

![Ekran görüntüsü 2025-01-03 164859](https://github.com/user-attachments/assets/ecff769f-678e-4e15-802a-96d2e7a7b019)

![Ekran görüntüsü 2025-01-03 165019](https://github.com/user-attachments/assets/940d80dd-82cb-4da8-8399-0b4d6a276e85)

![Ekran görüntüsü 2025-01-03 165035](https://github.com/user-attachments/assets/2a3dd5f6-1be7-43ca-b727-c6f3fed399a2)

![Ekran görüntüsü 2025-01-03 165051](https://github.com/user-attachments/assets/a2ffa1f4-be0d-4df4-8b6d-b614dc325299)

**Solution Structure**

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
├── common/
│   ├── SharedKernel (RabbitMQ, BaseEntity)
│   └── EventBus (RabbitMQ, MediatR vb)
│
├── tests/  (UnitTest, IntegrationTest)
│   ├── Gateway.Tests
│   ├── HotelService.Tests
│   └── ReportService.Tests
│
├── docker-compose.yml
│   
├── README.md
└── .gitignore
