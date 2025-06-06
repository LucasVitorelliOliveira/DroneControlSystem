using DroneControlSystem.Server.Services;

// 1. Cria um construtor de aplicação web. É a base de qualquer app ASP.NET Core/gRPC.
var builder = WebApplication.CreateBuilder(args);

// 2. Adiciona os serviços gRPC ao container de injeção de dependência.
//    Isso permite que o sistema gerencie o ciclo de vida dos nossos serviços.
builder.Services.AddGrpc();

// 3. Constrói a aplicação.
var app = builder.Build();

// 4. Mapeia nosso serviço gRPC para o pipeline de roteamento.
//    Quando uma chamada chegar para o 'RegistrationService', o Kestrel saberá
//    que deve encaminhá-la para uma instância de 'RegistrationServiceImpl'.
app.MapGrpcService<RegistrationServiceImpl>();

// Endpoint de diagnóstico. Se você acessar http://localhost:5000 no navegador, verá esta mensagem.
app.MapGet("/", () => "O servidor gRPC está no ar. A comunicação deve ser feita via um cliente gRPC.");

// 5. Inicia o servidor e o faz escutar na porta 5000 para qualquer endereço IP (0.0.0.0).
Console.WriteLine("Iniciando servidor gRPC na porta 5000...");
app.Run("http://0.0.0.0:5000");

// Esta linha só será alcançada quando o servidor for encerrado (ex: com Ctrl+C).
Console.WriteLine("Servidor encerrado.");;