using DroneControlSystem.Protos; // Importa as classes geradas (RegisterRequest, etc.)
using Grpc.Core;

namespace DroneControlSystem.Server.Services
{
    // A classe herda de 'RegistrationService.RegistrationServiceBase', que foi gerada pelo Grpc.Tools.
    // Isso nos força a implementar os métodos definidos no serviço do .proto.
    public class RegistrationServiceImpl : RegistrationService.RegistrationServiceBase
    {
        // Usar ILogger para registrar eventos é uma ótima prática.
        private readonly ILogger<RegistrationServiceImpl> _logger;

        public RegistrationServiceImpl(ILogger<RegistrationServiceImpl> logger)
        {
            _logger = logger;
        }

        // Esta é a implementação real do nosso método RPC.
        public override Task<RegisterResponse> RegisterDrone(RegisterRequest request, ServerCallContext context)
        {
            // 1. Gera um ID único para o drone que acaba de se conectar.
            var newDroneId = $"Drone_{Guid.NewGuid().ToString().Substring(0, 8)}";
            
            // 2. Loga a informação no console do servidor. Essencial para debugging.
            _logger.LogInformation(
                "Novo drone tentando se registrar! Hardware ID: {HardwareId}, foi atribuído o ID: {AssignedId}", 
                request.DroneHardwareId, newDroneId);

            // 3. Loga as capacidades que o drone informou.
            foreach (var capability in request.Capabilities)
            {
                _logger.LogInformation(" -> Drone reportou a capacidade: {Capability}", capability);
            }

            // 4. Monta o objeto de resposta com as informações que o drone precisará.
            var response = new RegisterResponse
            {
                AssignedDroneId = newDroneId,
                RabbitmqHost = "localhost", // Endereço do seu broker. Use "localhost" para testes locais.
                TelemetryExchange = "telemetry_exchange",
                CommandsExchange = "commands_exchange"
            };

            // 5. Retorna a resposta. Como a operação é rápida, usamos Task.FromResult.
            // Para operações lentas (ex: acesso a banco de dados), usaríamos 'async/await'.
            return Task.FromResult(response);
        }
    }
}